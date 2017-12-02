using MAIF.classes;
using MAIF.ControllsClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;


namespace MAIF
{
    public partial class MainForm : Form
    {
        public String currentXmlPath;
        private List<RevitValue> revits;
       
        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            currentXmlPath = "params.xml";

            
            //var x = Utilities.ConvertPow("Math.Pow(1,1)");            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStrip1.Renderer = new CustomMenuRenderer();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open import file";
            theDialog.Filter = "XML files|*.xml";
            theDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentXmlPath = theDialog.FileName;
                    if ((myStream = theDialog.OpenFile()) != null)
                    {
                        revits = RevitImport.GetValuesFromXml(currentXmlPath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open import file";
            theDialog.Filter = "XML files|*.xml";
            theDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentXmlPath = theDialog.FileName;
                    if ((myStream = theDialog.OpenFile()) != null)
                    {
                        using (StreamReader reader = new StreamReader(myStream))
                        {
                            var groups = (List<Group>)(new XmlSerializer(typeof(List<Group>), Utilities.xRoot)).Deserialize(reader);

                            RevitImport.MapParamsFromRevit(revits, groups);
                            CalcForm t = new CalcForm(groups);
                            t.Show(this);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();

            var myFile = new DirectoryInfo(path).GetFiles().Where(x => x.Name.IndexOf("params_") >= 0)
             .OrderByDescending(f => f.LastWriteTime)
             .FirstOrDefault();

            if (myFile != null)
                currentXmlPath = myFile.Name;

            var groups = Utilities.GetParamsFromXML(currentXmlPath);
            if (groups.Count() > 0)
            {

                RevitImport.MapParamsFromRevit(revits, groups);
                CalcForm t = new CalcForm(groups);
                t.Show(this);
            }

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            HistoryForm t = new HistoryForm();
            t.Show(this);
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {

        }
    }
}
