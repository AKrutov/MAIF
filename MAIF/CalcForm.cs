﻿using MAIF.classes;
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

        public CalcForm(List<Group> groups)
        {
            InitializeComponent();
            controllGroups = groups;
        }

        private void CalcForm_Load(object sender, EventArgs e)
        {
            mainControlPanel.Width = mainControlPanel.Parent.Width;
            mainControlPanel.Height = mainControlPanel.Parent.Height - 100;

            foreach (var item in this.controllGroups)
            {
                ControlGroup gBox = new ControlGroup(item);
                this.paramGroups.Add(gBox.asControl());

                this.maxWidth = (gBox.MaxWidth > this.maxWidth) ? gBox.MaxWidth : this.maxWidth;
            }

            
            mainControlPanel.Controls.Add(this.paramGroups[this.currentPanel]);

            foreach (ControlGroup item in this.paramGroups)
            {
                item.SetWidth(this.maxWidth);    
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
            if (this.currentPanel < this.paramGroups.Count - 1 && ((ControlGroup)this.paramGroups[this.currentPanel]).IsValid() )
            {
                this.currentPanel = this.currentPanel + 1;

                mainControlPanel.Controls.Clear();
                mainControlPanel.Controls.Add(this.paramGroups[this.currentPanel]);

                prevBtn.Enabled = true;
                if (this.currentPanel == this.paramGroups.Count - 1)
                    nextBtn.Enabled = false;
                else
                    nextBtn.Enabled = true;
            }      
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            if(this.currentPanel > 0)
            {
                this.currentPanel = this.currentPanel - 1;

                mainControlPanel.Controls.Clear();
                mainControlPanel.Controls.Add(this.paramGroups[this.currentPanel]);

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
            
            foreach(MAIF.ControllsClasses.ControlGroup group in this.paramGroups)
            {

                #if DEBUG
                    allParams.AddRange(group.CurrentGroup.Params);
                #endif

                #if RELEASE
                if (group.IsValid())
                {
                    allParams.AddRange(group.CurrentGroup.Params);
                }
                else
                    throw new Exception("Данные не валидны!");
                #endif
            }

            XmlSerializer ser = new XmlSerializer(typeof(List<Group>));

            using (FileStream fs = new FileStream(@"params_"+DateTime.Now.ToShortDateString()+"_"+DateTime.Now.Ticks.ToString()+".xml", FileMode.Create))
            {
                ser.Serialize(fs, controllGroups);
            }

            var resultForm = new ResultForm(allParams);
            resultForm.Show();
            h.Info("Выполнен расчет пользователем " + Environment.UserName);
        }
    }  
}
