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

        public static Dictionary<string, string> ConvertParamsToValues(List<Param> paramList)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach(var param in paramList)
            {
                dict.Add(param.Name, param.Value);
            }
            dict.Add("current_date", DateTime.Now.Date.ToShortDateString());
            dict.Add("default_picture", @"http://www.ipsnews.net/Library/2017/04/DSC_3517-427x472.jpeg");
            return dict;
        }
    }


}
