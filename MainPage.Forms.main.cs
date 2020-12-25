﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;

namespace StokOtomasyonu
{
    public partial class mainPage : Form
    {
        public mainPage()
        {
            InitializeComponent();
        }


        private void mainPage_Load(object sender, EventArgs e)
        {
            draw();

        }

        public void draw()
        {
            addStr.count();

            Button[] stockroomButtons = new Button[4];
            stockroomButtons[0] = stockroomBtn1;
            stockroomButtons[1] = stockroomBtn2;
            stockroomButtons[2] = stockroomBtn3;
            stockroomButtons[3] = stockroomBtn4;

            Button[] graphButtons = new Button[4];
            graphButtons[0] = graphButton1;
            graphButtons[1] = graphButton2;
            graphButtons[2] = graphButton3;
            graphButtons[3] = graphButton4;

            for (int i = 0; i < addStr.countStockroom; i++)
            {
                checkCurrentCapacity(i + 1);
                stockroomButtons[i].Visible = true;
                graphButtons[i].Visible = true;
                stockroomButtons[i].Text = setName(i + 1);
                graphButtons[i].Text = setName(i + 1) + " - " + currentCapacity.ToString(); ;

                graphButtons[i].Width = (int.Parse(currentCapacity.ToString()) * 3); ;
            }
        }


        DB database = new DB();
        loginPage loginpage = new loginPage();
        addStr addStr = new addStr();
        deletePrd deletePrd;
        deleteUser deleteuser;


        public int value;
        public static bool stt = false;//new user is admin? stt-false -- > common
        public static string store;
        public static string productType;


        private void stockroomBtn1_Click(object sender, EventArgs e)
        {
            store = "1";
            panel2.Visible = true;
            productTable.DataSource = null;
            timer.Enabled = true;
            deleteStockroom.Visible = true;
            updateCapacity();
        }


        private void stockroomBtn2_Click(object sender, EventArgs e)
        {
            store = "2";
            panel2.Visible = true;
            productTable.DataSource = null;
            timer.Enabled = true;
            deleteStockroom.Visible = true;
            updateCapacity();
        }


        private void stockroomBtn3_Click(object sender, EventArgs e)
        {
            store = "3";
            panel2.Visible = true;
            productTable.DataSource = null;
            timer.Enabled = true;
            deleteStockroom.Visible = true;
            updateCapacity();
        }


        private void stockroomBtn4_Click(object sender, EventArgs e)
        {
            store = "4";
            panel2.Visible = true;
            productTable.DataSource = null;
            timer.Enabled = true;
            deleteStockroom.Visible = true;
            updateCapacity();
        }


        private void addStockroom_Click(object sender, EventArgs e)
        {
            if (loginpage.IsAdmin())
            {
                addStr addStocroom = new addStr();
                addStocroom.Show();
            }
            else
            {
                MessageBox.Show("You cant add new Stockroom!");
            }
        }


        private void deleteStockroom_Click(object sender, EventArgs e)
        {
            //refresh combobox
            cmbDeleteStockroom.DataSource = null;
            cmbDeleteStockroom.Items.Clear();
            this.AutoScrollPosition = new Point(0, 0);


            if (addStr.countStockroom >= 1)
            {
                if (loginpage.IsAdmin())
                {
                    for (int i = 0; i < addStr.countStockroom; i++)
                    {
                        cmbDeleteStockroom.Items.Add(setName(i + 1).ToString());
                    }
                    cmbDeleteStockroom.SelectedIndex = 0;
                    pnlConfirmDelete.Visible = true;
                    label5.Text = " 'DELETE-DELETE " + cmbDeleteStockroom.SelectedItem.ToString() + "'";
                }
                else
                {
                    MessageBox.Show("You cant delete Stockroom!");
                }
            }
            else
            {
                MessageBox.Show("Couldnt find any Stockroom!");
            }
        }


        private void cmbDeleteStockroom_SelectedIndexChanged(object sender, EventArgs e)
        {
            label5.Text = "DELETE-DELETE " + cmbDeleteStockroom.SelectedItem.ToString();
        }


