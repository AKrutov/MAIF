using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAIF
{
    public partial class ResultForm : Form
    {
        String currentTemplateName = (Program.IsEnergy) ? "template3.htm" : "template2.htm";
        Dictionary<string, string> values;
        List<Group> initialGroups;
        String htmlTemplateName;

        public ResultForm(List<Group> groups, List<Param> paramList)
        {
            InitializeComponent();
           // if(Program.IsEnergy)

            initialGroups = groups;
            values = Utilities.ConvertParamsToValues(paramList);
            values = SettingsHelper.ProcessValues(values);

            if(Program.IsEnergy)
            {
                if(values.ContainsKey("common_energy_class")&& values.ContainsKey("common_energy_base_r"))
                    values["common_energy_class"] = EnergyClass.GetEnergyClass(Double.Parse(values["common_energy_base_r"]));
                if (values.ContainsKey("e_picture2") && values.ContainsKey("common_energy_class"))
                    values["e_picture2"] = "Images\\"+ values["common_energy_class"].ToLower() + ".png";
                if (values.ContainsKey("e_picture1") && values.ContainsKey("common_energy_class"))
                    values["e_picture1"] = "Images\\e_class.png";
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            String docxFileTmpName = path + "\\" + htmlTemplateName.Replace(".htm", ".docx");
            ExportHelper.ConvertHtml2Docx(path + "\\" + htmlTemplateName, docxFileTmpName);

            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "docx files (*.docx)|*.docx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Title = "Save word Files";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "docx";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    string newDirectory = saveFileDialog1.FileName;
                    System.IO.File.Copy(docxFileTmpName, newDirectory, true);

                }
            }
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            if (initialGroups.Count > 0)
            {
                if (initialGroups[0].Params.Any(x => x.Name == "building_picture"))
                {
                    values["default_picture"] = initialGroups[0].Params.FirstOrDefault(x => x.Name == "building_picture").Value;
                }

            }
            RefreshTemplate();
        }

        public void RefreshTemplate()
        {
            string curDir = Directory.GetCurrentDirectory();
            htmlTemplateName = ExportHelper.FillTemplateHtml(values, currentTemplateName);
            this.webBrowser1.Url = new Uri(String.Format("file:///{0}/" + htmlTemplateName, curDir));
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            String pdfFileTmpName = path + "\\" + htmlTemplateName.Replace(".htm", ".pdf");
            ExportHelper.ConvertWord2PDF(path + "\\" + htmlTemplateName, pdfFileTmpName);

            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Title = "Save PDF Files";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    string newDirectory = saveFileDialog1.FileName;
                    System.IO.File.Copy(pdfFileTmpName, newDirectory, true);

                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ((WebBrowser)webBrowser1).ShowPrintPreviewDialog();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (currentTemplateName == "template1.htm")
                currentTemplateName = "template2.htm";
            else
            {
                if (currentTemplateName == "template2.htm")
                    currentTemplateName = "template1.htm";
                else
                {
                    if (currentTemplateName == "template3.htm")
                        currentTemplateName = "template4.htm";
                    else
                        if (currentTemplateName == "template4.htm")
                            currentTemplateName = "template3.htm";
                }
            }

            string curDir = Directory.GetCurrentDirectory();
            htmlTemplateName = ExportHelper.FillTemplateHtml(values, currentTemplateName);
            this.webBrowser1.Url = new Uri(String.Format("file:///{0}/" + htmlTemplateName, curDir));
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            Stream myStream;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg; *.jpeg; *.jpe; *.png; *.PNG; *.JPG | All files(*.*) | *.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Select image file";
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string newFileName = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf('\\')) + ".png";
                string newFilePath = path + "\\Images" + newFileName;
                using (var image = Image.FromFile(openFileDialog.FileName))
                using (var newImage = Utilities.ScaleImage(image, 250, 200))
                {
                    newImage.Save(newFilePath, ImageFormat.Png);
                }

                // System.IO.File.Copy(openFileDialog.FileName, path + "\\Images\\" + newFileName, true);
                values["default_picture"] = newFilePath;

                if (initialGroups.Count > 0)
                {
                    if (initialGroups[0].Params.Any(x => x.Name == "building_picture"))
                    {
                        initialGroups[0].Params.FirstOrDefault(x => x.Name == "building_picture").Value = newFilePath;
                    }
                    else
                        initialGroups[0].Params.Add(new Param() { IsHidden = "1", Name = "building_picture", Value = newFilePath, Desc = "Фото объекта" });
                }

                System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["MainForm"];

                string currentXmlPath = ((MainForm)f).currentXmlPath;
                Utilities.SaveParamsToXML(initialGroups, currentXmlPath);

                RefreshTemplate();

            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

            string path = Directory.GetCurrentDirectory();
            Stream myStream;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "XML files (*.xml) | *.XML; *.xml; | All files(*.*) | *.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Title = "Save XML Files";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();                 
                    Utilities.SaveParamsToXML(initialGroups, saveFileDialog1.FileName, true);
                }
            }

        }
    }
}

