using System;
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
                {
                    var x = this.CurrentParam.Value.ToString();
                    if (this.CurrentParam.Units == "%")
                    {
                        x = ((Int32)(Double.Parse(this.CurrentParam.Value) * 100)).ToString();
                    }
                    this.Text = x;
                }
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
            bool result = true;
            if (this.CurrentParam.IsRequired == "1" && this.CurrentParam.IsHidden != "1")
            {
                double value;
                return (this.Text != "" && double.TryParse(this.Text.Replace('.',','), out value ));
            }

            if (this.CurrentParam.IsHidden != "1" && this.Text.IndexOfAny(new char[] { '%', '!', '=' }) > 0)
                result = false;

            return result;
        }
    }
}
