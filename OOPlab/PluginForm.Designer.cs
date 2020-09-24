namespace OOPlab
{
    partial class PluginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbbPlugin = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(211, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "Выберите плагин";
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(357, 159);
            this.btnSkip.Margin = new System.Windows.Forms.Padding(4);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(140, 62);
            this.btnSkip.TabIndex = 6;
            this.btnSkip.Text = "Пропустить";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(100, 159);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(128, 62);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbbPlugin
            // 
            this.cbbPlugin.FormattingEnabled = true;
            this.cbbPlugin.Location = new System.Drawing.Point(100, 105);
            this.cbbPlugin.Margin = new System.Windows.Forms.Padding(4);
            this.cbbPlugin.Name = "cbbPlugin";
            this.cbbPlugin.Size = new System.Drawing.Size(396, 24);
            this.cbbPlugin.TabIndex = 4;
            // 
            // PluginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbbPlugin);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PluginForm";
            this.Text = "PluginForm";
            this.Load += new System.EventHandler(this.PluginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cbbPlugin;
    }
}