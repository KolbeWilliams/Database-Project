using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Database_Front_End
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            TablesComboBox.Items.AddRange(new string[] {"Author", "Book", "Book_Copy", "Customer", "Employee", "Section"});
            ViewsComboBox.Items.AddRange(new string[] { "Average_Price_For_Author", "Section_Authors" });
            tabControl1.Dock = DockStyle.Fill;
            DataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DataGrid2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ProgrammabilityDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            string searchText = QueryTextBox.Text;
            //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;IntegratedSecurity = True; Initial Catalog =< DatabaseName > ";
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Library";
            try
            {
                string query = searchText;
                DataTable resultsTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        //command.Parameters.AddWithValue("@SearchTerm", searchText);
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(resultsTable);
                        }
                    }
                }
                DataGrid.DataSource = resultsTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void TablesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Library";
            try
            {
                string query = "SELECT * FROM " + TablesComboBox.Text + ";";
                DataTable resultsTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        //command.Parameters.AddWithValue("@SearchTerm", searchText);
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(resultsTable);
                        }
                    }
                }
                DataGrid2.DataSource = resultsTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void ViewsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Library";
            try
            {
                string query;
                if(ViewsComboBox.Text == "Average_Price_For_Author")
                {
                    query = "SELECT * FROM " + ViewsComboBox.Text + ";";
                }
                else
                {
                    query = "SELECT * FROM " + ViewsComboBox.Text + " ORDER BY genre;";
                }
                DataTable resultsTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        //command.Parameters.AddWithValue("@SearchTerm", searchText);
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(resultsTable);
                        }
                    }
                }
                ProgrammabilityDataGrid.DataSource = resultsTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FunctionButton_Click(object sender, EventArgs e)
        {
            string searchText = FirstNameTextBox.Text;
            string searchText2 = LastNameTextBox.Text;
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Library";
            try
            {
                string query = "SELECT dbo.Author_Price(@FirstName, @LastName) AS Average_Price;";
                DataTable resultsTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@FirstName", searchText);
                        command.Parameters.AddWithValue("@LastName", searchText2);
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(resultsTable);
                        }
                    }
                }
                ProgrammabilityDataGrid.DataSource = resultsTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ProcedureButton_Click(object sender, EventArgs e)
        {
            string searchText = ProcedureTextBox.Text;
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Library";
            try
            {
                string query = "EXEC Offboarding " + searchText + ";";
                DataTable resultsTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        //command.Parameters.AddWithValue("@SearchTerm", searchText);
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(resultsTable);
                        }
                    }
                }
                ProgrammabilityDataGrid.DataSource = resultsTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}