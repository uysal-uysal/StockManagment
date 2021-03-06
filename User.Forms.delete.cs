﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace StokOtomasyonu
{
    public partial class deleteUser : Form
    {
        public deleteUser()
        {
            InitializeComponent();
        }


        DB database = new DB();


        private void Form1_Load(object sender, EventArgs e)
        {
            userTable.DataSource = database.ListDatas("users").Tables[0]; // --> list datas to datagridview
            database.Disconnect();
        }


        private void deleteButton_Click(object sender, EventArgs e)
        {
            //delete selected user
            string query = $"DELETE FROM users WHERE username = '{userTable.SelectedRows[0].Cells[1].Value.ToString()}'";

            try
            {
                database.ExecuteQuery(query);
                userTable.DataSource = database.ListDatas("users").Tables[0];
            }
            catch (Exception err)
            {
                MessageBox.Show("err" + MessageBox.Show(err.Message) + MessageBoxButtons.OK + MessageBoxIcon.Error);
            }
            finally
            {
                database.Disconnect();
            }
        }


        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
