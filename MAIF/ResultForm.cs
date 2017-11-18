using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAIF
{
    public partial class ResultForm : Form
    {
        String currentTemplateName = "template1.html";
        //String fileName;
        Dictionary<string, string> values;
        String htmlTemplateName;

        public ResultForm(List<Param> paramList)
        {
            InitializeComponent();
        
            values = Utilities.ConvertParamsToValues(paramList);
            values = SettingsHelper.ProcessValues(values);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //string name = ExportHelper.FillTemplateDocx(values);

            string path = Directory.GetCurrentDirectory();
            //string name = ExportHelper.FillTemplateDocx(values);
            //// var result = ExportHelper.PdfSharpConvert(name);
            ExportHelper.ConvertHtml2Docx(path + "\\" + htmlTemplateName, path + "\\" + htmlTemplateName.Replace(".htm", ".docx"));
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            htmlTemplateName = ExportHelper.FillTemplateHtml(values, currentTemplateName);
            this.webBrowser1.Url = new Uri(String.Format("file:///{0}/"+ htmlTemplateName, curDir));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            //string name = ExportHelper.FillTemplateDocx(values);
            // var result = ExportHelper.PdfSharpConvert(name);
            ExportHelper.ConvertWord2PDF(path + "\\" + htmlTemplateName, path+ "\\" + htmlTemplateName.Replace(".htm",".pdf"));
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ((WebBrowser)webBrowser1).ShowPrintPreviewDialog();            
        }
    }
}
