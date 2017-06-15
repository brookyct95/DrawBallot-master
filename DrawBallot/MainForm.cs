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
using System.Media;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using ClosedXML.Excel;
using System.Windows.Forms;

namespace DrawBallot
{
    public partial class MainForm : Form
    {
        DataForm dataForm = new DataForm();
        //SQLiteConnection mConn;
        //SQLiteDataAdapter mAdapter;
        //DataTable mTable = new DataTable();
        List<string> IDList = new List<string>();
        int index;
        bool drawPushed = false;
        Timer timer = new Timer();
        List<Label> idList = new List<Label>();
        List<Label> LNList = new List<Label>();
        List<Label> FNList = new List<Label>();
        Setting setting = new Setting();
        Winner winner = new Winner();
        public MainForm()
        {
            InitializeComponent();
            


        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            Stream str = Properties.Resources.DrumRoll;
            SoundPlayer player = new SoundPlayer(str);
            player.Load();
            if (!drawPushed)
            {
                IDList.Clear();
                player.PlayLooping();
                for (int i = 0; i < 10; i++)
                {
                    if (i < cbQuantity.SelectedIndex + 1)
                    {
                        idList[i].Visible = true;
                        FNList[i].Visible = true;
                        LNList[i].Visible = true;
                    }
                    else
                    {
                        idList[i].Visible = false;
                        FNList[i].Visible = false;
                        LNList[i].Visible = false;
                    }
                }
                //string mDbPath = Application.StartupPath + "/DatabaseFinal.sqlite";
                //mConn = new SQLiteConnection("Data Source=" + mDbPath);
                //mConn.Open();
                //SQLiteCommand sql = new SQLiteCommand("Select * From PARTICIPANT", mConn);
                //SQLiteDataReader reader = sql.ExecuteReader();

                //while (reader.Read())
                //{
                int n = setting.Table.Rows.Count;
                int m = setting.Table.Columns.Count;
                for (int i = 0; i < n; i++)
                {
                    if (!setting.Table.Rows[i].Field<bool>(m-1))
                    {
                        IDList.Add(setting.Table.Rows[i].Field<string>(0));
                    }
                }
                //}

                //
                IDList.Shuffle();
                index = 0;
                
                //

                timer.Interval = 150;
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
                drawButton.Text = "Stop";
                drawPushed = true;
            }
            else
            {
                player.Stop();
                timer.Stop();
                drawButton.Text = "Draw";
                drawPushed = false;
                int n = cbQuantity.SelectedIndex + 1;
                DataRow result = setting.Table.NewRow();
                for (int i = 0;i<n;i++)
                {
                    string cmd = setting.Table.Columns[0].Caption + " ='" + this.idList[i].Text + "'";
                    result = setting.Table.Select(cmd).FirstOrDefault();
                    result[3] = true;
                    DataRow newrow = winner.Table.NewRow();
                    newrow["ID"] = idList[i].Text;
                    newrow["First Name"] = FNList[i].Text;
                    newrow["Last Name"] = LNList[i].Text;
                    newrow["Prize"] = cbPrize.SelectedValue.ToString();
                    newrow["Date"] = DateTime.Now.ToString();
                    winner.Table.Rows.Add(newrow);
                }              
                //mConn.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataButton_Click(object sender, EventArgs e)
        {

        }

        void timer_Tick(Object Sender, EventArgs e)
        {
            DataRow result1 = setting.Table.NewRow(); 
                for (int i = 0; i < 10; i++)
                {
                    this.idList[i].Text = IDList[(index + i) % IDList.Count];

                    string cmd1 = setting.Table.Columns[0].Caption + "='" + this.idList[i].Text + "'";
                    result1 = setting.Table.Select(cmd1).FirstOrDefault();


                    //string strSql = "Select * From PARTICIPANT where ID =" + IDList[(index+i)%IDList.Count] + ";";
                    //SQLiteCommand sql = new SQLiteCommand(strSql, mConn);
                    //SQLiteDataReader reader = sql.ExecuteReader();
                    //while (reader.Read())

                    FNList[i].Text = result1[1].ToString();
                    LNList[i].Text = result1[2].ToString();

                    index++;
                    if (index >= IDList.Count)
                        index = 0;
                }
            
          
            
               
                
                   
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dataForm.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            setting.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            idList.Add(lID1);
            idList.Add(lID2);
            idList.Add(lID3);
            idList.Add(lID4);
            idList.Add(lID5);
            idList.Add(lID6);
            idList.Add(lID7);
            idList.Add(lID8);
            idList.Add(lID9);
            idList.Add(lID10);

            LNList.Add(lLN1);
            LNList.Add(lLN2);
            LNList.Add(lLN3);
            LNList.Add(lLN4);
            LNList.Add(lLN5);
            LNList.Add(lLN6);
            LNList.Add(lLN7);
            LNList.Add(lLN8);
            LNList.Add(lLN9);
            LNList.Add(lLN10);

            FNList.Add(lFN1);
            FNList.Add(lFN2);
            FNList.Add(lFN3);
            FNList.Add(lFN4);
            FNList.Add(lFN5);
            FNList.Add(lFN6);
            FNList.Add(lFN7);
            FNList.Add(lFN8);
            FNList.Add(lFN9);
            FNList.Add(lFN10);

            cbQuantity.SelectedIndex = 0;
            //
            


            for (int i = 0; i < 10; i++)
            {
                idList[i].Visible = false;
                FNList[i].Visible = false;
                LNList[i].Visible = false;
            }
            cbPrize.DataSource = setting.listPrize;
            //
            ToolStripControlHost host = new ToolStripControlHost(drawButton);
            ToolStripControlHost box1 = new ToolStripControlHost(cbQuantity);
            ToolStripControlHost box2 = new ToolStripControlHost(cbPrize);
            toolStrip1.Items.Add(host);
            toolStrip1.Items.Add(box1);
            toolStrip1.Items.Add(box2);
            //load exel file
            if (File.Exists("Participant.csv"))
            {
                setting.Table = Methods.ConvertCSVtoDataTable1("Participant.csv");
            }
            else
            { 
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Choose data source";
                ofd.Filter = "Excel File (*.xlsx)|*.xlsx|07-2003 Excel File (*.xls)|*.xls|all file (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    /*setting.Table.Clear();

                    Excel.Application xlApp = new Excel.Application();
                    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(ofd.FileName);
                    Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
                    Excel.Range xlRange = xlWorksheet.UsedRange;

                    int RowCount = xlRange.Rows.Count;
                    int ColCount = xlRange.Columns.Count;

                    string tempID = null;
                    string tempFirstName = null;
                    string tempLastName = null;

                    for (int i = 1; i <= RowCount; i++)
                    {
                        tempID = xlRange.Cells[i, 1].Value2.ToString();
                        tempFirstName = xlRange.Cells[i, 2].Value2.ToString();
                        tempLastName = xlRange.Cells[i, 3].Value2.ToString();
                        DataRow newrow = setting.Table.NewRow();
                        newrow[0] = tempID;
                        newrow[1] = tempFirstName;
                        newrow[2] = tempLastName;
                        setting.Table.Rows.Add(newrow);
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
                    Marshal.ReleaseComObject(xlApp);*/

                    File.WriteAllText("Participant.csv", Methods.ExcelToCSV(ofd.FileName, ';').ToString());
                    setting.Table = Methods.ConvertCSVtoDataTable1("Participant.csv");
                    MessageBox.Show("Loading Done");
                }   
            }
            if (File.Exists("Winner.csv"))
            {
                winner.Table = Methods.ConvertCSVtoDataTable2("Winner.csv");
            }
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            //history.ShowDialog();
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setting.ShowDialog();
        }


        
        public Image pBox1Image
        {
            get
            {
                return this.pictureBox1.Image;
            }
            set
            {
                this.pictureBox1.Image = value;
            }
        }

        private void winnerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            winner.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.WriteAllText("Participant.csv", Methods.DataTableToCSV(setting.Table,';').ToString());
            File.WriteAllText("Winner.csv", Methods.DataTableToCSV(winner.Table, ';').ToString());
        }
    }
}
