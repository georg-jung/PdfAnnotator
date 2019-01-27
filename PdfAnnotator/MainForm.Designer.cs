namespace PdfAnnotator
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.openPdfMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordsView = new System.Windows.Forms.ListView();
            this.wordHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.countHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openPdfMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(942, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // openPdfMenuItem
            // 
            this.openPdfMenuItem.Name = "openPdfMenuItem";
            this.openPdfMenuItem.Size = new System.Drawing.Size(87, 20);
            this.openPdfMenuItem.Text = "PDF öffnen...";
            this.openPdfMenuItem.Click += new System.EventHandler(this.openPdfMenuItem_Click);
            // 
            // wordsView
            // 
            this.wordsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.wordHeader,
            this.countHeader});
            this.wordsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wordsView.FullRowSelect = true;
            this.wordsView.Location = new System.Drawing.Point(0, 24);
            this.wordsView.Name = "wordsView";
            this.wordsView.Size = new System.Drawing.Size(942, 602);
            this.wordsView.TabIndex = 1;
            this.wordsView.UseCompatibleStateImageBehavior = false;
            this.wordsView.View = System.Windows.Forms.View.Details;
            // 
            // wordHeader
            // 
            this.wordHeader.Text = "Wort";
            this.wordHeader.Width = 300;
            // 
            // countHeader
            // 
            this.countHeader.Text = "Anzahl";
            this.countHeader.Width = 100;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 626);
            this.Controls.Add(this.wordsView);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "PdfAnnotator - Georg Jung";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem openPdfMenuItem;
        private System.Windows.Forms.ListView wordsView;
        private System.Windows.Forms.ColumnHeader wordHeader;
        private System.Windows.Forms.ColumnHeader countHeader;
    }
}

