using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Ado_Net_Providers_Demo
{
    public partial class Form1 : Form
    {
        DbConnection dbConnection = null;
        DbProviderFactory dbProviderFactory = null;
        string providerName = "";
        public Form1()
        {
            InitializeComponent();
            btnRequest.Enabled = false;
            btnGetAllProvider.Click += BtnGetAllProvider_Click;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            btnRequest.Click += BtnRequest_Click;
            textBox2.TextChanged += TextBox2_TextChanged;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 5)
            {
                btnRequest.Enabled = true;
            }
            else
            {
                btnRequest.Enabled = false;
            }
        }

        private void BtnRequest_Click(object sender, EventArgs e)
        {
            dbConnection.ConnectionString = textBox1.Text;
            DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = dbConnection.CreateCommand();
            dbDataAdapter.SelectCommand.CommandText = textBox2.Text;
            DataTable dataTable = new DataTable();
            try
            {
               
                dbDataAdapter.Fill(dataTable);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbProviderFactory = DbProviderFactories.GetFactory(comboBox1.SelectedItem.ToString());
            dbConnection = dbProviderFactory.CreateConnection();
            providerName = GetConnectionStringProvider(comboBox1.SelectedItem.ToString());
            textBox1.Text = providerName;
        }

        private string GetConnectionStringProvider(string providerName)
        {
            string returnValue = "";
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            if(settings != null)
            {
                foreach (ConnectionStringSettings item in settings)
                {
                    if(item.ProviderName == providerName)
                    {
                        returnValue = item.ConnectionString;
                        break;
                    }
                    
                }
            }

            return returnValue;

        }



        private void BtnGetAllProvider_Click(object sender, EventArgs e)
        {
            DataTable dataTable = DbProviderFactories.GetFactoryClasses();
            dataGridView1.DataSource = dataTable;
            comboBox1.Items.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                comboBox1.Items.Add(row["InvariantName"]);
            }
        }
    }
}
