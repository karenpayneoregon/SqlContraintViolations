namespace WindowsFormsApp1
{
    partial class Form1
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
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.cmdSetupForFailure = new System.Windows.Forms.Button();
            this.cmdUpdateCurrent = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).BeginInit();
            this.Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataGridView1
            // 
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView1.Location = new System.Drawing.Point(0, 0);
            this.DataGridView1.Name = "DataGridView1";
            this.DataGridView1.Size = new System.Drawing.Size(411, 334);
            this.DataGridView1.TabIndex = 3;
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.cmdSetupForFailure);
            this.Panel1.Controls.Add(this.cmdUpdateCurrent);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel1.Location = new System.Drawing.Point(0, 334);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(411, 49);
            this.Panel1.TabIndex = 2;
            // 
            // cmdSetupForFailure
            // 
            this.cmdSetupForFailure.Location = new System.Drawing.Point(143, 11);
            this.cmdSetupForFailure.Name = "cmdSetupForFailure";
            this.cmdSetupForFailure.Size = new System.Drawing.Size(123, 23);
            this.cmdSetupForFailure.TabIndex = 1;
            this.cmdSetupForFailure.Text = "Set for violation";
            this.cmdSetupForFailure.UseVisualStyleBackColor = true;
            this.cmdSetupForFailure.Click += new System.EventHandler(this.cmdSetupForFailure_Click);
            // 
            // cmdUpdateCurrent
            // 
            this.cmdUpdateCurrent.Location = new System.Drawing.Point(13, 11);
            this.cmdUpdateCurrent.Name = "cmdUpdateCurrent";
            this.cmdUpdateCurrent.Size = new System.Drawing.Size(124, 23);
            this.cmdUpdateCurrent.TabIndex = 0;
            this.cmdUpdateCurrent.Text = "Update current";
            this.cmdUpdateCurrent.UseVisualStyleBackColor = true;
            this.cmdUpdateCurrent.Click += new System.EventHandler(this.cmdUpdateCurrent_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 383);
            this.Controls.Add(this.DataGridView1);
            this.Controls.Add(this.Panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code sample";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).EndInit();
            this.Panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.DataGridView DataGridView1;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Button cmdSetupForFailure;
        internal System.Windows.Forms.Button cmdUpdateCurrent;
    }
}

