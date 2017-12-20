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
        public String currentCleanXmlPath;
        private List<RevitValue> revits;

        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            if (Program.IsEnergy)
            {
                currentCleanXmlPath = "params_clean_e.maif";
                currentXmlPath = currentCleanXmlPath;
            }
            else
            {
                currentCleanXmlPath = "params_clean.maif";
                currentXmlPath = currentCleanXmlPath;
            }
            
            var imagePath = (Program.IsEnergy) ? "main1.png" : "main0.png";

            toolStripContainer3.ContentPanel.BackgroundImage = Image.FromFile("Images\\" + imagePath);
            toolStripContainer3.ContentPanel.BackgroundImageLayout = ImageLayout.Center;

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
            //Import from external XML, no encryption
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
            //Open from file
            Stream myStream = null;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open import file";
            theDialog.Filter = "XML files|*.xml; *.maif";
            theDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentXmlPath = theDialog.FileName;
                    if ((myStream = theDialog.OpenFile()) != null)
                    {
                        if (currentXmlPath.EndsWith(".maif"))
                        {
                            string xml = SecurityClass.GetFileData(currentXmlPath);
                            using (TextReader reader = new StringReader(xml))
                            {
                                var groups = (List<Group>)(new XmlSerializer(typeof(List<Group>), Utilities.xRoot)).Deserialize(reader);

                                RevitImport.MapParamsFromRevit(revits, groups);
                                CalcForm t = new CalcForm(groups);
                                t.Show(this);

                            }
                        }
                        else
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //run 
            string path = Directory.GetCurrentDirectory();

            var myFile = new DirectoryInfo(path).GetFiles().Where(x => x.Name.IndexOf("params_") >= 0)
             .OrderByDescending(f => f.LastWriteTime)
             .FirstOrDefault();

            if (myFile != null)
                currentXmlPath = myFile.Name;

            var groups = Utilities.GetParamsFromXMLWithEncryption(currentXmlPath);
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
        private StringBuilder _pressedKeys = new StringBuilder();
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            var letter = (char)e.KeyChar;

            if (!char.IsLetterOrDigit(letter))
            {
                return;
            }

            _pressedKeys.Append(letter);

            if (_pressedKeys.ToString().ToLower() == "adm")
            {
                _pressedKeys.Clear();
                AdminForm f = new AdminForm();
                f.Show();
            }
            if (_pressedKeys.ToString().Count() > 3) _pressedKeys.Clear();
        }

    }
}

