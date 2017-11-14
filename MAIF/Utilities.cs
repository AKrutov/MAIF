using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MAIF
{
    class Utilities
    {
        public static double Evaluate(string expression)
        {
            var loDataTable = new DataTable();
            var loDataColumn = new DataColumn("Eval", typeof(double), expression);
            loDataTable.Columns.Add(loDataColumn);
            loDataTable.Rows.Add(0);
            return (double)(loDataTable.Rows[0]["Eval"]);
        }
    }


}
