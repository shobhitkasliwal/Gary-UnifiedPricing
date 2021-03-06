﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;
using Unified_Price_for_Var.Properties;

namespace Unified_Price_for_Var
{
    public partial class frmViewPricing : Form
    {
        public DataTable _dataTableItems;

        public frmViewPricing()
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            InitializeComponent();
           
        }

        public void FillGridByCustomer()
        {
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
		//old	var prices = Db.ExecuteDataTable("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}' ORDER BY [Item Number]", cmbCustomers.SelectedValue);

            var prices = Db.ExecuteDataTable("SELECT P.[ID] as IDn, P.[Customer Number], P.[Item Number] as [ItemN], P.[Current Price] as [CurP], I.[Item Description] as [ItemDS], P.[Customer Item Number] as [CustIN], P.[Old Price] as [OldP], P.[Notes] as [NotesP], format(P.[QuoteDate],'mmm dd yyyy') as[QuoteDateP], P.[Last12MonthQTY] as [Last12MonthQTYP]  FROM tblPricing P left join tblItems I on P.[Item Number] = I.[Item Number] WHERE P.[Customer Number] = '{0}' ORDER BY P.[Item Number]", cmbCustomers.SelectedValue);  
            
            gridPrices.Rows.Clear();
            foreach (DataRow price in prices.Rows)
            {
          //old      gridPrices.Rows.Add(price["Item Number"], ((decimal)price["Current Price"]).ToString("0.0000"), ((decimal)price["Old Price"]).ToString("0.0000"), price["Item Description"], price["Customer Item Number"], price["ID"]);
                gridPrices.Rows.Add(price["ItemN"], ((decimal)price["CurP"]).ToString("0.0000"), ((decimal)price["OldP"]).ToString("0.0000"), price["ItemDS"], price["NotesP"], price["CustIN"], price["Last12MonthQTYP"], price["QuoteDateP"], price["IDn"]);
            }
            lblTotalPricesForCust.Text = "Total: " + prices.Rows.Count;
            lbl11.Text = cmbCustomers.SelectedValue.ToString().Replace("&","&&");  //Showing up customers number like: AS or CFT
            
        }

        private void frmViewPricing_Load(object sender, EventArgs e)
        {
           // this.Visible = false;

            var dataTable = Db.ExecuteDataTable("SELECT [Customer Number], [Customer Bill Name] + ' (' + [Customer Number] + ')' AS [Combinet Name] FROM tblCustomers ORDER BY [Customer Bill Name]");
            cmbCustomers.DataSource = dataTable;
            cmbCustomers.DisplayMember = "Combinet Name";
            cmbCustomers.ValueMember = "Customer Number";
         

            FillGridByCustomer();

            _dataTableItems = Db.ExecuteDataTable("SELECT [Item Number] FROM tblItems ORDER BY [Item Number]");

            cmbQuick_Check.DataSource = _dataTableItems;
            cmbQuick_Check.DisplayMember = "Item Number";
            cmbQuick_Check.ValueMember = "Item Number";

          //  this.Visible = true;
        }

        private void btnAddPrice_Click(object sender, EventArgs e)
        {
            
            cmbCustomers.Enabled = false;
            gridPrices.Enabled = false;

            grpPriceChange.Visible = false;
            grpPriceInfo.Visible = true;

            cmbQuick_Check.Enabled = false;
            btnMarkAsOld.Enabled = false;
            btnCopy.Enabled = false;
            btnCheckDuplicate.Enabled = false;
            btnRollBack.Enabled = true;
            btnEmail.Enabled = true;
            btnChange_Price.Enabled = false;
            btnDeletePrice.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnAddPrice.Enabled = false;
            btnUpdSPQ.Enabled = false;

            cmbItemNumb.Enabled = true;
            txtCurrent_Price.Enabled = true;
      //               txtCurrent_Price.Text = "0.0000";
            txtCurrent_Price.Text = " ";
      //               txtItem_description.Enabled = true;
            txtItem_description.Enabled = false;        
            txtItem_description.Text = " ";
            txtCustomer_Item_Number.Enabled = true;
            txtNotes.Enabled = true;
            txtNotes.Text = " ";
            txtStdQTY_Add.Enabled = true;
            txtStdQTY_Add.Text = "0";

            cmbItemNumb.DataSource = _dataTableItems;
            cmbItemNumb.DisplayMember = "Item Number";
            cmbItemNumb.ValueMember = "Item Number";
        }

