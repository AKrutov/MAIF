using MAIF.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAIF.ControllsClasses
{
    public class MultipleTextBox : System.Windows.Forms.Panel, IAbstractControll 
    {
        private string[] scope;
        private string[] formulas;
        private string[] values;

        private int controlSpace = 10;

        public Param CurrentParam { set; get; }

        public MultipleTextBox(Param param, int cols)
        {
            this.Height = 22;
            this.Width = 700;
            
            this.CurrentParam = param;

            if (this.CurrentParam.Scope != null)
                scope = this.CurrentParam.Scope.Split(';');
            else
                scope = new string[] { "h1", "h2", "h3" };

            if (this.CurrentParam.Value != null)
                values = this.CurrentParam.Value.Split(';');
            else
                values = new string[] { "", "", "" };

            if (this.CurrentParam.Formula != null)
                formulas = this.CurrentParam.Formula.Split(';');
            else
                formulas = new string[] { "", "", "" };

            int controlsCount = scope.Count();
            int controlWidth = this.Width / cols - controlSpace * cols;

            for (int n = 0; n < cols; n++)
            {
                TextBox ctrl = new TextBox();
                ctrl.Width = controlWidth;
                ctrl.Location = new System.Drawing.Point(controlWidth * n, 0);
                ctrl.TextChanged += ctrl_TextChanged;
                ctrl.Text = (n <= this.values.Length - 1) ? values[n] : "";

                if (!scope.Contains("h" + (n + 1).ToString()))
                {
                    ctrl.Hide();
                }

                this.Controls.Add(ctrl);
                 
            }
        }

        void ctrl_TextChanged(object sender, EventArgs e)
        {
            this.CurrentParam.Value = ((TextBox)sender).Text;
        }

        public void Fill(Param param){}

        public Control AsControl()
        {
            return (Control)this;
        }

        public Control AsControl(int Width)
        {
            this.Width = Width;
            return this.AsControl();
        }

        public bool Validate()
        {
            return true;
        }
    }
}