        private void btnDeleteStockroom_Click(object sender, EventArgs e)
        {
            string[] category = { "bread", "drinks", "fruits", "menswear", "womenswear", "vegetables", "tools" };

            if (txtConfirm.Text.Equals(label5.Text))
            {
                foreach (var item in category)
                {
                    string deletePrd = $"DELETE FROM {item} WHERE warehouse = {store}";
                    string deleteStr = $"DELETE FROM stockroom WHERE name = '{cmbDeleteStockroom.SelectedItem.ToString()}'";

                    try
                    {
                        database.ExecuteQuery(deletePrd);
                        database.ExecuteQuery(deleteStr);
                        pnlConfirmDelete.Visible=false;
                        panel2.Visible = false;
                        draw();
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
            }
            else
            {
                MessageBox.Show("Wrong!");
            }
        }





        public void refreshBtn_Click(object sender, EventArgs e)
        {
            mainPage_Load(null, EventArgs.Empty);
        }


        public string setName(int id)
        {
            string query = $"SELECT name FROM stockroom WHERE id={id}";
            MySqlDataReader reader = database.Reader(query);
            string name = "";
            while (reader.Read())
            {
                name = reader[0].ToString();
            }
            database.Disconnect();
            return name;

        }


        private void newUser_Click(object sender, EventArgs e)
        {
            addAdmin.Visible = true;
            addCommon.Visible = true;
        }


        private void addAdmin_Click(object sender, EventArgs e)
        {
            if (loginpage.IsAdmin())
            {
                stt = true;
                regPage register = new regPage();
                register.Show();
            }
            else
            {
                MessageBox.Show("only admins can add new admin!");
            }
        }


        private void addCommon_Click(object sender, EventArgs e)
        {
            stt = false;//new user -- > common
            regPage register = new regPage();
            register.Show();
        }


        private void delUser_Click(object sender, EventArgs e)
        {
            if (loginpage.IsAdmin())
            {
                if (deleteuser == null || deleteuser.IsDisposed)
                {
                    deleteuser = new deleteUser();
                    deleteuser.Show();
                }
            }
            else
            {
                MessageBox.Show("you cant delete any user!");
            }
        }
        

        private void category_Click(object sender, EventArgs e)
        {
            category category = new category();
            category.Show();
        }


        private void addProduct_Click(object sender, EventArgs e)
        {
            addPrd addProduct = new addPrd();
            addProduct.Show();
        }


        private void deleteProduct_Click(object sender, EventArgs e)
        {
            if (loginpage.IsAdmin())
            {
                if (deletePrd == null || deletePrd.IsDisposed)
                {
                    deletePrd = new deletePrd();
                    deletePrd.Show();
                }
            }
            else
            {
                MessageBox.Show("you cant delete any product!");
            }
        }


        private void categoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            productType = categoryList.SelectedItem.ToString();
            addProduct.Visible = true;
            deleteProduct.Visible = true;
            productTable.Visible = true;
            increase.Visible = true;
            reduce.Visible = true;
            prdValue.Visible = true;
            refreshTable.Visible = true;
            productTable.DataSource = database.ListProducts(mainPage.productType, mainPage.store).Tables[0];
            database.Disconnect();
        }


        private void prdValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                value = Int32.Parse(prdValue.Text);
            }
            catch (Exception err)
            {
                MessageBox.Show("err" + MessageBox.Show(err.Message) + MessageBoxButtons.OK + MessageBoxIcon.Error);
            }
        }


        //only type number at increase/reduce textbox
        private void prdValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void increase_Click(object sender, EventArgs e)
        {
            //productTable.SelectedRows[0].Cells[0].Value.ToString() --> Selected rows stock
            string query = $"UPDATE {mainPage.productType} SET stock = stock + '{value}' " +
                           $"WHERE id= '{productTable.SelectedRows[0].Cells[0].Value.ToString()}' AND warehouse='{mainPage.store}'";

            checkTotalCapacity(int.Parse(store));
            checkCurrentCapacity(int.Parse(store));

            try
            {
                if (currentCapacity + value <= totalCapacity)
                {
                    database.ExecuteQuery(query);
                    updateCapacity();
                    draw();
                    productTable.DataSource = database.ListProducts(mainPage.productType, mainPage.store).Tables[0];
                }
                else
                {
                    MessageBox.Show("Not enough capacity!");
                }
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


        private void reduce_Click(object sender, EventArgs e)
        {
            string query = $"UPDATE {mainPage.productType} SET stock = stock - '{value}' " +
                           $"WHERE id= '{productTable.SelectedRows[0].Cells[0].Value.ToString()}'AND warehouse='{mainPage.store}'";

            try
            {
                if (Int32.Parse(productTable.SelectedRows[0].Cells[2].Value.ToString()) > 0 && value <= Int32.Parse(productTable.SelectedRows[0].Cells[2].Value.ToString()))
                {
                    database.ExecuteQuery(query);
                    updateCapacity();
                    draw();
                    productTable.DataSource = database.ListProducts(mainPage.productType, mainPage.store).Tables[0];
                }
                else
                {
                    MessageBox.Show("Stock cant be less than 0!");
                }
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


        //timer for slide effect 
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.HorizontalScroll.Value >= 1222)
            {
                this.timer.Enabled = false;
            }
            else
            {
                int x = this.HorizontalScroll.Value + this.HorizontalScroll.SmallChange * 6;
                this.AutoScrollPosition = new Point(x, 0);
            }
        }


        private void exit_Click(object sender, EventArgs e)
        {
            DialogResult x = MessageBox.Show("Do you want to exit", "Are you sure?", MessageBoxButtons.YesNo);
            if (x == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
        }


        private void main_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            DialogResult x = MessageBox.Show("Do you want to exit", "Are you sure?", MessageBoxButtons.YesNo);
            if (x == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
            else if (x == DialogResult.No)
            {
                e.Cancel = true;
            }
        }


        public static int totalCapacity;
        public void checkTotalCapacity(int stockroom)
        {
            string checkcapacity = $"SELECT capacity FROM stockroom WHERE id={stockroom}";
            MySqlDataReader reader = database.Reader(checkcapacity);

            try
            {
                while (reader.Read())
                {
                    totalCapacity = int.Parse(reader[0].ToString()); 
                }
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


        public static int currentCapacity = 0;
        public void checkCurrentCapacity(int stockroom)
        {
            currentCapacity = 0;
            string[] category = { "bread", "drinks", "fruits", "menswear","womenswear","vegetables","tools" };

            foreach (var item in category)
            {
                string checkcurrentcapacity = $"SELECT stock FROM {item} WHERE warehouse={stockroom}";
                MySqlDataReader reader = database.Reader(checkcurrentcapacity);

                try
                {
                    while (reader.Read())
                    {
                        currentCapacity += int.Parse(reader[0].ToString());
                    }
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
        }

        public void updateCapacity()
        {
            checkTotalCapacity(int.Parse(store));
            checkCurrentCapacity(int.Parse(store));
            lblCapacity.Text = currentCapacity.ToString() + " / " + totalCapacity.ToString();
        }

        private void refreshTable_Click(object sender, EventArgs e)
        {
            try
            {
                productTable.DataSource = database.ListProducts(mainPage.productType, mainPage.store).Tables[0];
                updateCapacity();
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
    }
}
