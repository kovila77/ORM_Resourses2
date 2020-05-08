namespace ORM_Resourses
{
    partial class fRConsume
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpRes = new System.Windows.Forms.TabPage();
            this.dgvResources = new System.Windows.Forms.DataGridView();
            this.tpResConsume = new System.Windows.Forms.TabPage();
            this.dgvRConsume = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьЗановоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отменитьИзмененияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl.SuspendLayout();
            this.tpRes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResources)).BeginInit();
            this.tpResConsume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRConsume)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpRes);
            this.tabControl.Controls.Add(this.tpResConsume);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(513, 243);
            this.tabControl.TabIndex = 0;
            // 
            // tpRes
            // 
            this.tpRes.Controls.Add(this.dgvResources);
            this.tpRes.Location = new System.Drawing.Point(4, 22);
            this.tpRes.Name = "tpRes";
            this.tpRes.Padding = new System.Windows.Forms.Padding(3);
            this.tpRes.Size = new System.Drawing.Size(505, 217);
            this.tpRes.TabIndex = 0;
            this.tpRes.Text = "Ресурсы";
            this.tpRes.UseVisualStyleBackColor = true;
            // 
            // dgvResources
            // 
            this.dgvResources.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResources.Location = new System.Drawing.Point(3, 3);
            this.dgvResources.Name = "dgvResources";
            this.dgvResources.Size = new System.Drawing.Size(499, 211);
            this.dgvResources.TabIndex = 0;
            this.dgvResources.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResources_CellEndEdit);
            this.dgvResources.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvResources_CellValidating);
            this.dgvResources.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvResources_UserDeletingRow);
            // 
            // tpResConsume
            // 
            this.tpResConsume.Controls.Add(this.dgvRConsume);
            this.tpResConsume.Location = new System.Drawing.Point(4, 22);
            this.tpResConsume.Name = "tpResConsume";
            this.tpResConsume.Padding = new System.Windows.Forms.Padding(3);
            this.tpResConsume.Size = new System.Drawing.Size(660, 217);
            this.tpResConsume.TabIndex = 1;
            this.tpResConsume.Text = "Потребление ресурсов";
            this.tpResConsume.UseVisualStyleBackColor = true;
            // 
            // dgvRConsume
            // 
            this.dgvRConsume.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRConsume.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRConsume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRConsume.Location = new System.Drawing.Point(3, 3);
            this.dgvRConsume.Name = "dgvRConsume";
            this.dgvRConsume.Size = new System.Drawing.Size(654, 211);
            this.dgvRConsume.TabIndex = 0;
            this.dgvRConsume.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRConsume_CellEndEdit);
            this.dgvRConsume.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvRConsume_CellValidating);
            this.dgvRConsume.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvRConsume_UserDeletingRow);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.правкаToolStripMenuItem,
            this.отменитьИзмененияToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(513, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.загрузитьЗановоToolStripMenuItem});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.правкаToolStripMenuItem.Text = "Правка";
            // 
            // загрузитьЗановоToolStripMenuItem
            // 
            this.загрузитьЗановоToolStripMenuItem.Name = "загрузитьЗановоToolStripMenuItem";
            this.загрузитьЗановоToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.загрузитьЗановоToolStripMenuItem.Text = "Загрузить заново";
            this.загрузитьЗановоToolStripMenuItem.Click += new System.EventHandler(this.ReloadData);
            // 
            // отменитьИзмененияToolStripMenuItem
            // 
            this.отменитьИзмененияToolStripMenuItem.Name = "отменитьИзмененияToolStripMenuItem";
            this.отменитьИзмененияToolStripMenuItem.Size = new System.Drawing.Size(207, 20);
            this.отменитьИзмененияToolStripMenuItem.Text = "Отменить редактирование ячейки";
            this.отменитьИзмененияToolStripMenuItem.Click += new System.EventHandler(this.CancelEdit);
            // 
            // fRConsume
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 267);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "fRConsume";
            this.Text = "Потребление ресурсов";
            this.tabControl.ResumeLayout(false);
            this.tpRes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResources)).EndInit();
            this.tpResConsume.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRConsume)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpRes;
        private System.Windows.Forms.TabPage tpResConsume;
        private System.Windows.Forms.DataGridView dgvResources;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.DataGridView dgvRConsume;
        private System.Windows.Forms.ToolStripMenuItem загрузитьЗановоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отменитьИзмененияToolStripMenuItem;
    }
}

