﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;                      //  version to send
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;


namespace Unified_Price_for_Var
{
    public partial class frmReports : Form
    {
        public DataTable _dataTableItems;
        public DataTable _dataTableItemsFrom;
        public DataTable _dataTableItemsTo;
        public DataTable _dataTableCustomer;
        public DataTable _dataTableCustomer1;
        public DataTable _dataTableSwing;
        public DataTable _dataTableCustomerFrom;
        public DataTable _dataTableCustomerTo;
        public DataTable _dataTableFamily;
        public DataTable _dataTableFamily1;
        public DataTable _dataTableCompItems;
        public DataTable _dataTableItemsFrom5;
        public DataTable _dataTableItemsTo5;
        public DataTable _dataTableCustomer5;


        public frmReports()
        {
            InitializeComponent();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            this.Visible = false;

            _dataTableItemsFrom5 = Db.ExecuteDataTable("SELECT [Item Number] FROM tblItems ORDER BY [Item Number]");
            cmbFrom5.DataSource = _dataTableItemsFrom5;
            cmbFrom5.DisplayMember = "Item Number";
            cmbFrom5.ValueMember = "Item Number";

            _dataTableItemsTo5 = Db.ExecuteDataTable("SELECT [Item Number] FROM tblItems ORDER BY [Item Number]");
            cmbTo5.DataSource = _dataTableItemsTo5;
            cmbTo5.DisplayMember = "Item Number";
            cmbTo5.ValueMember = "Item Number";

            _dataTableCustomer = Db.ExecuteDataTable("SELECT [Customer Number], [Customer Bill Name] + ' (' + [Customer Number] + ')' AS [Combinet Name] FROM tblCustomers ORDER BY [Customer Number]");
            cmbCust.DataSource = _dataTableCustomer;
            cmbCust.DisplayMember = "Combinet Name";
            cmbCust.ValueMember = "Customer Number";

            _dataTableCustomer5 = Db.ExecuteDataTable("SELECT [Customer Number], [Customer Bill Name] + ' (' + [Customer Number] + ')' AS [Combinet Name] FROM tblCustomers ORDER BY [Customer Number]");
            cmbCust5.DataSource = _dataTableCustomer5;
            cmbCust5.DisplayMember = "Combinet Name";
            cmbCust5.ValueMember = "Customer Number";

            _dataTableSwing = Db.ExecuteDataTable("SELECT [Swing Number], [Swing Name], [Swing Number] + ' - ' + [Swing Name] AS [Header] FROM tblSwingNumbers ORDER BY [Swing Name]");
            cmbSwing.DataSource = _dataTableSwing;
            cmbSwing.DisplayMember = "Header";
            cmbSwing.ValueMember = "Swing Number";

            _dataTableCustomerFrom = Db.ExecuteDataTable("SELECT [Customer Number], [Customer Bill Name] + ' (' + [Customer Number] + ')' AS [Combinet Name] FROM tblCustomers ORDER BY [Customer Number]");
            cmbCustFrom.DataSource = _dataTableCustomerFrom;
            cmbCustFrom.DisplayMember = "Combinet Name";
            cmbCustFrom.ValueMember = "Customer Number";

            _dataTableCustomerTo = Db.ExecuteDataTable("SELECT [Customer Number], [Customer Bill Name] + ' (' + [Customer Number] + ')' AS [Combinet Name] FROM tblCustomers ORDER BY [Customer Number]");
            cmbCustTo.DataSource = _dataTableCustomerTo;
            cmbCustTo.DisplayMember = "Combinet Name";
            cmbCustTo.ValueMember = "Customer Number";

            _dataTableCompItems = Db.ExecuteDataTable("SELECT [Item Number] FROM tblItems ORDER BY [Item Number]");
            cmbCompItem.DataSource = _dataTableCompItems;
            cmbCompItem.DisplayMember = "Item Number";
            cmbCompItem.ValueMember = "Item Number";

            this.Visible = true;
        }
        //=========================================================================
        private void rdoPrintByItemNumb_CheckedChanged(object sender, EventArgs e)
        {
            _dataTableItems = Db.ExecuteDataTable("SELECT [Item Number] FROM tblItems ORDER BY [Item Number]");
            cmbItem.DataSource = _dataTableItems;
            cmbItem.DisplayMember = "Item Number";
            cmbItem.ValueMember = "Item Number";

            cmbItem.Enabled = true;
            cmbFamily.Enabled = false;
            panel4.Visible = false;
            panel1.Visible = false;


            if (rdoPrintByItemNumb.Checked)
                rdoPrintByItemNumb.Font = new Font(rdoPrintByItemNumb.Font.FontFamily, rdoPrintByItemNumb.Font.Size, FontStyle.Bold);
            else
                rdoPrintByItemNumb.Font = new Font(rdoPrintByItemNumb.Font.FontFamily, rdoPrintByItemNumb.Font.Size, FontStyle.Regular);

        }
        //======================================================================

