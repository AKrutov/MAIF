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
        //String fileName;
        Dictionary<string, string> values;
        public ResultForm()
        {
            InitializeComponent();
            //this.fileName = fileName;
            values = new Dictionary<string, string>();
            values.Add("test1", "1");
            values.Add("test2", "2");
            values.Add("test3", "test");
            values.Add("test4", "%test1%+%test2%");
            values = SettingsHelper.ProcessValues(values);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string name = ExportHelper.FillTemplateDocx(values);
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            string name = ExportHelper.FillTemplateHtml(values);
            this.webBrowser1.Url = new Uri(String.Format("file:///{0}/"+name, curDir));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            string name = ExportHelper.FillTemplateDocx(values);
            // var result = ExportHelper.PdfSharpConvert(name);
            ExportHelper.ConvertWord2PDF(path + "\\" + name, path+ "\\" + name.Replace(".docx",".pdf"));
        }
    }
}
