using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
namespace SimpleCRUDForBeginner
{
    public partial class FormStudent : Form
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        SQLiteDataAdapter adapter;
        DataSet dataSet = new DataSet();
        DataTable dataTable = new DataTable();
        String connectString;

        int id;
        bool isDoubleClick = false;
        public FormStudent()
        {
            InitializeComponent();
            connectString = @"Data source=D:\Azri's Project\.NET Core\database\net_db.db;version=3";
            RetrieveData();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            RetrieveData();
        }

        private void RetrieveData()
        {
            try
            {
                conn = new SQLiteConnection(connectString);
                conn.Open();
                String query = "SELECT * FROM Student";
                adapter = new SQLiteDataAdapter(query, conn);
                dataSet.Reset();
                adapter.Fill(dataSet);
                dataTable = dataSet.Tables[0];
                dataGridViewStudent.DataSource = dataTable;
                conn.Close();
                dataGridViewStudent.Columns[0].HeaderText = "id";
                dataGridViewStudent.Columns[1].HeaderText = "First Name";
                dataGridViewStudent.Columns[2].HeaderText = "Last Name";
                dataGridViewStudent.Columns[3].HeaderText = "Email";
                dataGridViewStudent.Columns[4].HeaderText = "Password";
                dataGridViewStudent.Columns[5].HeaderText = "Gender";
                dataGridViewStudent.Columns[0].Visible = false;
                dataGridViewStudent.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridViewStudent.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridViewStudent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearData()
        {
            id = 0;
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            emailTextBox.Text = "";
            passwordTextBox.Text = "";
            genderTextBox.Text = "";
            dataGridViewStudent.ClearSelection();
            dataGridViewStudent.CurrentCell = null;
            isDoubleClick = false;
        }


        private void GetIdStudent()
        {
            //id = Convert.ToInt32(dataGridViewStudent.SelectedRows[0].Cells[0].Value);
         
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (firstNameTextBox.Text != "" || lastNameTextBox.Text != "" || emailTextBox.Text != ""
                || passwordTextBox.Text != "" || genderTextBox.Text != "")
            {

                try
                {
                    conn = new SQLiteConnection(connectString);
                    cmd = new SQLiteCommand();
                    cmd.CommandText = @"INSERT INTO Student(first_name,last_name,email,password,gender) 
                               VALUES(@fname,@lname,@email,@password,@gender)";
                    cmd.Connection = conn;
                    cmd.Parameters.Add(new SQLiteParameter("@fname", firstNameTextBox.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@lname", lastNameTextBox.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@email", emailTextBox.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@password", passwordTextBox.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@gender", genderTextBox.Text));

                    conn.Open();

                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        MessageBox.Show("Successfully Created!");
                        firstNameTextBox.Text = "";
                        lastNameTextBox.Text = "";
                        emailTextBox.Text = "";
                        passwordTextBox.Text = "";
                        genderTextBox.Text = "";
                        RetrieveData();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Required Failed");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            id = Convert.ToInt32(dataGridViewStudent.SelectedRows[0].Cells[0].Value);
            DialogResult dialogResult =
                MessageBox.Show("Do you want to delete this record ? ",
                "Warning",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

            cmd = new SQLiteCommand();
            if(dialogResult == DialogResult.Yes)
            {
                try
                {
                    conn = new SQLiteConnection(connectString);
                    conn.Open();
                    cmd.CommandText = @"DELETE FROM Student WHERE ID = '" + id + "'";
                    cmd.Connection = conn;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        id = 0;
                        dataGridViewStudent.ClearSelection();
                        dataGridViewStudent.CurrentCell = null;
                        RetrieveData();
                        dataGridViewStudent.ClearSelection();
                        dataGridViewStudent.CurrentCell = null;
                        MessageBox.Show("Successfully delted!");
                        isDoubleClick = false;

                    }
                    conn.Close();   
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if(dialogResult == DialogResult.No)
            {
                
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            //if (isDoubleClick)
            //{
                try
                {
                    id = Convert.ToInt32(dataGridViewStudent.SelectedRows[0].Cells[0].Value);
                    conn.Open();
                    cmd = new SQLiteCommand();
                    cmd.CommandText = @"UPDATE Student SET first_name=@fname,last_name=@lname,
                                    email=@email, password=@password,gender=@gender
                                    WHERE ID = '" + id + "'";
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@fname", firstNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@lname", lastNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@email", emailTextBox.Text);
                    cmd.Parameters.AddWithValue("@password", passwordTextBox.Text);
                    cmd.Parameters.AddWithValue("@gender", genderTextBox.Text);

                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        MessageBox.Show("Successfully Updated");
                        ClearData();
                        RetrieveData();
                        id = 0;
                        dataGridViewStudent.ClearSelection();
                        dataGridViewStudent.CurrentCell = null;
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
           // }
           
        }

        
    }
}
