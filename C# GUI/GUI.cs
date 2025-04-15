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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Library_Database_Front_End
{
    public partial class Form1 : Form
    {

        public string[] getColumnsName(string table)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Library";

            List<string> columnNames = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM [{table}] WHERE 1 = 0"; // Get structure only
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    DataTable schemaTable = reader.GetSchemaTable();
                    foreach (DataRow row in schemaTable.Rows)
                    {
                        columnNames.Add(row["ColumnName"].ToString()); // No casting issues here
                    }
                }
            }
            return columnNames.ToArray();
        }

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            TablesComboBox.Items.AddRange(new string[] { "Author", "Book", "Book_Copy", "Customer", "Employee", "Section" });
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
                if (ViewsComboBox.Text == "Average_Price_For_Author")
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
                string query = "EXEC Offboarding " + searchText + "; SELECT * FROM Employee;";
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

        private void InsertButton_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Library";
            string table = TablesComboBox.Text;
            string searchText = InsertionTextBox.Text;
            try
            {
                // Get the column names and convert them to a List<string>
                List<string> columnList = new List<string>(getColumnsName(table));
                // Remove the first element (auto-increment column) from the list
                if (table != "Book" && table != "Book_Copy")
                {
                    columnList.RemoveAt(0);  // Removing the auto-increment column
                }
                if (table == "Employee")
                {
                    columnList.RemoveAt(4);  // Removing the age column (derived attribute)
                }
                string columns = string.Join(", ", columnList);
                string[] values = searchText.Split(',');

                List<string> formattedValues = new List<string>();
                foreach (string value in values)
                {
                    string trimmedValue = value.Trim();
                    if (int.TryParse(trimmedValue, out _))  // Check if it's an integer
                    {
                        formattedValues.Add(trimmedValue);  // Add as is
                    }
                    else if (decimal.TryParse(trimmedValue, out _))  // Check if it's a decimal number
                    {
                        formattedValues.Add(trimmedValue);  // Add as is
                    }
                    else
                    {
                        // Otherwise, it's a string, so add it with single quotes
                        formattedValues.Add($"'{trimmedValue}'");
                    }
                }
                string valuesString = string.Join(", ", formattedValues);
                string query = $"INSERT INTO {table} ({columns}) VALUES ({valuesString}); SELECT * FROM {table};";

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

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Library";
                string table = TablesComboBox.Text;
                string searchText = DeleteTextBox.Text;
                // Get the column names and convert them to a List<string>
                List<string> columnList = new List<string>(getColumnsName(table));
                string column = columnList[0];
                string query = $"DELETE FROM {table} WHERE {column} = {searchText}; SELECT * FROM {table};";

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
    }
}