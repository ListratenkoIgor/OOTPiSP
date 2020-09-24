namespace OOPlab
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbbType = new System.Windows.Forms.ComboBox();
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.btnChangeItem = new System.Windows.Forms.Button();
            this.btnItemInfo = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.lvOrders = new System.Windows.Forms.ListView();
            this.lblName = new System.Windows.Forms.Label();
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveFileAs = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.msMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbbType
            // 
            this.cbbType.FormattingEnabled = true;
            this.cbbType.Location = new System.Drawing.Point(12, 404);
            this.cbbType.Name = "cbbType";
            this.cbbType.Size = new System.Drawing.Size(164, 21);
            this.cbbType.TabIndex = 16;
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDeleteItem.Location = new System.Drawing.Point(669, 404);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(119, 40);
            this.btnDeleteItem.TabIndex = 15;
            this.btnDeleteItem.Text = "Удалить";
            this.btnDeleteItem.UseVisualStyleBackColor = true;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnChangeItem
            // 
            this.btnChangeItem.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnChangeItem.Location = new System.Drawing.Point(514, 404);
            this.btnChangeItem.Name = "btnChangeItem";
            this.btnChangeItem.Size = new System.Drawing.Size(119, 40);
            this.btnChangeItem.TabIndex = 14;
            this.btnChangeItem.Text = "Изменить";
            this.btnChangeItem.UseVisualStyleBackColor = true;
            this.btnChangeItem.Click += new System.EventHandler(this.btnChangeItem_Click);
            // 
            // btnItemInfo
            // 
            this.btnItemInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnItemInfo.Location = new System.Drawing.Point(350, 404);
            this.btnItemInfo.Name = "btnItemInfo";
            this.btnItemInfo.Size = new System.Drawing.Size(119, 40);
            this.btnItemInfo.TabIndex = 13;
            this.btnItemInfo.Text = "Свойства";
            this.btnItemInfo.UseVisualStyleBackColor = true;
            this.btnItemInfo.Click += new System.EventHandler(this.btnItemInfo_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddItem.Location = new System.Drawing.Point(193, 404);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(119, 40);
            this.btnAddItem.TabIndex = 12;
            this.btnAddItem.Text = "Добавить";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // lvOrders
            // 
            this.lvOrders.HideSelection = false;
            this.lvOrders.Location = new System.Drawing.Point(12, 84);
            this.lvOrders.Name = "lvOrders";
            this.lvOrders.Size = new System.Drawing.Size(776, 294);
            this.lvOrders.TabIndex = 11;
            this.lvOrders.UseCompatibleStateImageBehavior = false;
            this.lvOrders.SelectedIndexChanged += new System.EventHandler(this.lvOrders_SelectedIndexChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblName.Location = new System.Drawing.Point(344, 40);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(122, 31);
            this.lblName.TabIndex = 10;
            this.lblName.Text = "Магазин";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // msMenu
            // 
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile});
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Size = new System.Drawing.Size(828, 24);
            this.msMenu.TabIndex = 17;
            this.msMenu.Text = "menuStrip1";
            this.msMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.msMenu_ItemClicked);
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOpenFile,
            this.miSaveFileAs});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(48, 20);
            this.miFile.Text = "Файл";
            // 
            // miSaveFileAs
            // 
            this.miSaveFileAs.Name = "miSaveFileAs";
            this.miSaveFileAs.Size = new System.Drawing.Size(180, 22);
            this.miSaveFileAs.Text = "Сохранить как";
            this.miSaveFileAs.Click += new System.EventHandler(this.miSaveFileAs_Click);
            // 
            // miOpenFile
            // 
            this.miOpenFile.Name = "miOpenFile";
            this.miOpenFile.Size = new System.Drawing.Size(180, 22);
            this.miOpenFile.Text = "Открыть";
            this.miOpenFile.Click += new System.EventHandler(this.miOpenFile_Click);
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 494);
            this.Controls.Add(this.cbbType);
            this.Controls.Add(this.btnDeleteItem);
            this.Controls.Add(this.btnChangeItem);
            this.Controls.Add(this.btnItemInfo);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.lvOrders);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.msMenu);
            this.Name = "MainForm";
            this.Text = "mainForm";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbType;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnChangeItem;
        private System.Windows.Forms.Button btnItemInfo;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.ListView lvOrders;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miSaveFileAs;
        private System.Windows.Forms.ToolStripMenuItem miOpenFile;
//        private System.Windows.Forms.ToolStripMenuItem хуевПоглотатьToolStripMenuItem;       
  //      private System.Windows.Forms.ToolStripMenuItem володяКучеренкоГейToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.SaveFileDialog dlgSaveFile;
    }
}

