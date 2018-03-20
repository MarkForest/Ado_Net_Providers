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
    }
}
