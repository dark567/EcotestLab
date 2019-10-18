namespace JResultsAdd
{
    partial class JResultsAddEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JResultsAddEdit));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new MACButtonLib.MACButton();
            this.button1 = new MACButtonLib.MACButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Дата";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(73, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(109, 20);
            this.textBox1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.ButtonColorDefault = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(162)))), ((int)(((byte)(214)))));
            this.button2.ButtonColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(130)))), ((int)(((byte)(132)))));
            this.button2.ButtonColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(122)))), ((int)(((byte)(87)))));
            this.button2.ButtonColorNormal = System.Drawing.Color.Crimson;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(540, 349);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 36);
            this.button2.TabIndex = 0;
            this.button2.Text = "Отмена";
            this.button2.TextColorDefault = System.Drawing.Color.Black;
            this.button2.TextColorDisabled = System.Drawing.Color.Black;
            this.button2.TextColorHover = System.Drawing.Color.White;
            this.button2.TextColorNormal = System.Drawing.Color.Black;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.ButtonColorDefault = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(162)))), ((int)(((byte)(214)))));
            this.button1.ButtonColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(130)))), ((int)(((byte)(132)))));
            this.button1.ButtonColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(122)))), ((int)(((byte)(87)))));
            this.button1.ButtonColorNormal = System.Drawing.Color.Green;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(413, 349);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ок";
            this.button1.TextColorDefault = System.Drawing.Color.Black;
            this.button1.TextColorDisabled = System.Drawing.Color.Black;
            this.button1.TextColorHover = System.Drawing.Color.White;
            this.button1.TextColorNormal = System.Drawing.Color.Black;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(674, 398);
            this.panel1.TabIndex = 2;
            // 
            // JResultsAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 398);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(690, 437);
            this.MinimumSize = new System.Drawing.Size(690, 437);
            this.Name = "JResultsAddEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[Изменение] Редактирование параметров анализа";
            this.Load += new System.EventHandler(this.JResultsAddEdit_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private MACButtonLib.MACButton button2;
        private MACButtonLib.MACButton button1;
        private System.Windows.Forms.Panel panel1;
    }
}