using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using OOPLab.Attributes;
using OOPLab.Items;
using OOPLab.MyClasses;


namespace OOPlab
{
    public partial class MainForm : Form
    {
        public static Plugin _curr_Plugin = null;
        string ItemNamespace = "OOPLab.Items";
        public List<Ammunition> Catalog = new List<Ammunition>();
        public static FileCreator[] FileCreators = { new BinaryFileCreator(), new JsonFileCreator(), new AuthorFileCreator() };
        public MainForm()
        {
            InitializeComponent();
            ColumnHeader header;
            string[] Names = { "Name", "Category", "Price", "Count", "Country" };
            int[] Length = { 200, 160, 120, 150, 150 };
            for (int index = 0; index < 5; index++)
            {
                header = new ColumnHeader();
                header.Text = Names[index];
                header.Width = Length[index];
                lvOrders.Columns.Add(header);
            }
            lvOrders.View = View.Details;
            
            List<Type> classes = Classes.GetClassesFromNamespace(ItemNamespace);
            foreach (Type index in classes)
            {
                cbbType.Items.Add(index.Name);
            }
            dlgOpenFile.Filter = "Binary file|*.bin|Json file|*.json|MyJson|*.myjson";
            dlgSaveFile.Filter = "Binary file|*.bin|Json file|*.json|MyJson|*.myjson";
        }
        public void AddLinetoListView()
        {
            ListViewItem LVI = new ListViewItem();
            for (int index = 0; index < 5; index++)
            {
                ListViewItem.ListViewSubItem LVSI = new ListViewItem.ListViewSubItem();
                LVI.SubItems.Add(LVSI);
            }
            lvOrders.Items.Add(LVI);
        }
        private void ShowListView()
        {
            int index = 0;
            lvOrders.Items.Clear();
            foreach (Ammunition item in Catalog)
            {
                AddLinetoListView();
                lvOrders.Items[index].SubItems[0].Text = item.Name;
                var CategoryEnumerator = ((item.GetType()).GetCustomAttributes<CategoryNameAttribute>()).GetEnumerator();
                CategoryEnumerator.MoveNext();
                lvOrders.Items[index].SubItems[1].Text = CategoryEnumerator.Current.Value.ToString(); 
                lvOrders.Items[index].SubItems[2].Text = item.Price.ToString();
                lvOrders.Items[index].SubItems[3].Text = item.Count.ToString();
                lvOrders.Items[index].SubItems[4].Text = item.Country;
                index++;
            }
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (cbbType.SelectedIndex != -1)
            {
                object obj = null;
                CRUDForm form = null;
                Assembly asm = typeof(Ammunition).Assembly;
                List<Type> classes = Classes.GetClassesFromNamespace(ItemNamespace);
                foreach (Type type in classes)
                {
                    if (type.Name == (cbbType.Items[cbbType.SelectedIndex]).ToString())
                    {
                        form = new CRUDForm(Classes.TMode.create, type, obj, this, this);
                        break;
                    }
                }
                if (form == null)
                    return;
                form.ShowDialog();
            }
            ShowListView();
        }
        private void btnItemInfo_Click(object sender, EventArgs e)
        {
            int catalogIndex = 0;
            for (int index = 0; index <= Catalog.Count - 1; index++)
            {
                if (lvOrders.Items[catalogIndex].Selected)
                {
                    break;
                }
                catalogIndex++;
            }    
            if (catalogIndex == -1)
            {
                return;
            }
            object obj = Catalog[catalogIndex];                        
            CRUDForm form = new CRUDForm(Classes.TMode.read, obj.GetType(), obj, this, this);
            if (form == null)
                return;
            form.ShowDialog();

            ShowListView();
        }
        private void btnChangeItem_Click(object sender, EventArgs e)
        {
            int catalogIndex = 0;
            for (int index = 0; index <= Catalog.Count - 1; index++)
            {
                if (lvOrders.Items[catalogIndex].Selected)
                {
                    break;
                }
                catalogIndex++;
            }
            if (catalogIndex == -1)
            {
                return;
            }
            object obj = Catalog[catalogIndex];
            CRUDForm form = new CRUDForm(Classes.TMode.update, obj.GetType(), obj, this, this);
            if (form == null)
                return; form.ShowDialog();
            ShowListView();
        }
        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            int jindex = 0;
            for (int index = 0; index <= Catalog.Count - 1; index++)
            {

                if (lvOrders.Items[jindex].Selected)
                {
                    Catalog.RemoveAt(index);    
                    break;
                }
                jindex++;
            }
            ShowListView();
        }
        private void MainForm_Activated(object sender, EventArgs e)
        {
            ShowListView();
        }       
        private void lvOrders_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void mainForm_Load(object sender, EventArgs e)
        {

        }
        private void msMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void miOpenFile_Click(object sender, EventArgs e)
        {
            if (dlgOpenFile.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = dlgOpenFile.FileName;
            byte[] serialized = null;
            byte[] data = null;
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                serialized = new byte[(int)fs.Length];
                fs.Read(serialized, 0, serialized.Length);
            }
            switch (Plugin.FindPlugin(filename))
            {
                case -1:
                    MessageBox.Show("Соответствующий плагин отсутствует!!!");
                    return;
                case 1:
                    data = Plugin.ActivatePlugin(MainForm._curr_Plugin, serialized, false);
                    break;
                case 0:
                    data = serialized;
                    break;
            }
            Catalog = FileCreators[dlgOpenFile.FilterIndex - 1].OpenFile(data);
            ShowListView();
        }
        private void miSaveFileAs_Click(object sender, EventArgs e)
        {
            if (dlgSaveFile.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = dlgSaveFile.FileName;
            byte[] data = FileCreators[dlgSaveFile.FilterIndex - 1].SaveFile(Catalog);
            PluginForm pluginForm = new PluginForm(data, filename);
            pluginForm.ShowDialog();
        }
    }
}
/*<runtime><loadFromRemoteSources enabled="true|false"/></runtime>*/