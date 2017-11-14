using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAIF
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
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
            ImportForm t = new ImportForm();
            t.Show(this);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SettingsForm t = new SettingsForm();
            t.Show(this);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            CalcForm t = new CalcForm();
            t.Show(this);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            HistoryForm t = new HistoryForm();
            t.Show(this);
        }
    }
}
