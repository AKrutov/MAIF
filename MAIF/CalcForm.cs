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
using System.Text.RegularExpressions;
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
        string picturePath = "";

        public CalcForm(List<Group> groups)
        {
            InitializeComponent();
            controllGroups = groups;
        }

        private void CalcForm_Load(object sender, EventArgs e)
        {
            //mainControlPanel.Width = mainControlPanel.Parent.Width;
            //mainControlPanel.Height = mainControlPanel.Parent.Height - 100;

            toolStripContainer1.ContentPanel.Width = toolStripContainer1.Parent.Width;
            toolStripContainer1.ContentPanel.Height = toolStripContainer1.Parent.Height - 80;

            foreach (var item in this.controllGroups)
            {
                ControlGroup gBox = new ControlGroup(item);
                this.paramGroups.Add(gBox.asControl());
                
                this.maxWidth = (gBox.MaxWidth > this.maxWidth) ? gBox.MaxWidth : this.maxWidth;
            }

            //mainControlPanel.Controls.Add(this.paramGroups[this.currentPanel]);
            toolStripContainer1.ContentPanel.Controls.Add(this.paramGroups[this.currentPanel]);

            foreach (ControlGroup item in this.paramGroups)
            {
                item.SetWidth(this.maxWidth);
            }

            //mainControlPanel.Width = this.maxWidth; // Панель
            //mainControlPanel.Parent.Width = this.maxWidth + 25; // Форма
            //buttonPanel.Controls[2].Left = mainControlPanel.Parent.Width - 186; //Двигаем кнопку

            toolStripContainer1.Width = this.maxWidth; // Панель
            toolStripContainer1.Parent.Width = this.maxWidth + 25; // Форма
            buttonPanel.Controls[2].Left = toolStripContainer1.Parent.Width - 186; //Двигаем кнопку

            initButtons(currentPanel);            
        }

        private Control[] getControls(Group paramsGroup)
        {
            IAbstractControll[] _controls = new IAbstractControll[paramsGroup.Params.Count];
            return (Control[])_controls;
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            if (this.currentPanel < this.paramGroups.Count - 1 && ((ControlGroup)this.paramGroups[this.currentPanel]).IsValid())
            {
                this.currentPanel = this.currentPanel + 1;
                initButtons(this.currentPanel);
                //mainControlPanel.Controls.Clear();
                //mainControlPanel.Controls.Add(this.paramGroups[this.currentPanel]);

                toolStripContainer1.ContentPanel.Controls.Clear();
                toolStripContainer1.ContentPanel.Controls.Add(this.paramGroups[this.currentPanel]);

                prevBtn.Enabled = true;
                if (this.currentPanel == this.paramGroups.Count - 1)
                    nextBtn.Enabled = false;
                else
                    nextBtn.Enabled = true;
            }
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            if (this.currentPanel > 0)
            {
                this.currentPanel = this.currentPanel - 1;
                initButtons(this.currentPanel);
                //mainControlPanel.Controls.Clear();
                //mainControlPanel.Controls.Add(this.paramGroups[this.currentPanel]);

                toolStripContainer1.ContentPanel.Controls.Clear();
                toolStripContainer1.ContentPanel.Controls.Add(this.paramGroups[this.currentPanel]);

                nextBtn.Enabled = true;

                if (this.currentPanel == 0)
                    prevBtn.Enabled = false;
                else
                    prevBtn.Enabled = true;

            }
        }

        private void calculateBtn_Click(object sender, EventArgs e)
        {
            LogHelper h = new LogHelper(true);
            h.Info("Запущен расчет пользователем " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);

            List<Param> allParams = new List<Param>();

            foreach (MAIF.ControllsClasses.ControlGroup group in this.paramGroups)
            {

                if (group.IsValid())
                {
                    allParams.AddRange(group.CurrentGroup.Params);
                }
#if DEBUG
                else
                    throw new Exception("Данные не валидны!");
#endif
            }
            
            System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["MainForm"];
            ((MainForm)f).currentXmlPath = Utilities.SaveParamsToXML(this.controllGroups);

            var resultForm = new ResultForm(this.controllGroups,allParams);
            resultForm.Show();
            h.Info("Выполнен расчет пользователем " + Environment.UserName);
        }

        private void toolStripButtonStep_Click(object sender, EventArgs e )
        {
            var buttonName = (ToolStripButton)sender;
            this.currentPanel = Int32.Parse(Regex.Match(buttonName.Name, @"\d+").Value) - 1;

            initButtons(this.currentPanel);

            toolStripContainer1.ContentPanel.Controls.Clear();
            toolStripContainer1.ContentPanel.Controls.Add(this.paramGroups[this.currentPanel]);

            nextBtn.Enabled = true;

            if (this.currentPanel == 0)
                prevBtn.Enabled = false;
            else
                prevBtn.Enabled = true;

            if (this.currentPanel == this.paramGroups.Count - 1)
                nextBtn.Enabled = false;
            else
                nextBtn.Enabled = true;
        }
        
        private void initButtons(int currentStepNumber)
        {
            var buttons = ((System.Windows.Forms.ToolStrip)toolStripContainer1.TopToolStripPanel.Controls[0]).Items;
            string path = Directory.GetCurrentDirectory();

            for (int i = 0; i <= currentStepNumber; i++)
            {
                buttons[i].Image = Image.FromFile(path + "\\Images\\" + (i + 1) + "-active.png");
                buttons[i].ImageAlign = ContentAlignment.MiddleCenter;
                buttons[i].ImageScaling = ToolStripItemImageScaling.None;
                buttons[i].Enabled = true;

                buttons[i].Click += new System.EventHandler(this.toolStripButtonStep_Click);
            }

            for (int i = currentStepNumber+1; i < buttons.Count; i++)
            {
                buttons[i].Image = Image.FromFile(path + "\\Images\\" + (i + 1) + "-disabled.png");
                buttons[i].ImageAlign = ContentAlignment.MiddleCenter;
                buttons[i].ImageScaling = ToolStripItemImageScaling.None;
                buttons[i].Enabled = false;

                if (i >= paramGroups.Count) buttons[i].Visible = false;
            }
        }
    }
}
