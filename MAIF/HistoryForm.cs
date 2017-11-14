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
    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"MAIF.log");
            while ((line = file.ReadLine()) != null)
            {
                if (line.IndexOf("[INFO]") >= 0){
                    listBox1.Items.Add(line);
                }
                counter++;
            }

            file.Close();
            //System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
           // System.Console.ReadLine();
        }
    }
}
