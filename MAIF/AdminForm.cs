﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MAIF
{
    public partial class AdminForm : Form
    {
        String currentXmlPath;
        public AdminForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Open from file
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open import file";
            theDialog.Filter = "XML files|*.xml; *.maif";
            theDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentXmlPath = theDialog.FileName;

                    if (currentXmlPath.EndsWith(".maif"))
                    {
                        string xml = SecurityClass.GetFileData(currentXmlPath);
                        richTextBox1.Text = xml;
                    }
                    else
                    {
                        richTextBox1.Text = File.ReadAllText(currentXmlPath);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool saveCurrent = true;
            if (String.IsNullOrWhiteSpace(currentXmlPath))
            {
                string path = Directory.GetCurrentDirectory();
                Stream myStream;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "MAIF files (*.maif) | *.MAIF; *.maif; | All files(*.*) | *.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Title = "Save MAIF Files";
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "maif";
                saveFileDialog1.OverwritePrompt = true;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentXmlPath = saveFileDialog1.FileName;
                    saveCurrent = false;
                }
            }

            using (StringWriter textWriter = new StringWriter())
            {
                SecurityClass.PutFileData(currentXmlPath, richTextBox1.Text);
                MessageBox.Show("Saved!");

                if (!saveCurrent) currentXmlPath = "";
            }
        }
    }
}