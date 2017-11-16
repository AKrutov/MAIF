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
            if (this.CurrentParam.IsRequired == "1")
            {
                return this.Text != "";
            }
            return true;
        }
    }
}
