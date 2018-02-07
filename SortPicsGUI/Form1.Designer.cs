namespace SortPicsGUI
{
    partial class SortPics
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
            this.findButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.moveButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_label = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // findButton
            // 
            this.findButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.findButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findButton.Location = new System.Drawing.Point(201, 370);
            this.findButton.Margin = new System.Windows.Forms.Padding(5);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(152, 34);
            this.findButton.TabIndex = 1;
            this.findButton.Text = "Find some pics!";
            this.findButton.UseVisualStyleBackColor = false;
            this.findButton.Click += new System.EventHandler(this.FindPics_Click);
            // 
            // listBox1
            // 
            this.listBox1.Enabled = false;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 20);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(715, 329);
            this.listBox1.TabIndex = 2;
            // 
            // moveButton
            // 
            this.moveButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.moveButton.Enabled = false;
            this.moveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moveButton.Location = new System.Drawing.Point(363, 370);
            this.moveButton.Margin = new System.Windows.Forms.Padding(5);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(152, 34);
            this.moveButton.TabIndex = 3;
            this.moveButton.Text = "Move some pics!";
            this.moveButton.UseVisualStyleBackColor = false;
            this.moveButton.Click += new System.EventHandler(this.moveButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.status_label});
            this.statusStrip1.Location = new System.Drawing.Point(0, 425);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(739, 22);
            this.statusStrip1.TabIndex = 4;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel1.Text = "Status: ";
            // 
            // status_label
            // 
            this.status_label.Name = "status_label";
            this.status_label.Size = new System.Drawing.Size(0, 17);
            // 
            // SortPics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 447);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.moveButton);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.findButton);
            this.Name = "SortPics";
            this.Text = "SortPics";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button findButton;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button moveButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel status_label;
    }
}

