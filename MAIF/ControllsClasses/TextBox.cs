﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAIF.classes.ControllsClasses
{
    public class TextBox : System.Windows.Forms.TextBox , IAbstractControll 
    {
        public Param CurrentParam { set; get; }
        public void Fill(Param param)
        {
            this.CurrentParam = param;
            if (this.CurrentParam.Value == "%current_date%") this.Text = DateTime.Now.ToShortDateString();
            if (!String.IsNullOrWhiteSpace(this.CurrentParam.Value))
                if (this.CurrentParam.Value.IndexOfAny(new char[] { '%', '!', '=' }) < 0)
                    this.Text = this.CurrentParam.Value;
        }
        public Control AsControl()
        {
            this.Width = 400;

            return (TextBox)this;
        }
        public Control AsControl(int Width)
        {
            this.Width = Width;

            return (TextBox)this;
        }

        public bool Validate()
        {
            if (this.CurrentParam.IsRequired == "1" && this.CurrentParam.IsHidden != "1")
            {
                float result;
                return (this.Text != "" && float.TryParse(this.Text,out result));
            }
            return true;
        }
    }
}
