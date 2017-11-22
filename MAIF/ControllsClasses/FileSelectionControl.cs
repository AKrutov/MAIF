using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAIF.classes.ControllsClasses
{
    class FileSelectionControl : System.Windows.Forms.TextBox, IAbstractControll
    {
        public Param CurrentParam { set; get; }
        public void Fill(Param param)
        {
            this.CurrentParam = param;
            
            if (!String.IsNullOrWhiteSpace(this.CurrentParam.Value))               
                    this.Text = this.CurrentParam.Value.ToString();
                
        }
        public Control AsControl()
        {
            this.Width = 400;

            return (FileSelectionControl)this;
        }
        public Control AsControl(int Width)
        {
            this.Width = Width;

            return (FileSelectionControl)this;
        }

        public bool Validate()
        {
            bool result = true;
            return result;
        }
    }
}
