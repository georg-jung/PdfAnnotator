namespace PdfAnnotator
{
    partial class AllDocumentsForm
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
            this.documentsGroupBox = new System.Windows.Forms.GroupBox();
            this.documentsListView = new System.Windows.Forms.ListView();
            this.documentHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastSeenHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contentMd5Header = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.annotationControlsPanel = new System.Windows.Forms.Panel();
            this.showAllAnnotationsForDocumentButton = new System.Windows.Forms.Button();
            this.deleteDocumentButton = new System.Windows.Forms.Button();
            this.documentsGroupBox.SuspendLayout();
            this.annotationControlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // documentsGroupBox
            // 
            this.documentsGroupBox.Controls.Add(this.documentsListView);
            this.documentsGroupBox.Controls.Add(this.annotationControlsPanel);
            this.documentsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.documentsGroupBox.Name = "documentsGroupBox";
            this.documentsGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.documentsGroupBox.Size = new System.Drawing.Size(800, 450);
            this.documentsGroupBox.TabIndex = 2;
            this.documentsGroupBox.TabStop = false;
            this.documentsGroupBox.Text = "Documents";
            // 
            // documentsListView
            // 
            this.documentsListView.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.documentsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.documentHeader,
            this.lastSeenHeader,
            this.contentMd5Header});
            this.documentsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentsListView.FullRowSelect = true;
            this.documentsListView.GridLines = true;
            this.documentsListView.Location = new System.Drawing.Point(6, 19);
            this.documentsListView.MultiSelect = false;
            this.documentsListView.Name = "documentsListView";
            this.documentsListView.Size = new System.Drawing.Size(788, 389);
            this.documentsListView.TabIndex = 2;
            this.documentsListView.UseCompatibleStateImageBehavior = false;
            this.documentsListView.View = System.Windows.Forms.View.Details;
            // 
            // documentHeader
            // 
            this.documentHeader.Text = "File";
            this.documentHeader.Width = 180;
            // 
            // lastSeenHeader
            // 
            this.lastSeenHeader.Text = "Last Seen";
            this.lastSeenHeader.Width = 120;
            // 
            // contentMd5Header
            // 
            this.contentMd5Header.Text = "MD5";
            this.contentMd5Header.Width = 450;
            // 
            // annotationControlsPanel
            // 
            this.annotationControlsPanel.Controls.Add(this.showAllAnnotationsForDocumentButton);
            this.annotationControlsPanel.Controls.Add(this.deleteDocumentButton);
            this.annotationControlsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.annotationControlsPanel.Location = new System.Drawing.Point(6, 408);
            this.annotationControlsPanel.Name = "annotationControlsPanel";
            this.annotationControlsPanel.Size = new System.Drawing.Size(788, 36);
            this.annotationControlsPanel.TabIndex = 3;
            // 
            // showAllAnnotationsForDocumentButton
            // 
            this.showAllAnnotationsForDocumentButton.Location = new System.Drawing.Point(165, 6);
            this.showAllAnnotationsForDocumentButton.Name = "showAllAnnotationsForDocumentButton";
            this.showAllAnnotationsForDocumentButton.Size = new System.Drawing.Size(150, 23);
            this.showAllAnnotationsForDocumentButton.TabIndex = 1;
            this.showAllAnnotationsForDocumentButton.Text = "Show Annotations...";
            this.showAllAnnotationsForDocumentButton.UseVisualStyleBackColor = true;
            this.showAllAnnotationsForDocumentButton.Click += new System.EventHandler(this.ShowAllAnnotationsForDocumentButton_Click);
            // 
            // deleteDocumentButton
            // 
            this.deleteDocumentButton.Location = new System.Drawing.Point(9, 6);
            this.deleteDocumentButton.Name = "deleteDocumentButton";
            this.deleteDocumentButton.Size = new System.Drawing.Size(150, 23);
            this.deleteDocumentButton.TabIndex = 0;
            this.deleteDocumentButton.Text = "Delete";
            this.deleteDocumentButton.UseVisualStyleBackColor = true;
            // 
            // AllDocumentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.documentsGroupBox);
            this.Name = "AllDocumentsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "All Documents";
            this.Load += new System.EventHandler(this.AllDocumentsForm_Load);
            this.documentsGroupBox.ResumeLayout(false);
            this.annotationControlsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox documentsGroupBox;
        private System.Windows.Forms.ListView documentsListView;
        private System.Windows.Forms.ColumnHeader contentMd5Header;
        private System.Windows.Forms.ColumnHeader documentHeader;
        private System.Windows.Forms.Panel annotationControlsPanel;
        private System.Windows.Forms.Button showAllAnnotationsForDocumentButton;
        private System.Windows.Forms.Button deleteDocumentButton;
        private System.Windows.Forms.ColumnHeader lastSeenHeader;
    }
}