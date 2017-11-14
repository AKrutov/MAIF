using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAIF
{
    public class CustomMenuRenderer : ToolStripProfessionalRenderer
    {
        public CustomMenuRenderer() : base(new MyColors()) { }
    }

    public class MyColors : ProfessionalColorTable
    {
        //public override Color ButtonSelectedHighlight
        //{
        //    get { return Color.Yellow; }
        //}
        //public override Color CheckSelectedBackground
        //{
        //    get { return Color.Orange; }
        //}
        //public override Color ButtonSelectedGradientMiddle
        //{
        //    get { return Color.Yellow; }
        //}
    }
}
