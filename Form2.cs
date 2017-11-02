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

namespace sqlRemoteDraft
{
    public partial class Form2 : Form
    {
        private SqlConnection sqlConnection = null;
        public Form2(SqlConnection connection)
        {
            InitializeComponent();
            sqlConnection = connection;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlCommand insertStudenCommand = new SqlCommand("INSERT INTO [Students] (Name, Surname, Birthday) VALUES (@Name, @Surname, @Birthday)", sqlConnection);
            insertStudenCommand.Parameters.AddWithValue("Name", textBox1.Text);
            insertStudenCommand.Parameters.AddWithValue("Surname", textBox2.Text);
            insertStudenCommand.Parameters.AddWithValue("Birthday", Convert.ToDateTime(textBox3.Text));

            try
            {
                await insertStudenCommand.ExecuteNonQueryAsync();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
