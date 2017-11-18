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
        private Param currentParam;

        public Param CurrentParam
        {
            get
            {
                List<string> values = new List<string>();
                if (currentParam.IsHidden != "1")
                {
                    foreach (TextBox item in this.Controls)
                    {
                        values.Add(item.Text);
                    }
                    currentParam.Value = string.Join(";", values);
                }
              //  else
                //    currentParam.Value = "";
                return currentParam;
            }
            set
            {
                this.currentParam = value;
            }
        }

        public MultipleTextBox(Param param, int cols)
        {
            this.Height = 22;
            this.Width = 700;

            this.currentParam = param;

            if (this.currentParam.Scope != null)
                scope = this.currentParam.Scope.Split(';');
            else
                scope = new string[] { "h1", "h2", "h3" };

            if (this.currentParam.Value != null)
                values = this.currentParam.Value.Split(';');
            else
                values = new string[] { "", "", "" };

            if (this.currentParam.Formula != null)
                formulas = this.currentParam.Formula.Split(';');
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
                
                if (scope.Contains("h" + (n + 1).ToString()))
                {
                    ctrl.Text = values[n];
                    this.Controls.Add(ctrl);
                }
                
            }
        }

        void ctrl_TextChanged(object sender, EventArgs e)
        {
            this.currentParam.Value = ((TextBox)sender).Text;
        }

        public void Fill(Param param) { }

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
            bool result = true;

            if (this.CurrentParam.IsHidden != "1" && this.Text.IndexOfAny(new char[] { '%', '!', '=' }) > 0)
                result = false;

            return result;
        }
    }
}