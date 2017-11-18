using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAIF.classes.ControllsClasses
{
    public class DropDown : System.Windows.Forms.ComboBox, IAbstractControll
    {
        public Param CurrentParam { set; get; }
        

        public void AddItem(DropDownItem item)
        {
            this.Items.Add(item.Name);
        }

        public void Fill(Param param)
        {
            this.CurrentParam = param;
            this.Name = param.Name;
            this.Width = 400;

            foreach (var item in param.Values)
	        {
                this.Items.Add(item);
	        }

            if(param.Value != null){
                var x = param.Value.ToString();
                if (param.Units == "%")
                {
                    x = ((Int32)(Utilities.AccurateParse(param.Value) * 100)).ToString();
                }
                int index = this.FindStringExact(x);
                this.SelectedItem = this.Items[index];
            }   
            
        }

        public Control AsControl()
        {
            return (ComboBox) this;
        }

        public Control AsControl(int Width)
        {
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Width = Width;
            return (ComboBox) this;
        }

        public bool Validate(){
            if(this.CurrentParam.IsRequired == "1")
             {
                 return (this.SelectedItem != null && this.SelectedItem.ToString() != "");
            }

            return true;
        }

    }
    public class DropDownItem : Object
    {
        public string Name;
        public object Value;

        public DropDownItem(string Name, object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
    
}
