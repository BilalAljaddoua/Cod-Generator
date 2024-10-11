namespace My_Cod_Generator
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
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsernamr = new System.Windows.Forms.TextBox();
            this.btnGenertCodBussniss = new System.Windows.Forms.Button();
            this.cbAllTables = new System.Windows.Forms.ComboBox();
            this.cbDatabase = new System.Windows.Forms.ComboBox();
            this.txtAccess = new System.Windows.Forms.TextBox();
            this.btnGeneratFileAccess = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGeneratCodBussniss = new System.Windows.Forms.Button();
            this.btnGeneratFileBussniss = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBussness = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button10 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.txtBussnissPath = new System.Windows.Forms.TextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.cbAutoGenerat = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(169, 97);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(272, 34);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "123456";
            // 
            // txtUsernamr
            // 
            this.txtUsernamr.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsernamr.Location = new System.Drawing.Point(169, 31);
            this.txtUsernamr.Name = "txtUsernamr";
            this.txtUsernamr.Size = new System.Drawing.Size(272, 34);
            this.txtUsernamr.TabIndex = 1;
            this.txtUsernamr.Text = "sa";
            // 
            // btnGenertCodBussniss
            // 
            this.btnGenertCodBussniss.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenertCodBussniss.Location = new System.Drawing.Point(11, 39);
            this.btnGenertCodBussniss.Name = "btnGenertCodBussniss";
            this.btnGenertCodBussniss.Size = new System.Drawing.Size(231, 34);
            this.btnGenertCodBussniss.TabIndex = 2;
            this.btnGenertCodBussniss.Text = "Generat Access Cod";
            this.btnGenertCodBussniss.UseVisualStyleBackColor = true;
            this.btnGenertCodBussniss.Click += new System.EventHandler(this.GeneratCod_Click);
            // 
            // cbAllTables
            // 
            this.cbAllTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAllTables.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAllTables.FormattingEnabled = true;
            this.cbAllTables.Location = new System.Drawing.Point(169, 227);
            this.cbAllTables.Name = "cbAllTables";
            this.cbAllTables.Size = new System.Drawing.Size(272, 34);
            this.cbAllTables.TabIndex = 3;
            // 
            // cbDatabase
            // 
            this.cbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDatabase.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDatabase.FormattingEnabled = true;
            this.cbDatabase.Location = new System.Drawing.Point(169, 163);
            this.cbDatabase.Name = "cbDatabase";
            this.cbDatabase.Size = new System.Drawing.Size(272, 34);
            this.cbDatabase.TabIndex = 0;
            this.cbDatabase.SelectedIndexChanged += new System.EventHandler(this.cbDatabase_SelectedIndexChanged);
            // 
            // txtAccess
            // 
            this.txtAccess.Location = new System.Drawing.Point(568, 86);
            this.txtAccess.Multiline = true;
            this.txtAccess.Name = "txtAccess";
            this.txtAccess.Size = new System.Drawing.Size(344, 534);
            this.txtAccess.TabIndex = 4;
            // 
            // btnGeneratFileAccess
            // 
            this.btnGeneratFileAccess.Enabled = false;
            this.btnGeneratFileAccess.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGeneratFileAccess.Location = new System.Drawing.Point(11, 114);
            this.btnGeneratFileAccess.Name = "btnGeneratFileAccess";
            this.btnGeneratFileAccess.Size = new System.Drawing.Size(231, 36);
            this.btnGeneratFileAccess.TabIndex = 5;
            this.btnGeneratFileAccess.Text = "Generat File";
            this.btnGeneratFileAccess.UseVisualStyleBackColor = true;
            this.btnGeneratFileAccess.Click += new System.EventHandler(this.GeneratFile_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-2, 58);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1335, 722);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Linen;
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtUsernamr);
            this.tabPage1.Controls.Add(this.cbDatabase);
            this.tabPage1.Controls.Add(this.txtBussness);
            this.tabPage1.Controls.Add(this.txtAccess);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.cbAllTables);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1327, 693);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnGeneratFileAccess);
            this.panel1.Controls.Add(this.btnGeneratCodBussniss);
            this.panel1.Controls.Add(this.btnGenertCodBussniss);
            this.panel1.Controls.Add(this.btnGeneratFileBussniss);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(36, 321);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 184);
            this.panel1.TabIndex = 8;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnGeneratCodBussniss
            // 
            this.btnGeneratCodBussniss.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGeneratCodBussniss.Location = new System.Drawing.Point(273, 43);
            this.btnGeneratCodBussniss.Name = "btnGeneratCodBussniss";
            this.btnGeneratCodBussniss.Size = new System.Drawing.Size(232, 34);
            this.btnGeneratCodBussniss.TabIndex = 7;
            this.btnGeneratCodBussniss.Text = "Generat Bussniss Cod";
            this.btnGeneratCodBussniss.UseVisualStyleBackColor = true;
            this.btnGeneratCodBussniss.Click += new System.EventHandler(this.button6_Click_1);
            // 
            // btnGeneratFileBussniss
            // 
            this.btnGeneratFileBussniss.Enabled = false;
            this.btnGeneratFileBussniss.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGeneratFileBussniss.Location = new System.Drawing.Point(273, 118);
            this.btnGeneratFileBussniss.Name = "btnGeneratFileBussniss";
            this.btnGeneratFileBussniss.Size = new System.Drawing.Size(231, 36);
            this.btnGeneratFileBussniss.TabIndex = 5;
            this.btnGeneratFileBussniss.Text = "Generat File";
            this.btnGeneratFileBussniss.UseVisualStyleBackColor = true;
            this.btnGeneratFileBussniss.Click += new System.EventHandler(this.button8_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightGray;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1002, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 34);
            this.label2.TabIndex = 6;
            this.label2.Text = "Data Bussniss";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightGray;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(679, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 34);
            this.label1.TabIndex = 6;
            this.label1.Text = "Data Access ";
            // 
            // txtBussness
            // 
            this.txtBussness.Location = new System.Drawing.Point(924, 86);
            this.txtBussness.Multiline = true;
            this.txtBussness.Name = "txtBussness";
            this.txtBussness.Size = new System.Drawing.Size(344, 534);
            this.txtBussness.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Black;
            this.tabPage2.Controls.Add(this.button10);
            this.tabPage2.Controls.Add(this.button7);
            this.tabPage2.Controls.Add(this.txtBussnissPath);
            this.tabPage2.Controls.Add(this.txtPath);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.button9);
            this.tabPage2.Controls.Add(this.button5);
            this.tabPage2.Controls.Add(this.cbAutoGenerat);
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1327, 693);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(882, 172);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(272, 34);
            this.button10.TabIndex = 13;
            this.button10.Text = "Select Folder For Bussniss";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.Location = new System.Drawing.Point(882, 113);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(272, 34);
            this.button7.TabIndex = 13;
            this.button7.Text = "Select Folder For Access";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // txtBussnissPath
            // 
            this.txtBussnissPath.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBussnissPath.Location = new System.Drawing.Point(118, 172);
            this.txtBussnissPath.Name = "txtBussnissPath";
            this.txtBussnissPath.Size = new System.Drawing.Size(660, 34);
            this.txtBussnissPath.TabIndex = 12;
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPath.Location = new System.Drawing.Point(118, 113);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(660, 34);
            this.txtPath.TabIndex = 12;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(118, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(272, 34);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "sa";
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Location = new System.Drawing.Point(785, 303);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(369, 55);
            this.button9.TabIndex = 11;
            this.button9.Text = "Generat Data Bussness Files";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(188, 303);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(369, 55);
            this.button5.TabIndex = 11;
            this.button5.Text = "Generat DataAccess  Files";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // cbAutoGenerat
            // 
            this.cbAutoGenerat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAutoGenerat.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAutoGenerat.FormattingEnabled = true;
            this.cbAutoGenerat.Location = new System.Drawing.Point(882, 25);
            this.cbAutoGenerat.Name = "cbAutoGenerat";
            this.cbAutoGenerat.Size = new System.Drawing.Size(272, 34);
            this.cbAutoGenerat.TabIndex = 6;
            this.cbAutoGenerat.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(506, 25);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(272, 34);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "123456";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(294, 18);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(265, 34);
            this.button3.TabIndex = 7;
            this.button3.Text = "Generat Cod Manual";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(802, 18);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(265, 34);
            this.button4.TabIndex = 7;
            this.button4.Text = "Generat Cod Automaticly";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Linen;
            this.label3.Location = new System.Drawing.Point(-8, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1334, 36);
            this.label3.TabIndex = 8;
            this.label3.Text = " ";
            // 
            // lbl1
            // 
            this.lbl1.BackColor = System.Drawing.Color.Cyan;
            this.lbl1.Location = new System.Drawing.Point(294, 12);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(265, 65);
            this.lbl1.TabIndex = 8;
            this.lbl1.Text = " ";
            // 
            // lbl2
            // 
            this.lbl2.BackColor = System.Drawing.Color.Black;
            this.lbl2.Location = new System.Drawing.Point(802, 12);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(265, 65);
            this.lbl2.TabIndex = 8;
            this.lbl2.Text = " ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(1333, 773);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.lbl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsernamr;
        private System.Windows.Forms.Button btnGenertCodBussniss;
        private System.Windows.Forms.ComboBox cbAllTables;
        private System.Windows.Forms.ComboBox cbDatabase;
        private System.Windows.Forms.TextBox txtAccess;
        private System.Windows.Forms.Button btnGeneratFileAccess;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox cbAutoGenerat;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBussness;
        private System.Windows.Forms.Button btnGeneratCodBussniss;
        private System.Windows.Forms.Button btnGeneratFileBussniss;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.TextBox txtBussnissPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Panel panel1;
    }
}

