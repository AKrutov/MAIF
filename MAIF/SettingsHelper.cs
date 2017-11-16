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

            foreach (var v in values)
            {
                String newValue = v.Value;
                if(v.Value!=null)
                    if (v.Value.IndexOf("%") >= 0&&v.Value.IndexOf("current_date")<0)
                        newValue = "f(" + v.Value + ")";

                newValues.Add(v.Key, newValue);
            }

            string allValues = string.Join(";", newValues.Select(x => x.Key + "=" + x.Value));
            while (i <= counterMax && allValues.IndexOf("%") >= 0)
            {
                foreach (var x in values)
                {
                    allValues = allValues.Replace("%" + x.Key + "%", x.Value);
                }
                i++;
            }
            var dict = allValues.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
               .Select(part => part.Split('='))
               .ToDictionary(split => split[0], split => split[1]);
            return EvaluateValues(dict);
        }

        public static Dictionary<string, string> EvaluateValues(Dictionary<string, string> values)
        {
            var newValues = new Dictionary<string, string>();
            foreach (var v in values)
            {
                String newValue = v.Value;
                if (v.Value.IndexOf("f(") >= 0)
                {
                    var formula = v.Value.Remove(0, 1);
                    newValue = Utilities.Evaluate(formula).ToString();
                }
                newValues.Add(v.Key, newValue);
            }
            return newValues;
        }
    }
}
