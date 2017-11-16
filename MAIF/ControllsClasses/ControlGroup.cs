﻿using MAIF.classes;
using MAIF.classes.ControllsClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAIF.ControllsClasses 
{
    class ControlGroup : GroupBox
    {
        protected Group currentGroup;
        public Group CurrentGroup { 
            get
            {
                currentGroup.Params.Clear();
                foreach (Control item in this.Controls)
                {
                    if (item.GetType() == typeof(MAIF.classes.ControllsClasses.TextBox) || item.GetType() == typeof(DropDown))
                    {
                        currentGroup.Params.Add(((IAbstractControll)item).CurrentParam);
                    }
                }
                return currentGroup;
            } 
            set 
            {
                this.currentGroup = value;
            } 
        }

        public int MaxWidth { get; set; } 
        private int multiplier = 6;

        public GroupBox asControl() 
        {
            return (GroupBox) this;
        }

        public ControlGroup(Group currentGroup)
        {
            this.currentGroup = currentGroup;
            

            int maxLabelLenght = 200;
            int maxInputLength = 200;
            int currentYPosition = 20;

            for (int i = 0; i < currentGroup.Params.Count; i++)
            {
                maxLabelLenght = (currentGroup.Params[i].Desc.Length * multiplier > maxLabelLenght) ? currentGroup.Params[i].Desc.Length * multiplier : maxLabelLenght;
                for (int n = 0; n < currentGroup.Params[i].Values.Count; n++)
                {
                    maxInputLength = (currentGroup.Params[i].Values[n].Length * multiplier > maxInputLength) ? currentGroup.Params[i].Values[n].Length * multiplier : maxInputLength;
                }
            }
            
            int _cWidth = 5 + maxInputLength + 5 + maxLabelLenght + 40;
            this.MaxWidth = (_cWidth > this.MaxWidth) ? _cWidth : this.MaxWidth;

            this.Text = currentGroup.Desc;
            this.Font = new Font(DefaultFont.FontFamily, DefaultFont.Size, FontStyle.Bold);
            this.Top = 20;
            this.Left = 5;
            this.Width = this.MaxWidth;

            for (int i = 0; i < currentGroup.Params.Count; i++)
            {
                Label caption = new Label();
                caption.Left = 5;
                caption.Top = currentYPosition;
                caption.Width = maxLabelLenght;
                caption.Text = currentGroup.Params[i].Desc;
                this.Controls.Add(caption);

                IAbstractControll _ctrl = new ControllFactory().CreateControl(currentGroup.Params[i]);
                if (_ctrl.GetType() == typeof(DropDown) && (currentGroup.Params[i].Allow_add == null || currentGroup.Params[i].Allow_add != "0"))
                {
                    ((DropDown)_ctrl).AddItem(new DropDownItem("-- Новое значение --", null));
                    ((DropDown)_ctrl).SelectedIndexChanged += ControlGroup_SelectedIndexChanged;     
                }

                if (_ctrl.GetType() == typeof(MAIF.classes.ControllsClasses.TextBox))
                {
                    ((MAIF.classes.ControllsClasses.TextBox)_ctrl).TextChanged += ControlGroup_TextChanged;
                }

                Control ctrl = _ctrl.AsControl(maxInputLength);
                ctrl.Location = new Point(maxLabelLenght + 5, currentYPosition);
                this.Controls.Add(ctrl);

                if (currentGroup.Params[i].Units != null)
                {
                    Label units = new Label();
                    units.Top = currentYPosition;
                    units.Left = maxLabelLenght + 5 + maxInputLength + 5;
                    units.Width = 30;
                    units.Text = currentGroup.Params[i].Units;
                    this.Controls.Add(units);
                }

                foreach (Control ct in this.Controls)
                {
                    ct.Font = new Font(DefaultFont.FontFamily, DefaultFont.Size, FontStyle.Regular);
                }

                currentYPosition = currentYPosition + 22;
            }

            this.Height = currentYPosition + 10;
        }

        void ControlGroup_TextChanged(object sender, EventArgs e)
        {
            ((MAIF.classes.ControllsClasses.TextBox)sender).CurrentParam.Value = ((MAIF.classes.ControllsClasses.TextBox)sender).Text;
        }

        void ControlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDown currentControl = (DropDown)sender;

            if ((string)currentControl.SelectedItem == "-- Новое значение --")
            {
                var form = new TextInputForm();
                form.Text = "Добавление нового элемента в справочник";
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    
                    currentControl.Items.RemoveAt(currentControl.Items.Count - 1);
                    currentControl.AddItem(new DropDownItem(form.NewValue, null));
                    currentControl.AddItem(new DropDownItem("-- Новое значение --", null));
                    currentControl.SelectedItem = currentControl.Items[currentControl.FindStringExact(form.NewValue)];

                    currentControl.CurrentParam.Values.Add(form.NewValue);
                    currentControl.CurrentParam.Value = form.NewValue;
                }
            }
        }
    }
}