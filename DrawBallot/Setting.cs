using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;

namespace DrawBallot
{
    public partial class Setting : Form
    {
        Form _form = null;
        //SQLiteConnection mConn;
        //SQLiteDataAdapter mAdapter;
        DataTable mDataTable = new DataTable();
        int Mode = 0;
        public BindingList<string> listPrize = new BindingList<string>();
        public Setting()
        {

            InitializeComponent();
            boxID.Visible = false;
            boxFN.Visible = false;
            boxLN.Visible = false;
            bCon1.Visible = false;
            bCan1.Visible = false;

            boxPrize.Visible = false;
            bCon2.Visible = false;
            bCan2.Visible = false;

            listPrize.Add("First Place");
            listPrize.Add("Second Place");
            listPrize.Add("Third Place");
            listBox1.DataSource = listPrize;
            

            mDataTable.Columns.Add("ID", typeof(string));
            mDataTable.Columns.Add("First Name", typeof(string));
            mDataTable.Columns.Add("Last Name", typeof(string));
            mDataTable.Columns.Add("isDrawed", typeof(bool));
            mDataTable.Columns["isDrawed"].DefaultValue = false;
            dataGridView1.DataSource = mDataTable;

        }

        private void Setting_Load(object sender, EventArgs e)
        {
            //string mDbPath = Application.StartupPath + "/DatabaseFinal.sqlite";
            //mConn = new SQLiteConnection("Data Source=" + mDbPath);
            //mConn.Open();
            //mAdapter = new SQLiteDataAdapter("SELECT * FROM [PARTICIPANT]", mConn);
            //mAdapter.Fill(mDataTable);

            //new SQLiteCommandBuilder(mAdapter);
            dataGridView1.DataSource = mDataTable;
        }

        /*public static List<string> ExcelReader(string fileLocation)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileLocation);
            xlWorkbook.SaveAs( fileLocation + ".csv", Excel.XlFileFormat.xlCSVWindows);
            xlWorkbook.Close(true);
            xlApp.Quit();
            List<string> valueList = null;
            using (StreamReader sr = new StreamReader(fileLocation + ".csv"))
            {
                string content = sr.ReadToEnd();
                valueList = new List<string>(
                    content.Split(
                        new string[] { "\r\n" },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                );
            }
            new FileInfo(fileLocation + ".csv").Delete();
            return valueList;
        }*/

        private void bLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel File (*.xlsx)|*.xlsx|07-2003 Excel File (*.xls)|*.xls|all file (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                

                mDataTable.Clear();

                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(ofd.FileName);
                Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                int RowCount = xlRange.Rows.Count;
                int ColCount = xlRange.Columns.Count;

                /*object[,] data = xlRange.Value2;

                // Create new Column in DataTable
                for (int cCnt = 1; cCnt <= xlRange.Columns.Count; cCnt++)
                {
                    textBox3.Text = cCnt.ToString();

                    var Column = new DataColumn();
                    Column.DataType = System.Type.GetType("System.String");
                    Column.ColumnName = xlRange[1,cCnt].ToString();
                    mDataTable.Columns.Add(Column);

                    // Create row for Data Table
                    for (int rCnt = 2; rCnt <= xlRange.Rows.Count; rCnt++)
                    {
                        textBox2.Text = rCnt.ToString();

                        string CellVal = String.Empty;
                        try
                        {
                            CellVal = Convert.ToString((data[rCnt, cCnt]));
                        }
                        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                        {
                            
                        }

                        DataRow Row;

                        // Add to the DataTable
                        if (cCnt == 1)
                        {

                            Row = mDataTable.NewRow();
                            Row[cCnt.ToString()] = CellVal;
                            mDataTable.Rows.Add(Row);
                        }
                        else
                        {

                            Row = mDataTable.Rows[rCnt];
                            Row[cCnt.ToString()] = CellVal;

                        }
                    }
                }*/
                string tempID = null;
                string tempFirstName = null;
                string tempLastName = null;
                
                for (int i = 1; i <= RowCount; i++)
                {
                    tempID = xlRange.Cells[i, 1].Value2.ToString();
                    tempFirstName = xlRange.Cells[i, 2].Value2.ToString();
                    tempLastName = xlRange.Cells[i, 3].Value2.ToString();
                    //SQLiteCommand sqlCmd = new SQLiteCommand("Insert into participant (ID,FIRSTNAME,LASTNAME) values (@ID,@FN,@LN);", mConn);
                    //SQLiteParameter p = new SQLiteParameter("@ID", System.Data.DbType.String);
                    //p.Value = tempID;
                    //sqlCmd.Parameters.Add(p);
                    //p = new SQLiteParameter("@FN", System.Data.DbType.String);
                    //p.Value = tempFirstName;
                    //sqlCmd.Parameters.Add(p);
                    //p = new SQLiteParameter("@LN", System.Data.DbType.String);
                    //p.Value = tempLastName;
                    //sqlCmd.Parameters.Add(p);

                    //try
                    //{
                    //sqlCmd.ExecuteNonQuery();
                    //}
                    //catch(Exception ex)
                    //{
                    // MessageBox.Show(ex.Message);
                    //}

                    //mAdapter = new SQLiteDataAdapter("SELECT * FROM [PARTICIPANT]", mConn);
                    //DataTable mTable = new DataTable(); // Don't forget initialize!
                    //mAdapter.Fill(mTable);
                    //new SQLiteCommandBuilder(mAdapter);
                    DataRow newrow = mDataTable.NewRow();
                    newrow[0] = tempID;
                    newrow[1] = tempFirstName;
                    newrow[2] = tempLastName;
                    mDataTable.Rows.Add(newrow);
                }
                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad

                //release com objects to fully kill excel process from running in the background
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                //close and release
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);

