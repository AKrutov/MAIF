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
    public partial class CalcForm : Form
    {
        List<Group> controllGroups;
        List<GroupBox> paramGroups = new List<GroupBox>();
        int currentPanel = 0;
        int maxWidth = 400;

        public CalcForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //String fileName = ExportHelper.Generate();

            ResultForm f = new ResultForm();
            f.Show();
            LogHelper h = new LogHelper(true);
            h.Info("Запущен расчет пользователем " + System.Security.Principal.WindowsIdentity.GetCurrent().Name); 
            h.Info("Выполнен расчет пользователем " + Environment.UserName);

        }

        private void CalcForm_Load(object sender, EventArgs e)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "root";
            xRoot.IsNullable = true;

            mainControlPanel.Width = mainControlPanel.Parent.Width;
            mainControlPanel.Height = mainControlPanel.Parent.Height - 100;

            using (StreamReader reader = new StreamReader("params.xml"))
            {
                this.controllGroups = (List<Group>)(new XmlSerializer(typeof(List<Group>), xRoot)).Deserialize(reader);
            }

            
            foreach (var item in this.controllGroups)
            {
                ControlGroup gBox = new ControlGroup(item);
                this.paramGroups.Add(gBox.asControl());

                this.maxWidth = (gBox.MaxWidth > this.maxWidth) ? gBox.MaxWidth : this.maxWidth;
            }

            
            mainControlPanel.Controls.Add(this.paramGroups[this.currentPanel]);

            foreach (GroupBox item in this.paramGroups)
            {
                item.Width = this.maxWidth;    
            }

            mainControlPanel.Width = this.maxWidth; // Панель
            mainControlPanel.Parent.Width = this.maxWidth + 25; // Форма
            buttonPanel.Controls[2].Left = mainControlPanel.Parent.Width - 186; //Двигаем кнопку
            
        }

        private Control[] getControls(Group paramsGroup)
        {
            IAbstractControll[] _controls = new IAbstractControll[paramsGroup.Params.Count];
            return (Control[])_controls;
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            if (this.currentPanel < this.paramGroups.Count-1)
            {
                this.currentPanel = this.currentPanel + 1;

                mainControlPanel.Controls.Clear();
                mainControlPanel.Controls.Add(this.paramGroups[this.currentPanel]);

            }
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            if(this.currentPanel > 0)
            {
                this.currentPanel = this.currentPanel - 1;

                mainControlPanel.Controls.Clear();
                mainControlPanel.Controls.Add(this.paramGroups[this.currentPanel]);
                
            }
        }

        private void calculateBtn_Click(object sender, EventArgs e)
        {

        }
    }  
}
