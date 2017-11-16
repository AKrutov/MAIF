using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAIF.classes
{
    public interface IAbstractControll
    {
        Param CurrentParam { set; get; }
        void Fill(Param param);
        Control AsControl();
        Control AsControl(int Width);
        bool Validate();
    }
}
