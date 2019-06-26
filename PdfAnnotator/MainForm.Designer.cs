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
            this.openLruPdfMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allAnnotationsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createPdfMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordsView = new System.Windows.Forms.ListView();
            this.wordHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.countHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.candidatesGroupBox = new System.Windows.Forms.GroupBox();
            this.candidateControlsHeader = new System.Windows.Forms.Panel();
            this.createAnnotationButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.annotationsGroupBox = new System.Windows.Forms.GroupBox();
            this.annotationsListView = new System.Windows.Forms.ListView();
            this.annotationSubjectWordHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contentHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.annotationControlsPanel = new System.Windows.Forms.Panel();
            this.autoSaveInfoLabel = new System.Windows.Forms.Label();
            this.deleteAnnotationButton = new System.Windows.Forms.Button();
            this.editAnnotationButton = new System.Windows.Forms.Button();
            this.toolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDatabaseToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromExistingDatabaseToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDatabseToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.candidatesGroupBox.SuspendLayout();
            this.candidateControlsHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.annotationsGroupBox.SuspendLayout();
            this.annotationControlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openPdfMenuItem,
            this.openLruPdfMenuItem,
            this.allAnnotationsMenuItem,
            this.createPdfMenuItem,
            this.toolsMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.ShowItemToolTips = true;
            this.mainMenu.Size = new System.Drawing.Size(942, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // openPdfMenuItem
            // 
            this.openPdfMenuItem.Name = "openPdfMenuItem";
            this.openPdfMenuItem.Size = new System.Drawing.Size(81, 20);
            this.openPdfMenuItem.Text = "Open PDF...";
            this.openPdfMenuItem.Click += new System.EventHandler(this.openPdfMenuItem_Click);
            // 
            // openLruPdfMenuItem
            // 
            this.openLruPdfMenuItem.Name = "openLruPdfMenuItem";
            this.openLruPdfMenuItem.Size = new System.Drawing.Size(149, 20);
            this.openLruPdfMenuItem.Text = "Open Recently Used PDF";
            // 
            // allAnnotationsMenuItem
            // 
            this.allAnnotationsMenuItem.Name = "allAnnotationsMenuItem";
            this.allAnnotationsMenuItem.Size = new System.Drawing.Size(110, 20);
            this.allAnnotationsMenuItem.Text = "All Annotations...";
            this.allAnnotationsMenuItem.Click += new System.EventHandler(this.allAnnotationsMenuItem_Click);
            // 
            // createPdfMenuItem
            // 
            this.createPdfMenuItem.Name = "createPdfMenuItem";
            this.createPdfMenuItem.Size = new System.Drawing.Size(86, 20);
            this.createPdfMenuItem.Text = "Create PDF...";
            this.createPdfMenuItem.Click += new System.EventHandler(this.createPdfMenuItem_Click);
            // 
            // wordsView
            // 
            this.wordsView.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.wordsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.wordHeader,
            this.countHeader});
            this.wordsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wordsView.FullRowSelect = true;
            this.wordsView.GridLines = true;
            this.wordsView.Location = new System.Drawing.Point(6, 19);
            this.wordsView.MultiSelect = false;
            this.wordsView.Name = "wordsView";
            this.wordsView.Size = new System.Drawing.Size(912, 221);
            this.wordsView.TabIndex = 1;
            this.wordsView.UseCompatibleStateImageBehavior = false;
            this.wordsView.View = System.Windows.Forms.View.Details;
            this.wordsView.ItemActivate += new System.EventHandler(this.wordsView_ItemActivate);
            // 
            // wordHeader
            // 
            this.wordHeader.Text = "Word";
            this.wordHeader.Width = 300;
            // 
            // countHeader
            // 
            this.countHeader.Text = "#";
            this.countHeader.Width = 100;
            // 
            // candidatesGroupBox
            // 
            this.candidatesGroupBox.Controls.Add(this.wordsView);
            this.candidatesGroupBox.Controls.Add(this.candidateControlsHeader);
            this.candidatesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.candidatesGroupBox.Location = new System.Drawing.Point(9, 9);
            this.candidatesGroupBox.Name = "candidatesGroupBox";
            this.candidatesGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.candidatesGroupBox.Size = new System.Drawing.Size(924, 283);
            this.candidatesGroupBox.TabIndex = 2;
            this.candidatesGroupBox.TabStop = false;
            this.candidatesGroupBox.Text = "Candidates";
            // 
            // candidateControlsHeader
            // 
            this.candidateControlsHeader.Controls.Add(this.createAnnotationButton);
            this.candidateControlsHeader.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.candidateControlsHeader.Location = new System.Drawing.Point(6, 240);
            this.candidateControlsHeader.Name = "candidateControlsHeader";
            this.candidateControlsHeader.Size = new System.Drawing.Size(912, 37);
            this.candidateControlsHeader.TabIndex = 4;
            // 
            // createAnnotationButton
            // 
            this.createAnnotationButton.Location = new System.Drawing.Point(9, 6);
            this.createAnnotationButton.Name = "createAnnotationButton";
            this.createAnnotationButton.Size = new System.Drawing.Size(75, 23);
            this.createAnnotationButton.TabIndex = 0;
            this.createAnnotationButton.Text = "Annotate";
            this.createAnnotationButton.UseVisualStyleBackColor = true;
            this.createAnnotationButton.Click += new System.EventHandler(this.createAnnotationButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.candidatesGroupBox);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(9);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.annotationsGroupBox);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(9);
            this.splitContainer1.Size = new System.Drawing.Size(942, 602);
            this.splitContainer1.SplitterDistance = 301;
            this.splitContainer1.TabIndex = 3;
            // 
            // annotationsGroupBox
            // 
            this.annotationsGroupBox.Controls.Add(this.annotationsListView);
            this.annotationsGroupBox.Controls.Add(this.annotationControlsPanel);
            this.annotationsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationsGroupBox.Location = new System.Drawing.Point(9, 9);
            this.annotationsGroupBox.Name = "annotationsGroupBox";
            this.annotationsGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.annotationsGroupBox.Size = new System.Drawing.Size(924, 279);
            this.annotationsGroupBox.TabIndex = 0;
            this.annotationsGroupBox.TabStop = false;
            this.annotationsGroupBox.Text = "Annotations";
            // 
            // annotationsListView
            // 
            this.annotationsListView.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.annotationsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.annotationSubjectWordHeader,
            this.contentHeader});
            this.annotationsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationsListView.FullRowSelect = true;
            this.annotationsListView.GridLines = true;
            this.annotationsListView.Location = new System.Drawing.Point(6, 19);
            this.annotationsListView.MultiSelect = false;
            this.annotationsListView.Name = "annotationsListView";
            this.annotationsListView.Size = new System.Drawing.Size(912, 218);
            this.annotationsListView.TabIndex = 2;
            this.annotationsListView.UseCompatibleStateImageBehavior = false;
            this.annotationsListView.View = System.Windows.Forms.View.Details;
            this.annotationsListView.ItemActivate += new System.EventHandler(this.annotationsListView_ItemActivate);
            // 
            // annotationSubjectWordHeader
            // 
            this.annotationSubjectWordHeader.Text = "Word";
            this.annotationSubjectWordHeader.Width = 150;
            // 
            // contentHeader
            // 
            this.contentHeader.Text = "Content";
            this.contentHeader.Width = 450;
            // 
            // annotationControlsPanel
            // 
            this.annotationControlsPanel.Controls.Add(this.autoSaveInfoLabel);
            this.annotationControlsPanel.Controls.Add(this.deleteAnnotationButton);
            this.annotationControlsPanel.Controls.Add(this.editAnnotationButton);
            this.annotationControlsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.annotationControlsPanel.Location = new System.Drawing.Point(6, 237);
            this.annotationControlsPanel.Name = "annotationControlsPanel";
            this.annotationControlsPanel.Size = new System.Drawing.Size(912, 36);
            this.annotationControlsPanel.TabIndex = 3;
            // 
            // autoSaveInfoLabel
            // 
            this.autoSaveInfoLabel.AutoSize = true;
            this.autoSaveInfoLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.autoSaveInfoLabel.Location = new System.Drawing.Point(171, 11);
            this.autoSaveInfoLabel.Name = "autoSaveInfoLabel";
            this.autoSaveInfoLabel.Size = new System.Drawing.Size(554, 13);
            this.autoSaveInfoLabel.TabIndex = 2;
            this.autoSaveInfoLabel.Text = "Your annotations are automatically saved. If you open a document with the same co" +
    "ntent again they will be restored.";
            // 
            // deleteAnnotationButton
            // 
            this.deleteAnnotationButton.Location = new System.Drawing.Point(90, 6);
            this.deleteAnnotationButton.Name = "deleteAnnotationButton";
            this.deleteAnnotationButton.Size = new System.Drawing.Size(75, 23);
            this.deleteAnnotationButton.TabIndex = 1;
            this.deleteAnnotationButton.Text = "Delete";
            this.deleteAnnotationButton.UseVisualStyleBackColor = true;
            this.deleteAnnotationButton.Click += new System.EventHandler(this.deleteAnnotationButton_Click);
            // 
            // editAnnotationButton
            // 
            this.editAnnotationButton.Location = new System.Drawing.Point(9, 6);
            this.editAnnotationButton.Name = "editAnnotationButton";
            this.editAnnotationButton.Size = new System.Drawing.Size(75, 23);
            this.editAnnotationButton.TabIndex = 0;
            this.editAnnotationButton.Text = "Edit";
            this.editAnnotationButton.UseVisualStyleBackColor = true;
            this.editAnnotationButton.Click += new System.EventHandler(this.editAnnotationButton_Click);
            // 
            // toolsMenuItem
            // 
            this.toolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportDatabaseToolsMenuItem,
            this.importFromExistingDatabaseToolsMenuItem,
            this.restoreDatabseToolsMenuItem});
            this.toolsMenuItem.Name = "toolsMenuItem";
            this.toolsMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsMenuItem.Text = "Tools";
            // 
            // exportDatabaseToolsMenuItem
            // 
            this.exportDatabaseToolsMenuItem.Name = "exportDatabaseToolsMenuItem";
            this.exportDatabaseToolsMenuItem.Size = new System.Drawing.Size(233, 22);
            this.exportDatabaseToolsMenuItem.Text = "Export Database";
            this.exportDatabaseToolsMenuItem.Click += new System.EventHandler(this.ExportDatabaseToolsMenuItem_Click);
            // 
            // importFromExistingDatabaseToolsMenuItem
            // 
            this.importFromExistingDatabaseToolsMenuItem.Name = "importFromExistingDatabaseToolsMenuItem";
            this.importFromExistingDatabaseToolsMenuItem.Size = new System.Drawing.Size(233, 22);
            this.importFromExistingDatabaseToolsMenuItem.Text = "Import from existing Database";
            this.importFromExistingDatabaseToolsMenuItem.Click += new System.EventHandler(this.ImportFromExistingDatabaseToolsMenuItem_Click);
            // 
            // restoreDatabseToolsMenuItem
            // 
            this.restoreDatabseToolsMenuItem.Name = "restoreDatabseToolsMenuItem";
            this.restoreDatabseToolsMenuItem.Size = new System.Drawing.Size(233, 22);
            this.restoreDatabseToolsMenuItem.Text = "Restore Database";
            this.restoreDatabseToolsMenuItem.Click += new System.EventHandler(this.RestoreDatabseToolsMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 626);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PdfAnnotator {0} - Georg Jung";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.candidatesGroupBox.ResumeLayout(false);
            this.candidateControlsHeader.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.annotationsGroupBox.ResumeLayout(false);
            this.annotationControlsPanel.ResumeLayout(false);
            this.annotationControlsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem openPdfMenuItem;
        private System.Windows.Forms.ListView wordsView;
        private System.Windows.Forms.ColumnHeader wordHeader;
        private System.Windows.Forms.ColumnHeader countHeader;
        private System.Windows.Forms.GroupBox candidatesGroupBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem createPdfMenuItem;
        private System.Windows.Forms.Panel candidateControlsHeader;
        private System.Windows.Forms.Button createAnnotationButton;
        private System.Windows.Forms.GroupBox annotationsGroupBox;
        private System.Windows.Forms.ListView annotationsListView;
        private System.Windows.Forms.ColumnHeader annotationSubjectWordHeader;
        private System.Windows.Forms.ColumnHeader contentHeader;
        private System.Windows.Forms.Panel annotationControlsPanel;
        private System.Windows.Forms.Button editAnnotationButton;
        private System.Windows.Forms.Button deleteAnnotationButton;
        private System.Windows.Forms.Label autoSaveInfoLabel;
        private System.Windows.Forms.ToolStripMenuItem openLruPdfMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allAnnotationsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDatabaseToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFromExistingDatabaseToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreDatabseToolsMenuItem;
    }
}

