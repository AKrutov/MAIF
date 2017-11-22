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
        private int multiplier = 6;
        protected Group currentGroup;
        public Group CurrentGroup { 
            get
            {
                currentGroup.Params.Clear();
                foreach (Control item in this.Controls)
                {
                    if (item.GetType() == typeof(MAIF.classes.ControllsClasses.TextBox) || item.GetType() == typeof(DropDown) || item.GetType() == typeof(MultipleTextBox))
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
        public void SetWidth(int value)
        {
            if (this.currentGroup.H1 == null)
                this.setSimpleWidth(value);
            else
                this.setColumnedWidth(value);
        }

        protected void setSimpleWidth(int value)
        {
            int ctrlWidthDiff = value - MaxWidth;
            this.Width = value;
            if (ctrlWidthDiff > 0)
            {
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    if (this.Controls[i].GetType() == typeof(MAIF.classes.ControllsClasses.DropDown) || this.Controls[i].GetType() == typeof(MAIF.classes.ControllsClasses.TextBox))
                        this.Controls[i].Width = this.Controls[i].Width + ctrlWidthDiff;

                    if (this.Controls[i].Name == "units")
                        this.Controls[i].Left = this.Controls[i].Left + ctrlWidthDiff;
                    }
                }
            }

        protected void setColumnedWidth(int value)
        {
            this.Width = value;
        }

        void ControlGroup_TextChanged(object sender, EventArgs e)
        {
            if(((MAIF.classes.ControllsClasses.TextBox)sender).CurrentParam.Units == "%")
            {
                var n = Utilities.AccurateParse(((MAIF.classes.ControllsClasses.TextBox)sender).Text);
                n = n / 100;
                ((MAIF.classes.ControllsClasses.TextBox)sender).CurrentParam.Value = n.ToString();
            }
            else
            ((MAIF.classes.ControllsClasses.TextBox)sender).CurrentParam.Value = ((MAIF.classes.ControllsClasses.TextBox)sender).Text;
        }

        protected void createSimpleGroup()
        {
            int maxTotalLength = 1200;
            int maxLabelLenght = 200;
            int maxInputLength = 200;
            int currentYPosition = 20;

            /* Расчет размеров формы */
            for (int i = 0; i < this.currentGroup.Params.Count; i++)
            {
                maxLabelLenght = (this.currentGroup.Params[i].Desc.Length * multiplier > maxLabelLenght) ? this.currentGroup.Params[i].Desc.Length * multiplier : maxLabelLenght;
                for (int n = 0; n < this.currentGroup.Params[i].Values.Count; n++)
                {
                    maxInputLength = (this.currentGroup.Params[i].Values[n].Length * multiplier > maxInputLength) ? this.currentGroup.Params[i].Values[n].Length * multiplier : maxInputLength;
                }
            }

            int _cWidth = 5 + maxInputLength + 5 + maxLabelLenght + 60;
            this.MaxWidth = (_cWidth > this.MaxWidth) ? _cWidth : this.MaxWidth;

            if (this.MaxWidth > maxTotalLength)
                this.MaxWidth = maxTotalLength;

            this.Width = this.MaxWidth;
            /* Конец расчета размеров */

            for (int i = 0; i < this.currentGroup.Params.Count; i++)
            {
                Label caption = new Label();
                caption.Name = "caption";
                caption.Left = 5;
                caption.Top = currentYPosition;
                caption.Width = maxLabelLenght;
                caption.Text = this.currentGroup.Params[i].Desc;
                this.Controls.Add(caption);

                IAbstractControll _ctrl = new ControllFactory().CreateControl(this.currentGroup.Params[i]);
                if (_ctrl.GetType() == typeof(DropDown) && (this.currentGroup.Params[i].Allow_add == null || this.currentGroup.Params[i].Allow_add != "0"))
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


                foreach (Control ct in this.Controls)
                    ct.Font = new Font(DefaultFont.FontFamily, DefaultFont.Size, FontStyle.Regular);

                if (this.currentGroup.Params[i].IsHidden == "1")
                {
                    caption.Hide();
                    ctrl.Hide();
                }
                else
                {
                    if (this.currentGroup.Params[i].Units != null)
                    {
                        Label units = new Label();
                        units.Name = "units";
                        units.Top = currentYPosition;
                        units.Left = maxLabelLenght + 5 + maxInputLength + 5;
                        units.Width = 55;
                        units.Text = this.currentGroup.Params[i].Units;
                        this.Controls.Add(units);
                    }

                    //ctrl.Enabled = (this.currentGroup.Params[i].IsHidden != "1");

                    currentYPosition = currentYPosition + 22;
                }
            }
            this.Height = currentYPosition + 10;
        }

        private void createMultyValueGroup()
        {
            // Добавляем заголовки колонок

            int columns = 0;
            int curHeadLabelXPosition = 360;
            int curHeadLabelWidth = 250;
            int curHeadLabelHeight = 50;
            int curHeadLabelTop = 20;
            int curRowHeight = 22;
            int curControlPadding = 10;
            int curLabelWidth = 350;
            int ctrlWidth = 600;

            int currentYPosition = curHeadLabelTop + curHeadLabelHeight;

            Label H1Label = new Label();
            Label H2Label = new Label();
            Label H3Label = new Label();

            if (this.currentGroup.H1 != null)
            {
                H1Label.Name = "h1lbl";
                H1Label.Top = curHeadLabelTop;
                H1Label.Text = this.currentGroup.H1;
                H1Label.Width = curHeadLabelWidth;
                H1Label.Height = curHeadLabelHeight;

                this.Controls.Add(H1Label);
                columns++;
            }

            if (this.currentGroup.H2 != null)
            {
                H2Label.Name = "h2lbl";
                H2Label.Top = curHeadLabelTop;
                H2Label.Text = this.currentGroup.H2;
                H2Label.Width = curHeadLabelWidth;
                H2Label.Height = curHeadLabelHeight;
                this.Controls.Add(H2Label);
                columns++;
            }

            if (this.currentGroup.H3 != null)
            {
                H3Label.Name = "h2lbl";
                H3Label.Top = curHeadLabelTop;
                H3Label.Text = this.currentGroup.H3;
                H3Label.Width = curHeadLabelWidth;
                H3Label.Height = curHeadLabelHeight;
                this.Controls.Add(H3Label);
                columns++;
            }


            for (int i = 0; i < this.currentGroup.Params.Count; i++)
            {
                Param currentParam = this.currentGroup.Params[i];

                Label caption = new Label();
                caption.Name = "caption";
                caption.Left = curControlPadding;
                caption.Top = currentYPosition;
                caption.Text = currentParam.Desc;
                caption.Width = curLabelWidth;
                if(currentParam.IsHidden != "1")
                    this.Controls.Add(caption);

                IAbstractControll _ctrl = new ControllFactory().CreateControl(this.currentGroup.Params[i], columns);
                Control ctrl = _ctrl.AsControl();

                ctrl.Location = new Point(curLabelWidth + curControlPadding, currentYPosition);
                
                if (ctrl.GetType() == typeof(DropDown))
                    ctrl.Width = ctrlWidth / columns - 30;
                else
                    ctrlWidth = (ctrlWidth < ctrl.Width) ? ctrl.Width : ctrlWidth;


                if (currentParam.IsHidden != "1")
                    currentYPosition = currentYPosition + curRowHeight;
                else
                    ctrl.Hide();
               

                this.Controls.Add(ctrl);
            }

            this.Height = currentYPosition + curRowHeight;

            foreach (Control ct in this.Controls)
                ct.Font = new Font(DefaultFont.FontFamily, DefaultFont.Size, FontStyle.Regular);

            H1Label.Width = H2Label.Width = H3Label.Width = ctrlWidth / columns - 30;
            H1Label.Left = curHeadLabelXPosition;
            H2Label.Left = curHeadLabelXPosition + H1Label.Width;
            H3Label.Left = curHeadLabelXPosition + H1Label.Width+ H2Label.Width;
        }

        public GroupBox asControl() 
        {
            return (GroupBox) this;
        }

        public ControlGroup(Group currentGroup)
        {
            this.currentGroup = currentGroup;

            this.Text = currentGroup.Desc;
            this.Font = new Font(DefaultFont.FontFamily, DefaultFont.Size, FontStyle.Bold);
            this.Top = 20;
            this.Left = 5;
            
            if(currentGroup.H1 != null || currentGroup.H2 != null || currentGroup.H3 != null)
                this.createMultyValueGroup();
            else
                this.createSimpleGroup();
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

            if (((MAIF.classes.ControllsClasses.DropDown)sender).CurrentParam.Units == "%")
            {
                var n = Utilities.AccurateParse(((MAIF.classes.ControllsClasses.DropDown)sender).SelectedItem.ToString());
                n = n / 100;
                ((MAIF.classes.ControllsClasses.DropDown)sender).CurrentParam.Value = n.ToString();
            }
            else
                ((MAIF.classes.ControllsClasses.DropDown)sender).CurrentParam.Value = ((MAIF.classes.ControllsClasses.DropDown)sender).SelectedItem.ToString();
        }

        public bool IsValid()
        {
            bool isValid = true;
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].GetType() == typeof(MAIF.classes.ControllsClasses.TextBox)
                    || this.Controls[i].GetType() == typeof(MAIF.classes.ControllsClasses.DropDown)
                    || this.Controls[i].GetType() == typeof(MAIF.ControllsClasses.MultipleTextBox)
                    )
                {
                    if (((IAbstractControll)this.Controls[i]).CurrentParam.IsHidden != "1")
                        if (((IAbstractControll)this.Controls[i]).Validate())
                        {
                            isValid = isValid && true;
                            ((Label)this.Controls[i - 1]).ForeColor = Color.Black;
                        }
                        else
                        {
                            isValid = isValid && false;
                            ((Label)this.Controls[i - 1]).ForeColor = Color.Red;
                        }

                }
            }
#if DEBUG
            return true;
#else
            return isValid;
#endif
        }

    }
}
