namespace FirstCardPlus.Forms
{
    partial class f_Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f_Main));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mb_file = new System.Windows.Forms.ToolStripMenuItem();
            this.mb_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.mb_report = new System.Windows.Forms.ToolStripMenuItem();
            this.mb_settings = new System.Windows.Forms.ToolStripMenuItem();
            this.dp_start = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dp_end = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(656, 407);
            this.listBox1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mb_file,
            this.mb_report,
            this.mb_settings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(886, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mb_file
            // 
            this.mb_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mb_exit});
            this.mb_file.Name = "mb_file";
            this.mb_file.Size = new System.Drawing.Size(48, 20);
            this.mb_file.Text = "Файл";
            // 
            // mb_exit
            // 
            this.mb_exit.Name = "mb_exit";
            this.mb_exit.Size = new System.Drawing.Size(108, 22);
            this.mb_exit.Text = "Выход";
            this.mb_exit.Click += new System.EventHandler(this.b_exit_Click);
            // 
            // mb_report
            // 
            this.mb_report.Name = "mb_report";
            this.mb_report.Size = new System.Drawing.Size(54, 20);
            this.mb_report.Text = "Срезы";
            // 
            // mb_settings
            // 
            this.mb_settings.Name = "mb_settings";
            this.mb_settings.Size = new System.Drawing.Size(79, 20);
            this.mb_settings.Text = "Настройки";
            this.mb_settings.Click += new System.EventHandler(this.mb_settings_Click);
            // 
            // dp_start
            // 
            this.dp_start.Location = new System.Drawing.Point(730, 27);
            this.dp_start.Name = "dp_start";
            this.dp_start.Size = new System.Drawing.Size(144, 20);
            this.dp_start.TabIndex = 2;
            this.dp_start.ValueChanged += new System.EventHandler(this.dp_start_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(674, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Дата от:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(673, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Дата до:";
            // 
            // dp_end
            // 
            this.dp_end.Enabled = false;
            this.dp_end.Location = new System.Drawing.Point(730, 53);
            this.dp_end.Name = "dp_end";
            this.dp_end.Size = new System.Drawing.Size(144, 20);
            this.dp_end.TabIndex = 5;
            this.dp_end.ValueChanged += new System.EventHandler(this.dp_end_ValueChanged);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button1.Location = new System.Drawing.Point(677, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(197, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Обновить статистику";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.обновитьToolStripMenuItem_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(677, 108);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(107, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Выгрузка в Exel";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(676, 131);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(128, 17);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Только рабочие дни";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // f_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 451);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dp_end);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dp_start);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(902, 1000);
            this.MinimumSize = new System.Drawing.Size(902, 490);
            this.Name = "f_Main";
            this.Text = "First-Card Plus";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mb_file;
        private System.Windows.Forms.ToolStripMenuItem mb_report;
        private System.Windows.Forms.DateTimePicker dp_start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dp_end;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolStripMenuItem mb_settings;
        private System.Windows.Forms.ToolStripMenuItem mb_exit;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}

