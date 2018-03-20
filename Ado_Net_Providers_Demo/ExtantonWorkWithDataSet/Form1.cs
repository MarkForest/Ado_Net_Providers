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

namespace ExtantonWorkWithDataSet
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=CR5-00\SQLEXPRESS;Initial Catalog=Library;Integrated Security=true;";
        SqlConnection sqlConnection = null;
        SqlDataAdapter sqlDataAdapter = null;
        DataSet dataSet = null;
        SqlCommandBuilder sqlCommandBuilder = null;
        public Form1()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(connectionString);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sqlDataAdapter.Update(dataSet, "table");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataSet = new DataSet();
            sqlDataAdapter = new SqlDataAdapter(textBox1.Text, sqlConnection);
            sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
            sqlDataAdapter.Fill(dataSet);

            DataViewManager dataViewManager = new DataViewManager(dataSet);
            dataViewManager.DataViewSettings[0].RowFilter = "id<10";
            dataViewManager.DataViewSettings[0].Sort = "id desc";
            DataView dataView = dataViewManager.CreateDataView(dataSet.Tables[0]);
            dataGridView1.DataSource = dataView;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(connectionString);

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.Transaction = sqlTransaction;

                sqlCommand.CommandText = @"create table tmp3( id int not null identity primary key, desc1 varchar(100), price int);";
                sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = @"insert into tmp3(desc1, price)values('some desc1 v1', 45)";
                sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = @"insert into tmp3(desc1, price)values('some desc2 v2', 455)";
                sqlCommand.ExecuteNonQuery();

                sqlTransaction.Commit();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                sqlTransaction.Rollback();
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