        private void grpBox1_Enter(object sender, EventArgs e)
        {

        }
//---------------------------------------------------------------------------------
        private void txtChange_Price_Click(object sender, EventArgs e)
        {

            var change = MessageBox.Show("Are you sure you like to change this Item price?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            double num;
            bool isNumber = double.TryParse(txtNotes.Text, out num);
            if (isNumber)
            {
            }
            else
            {
                MessageBox.Show("Std Pac QTY - should be number");
                return;
            }
            
            
            if (change == DialogResult.Yes)
            {
                var rows = gridPrices.SelectedRows;

                if (rows.Count != 1)
                {
                    MessageBox.Show("You must select one price item.");
                    return;
                }

                var row = rows[0];

               
                decimal currentPrice;
                if (!Decimal.TryParse(textBox2.Text, out currentPrice))
                {
                    MessageBox.Show("Please enter the valid price.");
                    return;
                }

                 if (txtNotes.Text == "")
                {
                    MessageBox.Show("Std Pak QTY can not be empty.");
                    return;
                }
              
                try
                {
                    row.Cells["PreviousPrice"].Value = row.Cells["CurrentPrice"].Value;
                    row.Cells["CurrentPrice"].Value = currentPrice;

                    Db.NonQuery("UPDATE tblPricing SET [Old Price] = [Current Price], [Current Price] = '{0}', IsNew = 1, Notes = '{1}', QuoteDate=Now()  WHERE ID = {2}", currentPrice, txtNotes.Text, row.Cells["ID"].Value.ToString());
  
                    var lead = Db.ExecuteDataRow("SELECT * FROM tblDistributionGroupDetail WHERE [Group_Customer_Name] = '{0}' AND Modifier = 'Lead'", cmbCustomers.SelectedValue);
                    if (lead != null)
                    {
                        var customers = Db.ExecuteDataTable("SELECT * FROM tblDistributionGroupDetail WHERE [Group number] = '{0}' AND Modifier <> 'Lead'", lead["Group number"]);
                        foreach (DataRow custRow in customers.Rows)
                        {
                            var price = Db.ExecuteDataRow("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}' AND [Item Number] = '{1}'", custRow["Group_Customer_Name"], row.Cells["ItemNumber"].Value.ToString());

                            decimal dec = Convert.ToDecimal(custRow["Percent"]) / 100;

                            Db.NonQuery("UPDATE tblPricing SET [Old Price] = [Current Price], [Current Price] = '{0}', IsNew = 1, QuoteDate=Now() WHERE ID = {1}", custRow["Modifier"].ToString().Equals("Increase") ? (currentPrice + (currentPrice * dec)) : (currentPrice - (currentPrice * dec)), price["ID"]);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }

            txtNotes.Text = "";
            FillGridByCustomer();
        }
//================================================================================
        private void txtDelete_Click(object sender, EventArgs e)
        {
            var numbersStr = string.Empty;
            var idsStr = "IN (";

            foreach (DataGridViewRow row in gridPrices.SelectedRows)
            {
                numbersStr += row.Cells["ItemNumber"].Value + ", ";
                idsStr += row.Cells["ID"].Value + ", ";
            }
            if (numbersStr.Length > 0)
                numbersStr = numbersStr.Substring(0, numbersStr.Length - 2);
            if (idsStr.Length > 0)
                idsStr = idsStr.Substring(0, idsStr.Length - 2);
            idsStr += ")";

            var result = MessageBox.Show("You are going to delete selected items: " + numbersStr + "\n Please, confirm this process.", "Important", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                try
                {
                    Db.NonQuery("DELETE * FROM tblPricing WHERE ID {0}", idsStr);
                    
                    foreach (DataGridViewRow row in gridPrices.SelectedRows)
                    {
                        gridPrices.Rows.RemoveAt(row.Index);
                    }

                    MessageBox.Show("Item Deleted.", "Good News");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //MessageBox.Show("Item NOT Deleted", "Error");
                }
            }
        }
//========================================================================================================
        private void btnMarkAsOld_Click(object sender, EventArgs e)
        {
            if (cmbCustomers.SelectedValue.ToString().Equals("-1"))
            {
                MessageBox.Show("Please select customer");
                return;
            }

            var check = MessageBox.Show("Please, confirm to 'MARK All PRICES AS OLD' for selected customer.", "Conformation", MessageBoxButtons.YesNo);
            if (check == DialogResult.Yes)
            {
                try
                {
                    using (var connection = new OleDbConnection(Db._connectionString))
                    {
                        connection.Open();
                        OleDbCommand command = connection.CreateCommand();
                        foreach (DataGridViewRow row in gridPrices.Rows)
                        {
                            command.CommandText = string.Format("UPDATE tblPricing SET [Old Price] = 0, IsNew = 0 WHERE ID = {0}", row.Cells["ID"].Value);
                            command.ExecuteNonQuery();
                            row.Cells["PreviousPrice"].Value = "0,000";
                        }
                    }
                    MessageBox.Show("Item  price(s) for selected customer was MARKED AS OLD.");
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
        }
//====================================================
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (cmbCustomers.SelectedValue.ToString().Equals("-1"))
            {
                MessageBox.Show("Please select customer");
                return;
            }
            this.Visible = false;
            frmCopyPrices nextScreen = new frmCopyPrices(cmbCustomers.SelectedValue.ToString(), cmbCustomers.Text);
            DialogResult result = nextScreen.ShowDialog();
            if (result == DialogResult.OK)
                FillGridByCustomer();
            this.Visible = true;
        }
//===========================================================
        private void btnCheckDuplicate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Will be done on the next stage.");
        }
        
        //=============================================================        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
          
            var custPricings = Db.ExecuteDataTable("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}' ORDER BY [Item Number] ASC", cmbCustomers.SelectedValue); 
            Db.NonQuery("DELETE FROM tblByCustomer_Report");

            for (int i = 0; i < custPricings.Rows.Count; i++)
            {
                decimal ASRCurrPriceDec = 0;
                decimal ASTCurrPriceDec = 0;

      //          var ASRCurrPrice = Db.ExecuteDataRow("SELECT TOP 1 [Current Price] FROM tblPricing WHERE [Customer Number] = 'ASR' AND [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);
      //          var ASTCurrPrice = Db.ExecuteDataRow("SELECT TOP 1 [Current Price] FROM tblPricing WHERE [Customer Number] = 'AST' AND [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);

      //          if (ASRCurrPrice != null)
      //              ASRCurrPriceDec = (decimal)ASRCurrPrice[0];
      //          if (ASTCurrPrice != null)
      //              ASTCurrPriceDec = (decimal)ASTCurrPrice[0];

                var cust = Db.ExecuteDataRow("SELECT * FROM tblCustomers WHERE [Customer Number] = '{0}'", custPricings.Rows[i]["Customer Number"]);

                Db.NonQuery("INSERT INTO tblByCustomer_Report ([Customer Number], [Customer Name], [Item Number], [Item Description], [Current Price], [Customer Item Number], [IsNew], [ASR Current Price], [AST Current Price], [Notes]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')",
                    custPricings.Rows[i]["Customer Number"],
                    cust["Customer Bill Name"].ToString().Replace("'", "''"),
                    custPricings.Rows[i]["Item Number"],
                    custPricings.Rows[i]["Item Description"].ToString().Replace("'", "''"),
                    custPricings.Rows[i]["Current Price"],
                    custPricings.Rows[i]["Customer Item Number"],
                    custPricings.Rows[i]["IsNew"],
                    ASRCurrPriceDec,
                    ASTCurrPriceDec,
                    custPricings.Rows[i]["Notes"]
                    );
            }
            Cursor.Current = Cursors.Default;

             var result = MessageBox.Show("\"Click on YES to print only NEW / CHANGED prices, or click NO to print all\"", "Warning", MessageBoxButtons.YesNo);
             if (result == DialogResult.No)
             {
                 Cursor.Current = Cursors.WaitCursor;
                 frmReport2_all_Viewer frm = new frmReport2_all_Viewer();
                 frm.Show();
                 Cursor.Current = Cursors.Default;
             }
             else
             {
                 Cursor.Current = Cursors.WaitCursor;
                 frmReport2_new_Viewer frm = new frmReport2_new_Viewer();
                 frm.Show();
                 Cursor.Current = Cursors.Default;
                
             }            
          }        
//==============================================================================================
        
        private void btnChangePrices_Click(object sender, EventArgs e)
        {
            
            Cursor.Current = Cursors.WaitCursor;

            var custPricings = Db.ExecuteDataTable("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}' ORDER BY [Item Number] ASC", cmbCustomers.SelectedValue);
            Db.NonQuery("DELETE FROM tblByCustomer_Report");
            Db.NonQuery("Delete from tblAST");
            Db.NonQuery("Delete from tblASR");

            Db.NonQuery("insert into tblAst ([Item Number], [Current price]) select tblPricing.[Item Number],  tblPricing.[Current Price] from tblPricing where tblPricing.[Customer number] = 'AST' ");
            Db.NonQuery("insert into tblAsr ([Item Number], [Current price]) select tblPricing.[Item Number],  tblPricing.[Current Price] from tblPricing where tblPricing.[Customer number] = 'ASR' ");

            
            for (int i = 0; i < custPricings.Rows.Count; i++)
            {
                decimal ASRCurrPriceDec = 0;
                decimal ASTCurrPriceDec = 0;

           //     var ASRCurrPrice = Db.ExecuteDataRow("SELECT TOP 1 [Current Price] FROM tblPricing WHERE [Customer Number] = 'ASR' AND [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);
           //     var ASTCurrPrice = Db.ExecuteDataRow("SELECT TOP 1 [Current Price] FROM tblPricing WHERE [Customer Number] = 'AST' AND [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);

                var ASRCurrPrice = Db.ExecuteDataRow("SELECT [Current Price] FROM tblASR WHERE [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);
                var ASTCurrPrice = Db.ExecuteDataRow("SELECT [Current Price] FROM tblAST WHERE [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);

                if (ASRCurrPrice != null)
                    ASRCurrPriceDec = (decimal)ASRCurrPrice[0];
                if (ASTCurrPrice != null)
                    ASTCurrPriceDec = (decimal)ASTCurrPrice[0];

                var cust = Db.ExecuteDataRow("SELECT * FROM tblCustomers WHERE [Customer Number] = '{0}'", custPricings.Rows[i]["Customer Number"]);

                Db.NonQuery("INSERT INTO tblByCustomer_Report ([Customer Number], [Customer Name], [Item Number], [Item Description], [Current Price], [Customer Item Number], [IsNew], [ASR Current Price], [AST Current Price], [Notes]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')",
                    custPricings.Rows[i]["Customer Number"],
                    cust["Customer Bill Name"].ToString().Replace("'", "''"),
                    custPricings.Rows[i]["Item Number"],
                    custPricings.Rows[i]["Item Description"].ToString().Replace("'", "''"),
                    custPricings.Rows[i]["Current Price"],
                    custPricings.Rows[i]["Customer Item Number"],
                    custPricings.Rows[i]["IsNew"],
                    ASRCurrPriceDec,
                    ASTCurrPriceDec,
                    custPricings.Rows[i]["Notes"]
                    );
            }
            Cursor.Current = Cursors.Default;

            var result = MessageBox.Show("\"Click on YES to print only NEW / CHANGED prices, or click NO to print all\"", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                Cursor.Current = Cursors.WaitCursor;
                frmReport1_all_Viewer frm = new frmReport1_all_Viewer();
                frm.Show();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                frmReport1_new_Viewer frm = new frmReport1_new_Viewer();
                frm.Show();
                Cursor.Current = Cursors.Default;

            }            
        }
//================================================================================================
        private void btnRollBack_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Please confirm to Roll Back All prices for selected customer", "Conformation", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
            var rollback = Db.ExecuteDataRow("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}' AND IsNew = Yes ", cmbCustomers.SelectedValue);
                             
               if (rollback != null)
                  {
                      var rollbackTable = Db.ExecuteDataTable("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}' AND IsNew = Yes ", cmbCustomers.SelectedValue);
                       
                   foreach (DataRow itemRow in rollbackTable.Rows)
                       {
                        var eachRow = Db.ExecuteDataRow("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}' AND IsNew = Yes ", cmbCustomers.SelectedValue);
                                        
                       Db.NonQuery("UPDATE tblPricing SET [Current Price] = [Old Price], [Old Price] = 0, IsNew = No WHERE ID = {0}", eachRow["ID"]);
               
                       }
                  }
               FillGridByCustomer();
            }
         }
//================================================================================================
        private void btnEmail_Click(object sender, EventArgs e)
        {
           
                        var custInformation = Db.ExecuteDataRow("Select [Email Address] From tblCustomers Where [Customer Number] = '{0}'", cmbCustomers.SelectedValue.ToString());

                                TextBox tB = new TextBox();
                                tB.Text = custInformation["Email Address"].ToString();

                                if (tB.Text != "")
                                {
                                    if (Validation.IsValidEmail(tB))
                                    {
                                        frmEmail1 nextScreen = new frmEmail1(tB.Text);
                                        DialogResult result = nextScreen.ShowDialog();
                                    }
                                }
                                else
                                {
                                   var answer =  MessageBox.Show("Customer does not have Email address. \n Would you like to process anyway?", "Importent",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (answer == DialogResult.Yes)
                                    {
                               //       frmEmail1 nextScreen = new frmEmail1(tB.Text);
                                        frmEmail1 nextScreen = new frmEmail1("empty");
                                        DialogResult result = nextScreen.ShowDialog();
                                    }
                                }
        }
//============================================================================================
            
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            cmbCustomers.Enabled = true;
            gridPrices.Enabled = true;

            grpPriceChange.Visible = true;
            grpPriceInfo.Visible = false;

            cmbQuick_Check.Enabled = true;
            btnMarkAsOld.Enabled = true;
            btnCopy.Enabled = true;
            btnCheckDuplicate.Enabled = true;
            btnRollBack.Enabled = true ;
            btnEmail.Enabled =  true;
            btnChange_Price.Enabled = true;
            btnDeletePrice.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnAddPrice.Enabled = true;
            btnUpdSPQ.Enabled = true;

            cmbItemNumb.Enabled = false;
            txtCurrent_Price.Enabled = false;
            txtCurrent_Price.Text = "0.0000";
            txtItem_description.Enabled = false;
            txtCustomer_Item_Number.Enabled = false;
        }
//==============================================================================
        private void btnSave_Click(object sender, EventArgs e)
        {
            var existsItem = Db.ExecuteDataTable("SELECT * FROM tblItems WHERE [Item Number] = '{0}'", cmbItemNumb.Text);
            if (existsItem.Rows.Count == 0)
            {
                MessageBox.Show("Item not exists in item master table. Before pricing new item ADD it to master table.", "Warning");
                return;
            }
         
            var exists = Db.ExecuteDataTable("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}' and [Item Number] = '{1}'", cmbCustomers.SelectedValue, cmbItemNumb.Text);
            if (exists.Rows.Count > 0)
            {
                MessageBox.Show("Item already exists in customer list");
                return;
            }
            
            if (cmbItemNumb.Text == "")
            {
                MessageBox.Show("You must select the Item to quote this Customer a Price for. \n Please select an Item");
            	return;
			}

	//		if (txtItem_description.Text == "")
	//		{
	//			MessageBox.Show("You must enter Item Description");
	//			return;
	//		}

			if(txtCurrent_Price.Text.Length == 0)
			{
				MessageBox.Show("You must enter Current Price");
				return;				
			}

        	var strPrice = txtCurrent_Price.Text;
			if(strPrice.Contains("."))
			{				
				if (strPrice.Substring(strPrice.IndexOf(".")).Length > 5)
				{
					MessageBox.Show("Digits after '.' could not be more than 4");
					return;
				}
			}

			if (strPrice.Contains(","))
			{
				if (strPrice.Substring(strPrice.IndexOf(",")).Length > 5)
				{
					MessageBox.Show("Digits after ',' could not be more than 4");
					return;
				}
			}

            decimal currentPrice;
            if (!Decimal.TryParse(txtCurrent_Price.Text, out currentPrice))
            {
                MessageBox.Show("Please enter currect price.");
                return;
            }

            		
            {
               //  Db.NonQuery("INSERT INTO tblPricing ([Customer Number], [Item Number], [Item Description], [Current Price], [Customer Item Number], [IsNew]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}' )",
                   Db.NonQuery("INSERT INTO tblPricing ([Customer Number], [Item Number], [Current Price], [Customer Item Number], [IsNew], [Notes], QuoteDate) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}',Now() )",
        
                    cmbCustomers.SelectedValue.ToString(),
                    cmbItemNumb.SelectedValue.ToString(),
               //    txtItem_description.Text,
               //    txtCurrent_Price.Text.Replace('.', ','),
                    txtCurrent_Price.Text,
                    txtCustomer_Item_Number.Text,
                    1,
                    txtStdQTY_Add.Text
                    );

                var lead = Db.ExecuteDataRow("SELECT * FROM tblDistributionGroupDetail WHERE [Group_Customer_Name] = '{0}' AND Modifier = 'Lead'", cmbCustomers.SelectedValue);
                if (lead != null)
                {
                    var customers = Db.ExecuteDataTable("SELECT * FROM tblDistributionGroupDetail WHERE [Group number] = '{0}' AND Modifier <> 'Lead'", lead["Group number"]);
                    foreach (DataRow custRow in customers.Rows)
                    {                        
                        decimal dec = Convert.ToDecimal(custRow["Percent"]) / 100;

                        Db.NonQuery("INSERT INTO tblPricing ([Customer Number], [Item Number], [Item Description], [Current Price],QuoteDate) VALUES ('{0}', '{1}', '{2}', '{3}',Now())",
                            custRow["Group_Customer_Name"],
                            cmbItemNumb.SelectedValue.ToString(),
                            txtItem_description.Text,
                            custRow["Modifier"].ToString().Equals("Increase") ? (currentPrice + (currentPrice * dec)) : (currentPrice - (currentPrice * dec)));
                    }
                }

                cmbCustomers.Enabled = true;
                gridPrices.Enabled = true;

                grpPriceChange.Visible = true;
                grpPriceInfo.Visible = false;

                cmbQuick_Check.Enabled = true;
                btnMarkAsOld.Enabled = true;
                btnCopy.Enabled = true;
                btnCheckDuplicate.Enabled = true;
                btnRollBack.Enabled = true;
                btnEmail.Enabled = true;
                btnChange_Price.Enabled = true;
                btnDeletePrice.Enabled = true;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnAddPrice.Enabled = true;
                btnUpdSPQ.Enabled = true;

                cmbItemNumb.Enabled = false;
                cmbItemNumb.Text = "";
                txtCurrent_Price.Enabled = false;
                txtCurrent_Price.Text = string.Format("#.0000");
                txtItem_description.Enabled = false;
                txtItem_description.Text = "";
                txtCustomer_Item_Number.Enabled = false;
                txtCustomer_Item_Number.Text = "";
                txtStdQTY_Add.Text = "";

                FillGridByCustomer();
            }
        }
//==============================================================================================================
        private void cmbCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillGridByCustomer();
        }

        private void gridPrices_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var rows = gridPrices.SelectedRows;

            if(rows.Count != 1)
            {
                grpPriceChange.Enabled = false;
            }
            else if (rows.Count == 1)
            {
                grpPriceChange.Enabled = true;
                var row = rows[0];       
                textBox1.Text = row.Cells["ItemNumber"].Value.ToString();
                textBox2.Text = row.Cells["CurrentPrice"].Value.ToString();
                textBox3.Text = row.Cells["ItemDescription"].Value.ToString();
                textBox4.Text = row.Cells["CustomerItemNumber"].Value.ToString();
                txtNotes.Text = row.Cells["Notes"].Value.ToString();
            }
        }

        private void cmbQuick_Check_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                foreach (DataGridViewRow row in gridPrices.Rows)
                    if (row.Cells["ItemNumber"].Value.ToString().Equals(cmbQuick_Check.Text))
                    {
                        row.Selected = true;
                        gridPrices.FirstDisplayedScrollingRowIndex = row.Index;
                    }
        }

