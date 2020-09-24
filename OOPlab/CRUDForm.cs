using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Specialized;
using OOPLab.Items;
using OOPLab.MyClasses;
using OOPLab.Attributes;

namespace OOPlab
{

    public partial class CRUDForm : Form
    {
        Type currentType = null;
        object currentObject = null;
        Classes.TMode currentMode;
        Form parentForm;
        MainForm mainForm;
        public object returnObj = null;
        Dictionary<Control, KeyValuePair<FieldInfo, object>> controlList = new Dictionary<Control, KeyValuePair<FieldInfo, object>>();
        Dictionary<string, object> objectList = new Dictionary<string, object>();

        public CRUDForm()
        {
            InitializeComponent();
        }
        public CRUDForm(Classes.TMode mode, Type type, object obj, Form parentform, MainForm mainForm)
        {
            InitializeComponent();
            currentObject = obj;
            currentType = type;
            controlList.Clear();
            this.parentForm = parentform;
            this.mainForm = mainForm;
            currentMode = mode;
            AddComponents(type, obj);

        }
        public object RecursiveCreateObject(FieldInfo fieldInfo)
        {
            returnObj = null;
            CRUDForm form = new CRUDForm(Classes.TMode.create, fieldInfo.FieldType, null, this, mainForm);
            form.ShowDialog();
            form.Dispose();
            return returnObj;
        }
        public Button CreateButton(int x, int y)
        {
            Button btn = new Button();
            btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            btn.Location = new System.Drawing.Point(this.Width / 2 - 80, y);
            btn.Name = "btnAcccept";
            btn.Size = new System.Drawing.Size(160, 80);
            btn.Text = "Готово";
            btn.UseVisualStyleBackColor = true;
            btn.Click += new System.EventHandler(Button_Click);
            return btn;
        }
        public Label CreateLabel(FieldInfo member, int x, int y, object obj = null)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.ForeColor = System.Drawing.Color.Black;
            lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            lbl.Location = new System.Drawing.Point(x, y);
            lbl.Name = "lbl" + member.Name;
            lbl.Size = new System.Drawing.Size(160, 20);
            lbl.Width = 160;
            lbl.Text = member.GetCustomAttributes<FieldNameAttribute>().SingleOrDefault().Value;