        private void rdoPrintByRange_CheckedChanged(object sender, EventArgs e)
        {
            _dataTableItemsFrom = Db.ExecuteDataTable("SELECT [Item Number] FROM tblItems ORDER BY [Item Number]");
            cmbFrom.DataSource = _dataTableItemsFrom;
            cmbFrom.DisplayMember = "Item Number";
            cmbFrom.ValueMember = "Item Number";

            _dataTableItemsTo = Db.ExecuteDataTable("SELECT [Item Number] FROM tblItems ORDER BY [Item Number]");
            cmbTo.DataSource = _dataTableItemsTo;
            cmbTo.DisplayMember = "Item Number";
            cmbTo.ValueMember = "Item Number";

            panel4.BringToFront();
            panel4.Visible = true;
            panel1.Visible = false;
            cmbFamily.Enabled = false;
            cmbItem.Enabled = false;


            if (rdoPrintByRange.Checked)
                rdoPrintByRange.Font = new Font(rdoPrintByRange.Font.FontFamily, rdoPrintByRange.Font.Size, FontStyle.Bold);
            else
                rdoPrintByRange.Font = new Font(rdoPrintByRange.Font.FontFamily, rdoPrintByRange.Font.Size, FontStyle.Regular);


        }
        //==========================================================================================================
        private void rdoPrintAll_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel4.Visible = false;
            cmbFamily.Enabled = false;
            cmbItem.Enabled = false;

            if (rdoPrintAll.Checked)
                rdoPrintAll.Font = new Font(rdoPrintAll.Font.FontFamily, rdoPrintAll.Font.Size, FontStyle.Bold);
            else
                rdoPrintAll.Font = new Font(rdoPrintAll.Font.FontFamily, rdoPrintAll.Font.Size, FontStyle.Regular);

        }
        //==========================================================================================================
        private void rdoPrintByFamily_CheckedChanged(object sender, EventArgs e)
        {
            _dataTableFamily = Db.ExecuteDataTable("Select distinct [Family] FROM tblItems ORDER BY [Family]");
            cmbFamily.DataSource = _dataTableFamily;
            cmbFamily.DisplayMember = "Family";
            cmbFamily.ValueMember = "Family";

            panel1.Visible = false;
            panel4.Visible = false;
            cmbFamily.Enabled = true;
            cmbItem.Enabled = false;

            if (rdoPrintByFamily.Checked)
                rdoPrintByFamily.Font = new Font(rdoPrintByFamily.Font.FontFamily, rdoPrintByFamily.Font.Size, FontStyle.Bold);
            else
                rdoPrintByFamily.Font = new Font(rdoPrintByFamily.Font.FontFamily, rdoPrintByFamily.Font.Size, FontStyle.Regular);

        }
        //=========================================================================================================
        private void rdoPrintByCustFamily_CheckedChanged(object sender, EventArgs e)
        {
            _dataTableCustomer1 = Db.ExecuteDataTable("SELECT [Customer Number] FROM tblCustomers ORDER BY [Customer Number]");
            cmbCust1.DataSource = _dataTableCustomer1;
            cmbCust1.DisplayMember = "Customer Name";
            cmbCust1.ValueMember = "Customer Number";

            _dataTableFamily1 = Db.ExecuteDataTable("Select distinct [Family] FROM tblItems ORDER BY [Family]");
            cmbFamily1.DataSource = _dataTableFamily1;
            cmbFamily1.DisplayMember = "Family";
            cmbFamily1.ValueMember = "Family";

            panel4.Visible = false;
            panel1.Visible = true;
            cmbFamily.Enabled = false;
            cmbItem.Enabled = false;


            if (rdoPrintByCustFamily.Checked)
                rdoPrintByCustFamily.Font = new Font(rdoPrintByCustFamily.Font.FontFamily, rdoPrintByCustFamily.Font.Size, FontStyle.Bold);
            else
                rdoPrintByCustFamily.Font = new Font(rdoPrintByCustFamily.Font.FontFamily, rdoPrintByCustFamily.Font.Size, FontStyle.Regular);

        }

