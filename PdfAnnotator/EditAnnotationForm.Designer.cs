namespace PdfAnnotator
{
    partial class EditAnnotationForm
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
            this.subjectWordTextBox = new System.Windows.Forms.TextBox();
            this.contentLabel = new System.Windows.Forms.Label();
            this.contentTextBox = new System.Windows.Forms.TextBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.templatesGroupBox = new System.Windows.Forms.GroupBox();
            this.templatesInfoLabel = new System.Windows.Forms.Label();
            this.templatesListView = new System.Windows.Forms.ListView();
            this.templateDescriptionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.templatesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subject word:";
            // 
            // subjectWordTextBox
            // 
            this.subjectWordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subjectWordTextBox.Location = new System.Drawing.Point(106, 26);
            this.subjectWordTextBox.Name = "subjectWordTextBox";
            this.subjectWordTextBox.ReadOnly = true;
            this.subjectWordTextBox.Size = new System.Drawing.Size(346, 20);
            this.subjectWordTextBox.TabIndex = 3;
            // 
            // contentLabel
            // 
            this.contentLabel.AutoSize = true;
            this.contentLabel.Location = new System.Drawing.Point(12, 55);
            this.contentLabel.Name = "contentLabel";
            this.contentLabel.Size = new System.Drawing.Size(47, 13);
            this.contentLabel.TabIndex = 2;
            this.contentLabel.Text = "Content:";
            // 
            // contentTextBox
            // 
            this.contentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contentTextBox.Location = new System.Drawing.Point(106, 52);
            this.contentTextBox.Multiline = true;
            this.contentTextBox.Name = "contentTextBox";
            this.contentTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.contentTextBox.Size = new System.Drawing.Size(346, 173);
            this.contentTextBox.TabIndex = 0;
            // 
            // applyButton
            // 
            this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applyButton.Location = new System.Drawing.Point(377, 383);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 1;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(12, 383);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // templatesGroupBox
            // 
            this.templatesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.templatesGroupBox.Controls.Add(this.templatesInfoLabel);
            this.templatesGroupBox.Controls.Add(this.templatesListView);
            this.templatesGroupBox.Location = new System.Drawing.Point(15, 231);
            this.templatesGroupBox.Name = "templatesGroupBox";
            this.templatesGroupBox.Size = new System.Drawing.Size(437, 146);
            this.templatesGroupBox.TabIndex = 4;
            this.templatesGroupBox.TabStop = false;
            this.templatesGroupBox.Text = "Templates";
            // 
            // templatesInfoLabel
            // 
            this.templatesInfoLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.templatesInfoLabel.Location = new System.Drawing.Point(3, 127);
            this.templatesInfoLabel.Name = "templatesInfoLabel";
            this.templatesInfoLabel.Size = new System.Drawing.Size(428, 13);
            this.templatesInfoLabel.TabIndex = 5;
            this.templatesInfoLabel.Text = "Select a template by double-clicking it to take it\'s content.";
            // 
            // templatesListView
            // 
            this.templatesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.templateDescriptionColumn});
            this.templatesListView.FullRowSelect = true;
            this.templatesListView.GridLines = true;
            this.templatesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.templatesListView.Location = new System.Drawing.Point(6, 19);
            this.templatesListView.Name = "templatesListView";
            this.templatesListView.Size = new System.Drawing.Size(425, 105);
            this.templatesListView.TabIndex = 0;
            this.templatesListView.UseCompatibleStateImageBehavior = false;
            this.templatesListView.View = System.Windows.Forms.View.Details;
            this.templatesListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TemplatesListView_MouseDoubleClick);
            // 
            // templateDescriptionColumn
            // 
            this.templateDescriptionColumn.Text = "";
            this.templateDescriptionColumn.Width = 400;
            // 
            // EditAnnotationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(464, 418);
            this.Controls.Add(this.templatesGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.contentTextBox);
            this.Controls.Add(this.contentLabel);
            this.Controls.Add(this.subjectWordTextBox);
            this.Controls.Add(this.label1);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(480, 280);
            this.Name = "EditAnnotationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Annotation";
            this.Load += new System.EventHandler(this.EditAnnotationForm_Load);
            this.templatesGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox subjectWordTextBox;
        private System.Windows.Forms.Label contentLabel;
        private System.Windows.Forms.TextBox contentTextBox;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox templatesGroupBox;
        private System.Windows.Forms.ListView templatesListView;
        private System.Windows.Forms.Label templatesInfoLabel;
        private System.Windows.Forms.ColumnHeader templateDescriptionColumn;
    }
}