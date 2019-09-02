namespace FirstCardPlus.Forms
{
    partial class f_Settings
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
            this.b_sm_load = new System.Windows.Forms.Button();
            this.b_sm_edit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // b_sm_load
            // 
            this.b_sm_load.Location = new System.Drawing.Point(116, 19);
            this.b_sm_load.Name = "b_sm_load";
            this.b_sm_load.Size = new System.Drawing.Size(156, 23);
            this.b_sm_load.TabIndex = 0;
            this.b_sm_load.Text = "Парс классов";
            this.b_sm_load.UseVisualStyleBackColor = true;
            this.b_sm_load.Click += new System.EventHandler(this.button1_Click);
            // 
            // b_sm_edit
            // 
            this.b_sm_edit.Location = new System.Drawing.Point(116, 48);
            this.b_sm_edit.Name = "b_sm_edit";
            this.b_sm_edit.Size = new System.Drawing.Size(156, 23);
            this.b_sm_edit.TabIndex = 1;
            this.b_sm_edit.Text = "Настройка смен";
            this.b_sm_edit.UseVisualStyleBackColor = true;
            this.b_sm_edit.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.b_sm_load);
            this.groupBox1.Controls.Add(this.b_sm_edit);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Смены";
            // 
            // f_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 319);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "f_Settings";
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.f_Settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button b_sm_load;
        private System.Windows.Forms.Button b_sm_edit;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}