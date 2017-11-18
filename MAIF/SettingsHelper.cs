using System;
using System.Collections.Generic;
using System.Globalization;
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

            int counter = 0;
            //Вот отсюда переписать
            while ((CheckIfNonCalculatedFormulaExists(resultValues)||resultValues.Count==0)&&counter<counterMax)
            {
                foreach (var v in newValues)
                {
                    if (!resultValues.ContainsKey(v.Key))
                        resultValues.Add(v.Key, v.Value);
                    if (resultValues[v.Key].IndexOf("f(") >= 0)
                        resultValues[v.Key] = UpdateFormulaVals(resultValues, resultValues[v.Key]);
                }

                counter++;
            }
            //string allValues = string.Join("!", newValues.Select(x => x.Key + "=" + x.Value));
            //while (i <= counterMax && allValues.IndexOf("%") >= 0)
            //{
            //    foreach (var x in values)
            //    {
            //        if (!String.IsNullOrWhiteSpace(x.Value))
            //        {
            //            if (x.Value.IndexOf(';') < 0)
            //                allValues = allValues.Replace("%" + x.Key + "%", x.Value);
            //            else
            //            {
            //                var a = x.Value.Split(new[] { ';' });
            //                for (int iterator = 0; iterator < a.Count(); iterator++)
            //                {
            //                    allValues = allValues.Replace("%" + x.Key + "[" + iterator + "]%", a[iterator]);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            allValues = allValues.Replace("%" + x.Key + "%", "0");
            //        }
            //    }
            //    i++;
            //}
            //var dict = allValues.Split(new[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            ////.Select(part => part.Split('='))
            ////.ToDictionary(split => split.Length > 0? String.Empty : split[0], split => split.Length>1?String.Empty: split[1]);
            //foreach (var val in dict)
            //{
            //    resultValues.Add(val.Split('=')[0], val.Split('=')[1]);
            //}
            return EvaluateValues(resultValues);
        }

        public static string UpdateFormulaVals(Dictionary<string, string> values, string formula)
        {
           
            foreach (var v in values)
            {
                //formula = v.Value;

                if (formula.IndexOf("%" + v.Key + "%") >= 0 && v.Value.IndexOf("f(") < 0)
                {
                    //var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "," };
                    //var value = Decimal.Parse(v.Value, numberFormatInfo);

                    formula = formula.Replace("%" + v.Key + "%", v.Value.Replace(",", "."));


                }
                if (formula.IndexOf("%" + v.Key + "[") >= 0 && v.Value.IndexOf("f(") < 0)
                {
                    var a = v.Value.Replace(",", ".").Split(new[] { ';' });
                    for (int iterator = 0; iterator < a.Count(); iterator++)
                    {
                        formula = formula.Replace("%" + v.Key + "[" + iterator + "]%", a[iterator]);
                    }
                }

                if (formula.IndexOf("%") < 0 && formula.IndexOf("f(") >= 0)
                {
                    //formula = EvalCustom(formula);

                    String result = "";
                    if (formula.IndexOf(';') >= 0)
                    {
                        var array = formula.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        List<String> results = new List<String>();
                        foreach (var a in array)
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
                        if (!String.IsNullOrWhiteSpace(formula))
                            result = EvalCustom(formula);
                        else
                            result = String.Empty;
                    }
                    formula = result;
                }
            }
            return formula;
        }

        public static bool CheckIfNonCalculatedFormulaExists(Dictionary<string, string> values)
        {
            foreach (var v in values)
            {
                if (v.Value.IndexOf("f(") >= 0 && v.Value.IndexOf("%") >= 0) return true;
            }
            return false;
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
                newValues.Add(v.Key, result) ;
            }
            return newValues;
        }

        public static string EvalCustom(string val)
        {
            String newValue = val;
            if (val.Any(char.IsDigit))
            {

                if (val.IndexOf("f(") >= 0)
                {
                    var formula = val.Remove(0, 1);
                    //formula = formula.Remove(formula.Length - 1);
                    if (formula != "()")
                        newValue = Math.Round(Utilities.Evaluate(formula),2).ToString();
                    else
                        newValue = "0";
                }
            }
            else
                val = "0";
            return newValue;
        }
    }
}
