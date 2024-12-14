using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace pirates
{
    public partial class Form1 : Form
    {
        string ConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\kyle_\\Downloads\\dpPirates.accdb";
        OleDbCommand cmd;
        OleDbConnection conn;
        OleDbDataAdapter adapter;
        DataTable dt;

        public Form1()
        {
            InitializeComponent();
            InitializeData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initializecbo();
            cboPiratesSearch.SelectedIndex = -1;
            btnSave.Enabled = false;
            cboPiratesGroup.SelectedIndex = -1;
            DisableTextBox();

        }

        public void InitializeData()
        {
            string query = "SELECT id, piratename AS ALIAS, givenname AS NAME, age AS AGE, pirategroup AS [PIRATE GROUP], bounty AS [BOUNTY (BELLY)] FROM pirates";
            
            using (conn = new OleDbConnection(ConnStr))
            {
                conn.Open();
                
                using (adapter = new OleDbDataAdapter(query,conn))
                {
                    dt = new DataTable();
                    adapter.Fill(dt);
                    grdview.DataSource = dt;
                    conn.Close();

                }
                grdview.Columns["age"].Visible = false;
                grdview.Columns["id"].Visible = false;
            }
        }

        public void initializecbo()
        {
            string query = "select distinct pirategroup from pirates";

            using (conn = new OleDbConnection(ConnStr))
            {
                conn.Open();

                using (adapter = new OleDbDataAdapter(query, conn))
                {
                    dt = new DataTable();
                    adapter.Fill(dt); 
                    conn.Close();

                    cboPiratesSearch.DataSource = dt;
                    cboPiratesSearch.DisplayMember = "pirategroup";

                    cboPiratesGroup.DataSource = dt;
                    cboPiratesGroup.DisplayMember = "pirategroup";
                    //cboPiratesSearch.ValueMember = "pirategroup";
                    
                }
                
            }
        }

        private void EnableTextBox()
        {
            txtAge.Enabled = true;
            txtAlias.Enabled = true;
            txtBounty.Enabled = true;
            cboPiratesGroup.Enabled = true;
            txtBounty.Enabled = true;
        }

        public void DisableTextBox()
        {
            txtAlias.Enabled = false;
            txtAge.Enabled = false;
            txtBounty.Enabled = false;
            cboPiratesGroup.Enabled = false;
            txtBounty.Enabled = false;
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            EnableTextBox();
            btnNewRecord.Enabled = false;
            btnSave.Enabled = true;

        }

        private void grdview_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            txtAlias.Text = grdview.SelectedCells[1].Value.ToString();
            txtName.Text = grdview.SelectedCells[2].Value.ToString();
            txtAge.Text = grdview.SelectedCells[3].Value.ToString();
            cboPiratesGroup.Text = grdview.SelectedCells[4].ToString();
            txtBounty.Text = grdview.SelectedCells[5].Value.ToString();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = "SELECT id, piratename AS ALIAS, givenname AS NAME, age AS AGE, pirategroup AS [PIRATE GROUP], bounty AS [BOUNTY(BELLY)]" +
                           " FROM pirates WHERE piratename LIKE @alias OR givenname LIKE @name AND pirategroup LIKE @pirategroup";
      
            using(conn = new OleDbConnection(ConnStr))
            {
                conn.Open();

                using(cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@alies", "%" + txtSearch.Text);
                    cmd.Parameters.AddWithValue("@name", "%" + txtSearch.Text);
                    cmd.Parameters.AddWithValue("@pirategroup", cboPiratesSearch.Text);
                    cmd.ExecuteNonQuery();
                    

                    //if (res > 0)
                    //{
                    //    MessageBox.Show("Successfully Added", "Add Data");
                    //}
                    conn.Close();
                }
            }
        }
    }
}