                //quit and release
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            boxID.Text = null;
            boxFN.Text = null;
            boxLN.Text = null;
            boxID.Visible = true;
            boxFN.Visible = true;
            boxLN.Visible = true;
            bAdd1.Enabled = false;
            bDel1.Enabled = false;
            bMod1.Enabled = false;
            bLoad1.Enabled = false;
            bCon1.Visible = true;
            bCan1.Visible = true;
            Mode = 1;
        }

        private void bDel_Click(object sender, EventArgs e)
        {
            int selectedRow;
            selectedRow = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows.RemoveAt(selectedRow);
        }

        private void bMod_Click(object sender, EventArgs e)
        {
            int selectedRow;
            selectedRow = dataGridView1.CurrentCell.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRow];
            boxID.Text = row.Cells["ID"].Value.ToString();
            boxFN.Text = row.Cells["First Name"].Value.ToString();
            boxLN.Text = row.Cells["Last Name"].Value.ToString();
            boxID.Visible = true;
            boxFN.Visible = true;
            boxLN.Visible = true;
            bAdd1.Enabled = false;
            bDel1.Enabled = false;
            bMod1.Enabled = false;
            bLoad1.Enabled = false;
            bCon1.Visible = true;
            bCan1.Visible = true;
            Mode = 2;
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            mDataTable.Clear();
        }

        private void bCon_Click(object sender, EventArgs e)
        {
            if (Mode == 1)
            {
                var newrow = mDataTable.NewRow();
                newrow[0] = boxID.Text;
                newrow[1] = boxFN.Text;
                newrow[2] = boxLN.Text;
                mDataTable.Rows.Add(newrow);
            }
            else
            {
                int selectedRow;
                selectedRow = dataGridView1.CurrentCell.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                row.Cells[0].Value = boxID.Text;
                row.Cells[1].Value = boxFN.Text;
                row.Cells[2].Value = boxLN.Text;

                dataGridView1.EndEdit();
                ((DataRowView)dataGridView1.CurrentRow.DataBoundItem).EndEdit();
            }

            boxID.Visible = false;
            boxFN.Visible = false;
            boxLN.Visible = false;
            bAdd1.Enabled = true;
            bDel1.Enabled = true;
            bMod1.Enabled = true;
            bLoad1.Enabled = true;
            bCon1.Visible = false;
            bCan1.Visible = false;
            //mAdapter.Update(mDataTable);
        }

        private void bCan_Click(object sender, EventArgs e)
        {
            boxID.Visible = false;
            boxFN.Visible = false;
            boxLN.Visible = false;
            bAdd1.Enabled = true;
            bDel1.Enabled = true;
            bMod1.Enabled = true;
            bLoad1.Enabled = true;
            bCon1.Visible = false;
            bCan1.Visible = false;
            Mode = 0;
        }

        private void bAdd2_Click(object sender, EventArgs e)
        {
            
            bAdd2.Enabled = false;
            bDel2.Enabled = false;
            bMod2.Enabled = false;
            bReset2.Enabled = false;

            boxPrize.Visible = true;
            bCon2.Visible = true;
            bCan2.Visible = true;
            Mode = 1;
        }

        private void bMod2_Click(object sender, EventArgs e)
        {
            bAdd2.Enabled = false;
            bDel2.Enabled = false;
            bMod2.Enabled = false;
            bReset2.Enabled = false;

            boxPrize.Visible = true;
            bCon2.Visible = true;
            bCan2.Visible = true;
            Mode = 2;
        }

        private void bCon2_Click(object sender, EventArgs e)
        {
            if (Mode == 1)
            {
                listPrize.Add(boxPrize.Text);
            }
            else
            {
                listPrize[listBox1.SelectedIndex]= boxPrize.Text;
            }
            bAdd2.Enabled = true;
            bDel2.Enabled = true;
            bMod2.Enabled = true;
            bReset2.Enabled = true;

            boxPrize.Visible = false;
            bCon2.Visible = false;
            bCan2.Visible = false;
            Mode = 0;
        }

        private void bBrowsePic_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //try
            //{
              //  if (ofd.ShowDialog() == DialogResult.OK)
                //{
                  //  Bitmap image = new Bitmap(ofd.OpenFile());
                    
                //}
            //}
            //catch (Exception ex)
            //{
              //  MessageBox.Show(ex.Message);
            //}
        }

        private void bCan2_Click(object sender, EventArgs e)
        {
            bAdd2.Enabled = true;
            bDel2.Enabled = true;
            bMod2.Enabled = true;
            bReset2.Enabled = true;

            boxPrize.Visible = false;
            bCon2.Visible = false;
            bCan2.Visible = false;
            Mode = 0;
        }

        private void bReset2_Click(object sender, EventArgs e)
        {
            listPrize.Clear();
            listPrize.Add("First Place");
            listPrize.Add("Second Place");
            listPrize.Add("Third Place");
        }

        private void bUp_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            string temp = listPrize[n];
            if (n >0)
            {
                listPrize.RemoveAt(n);
                listPrize.Insert(n - 1, temp);
                listBox1.SetSelected(n - 1,true);
            }
        }

        private void bDown_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            string temp = listPrize[n];
            if (n < listPrize.Count - 1)
            {
                listPrize.RemoveAt(n);
                listPrize.Insert(n + 1, temp);
                listBox1.SetSelected(n + 1, true);
            }
        }

        private void bDel2_Click(object sender, EventArgs e)
        {
            listPrize.RemoveAt(listBox1.SelectedIndex);
        }

        public DataTable Table
        {
            get
            {
                return this.mDataTable;
            }
            set
            {
                this.mDataTable = value;
            }
        }
    }
}
