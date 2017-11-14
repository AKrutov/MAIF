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
                int index = this.FindStringExact(param.Value.ToString());
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
