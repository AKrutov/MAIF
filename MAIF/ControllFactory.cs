using MAIF.classes.ControllsClasses;
using MAIF.ControllsClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAIF.classes
{
    public class ControllFactory
    {
        IAbstractControll ctrl;

        public IAbstractControll CreateControl(Param control, int cols = 0) 
        {
            if (control.Values.Count > 0)
            {
                this.ctrl = new DropDown();
            }
            else
            {
                if (cols > 0)
                    this.ctrl = new MultipleTextBox(control, cols);
                else
                    this.ctrl = new TextBox();
            } 
            
            this.ctrl.Fill(control);
            
            return this.ctrl;
        }
    }
}