        //=========================================================================================================
        private void rdoSwing1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSwing1.Checked)
                rdoSwing1.Font = new Font(rdoSwing1.Font.FontFamily, rdoSwing1.Font.Size, FontStyle.Bold);
            else
                rdoSwing1.Font = new Font(rdoSwing1.Font.FontFamily, rdoSwing1.Font.Size, FontStyle.Regular);

        }
        //=========================================================================================================
        private void rdoSwing2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSwing2.Checked)
                rdoSwing2.Font = new Font(rdoSwing2.Font.FontFamily, rdoSwing2.Font.Size, FontStyle.Bold);
            else
                rdoSwing2.Font = new Font(rdoSwing2.Font.FontFamily, rdoSwing2.Font.Size, FontStyle.Regular);

        }
        //============================================================================================================
        private void btnCompareCust_CheckedChanged(object sender, EventArgs e)
        {
            pnlCompItems.Visible = false;
            if (rdoCompareCust.Checked)
                rdoCompareCust.Font = new Font(rdoCompareCust.Font.FontFamily, rdoCompareCust.Font.Size, FontStyle.Bold);
            else
                rdoCompareCust.Font = new Font(rdoCompareCust.Font.FontFamily, rdoCompareCust.Font.Size, FontStyle.Regular);

        }
        //=========================================================================================================
        private void rdoCompareItems_CheckedChanged(object sender, EventArgs e)
        {
            pnlCompItems.Visible = true;
            if (rdoCompareItems.Checked)
                rdoCompareItems.Font = new Font(rdoCompareItems.Font.FontFamily, rdoCompareItems.Font.Size, FontStyle.Bold);
            else
                rdoCompareItems.Font = new Font(rdoCompareItems.Font.FontFamily, rdoCompareItems.Font.Size, FontStyle.Regular);

        }

        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        private void btnViewReport1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Db.NonQuery("DELETE FROM tblByItem_Report");
            String DefaultQuery = "INSERT INTO tblByItem_Report ( [Item Number], [Item Description], [Customer Name], [Customer Number], [Current Price], InDistrGroup, QuoteDate, Last12MonthQTY ) SELECT tblPricing.[item Number], tblPricing.[item description], tblCustomers.[Customer Bill Name],tblCustomers.[Customer Number], tblPricing.[Current Price], '' AS InDistrGroup, tblPricing.QuoteDate, tblPricing.Last12MonthQty FROM tblPricing  INNER JOIN tblCustomers ON tblPricing.[Customer Number] = tblCustomers.[Customer Number] ";

            if (rdoPrintByItemNumb.Checked)
            {
                //var items = Db.ExecuteDataTable("SELECT [Item Number], [Item Description], [Customer Number], [Current Price], [QuoteDate], [Last12MonthQTY] FROM tblPricing WHERE [Item Number] = '{0}' ORDER BY [Current Price] DESC", cmbItem.SelectedValue);

                //foreach (DataRow row in items.Rows)
                //{

                //    var cust = Db.ExecuteDataRow("SELECT * FROM tblCustomers WHERE [Customer Number] = '{0}'", row["Customer Number"]);
                //    if (cust != null)
                //    {
                //        Db.NonQuery("INSERT INTO tblByItem_Report ([Item Number], [Item Description], [Customer Number], [Customer Name], [Current Price], [InDistrGroup],[QuoteDate],[Last12MonthQTY]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}','{7}')",
                //            row["Item Number"],
                //            row["Item Description"].ToString().Replace("'", "''"),
                //            row["Customer Number"],
                //            cust["Customer Bill Name"].ToString().Replace("'", "''"),
                //            row["Current Price"],
                //            " ",
                //            row["QuoteDate"],
                //            row["Last12MonthQTY"]
                //            //isInGroup
                //            );
                //    }
                //}
                Db.NonQuery(DefaultQuery + " WHERE [Item Number] = '" + cmbItem.SelectedValue + "' ORDER BY [Current Price];");
                Cursor.Current = Cursors.Default;
                ReportViewers.frmByItem_Viewer frm = new ReportViewers.frmByItem_Viewer();
                frm.Text = "By Item Number Report";
                frm.Show();
            }

            //-------------------------------------------------------------------------------

            if (rdoPrintByRange.Checked)
            {
                //var items = Db.ExecuteDataTable("SELECT [Item Number], [Item Description], [Customer Number], [Current price], [QuoteDate], [Last12MonthQTY] FROM tblPricing WHERE [Item Number] between '{0}' and '{1}' ORDER BY [Item Number],[Current Price]", cmbFrom.SelectedValue, cmbTo.SelectedValue);
                //foreach (DataRow row in items.Rows)
                //{

                //    var cust = Db.ExecuteDataRow("SELECT [Customer Bill Name] FROM tblCustomers WHERE [Customer Number] = '{0}'", row["Customer Number"]);
                //    if (cust != null)
                //    {
                //        Db.NonQuery("INSERT INTO tblByItem_Report ([Item Number], [Item Description], [Customer Number], [Customer Name], [Current Price], [InDistrGroup],[QuoteDate],[Last12MonthQTY]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}','{7}')",
                //            row["Item Number"],
                //            row["Item Description"].ToString().Replace("'", "''"),
                //            row["Customer Number"],
                //            cust["Customer Bill Name"].ToString().Replace("'", "''"),
                //            row["Current Price"],
                //            " ",
                //            row["QuoteDate"],
                //            row["Last12MonthQTY"]
                //            );
                //    }
                //}
                Db.NonQuery(DefaultQuery + " WHERE [Item Number] between '{0}' and '{1}' ORDER BY [Item Number],[Current Price]", cmbFrom.SelectedValue, cmbTo.SelectedValue);
                Cursor.Current = Cursors.Default;
                ReportViewers.frmByItem_Viewer frm = new ReportViewers.frmByItem_Viewer();
                frm.Text = "By Range of Item Numbers";
                frm.Show();
            }
            //-------------------------------------------------------------------------------
            if (rdoPrintByFamily.Checked)
            {
                //var items = Db.ExecuteDataTable("Select [P.Item Number],[P.Customer Number], [P.Current Price], [I.Family], [I.Item Description], [QuoteDate], [Last12MonthQTY] From tblPricing P left join tblItems I on P.[Item Number] = I.[Item number] where I.[Family] = '{0}'", cmbFamily.SelectedValue);

                //foreach (DataRow row in items.Rows)
                //{

                //    var cust = Db.ExecuteDataRow("SELECT * FROM tblCustomers WHERE [Customer Number] = '{0}'", row["P.Customer Number"]);
                //    if (cust != null)
                //    {
                //        Db.NonQuery("INSERT INTO tblByItem_Report ([Item Number], [Item Description], [Customer Number], [Customer Name], [Current Price], [InDistrGroup],[QuoteDate],[Last12MonthQTY]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}','{7}')",
                //            row["P.Item Number"],
                //            row["I.Item Description"].ToString().Replace("'", "''"),
                //            row["P.Customer Number"],
                //            cust["Customer Bill Name"].ToString().Replace("'", "''"),
                //            row["P.Current Price"],
                //            row["I.Family"],
                //            row["QuoteDate"],
                //            row["Last12MonthQTY"]

                //            );
                //    }
                //}
                Db.NonQuery("INSERT INTO tblByItem_Report ( [Item Number], [Item Description], [Customer Name], [Customer Number], [Current Price], InDistrGroup, QuoteDate, Last12MonthQTY ) SELECT tblPricing.[item Number], tblPricing.[item description], tblCustomers.[Customer Bill Name], tblCustomers.[Customer Number], tblPricing.[Current Price], '' AS InDistrGroup, tblPricing.QuoteDate, tblPricing.Last12MonthQty FROM (tblPricing INNER JOIN tblItems ON tblPricing.[Item Number] = tblItems.[Item Number])  INNER JOIN tblCustomers ON tblPricing.[Customer Number] = tblCustomers.[Customer Number] where tblItems.[Family] = '{0}'", cmbFamily.SelectedValue);
                Cursor.Current = Cursors.Default;
                ReportViewers.frmByItem_Viewer frm = new ReportViewers.frmByItem_Viewer();
                frm.Text = "By Family Report";
                frm.Show();
            }

            //-------------------------------------------------------------------------------

            // Update here to remove the looping and do everythign at database.
            if (rdoPrintAll.Checked)
            {
                //var reallyItems = Db.ExecuteDataTable("SELECT * FROM tblItems");
                Db.NonQuery("INSERT INTO tblByItem_Report ( [Item Number], [Item Description], [Customer Name], [Customer Number], [Current Price], InDistrGroup, QuoteDate, Last12MonthQTY ) SELECT tblItems.[item Number], tblItems.[item description], tblCustomers.[Customer Bill Name], tblCustomers.[Customer Number], tblPricing.[Current Price], '' AS InDistrGroup, tblPricing.QuoteDate, tblPricing.Last12MonthQty FROM (tblItems INNER JOIN tblPricing ON tblItems.ID = tblPricing.ID) INNER JOIN tblCustomers ON tblPricing.[Customer Number] = tblCustomers.[Customer Number];");
                //foreach (DataRow itemRow in reallyItems.Rows)
                //{
                //    var items = Db.ExecuteDataTable("SELECT * FROM tblPricing WHERE [Item Number] = '{0}' ORDER BY [Current Price] DESC", itemRow["Item Number"]);
                //    foreach (DataRow row in items.Rows)
                //    {

                //        var cust = Db.ExecuteDataRow("SELECT * FROM tblCustomers WHERE [Customer Number] = '{0}'", row["Customer Number"]);
                //        if (cust != null)
                //        {
                //            Db.NonQuery("INSERT INTO tblByItem_Report ([Item Number], [Item Description], [Customer Number], [Customer Name], [Current Price], [InDistrGroup],[QuoteDate],[Last12MonthQTY]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}','{7}')",
                //                row["Item Number"],
                //                row["Item Description"].ToString().Replace("'", "''"),
                //                row["Customer Number"],
                //                cust["Customer Bill Name"].ToString().Replace("'", "''"),
                //                row["Current Price"],
                //                " ",
                //                row["QuoteDate"],
                //                row["Last12MonthQTY"]
                //            );
                //        }
                //    }
                //}
                Cursor.Current = Cursors.Default;
                ReportViewers.frmByItem_Viewer frm = new ReportViewers.frmByItem_Viewer();
                frm.Text = "All Items Report";
                frm.Show();
            }

            //-------------------------------------------------------------------------------
            if (rdoPrintByCustFamily.Checked)
            {
                //var items = Db.ExecuteDataTable("Select [P.Item Number],[P.Customer Number], [P.Current Price], [I.Family], [I.Item Description], [QuoteDate], [Last12MonthQTY] From tblPricing P left join tblItems I on P.[Item Number] = I.[Item number] where P.[Customer Number] = '{0}' and I.[Family] = '{1}' ", cmbCust1.SelectedValue, cmbFamily1.SelectedValue);

                //foreach (DataRow row in items.Rows)
                //{

                //    var cust = Db.ExecuteDataRow("SELECT [Customer Bill Name] FROM tblCustomers WHERE [Customer Number] = '{0}'", row["P.Customer Number"]);
                //    if (cust != null)
                //    {
                //        Db.NonQuery("INSERT INTO tblByItem_Report ([Item Number], [Item Description], [Customer Number], [Customer Name], [Current Price], [InDistrGroup],[QuoteDate],[Last12MonthQTY]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}','{7}')",
                //            row["P.Item Number"],
                //            row["I.Item Description"].ToString().Replace("'", "''"),
                //            row["P.Customer Number"],
                //            cust["Customer Bill Name"].ToString().Replace("'", "''"),
                //            row["P.Current Price"],
                //            row["I.Family"],
                //            row["QuoteDate"],
                //            row["Last12MonthQTY"]

                //            );
                //    }
                //}

                Db.NonQuery("INSERT INTO tblByItem_Report ( [Item Number], [Item Description], [Customer Name], [Customer Number], [Current Price], InDistrGroup, QuoteDate, Last12MonthQTY ) SELECT tblPricing.[item Number], tblPricing.[item description], tblCustomers.[Customer Bill Name], tblCustomers.[Customer Number], tblPricing.[Current Price], '' AS InDistrGroup, tblPricing.QuoteDate, tblPricing.Last12MonthQty FROM (tblPricing INNER JOIN tblItems ON tblPricing.[Item Number] = tblItems.[Item Number])  INNER JOIN tblCustomers ON tblPricing.[Customer Number] = tblCustomers.[Customer Number] where tblCustomers.[Customer Number] = '{0}' and tblItems.[Family] = '{1}' ", cmbCust1.SelectedValue, cmbFamily1.SelectedValue);
              
                Cursor.Current = Cursors.Default;
                ReportViewers.frmItemsByCustFamily_Viewer frm = new ReportViewers.frmItemsByCustFamily_Viewer();
                frm.Show();
            }
        }
        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        private void btnViewReport2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var custPricings = Db.ExecuteDataTable("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}'", cmbCust.SelectedValue);
            Db.NonQuery("DELETE FROM tblByCustomer_Report");

            for (int i = 0; i < custPricings.Rows.Count; i++)
            {
                decimal ASRCurrPriceDec = 0;
                decimal ASTCurrPriceDec = 0;
                var ASRCurrPrice = Db.ExecuteDataRow("SELECT TOP 1 [Current Price] FROM tblPricing WHERE [Customer Number] = 'ASR' AND [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);
                var ASTCurrPrice = Db.ExecuteDataRow("SELECT TOP 1 [Current Price] FROM tblPricing WHERE [Customer Number] = 'AST' AND [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);

                if (ASRCurrPrice != null)
                    ASRCurrPriceDec = (decimal)ASRCurrPrice[0];
                if (ASTCurrPrice != null)
                    ASTCurrPriceDec = (decimal)ASTCurrPrice[0];

                var cust = Db.ExecuteDataRow("SELECT * FROM tblCustomers WHERE [Customer Number] = '{0}'", custPricings.Rows[i]["Customer Number"]);

                Db.NonQuery("INSERT INTO tblByCustomer_Report ([Customer Number], [Customer Name], [Item Number], [Item Description], [Current Price], [Customer Item Number], [IsNew], [ASR Current Price], [AST Current Price]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                    custPricings.Rows[i]["Customer Number"],
                    cust["Customer Bill Name"].ToString().Replace("'", "''"),
                    custPricings.Rows[i]["Item Number"],
                    custPricings.Rows[i]["Item Description"].ToString().Replace("'", "''"),
                    custPricings.Rows[i]["Current Price"],
                    custPricings.Rows[i]["Customer Item Number"],
                    custPricings.Rows[i]["IsNew"],
                    ASRCurrPriceDec,
                    ASTCurrPriceDec
                    );
            }
            Cursor.Current = Cursors.Default;
            //        frmReportViewer frm = new frmReportViewer();
            frmReport2_all_Viewer frm = new frmReport2_all_Viewer();
            frm.Show();

        }
        //------------------------------------------------------------------------------------------------
        private void btnViewReport3_Click(object sender, EventArgs e)
        {
            if (rdoSwing2.Checked)
            {
                Cursor.Current = Cursors.WaitCursor;
                Db.NonQuery("DELETE FROM tblBySwing_Report");

                string a = cmbSwing.SelectedText.ToString();
                string b = cmbSwing.Text;

                var cutomers = Db.ExecuteDataTable("SELECT * FROM tblCustomers WHERE [Swing Number] = '{0}'", cmbSwing.SelectedValue);
                foreach (DataRow custRow in cutomers.Rows)
                {
                    Db.NonQuery("INSERT INTO tblBySwing_Report ([Swing Number], [Swing Name], [Customer Number], [Customer Name]) VALUES ('{0}', '{1}', '{2}', '{3}')",
                        cmbSwing.SelectedValue,
                        cmbSwing.Text,
                        custRow["Customer Number"],
                        custRow["Customer Bill Name"].ToString().Replace("'", "''")
                        );
                }
                Cursor.Current = Cursors.Default;
                ReportViewers.frmBySwing_Report_Viewer frm = new ReportViewers.frmBySwing_Report_Viewer();
                frm.Show();

            }
            else
            {
                MessageBox.Show("Please, press the Swing radio button first.");
            }

        }

        //----------------------------------------------------------------------------------------------

        private void btnViewReport4_Click(object sender, EventArgs e)
        {
            if (rdoCompareCust.Checked)
            {
                Db.NonQuery("DELETE FROM tblByCustomerComparison_Report");

                var custFrom = Db.ExecuteDataRow("SELECT * FROM tblCustomers WHERE [Customer Number] = '{0}'", cmbCustFrom.SelectedValue);
                var custTo = Db.ExecuteDataRow("SELECT * FROM tblCustomers WHERE [Customer Number] = '{0}'", cmbCustTo.SelectedValue);

                var itemsFrom = Db.ExecuteDataTable("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}'", custFrom["Customer Number"]);
                var itemsTo = Db.ExecuteDataTable("SELECT * FROM tblPricing WHERE [Customer Number] = '{0}'", custTo["Customer Number"]);

                foreach (DataRow itemFromRow in itemsFrom.Rows)
                {
                    var leadItem = itemsTo.AsEnumerable().Where(lt => lt.Field<string>("Item Number").Equals(itemFromRow["Item Number"])).SingleOrDefault();
                    if (leadItem == null)
                    {
                        Db.NonQuery("INSERT INTO tblByCustomerComparison_Report ([Customer1 Number], [Customer1 Name], [Item Number], [Item Description], [Current Price1], [Customer2 Number], [Customer2 Name], [Current Price2]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')",
                            custFrom["Customer Number"],
                            custFrom["Customer Bill Name"],
                            itemFromRow["Item Number"],
                            itemFromRow["Item Description"].ToString().Replace("'", "''"),
                            itemFromRow["Current Price"],
                            custTo["Customer Number"],
                            custTo["Customer Bill Name"],
                            //    "Doesn’t Buy this Item"
                            0
                            );
                    }
                    else
                    {
                        Db.NonQuery("INSERT INTO tblByCustomerComparison_Report ([Customer1 Number], [Customer1 Name], [Item Number], [Item Description], [Current Price1], [Customer2 Number], [Customer2 Name], [Current Price2]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')",
                            custFrom["Customer Number"],
                            custFrom["Customer Bill Name"],
                            itemFromRow["Item Number"],
                            itemFromRow["Item Description"].ToString().Replace("'", "''"),
                            itemFromRow["Current Price"],
                            custTo["Customer Number"],
                            custTo["Customer Bill Name"],
                            leadItem["Current Price"]
                            );


                    }
                }

                frmByCustomer_Comparison_Report_Viewer frm = new frmByCustomer_Comparison_Report_Viewer();
                frm.Show();
            }
            else
            {
                //    NOTES:  "chkAll" not active for now !!!!!!!
                // if (rdoCompareItems.Checked && chkAll.Checked)
                if (rdoCompareItems.Checked)
                {

                    Db.NonQuery("DELETE FROM tblByItemComparison_Report");
                    var itemPricing = Db.ExecuteDataRow("SELECT TOP 1 * FROM tblPricing WHERE [Item Number] = '{0}'", cmbCompItem.SelectedValue);

                    if (itemPricing == null)
                    {
                        MessageBox.Show("The Item you're comparing to is not exist in Pricing data Base.");
                    }
                    else
                    {
                        var cust = Db.ExecuteDataRow("SELECT * FROM tblCustomers WHERE [Customer Number] = '{0}'", itemPricing["Customer Number"]);
                        Db.NonQuery("INSERT INTO tblByItemComparison_Report ([Item Number], [Item Description], [Customer Number], [Customer Name], [Current Price]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                                itemPricing["Item Number"],
                                itemPricing["Item Description"],
                                cust["Customer Number"],
                                cust["Customer Bill Name"],
                                itemPricing["Current Price"]);


                        ReportViewers.frmByItem_Comparison_Report_Viewer frm = new ReportViewers.frmByItem_Comparison_Report_Viewer();
                        frm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Please, select Compare Customer or Compare Items radio Button");
                }
            }
        }
        //--------------------------------------------------------------------------

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //----------------------------------------------------------------------------------

        private void btnViewReport5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var custPricings = Db.ExecuteDataTable("SELECT [Customer Number], [Item Number], [Current Price], [Customer Item Number],[IsNew] FROM tblPricing WHERE [Customer Number] = '{0}' and [Item Number] between '{1}' and '{2}'", cmbCust5.SelectedValue, cmbFrom5.SelectedValue, cmbTo5.SelectedValue);

            Db.NonQuery("DELETE FROM tblByCustomer_Report");

            for (int i = 0; i < custPricings.Rows.Count; i++)
            {
                decimal ASRCurrPriceDec = 0;
                decimal ASTCurrPriceDec = 0;
                var ASRCurrPrice = Db.ExecuteDataRow("SELECT TOP 1 [Current Price] FROM tblPricing WHERE [Customer Number] = 'ASR' AND [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);
                var ASTCurrPrice = Db.ExecuteDataRow("SELECT TOP 1 [Current Price] FROM tblPricing WHERE [Customer Number] = 'AST' AND [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);

                if (ASRCurrPrice != null)
                    ASRCurrPriceDec = (decimal)ASRCurrPrice[0];
                if (ASTCurrPrice != null)
                    ASTCurrPriceDec = (decimal)ASTCurrPrice[0];

                //            var cust = Db.ExecuteDataRow("SELECT * FROM tblCustomers WHERE [Customer Number] = '{0}'", custPricings.Rows[i]["Customer Number"]);
                var cust = Db.ExecuteDataRow("SELECT [Customer Bill Name] FROM tblCustomers WHERE [Customer Number] = '{0}'", custPricings.Rows[i]["Customer Number"]);
                var itemDscr = Db.ExecuteDataRow("SELECT [Item Description] FROM tblItems WHERE [Item Number] = '{0}'", custPricings.Rows[i]["Item Number"]);

                Db.NonQuery("INSERT INTO tblByCustomer_Report ([Customer Number], [Customer Name], [Item Number], [Item Description], [Current Price], [Customer Item Number], [IsNew], [ASR Current Price], [AST Current Price]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                    custPricings.Rows[i]["Customer Number"],
                    cust["Customer Bill Name"].ToString().Replace("'", "''"),
                    custPricings.Rows[i]["Item Number"],
                    itemDscr["Item Description"].ToString().Replace("'", "''"),
                    custPricings.Rows[i]["Current Price"],
                    custPricings.Rows[i]["Customer Item Number"],
                    custPricings.Rows[i]["IsNew"],
                    ASRCurrPriceDec,
                    ASTCurrPriceDec
                    );
            }
            Cursor.Current = Cursors.Default;
            frmReport2_all_Viewer frm = new frmReport2_all_Viewer();
            frm.Show();

        }


        //--------------------------------------------------------------------------------------

    }
}
