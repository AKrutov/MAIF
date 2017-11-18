namespace MAIF
{
    partial class CalcForm
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
            System.Windows.Forms.Button calculateBtn;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalcForm));
            this.mainControlPanel = new System.Windows.Forms.Panel();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.nextBtn = new System.Windows.Forms.Button();
            this.prevBtn = new System.Windows.Forms.Button();
            calculateBtn = new System.Windows.Forms.Button();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // calculateBtn
            // 
            calculateBtn.Location = new System.Drawing.Point(490, 8);
            calculateBtn.Name = "calculateBtn";
            calculateBtn.Size = new System.Drawing.Size(163, 23);
            calculateBtn.TabIndex = 1;
            calculateBtn.Text = "Запустить расчет";
            calculateBtn.UseVisualStyleBackColor = true;
            calculateBtn.Click += new System.EventHandler(this.calculateBtn_Click);
            // 
            // mainControlPanel
            // 
            this.mainControlPanel.AutoScroll = true;
            this.mainControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainControlPanel.Location = new System.Drawing.Point(0, 0);
            this.mainControlPanel.Name = "mainControlPanel";
            this.mainControlPanel.Size = new System.Drawing.Size(657, 629);
            this.mainControlPanel.TabIndex = 1;
            // 
            // buttonPanel
            // 
            this.buttonPanel.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.buttonPanel.Controls.Add(this.nextBtn);
            this.buttonPanel.Controls.Add(this.prevBtn);
            this.buttonPanel.Controls.Add(calculateBtn);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 592);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(657, 37);
            this.buttonPanel.TabIndex = 2;
            // 
            // nextBtn
            // 
            this.nextBtn.Location = new System.Drawing.Point(120, 8);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(112, 23);
            this.nextBtn.TabIndex = 3;
            this.nextBtn.Text = "Следующая";
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // prevBtn
            // 
            this.prevBtn.Enabled = false;
            this.prevBtn.Location = new System.Drawing.Point(5, 8);
            this.prevBtn.Name = "prevBtn";
            this.prevBtn.Size = new System.Drawing.Size(112, 23);
            this.prevBtn.TabIndex = 2;
            this.prevBtn.Text = "Предыдущая";
            this.prevBtn.UseVisualStyleBackColor = true;
            this.prevBtn.Click += new System.EventHandler(this.prevBtn_Click);
            // 
            // CalcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 629);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.mainControlPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CalcForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Заполнение данных";
            this.Load += new System.EventHandler(this.CalcForm_Load);
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainControlPanel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.Button prevBtn;
    }
}