        private void txtItem_description_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);  
        }

        private void cmbQuick_Check_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);  
        }

        private void cmbItemNumb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);  
        }

        private void txtCustomer_Item_Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
//=====================================================================================================
		private void cmbQuick_Check_SelectedIndexChanged(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in gridPrices.Rows)
				if (row.Cells["ItemNumber"].Value.ToString().Equals(cmbQuick_Check.Text))
				{
					row.Selected = true;
					gridPrices.FirstDisplayedScrollingRowIndex = row.Index;
				}
		}
//====================================================================================================
        private void cmbItemNumb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = Db.ExecuteDataRow("SELECT * FROM tblItems WHERE [Item Number] = '{0}'", cmbItemNumb.SelectedValue);
            if (item != null)
                txtItem_description.Text = item["Item Description"].ToString();
        }

        private void lstBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //----------------------------------------------------------------
        //--------------- Change Notes column (Standard Pack QTY)
        //----------------------------------------------------------------


        private void btnUpdSPQ_Click(object sender, EventArgs e)
        {
                      
            var change = MessageBox.Show("Are you sure you like to change Std Pak Qty?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            double num;
            bool isNumber = double.TryParse(txtNotes.Text, out num); 
            if (isNumber) 
            {
            }  
            else
            {
                MessageBox.Show("Std Pac QTY - should be number");
                return;
            }
            
            
            
            if (change == DialogResult.Yes)
            {
                var rows = gridPrices.SelectedRows;

                if (rows.Count != 1)
                {
                    MessageBox.Show("You must select one item.");
                    return;
                }

                var row = rows[0];

                if (txtNotes.Text == "")
                {
                    MessageBox.Show("Std Pak QTY can not be empty.");
                    return;
                }
                                                                       
                Db.NonQuery("UPDATE tblPricing SET Notes = '{0}'  WHERE ID = {1}", txtNotes.Text, row.Cells["ID"].Value.ToString());
    
                txtNotes.Text = "";
                FillGridByCustomer();
                
            }
        }
//===================================================================================================
    }
}
