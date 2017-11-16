using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAIF
{
    class SettingsHelper
    {
        private static int counterMax = 10;
        public static Dictionary<string, string> ProcessValues(Dictionary<string, string> values)
        {
            int i = 0;
            var newValues = new Dictionary<string, string>();
            var resultValues = new Dictionary<string, string>();
            foreach (var v in values)
            {
                String newValue = v.Value;
                if (v.Value != null)
                    if (v.Value.IndexOf("%") >= 0 && v.Value.IndexOf("current_date") < 0)
                    {
                        if (v.Value.IndexOf(';') < 0)
                            newValue = "f(" + v.Value + ")";
                        else
                            newValue = "f(" + v.Value.Replace(";",");f(") + ")";
                    }

                newValues.Add(v.Key, newValue);
            }

            string allValues = string.Join("!", newValues.Select(x => x.Key + "=" + x.Value));
            while (i <= counterMax && allValues.IndexOf("%") >= 0)
            {
                foreach (var x in values)
                {
                    allValues = allValues.Replace("%" + x.Key + "%", x.Value);
                }
                i++;
            }
            var dict = allValues.Split(new[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            //.Select(part => part.Split('='))
            //.ToDictionary(split => split.Length > 0? String.Empty : split[0], split => split.Length>1?String.Empty: split[1]);
            foreach (var val in dict)
            {
                resultValues.Add(val.Split('=')[0], val.Split('=')[1]);
            }
            return EvaluateValues(resultValues);
        }

        public static Dictionary<string, string> EvaluateValues(Dictionary<string, string> values)
        {
            var newValues = new Dictionary<string, string>();
            foreach (var v in values)
            {
                String result = "";
                if (v.Value.IndexOf(';')>=0)
                {
                    var array = v.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    List<String> results = new List<String>();
                    foreach(var a in array)
                    {
                        if (!String.IsNullOrWhiteSpace(a))
                            results.Add(EvalCustom(a));
                        else
                            results.Add(String.Empty);
                    }
                    result = String.Join(";", results);
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(v.Value))
                        result = EvalCustom(v.Value);
                    else
                        result = String.Empty;
                }
                //String newValue = v.Value;
                //if (v.Value.IndexOf("f(") >= 0)
                //{
                //    var formula = v.Value.Remove(0, 1);
                //    newValue = Utilities.Evaluate(formula).ToString();
                //}
                newValues.Add(v.Key, result);
            }
            return newValues;
        }

        public static string EvalCustom(string val)
        {
            String newValue = val;
            if (val.IndexOf("f(") >= 0)
            {
                var formula = val.Remove(0, 1);
                if (formula != "()")
                    newValue = Utilities.Evaluate(formula).ToString();
                else
                    newValue = String.Empty;
            }
            return newValue;
        }
    }
}
