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
    public partial class frmDetails : Form
    {
        public frmDetails()
        {
            InitializeComponent();
        }
        DataSet ds;
        DataView dv1;
        string nameXMLfile = "Sets.xml";
        int currentRow = 0;
        void FillTextBox()
        {
            int columnsCount = dv1.Table.Columns.Count;
            string[] sTextBox = new string[columnsCount];
            int i = currentRow;
            txtCurrent.Text = (currentRow+1).ToString();

            for (int j = 0; j < columnsCount; j++)
            {
                if (!dv1.Table.Rows[i].ItemArray[j].Equals(DBNull.Value))
                {
                    sTextBox[j] = dv1.Table.Rows[i].ItemArray[j].ToString();
                }
                else sTextBox[j] = "";
            }
            cmbCode.Text = sTextBox[0];
            txtName.Text = sTextBox[1];
            txtDescription.Text = sTextBox[2];
            txtPrice.Text = sTextBox[3];
            txtSquare.Text = sTextBox[4];
            txtPict.Text = sTextBox[5];
            if (sTextBox[5] != "Нет" && sTextBox[5] != "")
            {
                pctPhoto.Load("Houses\\" + sTextBox[5]);
            }
            else
            {
                pctPhoto.Load("Houses\\0.bmp");
            }


        }
        void FillComboBox()
        {
            cmbCode.Items.Clear();
            for (int i = 0; i < dv1.Count; i++) cmbCode.Items.Add(dv1.Table.Rows[i].ItemArray[0].ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentRow = 0;
            FillTextBox();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currentRow = dv1.Count - 1;
            FillTextBox();
        }

        private void frmDetails_LocationChanged(object sender, EventArgs e)
        {

        }

        private void frmDetails_Load(object sender, EventArgs e)
        {
            nameXMLfile = frmMain.nameXMLfile;
            currentRow = frmMain.currentRow;
            ds = new DataSet();
            FileStream fsReadXml = new FileStream(nameXMLfile, FileMode.Open);
            ds.ReadXml(fsReadXml, XmlReadMode.InferTypedSchema);
            fsReadXml.Close();
            dv1 = new DataView(ds.Tables[0]);
            dataGridView1.DataSource = dv1;
            FillComboBox();
            FillTextBox();
        }

        private void cmbCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRow = cmbCode.SelectedIndex;
            FillTextBox();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentRow<dv1.Count-1)
            {
                currentRow++;
                FillTextBox();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currentRow >0)
            {
                currentRow--;
                FillTextBox();
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
