using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Sweet
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        DataSet ds;
        DataView dv1, dv2;
        public static string nameXMLfile = "Sets.xml";
        public static int currentRow = 0;
        bool isChangesSaved = true;
        void LoadXmlFile()
        {
            ds = new DataSet();
            FileStream fsReadXml = new FileStream(nameXMLfile, FileMode.Open);
            ds.ReadXml(fsReadXml, XmlReadMode.InferTypedSchema);
            fsReadXml.Close();
            dv1 = new DataView(ds.Tables[0]);
            dataGridView1.DataSource = dv1;
            string s = dataGridView1.Rows[0].Cells[0].Value.ToString();
            dv2 = new DataView(ds.Tables[1]);
            dv2.RowFilter = "CodeSet = '" + s + "'";
            dataGridView2.DataSource = dv2;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Название дома";
            dataGridView1.Columns[2].HeaderText = "Описание дома";
            dataGridView1.Columns[3].HeaderText = "Цена ($/м^2)";
            dataGridView1.Columns[4].HeaderText = "Площадь (м^2)";
            dataGridView1.Columns[5].HeaderText = "Картинка";
            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[3].Width = 50;
            dataGridView1.Columns[2].Width = dataGridView1.Width - 345;
            dataGridView1.Columns[4].Width = 55;
            dataGridView1.Columns[5].Width = 60;
            //dataGridView1.Columns[5].Width = 70;
            // dataGridView1.Columns[6].Width = 70;
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            currentRow = 0;
            isChangesSaved = true;
        }
        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            nameXMLfile = "Sets.xml";
            LoadXmlFile();
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadXmlFile();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            string s;
            int i = e.RowIndex;
            currentRow = i;
            if (!dataGridView1.Rows[i].Cells[1].Value.Equals(DBNull.Value))
            {
                lblSetName.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
                //string sPict = dataGridView2.Rows[i].Cells[10].Value.ToString();
                //if (sPict != "Нет" && sPict != "")
                //{
                //    pctImage.Load("Images\\" + sPict);
                //}
                //else pctImage.Load("Images\\0.bmp");
                s = dataGridView1.Rows[i].Cells[0].Value.ToString();
                dv2 = new DataView(ds.Tables[1]);
                dv2.RowFilter = "CodeSet = '" + s + "'";
                dataGridView2.DataSource = dv2;
            }
        }

        private void подробнаяИнформацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetails fDetails = new frmDetails();
            fDetails.ShowDialog();

        }

        private void добавлениеЗаписиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dv1.AddNew();
        }
        
        private void удалеиеЗаписиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить набор " + (currentRow+1) + "?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                isChangesSaved = false;
                dv1.Delete(currentRow);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Файлы (*.xml)|*.xml";
            openFileDialog1.FileName = "Sets.xmk";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                nameXMLfile = openFileDialog1.FileName;
                LoadXmlFile();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!isChangesSaved)
            {
                DialogResult result;
                result = MessageBox.Show("Сохранить изменения?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (result==DialogResult.Yes)
                {
                    SaveXmlFile();
                    isChangesSaved = false;
                }
                else if (result == DialogResult.No)
                {
                    isChangesSaved = true;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void поНToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dv1.Sort = "SetName";
        }

        private void поВесуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dv1.Sort = "Square DESC";
        } 

        private void поЦенеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dv1.Sort = "Price DESC";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                dv1.Sort = comboBox1.Text;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "") dv1.Sort = comboBox1.Text + " DESC";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                dv1.Sort = comboBox1.Text;
                if (radioButton2.Checked) dv1.Sort += " DESC";
                groupBox3.Enabled = true;
            }
            else
            {
                groupBox3.Enabled = false;
                groupBox4.Enabled = false;
                comboBox2.Text = "";
            }
        }

        private void поискToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void поНазваниюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = Microsoft.VisualBasic.Interaction.InputBox("Введите название набора");
            if (s!="")
            {
                string strSort = dv1.Sort;
                dv1.Sort = "SetName";
                int index = dv1.Find(s);
                if(index==-1)
                {
                    dv1.Sort = strSort;
                    MessageBox.Show("Такого набора нет!");
                }
                else
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[index].Cells[1].Selected = true;
                }
            }
        }

        private void фильтроватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // groupBox1.Visible = false;
            groupBox5.Visible = true;
        }

        private void сортировкаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                dv1.RowFilter = "SetName Like '%" + textBox1.Text + "%'";
                if (checkBox2.Checked)
                {
                    if (txtFrom.Text != "") dv1.RowFilter += " And Price >= " + txtFrom.Text;
                    if (txtTo.Text != "") dv1.RowFilter += " And Price <= " + txtTo.Text;
                }
                if (checkBox3.Checked)
                    if (comboBox4.Text != "") dv1.RowFilter += " And Square = " + comboBox4.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dv1.RowFilter = "SetName Like '%" + textBox1.Text + "%'";
            if (checkBox2.Checked)
            {
                if (txtFrom.Text == "") txtFrom.Text = "0";
                if (txtTo.Text == "") txtTo.Text = "50";
                dv1.RowFilter += "And Price >=" + txtFrom.Text + " And Price <=" + txtTo.Text;
            }
            if (checkBox3.Checked)
                if (comboBox4.Text != "") dv1.RowFilter += " And Square = " + comboBox4.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dv1.RowFilter = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            textBox1.Text = "";
            txtFrom.Text = "";
            txtTo.Text = "";
            comboBox4.Text = "";
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveXmlFile();
        }

        private void сохранитьКакToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.Filter = "Файлы XML (*.xml)|*.xml";
            saveFileDialog1.FileName = "Teams.xml";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                nameXMLfile = saveFileDialog1.FileName;
                SaveXmlFile();
                isChangesSaved = true;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            currentRow = i;
            if (!dataGridView2.Rows[i].Cells[1].Value.Equals(DBNull.Value))
            {

                string sPict = dataGridView2.Rows[i].Cells[11].Value.ToString();
                if (sPict != "Нет" && sPict != "")
                {
                    pctImage.Load("Images\\" + sPict);
                }
                else pctImage.Load("Images\\0.bmp");


            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                if (radioButton1.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + ", " + comboBox2.Text;
                }
                else if (radioButton2.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + " DESC, " + comboBox2.Text;
                }
                if (radioButton4.Checked) dv1.Sort += " DESC";
                groupBox4.Enabled = true;
            }
            else
            {
                groupBox4.Enabled = false;
                comboBox3.Text = "";
            }
        }

        

        

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text != "")
            {
                if (radioButton1.Checked == true && radioButton3.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + ", " + comboBox2.Text + ", " + comboBox3.Text;
                }
                else if (radioButton2.Checked == true && radioButton3.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + " DESC, " + comboBox2.Text + ", " + comboBox3.Text;
                }
                else if (radioButton1.Checked == true && radioButton4.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + ", " + comboBox2.Text + " DESC, " + comboBox3.Text;
                }
                else
                {
                    dv1.Sort = comboBox1.Text + " DESC, " + comboBox2.Text + " DESC, " + comboBox3.Text;
                }
                if (radioButton6.Checked) dv1.Sort += " DESC";
            }
            else
            {
                comboBox3.Text = "";
            }
        }

        

        private void обработкаДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                if (radioButton1.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + ", " + comboBox2.Text;
                }
                else
                {
                    dv1.Sort = comboBox1.Text + " DESC, " + comboBox2.Text;
                }
            }
        }

       
        

        

        private void radioButton4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                if (radioButton1.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + ", " + comboBox2.Text + " DESC";
                }
                else
                {
                    dv1.Sort = comboBox1.Text + " DESC, " + comboBox2.Text + " DESC";
                }
            }
        }

        private void radioButton5_CheckedChanged_1(object sender, EventArgs e)
        {
            if (comboBox3.Text != "")
            {
                if (radioButton1.Checked == true && radioButton3.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + ", " + comboBox2.Text + ", " + comboBox3.Text;
                }
                else if (radioButton2.Checked == true && radioButton3.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + " DESC, " + comboBox2.Text + ", " + comboBox3.Text;
                }
                else if (radioButton1.Checked == true && radioButton4.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + ", " + comboBox2.Text + " DESC, " + comboBox3.Text;
                }
                else
                {
                    dv1.Sort = comboBox1.Text + " DESC, " + comboBox2.Text + " DESC, " + comboBox3.Text;
                }
            }
        }

        private void radioButton6_CheckedChanged_1(object sender, EventArgs e)
        {
            if (comboBox3.Text != "")
            {
                if (radioButton1.Checked == true && radioButton3.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + ", " + comboBox2.Text + ", " + comboBox3.Text + " DESC";
                }
                else if (radioButton2.Checked == true && radioButton3.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + " DESC, " + comboBox2.Text + ", " + comboBox3.Text + " DESC";
                }
                else if (radioButton1.Checked == true && radioButton4.Checked == true)
                {
                    dv1.Sort = comboBox1.Text + ", " + comboBox2.Text + " DESC, " + comboBox3.Text + " DESC";
                }
                else
                {
                    dv1.Sort = comboBox1.Text + " DESC, " + comboBox2.Text + " DESC, " + comboBox3.Text + " DESC";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dv1.RowFilter = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            textBox1.Text = "";
            txtFrom.Text = "";
            txtTo.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
        }

        private void сортироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox5.Visible = false;
        }

        void SaveXmlFile()
        {
            FileStream fsWriteXml = new FileStream(nameXMLfile, FileMode.Create);
            ds.WriteXml(fsWriteXml);
            fsWriteXml.Close();
        }

    }
}