            return lbl;
        }
        public TextBox CreateEdit(FieldInfo member, int x, int y, object obj = null)
        {
            TextBox tb = new TextBox();
            tb.Location = new System.Drawing.Point(x, y);
            tb.Name = "edt" + member.Name;
            tb.Size = new System.Drawing.Size(160, 20);
            if (obj != null)
            {
                tb.Text = member.GetValue(obj).ToString();
            }
            if (currentMode == Classes.TMode.read)
            {
                tb.ReadOnly = true;
            }
            return tb;
        }
        public ComboBox CreateComboBox(FieldInfo member, int x, int y, object obj = null)
        {

            ComboBox cbb = new ComboBox();
            cbb.FormattingEnabled = true;
            cbb.Location = new System.Drawing.Point(x, y);
            cbb.Name = "cbb" + member.Name;
            cbb.Size = new System.Drawing.Size(160, 20); ;
            if (obj != null)
            {
                cbb.SelectedItem = member.GetValue(obj).ToString();
                cbb.Text = member.GetValue(obj).ToString();
            }
            if (currentMode == Classes.TMode.read)
            {
                cbb.Enabled = false;
                cbb.DropDownStyle = ComboBoxStyle.DropDownList;
                cbb.AutoCompleteMode = AutoCompleteMode.None;
            }
            cbb.KeyPress += new KeyPressEventHandler(ComboBox_KeyPress);
            return cbb;
        }
        public ComboBox CreateObjectComboBox(FieldInfo member, int x, int y, object obj = null)
        {

            ComboBox cob = new ComboBox();
            cob.FormattingEnabled = true;
            cob.Location = new System.Drawing.Point(x, y);
            cob.Name = "cob" + member.Name;
            cob.Size = new System.Drawing.Size(160, 20);
            cob.Items.Add("создать новый...");

            if (parentForm is MainForm)
            {
                foreach (Ammunition i in ((MainForm)parentForm).Catalog)
                {
                    if (i.GetType() == member.FieldType)
                        cob.Items.Add(i.Name);
                }
            }
            Ammunition item = null;
            if (obj != null)
            {
                item = (mainForm).Catalog.Find(o => o.Name == ((Ammunition)member.GetValue(obj)).Name && o.GetType() == member.FieldType);
                cob.SelectedItem = ((Ammunition)member.GetValue(obj)).Name.ToString();
                cob.Text = ((Ammunition)member.GetValue(obj)).Name.ToString();
            }
            cob.SelectedIndexChanged += new System.EventHandler(ObjectComboBox_SelectedIndexChanged);
            controlList.Add(cob, new KeyValuePair<FieldInfo, object>(member, item));
            cob.KeyPress += new KeyPressEventHandler(ComboBox_KeyPress);
            if (currentMode == Classes.TMode.read)
            {
                cob.DropDownStyle = ComboBoxStyle.DropDownList;
                cob.AutoCompleteMode = AutoCompleteMode.None;
            }
            return cob;
        }
        public void UpdateObjectComboBox(object obj = null)
        {
            Dictionary<Control, KeyValuePair<FieldInfo, object>> tempControlList = new Dictionary<Control, KeyValuePair<FieldInfo, object>>();
            foreach (KeyValuePair<Control, KeyValuePair<FieldInfo, object>> pair in controlList)
            {
                if (pair.Key.Name.ToString().Substring(0, 3) == "cob")
                {
                    ComboBox cob = (pair.Key as ComboBox);
                    FieldInfo member = pair.Value.Key;
                    object tempObj = pair.Value.Value;
                    cob.Items.Clear();
                    cob.Items.Add("создать новый...");

                    if (parentForm is MainForm)
                    {
                        foreach (Ammunition item in ((MainForm)parentForm).Catalog)
                        {
                            if (item.GetType() == member.FieldType)
                                cob.Items.Add(item.Name);
                        }
                    }
                    if (obj != null)
                    {
                        Ammunition item = (mainForm).Catalog[(mainForm).Catalog.IndexOf((Ammunition)obj)];
                        if (item.GetType() != member.FieldType)
                        {
                            continue;
                        }
                        cob.SelectedItem = (mainForm).Catalog.Find(x => x.Name == item.Name && x.GetType() == member.FieldType).ToString();
                    }
                }

            }
        }
        public void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }
        public CheckBox CreateCheckBox(FieldInfo member, int x, int y, object obj = null)
        {

            CheckBox ckb = new CheckBox();
            ckb.AutoSize = true;
            ckb.Location = new System.Drawing.Point(x, y);
            ckb.Name = "ckb" + member.Name;
            ckb.Size = new System.Drawing.Size(25, 25);
            ckb.TabIndex = 66;
            ckb.UseVisualStyleBackColor = true;
            if (obj != null)
            {
                ckb.Checked = Convert.ToBoolean(member.GetValue(obj));
            }
            ckb.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            return ckb;
        }
        public void Button_Click(object sender, EventArgs e)
        {
            Ammunition item = null;
            if (currentMode == Classes.TMode.read)
            {
                this.Close();
            }
            else
            if (AdditingCheck())
            {

                item = CreateObject();
                switch (currentMode)
                {
                    case Classes.TMode.create:
                        {
                            mainForm.Catalog.Add(item);
                            if (parentForm is CRUDForm)
                            {
                                (parentForm as CRUDForm).returnObj = item;
                            }
                            break;
                        }
                    case Classes.TMode.update:
                        {

                            int index = mainForm.Catalog.IndexOf((Ammunition)currentObject);
                            Ammunition replacedItem = mainForm.Catalog[index];
                            foreach (Ammunition obj in mainForm.Catalog)
                            {
                                FieldInfo[] fi = obj.GetType().GetFields();
                                foreach (FieldInfo member in fi)
                                {
                                    if (member.FieldType.IsClass && member.FieldType.Name != "String")
                                    {
                                        if (member.FieldType.Name == replacedItem.GetType().Name)
                                        {
                                            if (member.GetValue(obj) == replacedItem)
                                            {
                                                member.SetValue(obj, item);

                                            }
                                        }
                                    }
                                }
                            }
                            mainForm.Catalog.Insert(index, item);
                            mainForm.Catalog.Remove(replacedItem);
                            break;
                        }
                }
                this.Close();
            }
        }
        public bool CheckConvert(KeyValuePair<Control, KeyValuePair<FieldInfo, object>> pair)
        {
            try
            {
                switch (pair.Value.Key.FieldType.Name)
                {
                    case "Integer":
                        {
                            int i1 = Convert.ToInt32(pair.Key.Text);
                            break;
                        }
                    case "Int16":
                        {
                            Int16 i2 = Convert.ToInt16(pair.Key.Text);
                            break;
                        }
                    case "Int32":
                        {
                            Int32 i3 = Convert.ToInt32(pair.Key.Text);
                            break;
                        }
                    case "Int64":
                        {
                            Int64 i4 = Convert.ToInt64(pair.Key.Text);
                            break;
                        }
                    case "UInt16":
                        {
                            UInt16 i2 = Convert.ToUInt16(pair.Key.Text);
                            break;
                        }
                    case "UInt32":
                        {
                            UInt32 i3 = Convert.ToUInt32(pair.Key.Text);
                            break;
                        }
                    case "UInt64":
                        {
                            UInt64 i4 = Convert.ToUInt64(pair.Key.Text);
                            break;
                        }
                    case "Double":
                        {
                            Double i3 = Convert.ToDouble(pair.Key.Text);
                            break;
                        }
                    case "Single":
                        {
                            Single i3 = Convert.ToSingle(pair.Key.Text);
                            break;
                        }
                    case "Data":
                        {
                            DateTime i3 = Convert.ToDateTime(pair.Key.Text);
                            break;
                        }
                    default:
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        private bool AdditingCheck()
        {
            foreach (KeyValuePair<Control, KeyValuePair<FieldInfo, object>> pair in controlList)
            {

                if (pair.Key.Name.ToString().Substring(0, 3) == "ckb")
                {
                    continue;
                }

                if (pair.Key.Name.ToString().Substring(0, 3) == "cob")
                {

                    continue;
                }
                if (pair.Key.Name.ToString().Substring(0, 3) == "cbb")
                {
                    if (((pair.Key) as ComboBox).SelectedIndex == -1)
                    {
                        MessageBox.Show("Поле " + pair.Value.Key.Name.ToString() + " не выбрано");
                        return false;
                    }
                    continue;
                }
                if (pair.Key.Name.ToString().Substring(0, 3) == "edt")
                {
                    if (((pair.Key) as TextBox).Text.Length == 0)
                    {
                        MessageBox.Show("Поле " + pair.Value.Key.Name.ToString() + " не заполнено");
                        return false;
                    }
                    else
                    {
                        if (pair.Value.Key.FieldType.IsPrimitive)
                        {
                            if (!CheckConvert(pair))
                            {
                                MessageBox.Show("Проверьте введённые данные в поле: " + pair.Value.Key.Name.ToString(), "Ошибка");
                                return false;
                            }
                        }
                    }
                    continue;
                }
            }
            return true;
        }
        public Ammunition CreateObject()
        {
            NameValueCollection list = new NameValueCollection();
            foreach (KeyValuePair<Control, KeyValuePair<FieldInfo, object>> pair in controlList)
            {
                if (pair.Key.Name.ToString().Substring(0, 3) == "cob")
                {
                    objectList.Add(pair.Value.Key.Name.ToString(), pair.Value.Value);
                    continue;
                }
                if (pair.Key.Name.ToString().Substring(0, 3) == "edt")
                {
                    list[pair.Value.Key.Name.ToString()] = pair.Key.Text.ToString();
                    continue;
                }
                if (pair.Key.Name.ToString().Substring(0, 3) == "ckb")
                {
                    if ((pair.Key as CheckBox).Checked)
                    {
                        list[pair.Value.Key.Name.ToString()] = "True";
                    }
                    else
                    {
                        list[pair.Value.Key.Name.ToString()] = "False";
                    }
                    continue;
                }
                if (pair.Key.Name.ToString().Substring(0, 3) == "cbb")
                {
                    list[pair.Value.Key.Name.ToString()] = (pair.Key as ComboBox).Text;
                    continue;
                }
            }
            ConstructorInfo[] ci = currentType.GetConstructors();
            Ammunition item = null;
            foreach (ConstructorInfo c in ci)
            {
                ParameterInfo[] pi = c.GetParameters();
                if (pi.Length == 2)
                    if (pi[0].ParameterType.Name == "NameValueCollection" && pi[1].ParameterType.Name == "Dictionary`2")
                    {
                        object[] parameters = { list, objectList };
                        item = (Ammunition)c.Invoke(parameters);
                        break;
                    }
            }
            return item;
        }
        private void ObjectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == -1)
            {
                return;
            }

            if ((sender as ComboBox).SelectedIndex == 0)
            {
                KeyValuePair<FieldInfo, object> kv = controlList[(sender as ComboBox)];
                object obj = RecursiveCreateObject(kv.Key);
                if (obj != null)
                {
                    FieldInfo member = controlList[(sender as ComboBox)].Key;
                    controlList.Remove((sender as ComboBox));
                    controlList.Add((sender as ComboBox), new KeyValuePair<FieldInfo, object>(member, obj));
                    UpdateObjectComboBox(obj);
                }
                else
                {
                    (sender as ComboBox).SelectedIndex = -1;
                }
            }
            else
            {
                FieldInfo member = controlList[(sender as ComboBox)].Key;
                Ammunition item = (mainForm).Catalog.Find(x => x.Name == (sender as ComboBox).GetItemText((sender as ComboBox).SelectedItem) && x.GetType() == member.FieldType);
                controlList.Remove((sender as ComboBox));
                controlList.Add((sender as ComboBox), new KeyValuePair<FieldInfo, object>(member, item));
            }
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == -1)
            {
                return;
            }

        }
        public void CheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
        public void AddComponents(Type type, object obj)
        {
            int x = 12, y = 20;
            int height = 40;
            this.SuspendLayout();
            FieldInfo[] fi = type.GetFields();
            foreach (FieldInfo member in fi)
            {
                Label lbl = CreateLabel(member, x, y);
                this.Controls.Add(lbl);
                if (member.FieldType.IsClass && member.FieldType.Name != "String")
                {
                    ComboBox cob = CreateObjectComboBox(member, x + 165, y, obj);  
					this.Controls.Add(cob);            
                    y += 40;
                    continue;
                }
                if (member.FieldType.Name == "Boolean")
                {
                    CheckBox ckb = CreateCheckBox(member, x + 165, y, obj);

                    this.Controls.Add(ckb);
                    controlList.Add(ckb, new KeyValuePair<FieldInfo, object>(member, null));
                    y += 40;
                    continue;
                }
                if ((member.FieldType.IsEnum))
                {
                    ComboBox cbb = CreateComboBox(member, x + 165, y, obj);
                    Type tp = Type.GetType(member.FieldType.ToString());
                    FieldInfo[] fields = tp.GetFields();
                    foreach (var field in fields)
                    {

                        if (field.Name == "value__")
                            continue;
                        cbb.Items.Add(field.Name.ToString());

                    }
                    if (obj != null)
                    {
                        cbb.SelectedItem = Enum.Parse(member.FieldType, (member.GetValue(obj)).ToString()).ToString();
                        cbb.Text = cbb.GetItemText(cbb.SelectedItem);
                    }
                    cbb.SelectedIndexChanged += new System.EventHandler(ComboBox_SelectedIndexChanged);
                    this.Controls.Add(cbb);
                    controlList.Add(cbb, new KeyValuePair<FieldInfo, object>(member, null));

                    y += 40;
                    continue;
                }
                TextBox edt = CreateEdit(member, x + 165, y, obj);
                this.Controls.Add(edt);
                controlList.Add(edt, new KeyValuePair<FieldInfo, object>(member, null));
                y += 40;

            }
            Button btn = CreateButton(0, y);
            this.Controls.Add(btn);
            this.Height = height + y + 80;
            this.ResumeLayout();
        }
        private void CRUDForm_Load(object sender, EventArgs e)
        {

        }
    }
}
