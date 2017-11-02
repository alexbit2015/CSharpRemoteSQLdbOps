using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;

namespace sqlRemoteDraft
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection;
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["StudentsCS"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();

            listView2.GridLines = true;
            listView2.FullRowSelect = true;
            listView2.View = View.Details;

            listView2.Columns.Add("id");
            listView2.Columns.Add("Name");
            listView2.Columns.Add("Surname");
            listView2.Columns.Add("Birthday");

            await LoadStudentsAsync();


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();

        }

        private async Task LoadStudentsAsync() //SELECT
        {
            SqlDataReader sqlReader = null;
            SqlCommand getStudentsCommand = new SqlCommand("SELECT * FROM [Students]", sqlConnection);
            try
            {
                sqlReader = await getStudentsCommand.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[]
                        {
                            Convert.ToString(sqlReader["id"]),
                            Convert.ToString(sqlReader["Name"]),
                            Convert.ToString(sqlReader["Surname"]),
                            Convert.ToString(sqlReader["Birthday"]),

                        });
                    listView2.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            };

        }


        private async void toolStripButton5_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            await LoadStudentsAsync();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form2 insert = new Form2(sqlConnection);
            insert.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                UPDATE update = new UPDATE(sqlConnection, Convert.ToInt32(listView2.SelectedItems[0].SubItems[0].Text));
                update.Show();
            } else
            {
                MessageBox.Show("No any string selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void toolStripButton3_Click(object sender, EventArgs e)
        {
            if(listView2.SelectedItems.Count > 0)
            { 
            DialogResult res = MessageBox.Show("Are you realy want to delete it?", "Deleting string", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            switch (res)
            {
                case DialogResult.OK:
                    SqlCommand deleteStudentCommand = new SqlCommand("DELETE FROM [Students] WHERE [Id]=@id", sqlConnection);

                    deleteStudentCommand.Parameters.AddWithValue("Id", Convert.ToInt32(listView2.SelectedItems[0].SubItems[0].Text));

                    try
                    {
                        await deleteStudentCommand.ExecuteNonQueryAsync();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                        listView2.Items.Clear();
                        await LoadStudentsAsync();
                        break;
                }
                
            } else
            {
                MessageBox.Show("No any string selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("WorkinWithRemoteDb\nAlexbit, 2017", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
