using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;



namespace MAIF
{
    class Utilities
    {
        public static XmlRootAttribute xRoot = new XmlRootAttribute() { ElementName = "ArrayOfGroup", IsNullable = true };

        public static double Evaluate(string expression)
        {
            try
            {
                while (expression.IndexOf("Math.Pow") >= 0)
                {
                    expression = ConvertPow(expression);
                }
                while (expression.IndexOf("Math.Discont") >= 0)
                {
                    expression = ConvertDiscont(expression);
                }
                while (expression.IndexOf("Math.GetBaseLevel") >= 0)
                {
                    expression = ConvertBaseLevel(expression);
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

        public static int GetFormulaLen(string expression)
        {
            var countOpen = 0;
            var countClose = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(') countOpen++;
                if (expression[i] == ')') countClose++;
                if (countOpen == countClose && countOpen != 0) return i + 1;
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

            var len = GetFormulaLen(expression);
            if (len >= 0)
            {
                end = expression.Substring(len);
            }
            expression = expression.Substring(0, len);
            while (expression.Trim().EndsWith(")") && expression.Trim().StartsWith("(")) expression = expression.Substring(1, expression.Length - 2);

            var a = expression.Split(new char[] { ',' });

            var n = Math.Round(Decimal.Parse(a[1].Replace(".", ",")));

            //calc transform
            String result = "";
            for (int i = 0; i < n; i++)
            {
                result += a[0] + "*";
            }
            result = result.Substring(0, result.Length - 1);

            return start + result + end;
        }

        public static string CalculateDiscont(int years, decimal val)
        {
            decimal result = 0;
            for (int i = 1; i <= years; i++)
            {
                decimal tmp = (decimal)(1 / (Math.Pow((1 + (double)val), (double)i)));
                result += tmp;
            }
            return Math.Round(result, 2).ToString().Replace(",",".");
        }
        public static string ConvertDiscont(string expression)
        {
            //expression = "(1/Math.Pow((1+0.08),30))";
            var start = expression.Substring(0, expression.IndexOf("Math.Discont("));
            var end = String.Empty;
            expression = expression.Substring(expression.IndexOf("Math.Discont("));
            expression = expression.Substring(12);
            //expression = expression.Replace("Math.Pow", "");

            var len = GetFormulaLen(expression);
            if (len >= 0)
            {
                end = expression.Substring(len);
            }
            expression = expression.Substring(0, len);
            while (expression.Trim().EndsWith(")") && expression.Trim().StartsWith("(")) expression = expression.Substring(1, expression.Length - 2);

            var a = expression.Split(new char[] { ',' });

            var n = Math.Round(Decimal.Parse(a[1].Replace(".", ",")));

            String result = "";
            result = CalculateDiscont((int)n, (decimal)(Evaluate(a[0]))).ToString();

            return start + result + end;
        }

        public static string ConvertBaseLevel(string expression)
        {
            //expression = "(1/Math.Pow((1+0.08),30))";
            var start = expression.Substring(0, expression.IndexOf("Math.GetBaseLevel("));
            var end = String.Empty;
            expression = expression.Substring(expression.IndexOf("Math.GetBaseLevel("));
            expression = expression.Substring(17);

            var len = GetFormulaLen(expression);
            if (len >= 0)
            {
                end = expression.Substring(len);
            }
            expression = expression.Substring(0, len);
            while (expression.Trim().EndsWith(")") && expression.Trim().StartsWith("(")) expression = expression.Substring(1, expression.Length - 2);

            var a = expression.Split(new char[] { ',' });

            var n1 = Math.Round(Decimal.Parse(a[0].Replace(".", ",")));
            var n2 = Math.Round(Decimal.Parse(a[1].Replace(".", ",")));
            var n3 = Math.Round(Decimal.Parse(a[2].Replace(".", ",")));

            String result = "";
            result = EnergyClass.GetBaseLevel((int)n1,(int)n2,(int)n3);

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
            dict.Add("default_picture", @"Images\\house-1.png");
            return dict;
        }

        public static string SaveParamsToXML(List<Group> groups, string filename = "", bool removeFormula=false)
        {
            if(removeFormula)
            {
                groups.ForEach(x => x.Params.ForEach(y => y.Formula = ""));
            }

            if (String.IsNullOrWhiteSpace(filename)) filename = @"params_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.Ticks.ToString() + ".xml";
            XmlSerializer ser = new XmlSerializer(typeof(List<Group>));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                ser.Serialize(fs, groups);
            }
            return filename;
        }

        public static string SaveParamsToXMLWithEncryption(List<Group> groups, string filename = "")
        {
            //groups.ForEach(x => x.Params.ForEach(y => y.Value = ""));

            if (String.IsNullOrWhiteSpace(filename)) filename = @"params_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.Ticks.ToString() + ".maif";
            XmlSerializer ser = new XmlSerializer(typeof(List<Group>));
            using (StringWriter textWriter = new StringWriter())
            {
                ser.Serialize(textWriter, groups);
                return SecurityClass.PutFileData(filename, textWriter.ToString());
            }
        }

        public static List<Group> GetParamsFromXML(string paramsPath)
        {
            List<Group> groups = new List<Group>();

            string path = Directory.GetCurrentDirectory();

            using (StreamReader reader = new StreamReader(paramsPath))
            {
                groups = (List<Group>)(new XmlSerializer(typeof(List<Group>), xRoot)).Deserialize(reader);
            }

            return groups;
        }

        public static List<Group> GetParamsFromXMLWithEncryption(string paramsPath)
        {
        string xml = SecurityClass.GetFileData(paramsPath);                       

            List<Group> groups = new List<Group>();

            string path = Directory.GetCurrentDirectory();

            using (TextReader reader = new StringReader(xml))
            {
                groups = (List<Group>)(new XmlSerializer(typeof(List<Group>), xRoot)).Deserialize(reader);
            }

            return groups;
    }

        public static List<Group> CombineParamsFromXML(string paramsPath, string cleanParams, bool encrValues = false, bool encrClean = true)
        {
            List<Group> groups_clean = new List<Group>();
            List<Group> groups_values = new List<Group>();
            Dictionary<string, string> formulas = new Dictionary<string, string>();

            groups_values = encrValues ? GetParamsFromXMLWithEncryption(paramsPath) : GetParamsFromXML(paramsPath);
            groups_clean = encrClean ? GetParamsFromXMLWithEncryption(cleanParams) : GetParamsFromXML(cleanParams);

            foreach (var r in groups_clean)
            {
                foreach (var v in r.Params)
                {
                    if (!String.IsNullOrWhiteSpace(v.Formula))
                    {
                        formulas.Add(v.Name, v.Formula);
                    }
                }
            }

            foreach (var f in formulas)
            {
                var gr = groups_values.FirstOrDefault(x => x.Params.Any(z => z.Desc == f.Key));
                if (String.IsNullOrWhiteSpace(gr.Params.FirstOrDefault(z => z.Desc == f.Key).Formula))
                    gr.Params.FirstOrDefault(z => z.Desc == f.Key).Formula = f.Value;
            }

            return groups_values;
        }
        

        public static System.Drawing.Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
    }
}