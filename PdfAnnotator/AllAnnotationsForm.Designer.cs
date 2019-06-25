namespace PdfAnnotator
{
    partial class AllAnnotationsForm
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
            this.annotationsGroupBox = new System.Windows.Forms.GroupBox();
            this.annotationsListView = new System.Windows.Forms.ListView();
            this.annotationSubjectWordHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contentHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.documentHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inCurrentPdfHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.annotationControlsPanel = new System.Windows.Forms.Panel();
            this.takeAnnotationButton = new System.Windows.Forms.Button();
            this.copyAnnotationContentButton = new System.Windows.Forms.Button();
            this.annotationsGroupBox.SuspendLayout();
            this.annotationControlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // annotationsGroupBox
            // 
            this.annotationsGroupBox.Controls.Add(this.annotationsListView);
            this.annotationsGroupBox.Controls.Add(this.annotationControlsPanel);
            this.annotationsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.annotationsGroupBox.Name = "annotationsGroupBox";
            this.annotationsGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.annotationsGroupBox.Size = new System.Drawing.Size(800, 450);
            this.annotationsGroupBox.TabIndex = 1;
            this.annotationsGroupBox.TabStop = false;
            this.annotationsGroupBox.Text = "Annotations";
            // 
            // annotationsListView
            // 
            this.annotationsListView.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.annotationsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.annotationSubjectWordHeader,
            this.contentHeader,
            this.documentHeader,
            this.inCurrentPdfHeader});
            this.annotationsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationsListView.FullRowSelect = true;
            this.annotationsListView.GridLines = true;
            this.annotationsListView.Location = new System.Drawing.Point(6, 19);
            this.annotationsListView.MultiSelect = false;
            this.annotationsListView.Name = "annotationsListView";
            this.annotationsListView.Size = new System.Drawing.Size(788, 389);
            this.annotationsListView.TabIndex = 2;
            this.annotationsListView.UseCompatibleStateImageBehavior = false;
            this.annotationsListView.View = System.Windows.Forms.View.Details;
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
            // documentHeader
            // 
            this.documentHeader.Text = "PDF";
            this.documentHeader.Width = 120;
            // 
            // inCurrentPdfHeader
            // 
            this.inCurrentPdfHeader.Text = "Relevant";
            // 
            // annotationControlsPanel
            // 
            this.annotationControlsPanel.Controls.Add(this.copyAnnotationContentButton);
            this.annotationControlsPanel.Controls.Add(this.takeAnnotationButton);
            this.annotationControlsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.annotationControlsPanel.Location = new System.Drawing.Point(6, 408);
            this.annotationControlsPanel.Name = "annotationControlsPanel";
            this.annotationControlsPanel.Size = new System.Drawing.Size(788, 36);
            this.annotationControlsPanel.TabIndex = 3;
            // 
            // takeAnnotationButton
            // 
            this.takeAnnotationButton.Location = new System.Drawing.Point(9, 6);
            this.takeAnnotationButton.Name = "takeAnnotationButton";
            this.takeAnnotationButton.Size = new System.Drawing.Size(100, 23);
            this.takeAnnotationButton.TabIndex = 0;
            this.takeAnnotationButton.Text = "Take";
            this.takeAnnotationButton.UseVisualStyleBackColor = true;
            this.takeAnnotationButton.Click += new System.EventHandler(this.takeAnnotationButton_Click);
            // 
            // copyAnnotationContentButton
            // 
            this.copyAnnotationContentButton.Location = new System.Drawing.Point(115, 6);
            this.copyAnnotationContentButton.Name = "copyAnnotationContentButton";
            this.copyAnnotationContentButton.Size = new System.Drawing.Size(100, 23);
            this.copyAnnotationContentButton.TabIndex = 1;
            this.copyAnnotationContentButton.Text = "Copy Content";
            this.copyAnnotationContentButton.UseVisualStyleBackColor = true;
            this.copyAnnotationContentButton.Click += new System.EventHandler(this.CopyAnnotationContentButton_Click);
            // 
            // AllAnnotationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.annotationsGroupBox);
            this.Name = "AllAnnotationsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "All Annotations";
            this.Load += new System.EventHandler(this.AllAnnotationsForm_Load);
            this.annotationsGroupBox.ResumeLayout(false);
            this.annotationControlsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox annotationsGroupBox;
        private System.Windows.Forms.ListView annotationsListView;
        private System.Windows.Forms.ColumnHeader annotationSubjectWordHeader;
        private System.Windows.Forms.ColumnHeader contentHeader;
        private System.Windows.Forms.ColumnHeader documentHeader;
        private System.Windows.Forms.Panel annotationControlsPanel;
        private System.Windows.Forms.Button takeAnnotationButton;
        private System.Windows.Forms.ColumnHeader inCurrentPdfHeader;
        private System.Windows.Forms.Button copyAnnotationContentButton;
    }
}