using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using B_Layer;
 namespace My_Cod_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
DataTable dtTabelNames=new DataTable();
        private void Form1_Load(object sender, EventArgs e)
        {
              DataTable dataTable = clsDataBussinesLayer.GetAllDataBases();
            
            cbDatabase.DataSource = dataTable;
            cbDatabase.SelectedIndex = -1;
            cbDatabase.DisplayMember = "Name";
            cbAutoGenerat.DataSource = dataTable;
            cbDatabase.SelectedIndex = -1;
            cbAutoGenerat.DisplayMember = "Name";

        }

        private void GeneratCod_Click(object sender, EventArgs e)
        {
            txtAccess.Text = "";
            txtAccess.Text = clsDataBussinesLayer.GeneratCodForTable(cbAllTables.Text);
           btnGeneratFileAccess.Enabled = true;

        }

        private void GeneratFile_Click(object sender, EventArgs e)
        {

                string selectedFolder="";
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                      selectedFolder = folderBrowserDialog1.SelectedPath;
                 }
                else
                {
                return;
                 }
                string filePath = $@"{selectedFolder}\cls{cbAllTables.Text}Data.cs";

            // Check if the file already exists
            if (!File.Exists(filePath))
            {
                // Create a new file and write some content to it
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    writer.WriteLine($"{txtAccess.Text}");
 }

                MessageBox.Show("File created successfully.","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("File already exists.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAllTables.Enabled = true;
            string ConnectionString = $"Server=.;Database={cbDatabase.Text.ToString().Trim()};User Id={txtUsernamr.Text.ToString().Trim()};Password={txtPassword.Text.ToString().Trim()};";
            clsDataBussinesLayer.SetConnectionString(ConnectionString);
            DataTable dt = clsDataBussinesLayer.GetAllTables(cbDatabase.Text);
            if (dt.Rows.Count == 0&&cbDatabase.SelectedIndex!=-1&&cbAutoGenerat.SelectedIndex!=-1)
            {   MessageBox.Show("The Username or password is wrong , Please enter them correct", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            panel1.Enabled = false;
                return;
            }
            else { panel1.Enabled = true; }


                cbAllTables.Items.Clear();  
             foreach (DataRow row in dt.Rows)
            {
                cbAllTables.Items.Add(row["TABLE_NAME"].ToString());

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {         
            tabPage2.Hide();
            tabPage1.Show();
            lbl1.BackColor = Color.Cyan;
            lbl2.BackColor = Color.Black;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabPage1.Hide();

            tabPage2.Show();

            lbl2.BackColor = Color.Cyan;
            lbl1.BackColor = Color.Black;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtTabelNames= clsDataBussinesLayer.GetAllTables(cbAutoGenerat.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {

            string selectedFolder = "";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                selectedFolder = folderBrowserDialog1.SelectedPath;
            }
            else
            {
                return;
            }
            string filePath = $@"{selectedFolder}\";
            txtPath.Text = filePath;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            short count = 1;

                using (StreamWriter writer = File.CreateText($@"{txtPath.Text} clsDataSettings.cs"))
                {
                    writer.WriteLine($"{clsDataBussinesLayer.GeneratSettinges()}");
                }

            foreach(DataRow row in dtTabelNames.Rows)
            {
                
                string filePath = $@"{txtPath.Text} cls{row[0].ToString()}Data.cs";

                // Check if the file already exists

                if (!File.Exists(filePath))
                {        
                    count++;

                    // Create a new file and write some content to it
                    using (StreamWriter writer = File.CreateText(filePath))
                    {
                        writer.WriteLine($"{clsDataBussinesLayer.GeneratCodForTable(row[0].ToString())}");
                    }

                }
                else
                {
                    MessageBox.Show($"this File {row[0].ToString()}.cs already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
     
            }     
            if (count > 0) 
                {
                    MessageBox.Show($"{count} Files created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No File Created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
           txtBussness.Text= clsDataBussinesLayer.GeneratBussenss(cbAllTables.Text);
            btnGeneratFileBussniss.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string selectedFolder = "";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                selectedFolder = folderBrowserDialog1.SelectedPath;
            }
  
            string filePath = $@"{selectedFolder}\cls{cbAllTables.Text}Bussniss.cs";

            // Check if the file already exists
            if (!File.Exists(filePath))
            {
                // Create a new file and write some content to it
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    writer.WriteLine($"{txtBussness.Text}");
                }

                MessageBox.Show("File created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("File already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            short count = 0;
            foreach (DataRow row in dtTabelNames.Rows)
            {

                string filePath = $@"{txtBussnissPath.Text} cls{row[0].ToString()}Bussniss.cs";

                // Check if the file already exists
                if (!File.Exists(filePath))
                {
                    count++;

                    // Create a new file and write some content to it
                    using (StreamWriter writer = File.CreateText(filePath))
                    {
                        writer.WriteLine($"{clsDataBussinesLayer.GeneratBussenss(row[0].ToString())}");
                    }

                }
                else
                {
                    MessageBox.Show($"this File {row[0].ToString()}.cs already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            if (count > 0)
            {
                MessageBox.Show($"{count} Files created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No File Created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string selectedFolder = "";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                selectedFolder = folderBrowserDialog1.SelectedPath;
            }
            else
            {

            }
            string filePath = $@"{selectedFolder}\";
            txtBussnissPath.Text = filePath;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbAllTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGenertAccessCod.Enabled = true;
            btnGeneratBussnissCod.Enabled = true;
        }
    }
}