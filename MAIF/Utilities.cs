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
            try
            {
                while (expression.IndexOf("Math.Pow") >= 0)
                {
                    expression = ConvertPow(expression);
                }

                var loDataTable = new DataTable();
                var loDataColumn = new DataColumn("Eval", typeof(double), expression);
                loDataTable.Columns.Add(loDataColumn);
                loDataTable.Rows.Add(0);
                return (double)(loDataTable.Rows[0]["Eval"]);
            }
            catch (SyntaxErrorException ex)
            {
                var r = Utilities.AccurateParse(expression);
                if (r != 0) return r;
            }
            catch (Exception ex)
            {
            }
            return 0;
        }

        public static int PowFormulaLen(string expression)
        {
            var countOpen = 0;
            var countClose = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(') countOpen++;
                if (expression[i] == ')') countClose++;
                if (countOpen == countClose && countOpen != 0) return i+1;
            }
            return -1;
        }

        public static string ConvertPow(string expression)
        {
            //expression = "(1/Math.Pow((1+0.08),1))";
            var start = expression.Substring(0, expression.IndexOf("Math.Pow("));
            var end = String.Empty;
            expression = expression.Substring(expression.IndexOf("Math.Pow("));
            expression = expression.Substring(8);
            //expression = expression.Replace("Math.Pow", "");

            var len = PowFormulaLen(expression);
            if (len >= 0)
            {
                end = expression.Substring(len);
            }
            expression = expression.Substring(0, len);
            while (expression.Trim().EndsWith(")") && expression.Trim().StartsWith("(")) expression = expression.Substring(1, expression.Length - 2);


            var a = expression.Split(new char[] { ',' });

            var exp = Evaluate(AccurateParse(a[1]).ToString());
            var n = Math.Round(exp);

            String result = "";
            for (int i = 0; i < n; i++)
            {
                result += a[0] + "*";
            }
            result = result.Substring(0, result.Length - 1);

            return start + result + end;
        }

        public static Double AccurateParse(string value)
        {
            Double result = 0;

            if (Double.TryParse(value, out result)) return result;
            else if (Double.TryParse(value.Replace(".", ","), out result))
                return result;
            else if (Double.TryParse(value.Replace(",", "."), out result))
                return result;
            return result;
        }

        public static Dictionary<string, string> ConvertParamsToValues(List<Param> paramList)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var param in paramList)
            {
                if (param.IsNumeric == "1")
                {
                    String x = "0";
                    if (!String.IsNullOrWhiteSpace(param.Value))
                    {
                        //    x = (String.IsNullOrWhiteSpace(param.Formula)) ? "0" : param.Value;
                        if (param.Value.IndexOf(";") >= 0)
                        {
                            var a = param.Value.Split(new[] { ';' });
                            for (int iterator = 0; iterator < a.Count(); iterator++)
                            {
                                if (String.IsNullOrWhiteSpace(a[iterator]))
                                {
                                    a[iterator] = "0";
                                }
                            }
                            x = String.Join(";", a);
                        }
                        else
                            x = param.Value;
                        x = x.Replace(",", ".");
                    }
                    else
                        if (!String.IsNullOrWhiteSpace(param.Formula)) x = param.Formula;
                    dict.Add(param.Name, x);
                }
                else
                    dict.Add(param.Name, param.Value);
            }
            dict.Add("current_date", DateTime.Now.Date.ToShortDateString());
            dict.Add("default_picture", @"http://www.ipsnews.net/Library/2017/04/DSC_3517-427x472.jpeg");
            return dict;
        }
    }


}
