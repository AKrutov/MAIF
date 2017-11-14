using MAIF.classes;
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
        public Group CurrentGroup { get; set; }
        public int MaxWidth { get; set; } 
        public int Index;

        private int multiplier = 6;

        public GroupBox asControl() 
        {
            return (GroupBox) this;
        }

        public ControlGroup(Group currentGroup)
        {
            this.CurrentGroup = currentGroup;

            int maxLabelLenght = 200;
            int maxInputLength = 200;
            int currentYPosition = 20;
            
            for (int i = 0; i < this.CurrentGroup.Params.Count; i++)
            {
                maxLabelLenght = (this.CurrentGroup.Params[i].Desc.Length * multiplier > maxLabelLenght) ? this.CurrentGroup.Params[i].Desc.Length * multiplier : maxLabelLenght;
                for (int n = 0; n < this.CurrentGroup.Params[i].Values.Count; n++)
                {
                    maxInputLength = (this.CurrentGroup.Params[i].Values[n].Length * multiplier > maxInputLength) ? this.CurrentGroup.Params[i].Values[n].Length * multiplier : maxInputLength;
                }
            }
            
            int _cWidth = 5 + maxInputLength + 5 + maxLabelLenght + 40;
            this.MaxWidth = (_cWidth > this.MaxWidth) ? _cWidth : this.MaxWidth;            

            this.Text = this.CurrentGroup.Desc;
            this.Font = new Font(DefaultFont.FontFamily, DefaultFont.Size, FontStyle.Bold);
            this.Top = 20;
            this.Left = 5;
            this.Width = this.MaxWidth;

            for (int i = 0; i < this.CurrentGroup.Params.Count; i++)
            {
                Label caption = new Label();
                caption.Left = 5;
                caption.Top = currentYPosition;
                caption.Width = maxLabelLenght;
                caption.Text = this.CurrentGroup.Params[i].Desc;
                this.Controls.Add(caption);

                IAbstractControll _ctrl = new ControllFactory().CreateControl(this.CurrentGroup.Params[i]);
                if (_ctrl.GetType() == typeof(DropDown) && (this.CurrentGroup.Params[i].Allow_add == null || this.CurrentGroup.Params[i].Allow_add != "0"))
                {
                    ((DropDown)_ctrl).AddItem(new DropDownItem("-- Новое значение --", null));
                    ((DropDown)_ctrl).SelectedIndexChanged += ControlGroup_SelectedIndexChanged;     
                    /*  
                    Button addValueBtn = new Button();
                    addValueBtn.Text = "+";
                    addValueBtn.Font = new Font(DefaultFont.FontFamily, DefaultFont.Size, FontStyle.Bold);
                    addValueBtn.Width = 20;
                    addValueBtn.Height = 20;
                    addValueBtn.Location = new Point(maxLabelLenght + 5 + maxInputLength + 5, currentYPosition);

                    this.Controls.Add(addValueBtn);
                    */ 
                }
                Control ctrl = _ctrl.AsControl(maxInputLength);
                ctrl.Location = new Point(maxLabelLenght + 5, currentYPosition);
                this.Controls.Add(ctrl);

                if(this.CurrentGroup.Params[i].Units != null)
                {
                    Label units = new Label();
                    units.Top = currentYPosition;
                    units.Left = maxLabelLenght + 5 + maxInputLength + 5;
                    units.Width = 30;
                    units.Text = this.CurrentGroup.Params[i].Units;
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

        void ControlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DropDown)sender).SelectedItem == "-- Новое значение --")
            {
                var form = new TextInputForm();
                form.Text = "Добавление нового элемента в справочник";
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ((DropDown)sender).Items.RemoveAt(((DropDown)sender).Items.Count - 1);
                    ((DropDown)sender).AddItem(new DropDownItem(form.NewValue, null));
                    ((DropDown)sender).AddItem(new DropDownItem("-- Новое значение --", null));

                    ((DropDown)sender).SelectedItem = ((DropDown)sender).Items[((DropDown)sender).FindStringExact(form.NewValue)];
                }
            }
        }
    }
}
