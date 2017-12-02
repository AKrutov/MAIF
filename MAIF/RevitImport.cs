using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace MAIF
{
    public static class RevitImport
    {
        public static List<RevitValue> GetValuesFromXml(string path)
        {
            List<RevitValue> revits = new List<RevitValue>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(path));

            foreach (XmlNode node in doc.SelectNodes("ProjectInfo/FixKeys"))
            {
                //string text = node.InnerText; 
                foreach (XmlNode fix in node.ChildNodes)
                {
                    RevitValue v = new RevitValue();
                    v.UIKey = fix.ChildNodes[0].InnerText;
                    v.DBKey = fix.ChildNodes[1].InnerText;
                    v.value = fix.ChildNodes[2].InnerText;
                    revits.Add(v);
                }
            }

            foreach (XmlNode node in doc.SelectNodes("ProjectInfo/CustomKeys"))
            {
                //string text = node.InnerText; 
                foreach (XmlNode fix in node.ChildNodes)
                {
                    RevitValue v = new RevitValue();
                    v.UIKey = fix.ChildNodes[0].InnerText;
                    v.DBKey = fix.ChildNodes[1].InnerText;
                    v.value = fix.ChildNodes[2].InnerText;
                    revits.Add(v);
                }
            }
            return revits;
        }

        public static List<Group> MapParamsFromRevit(List<RevitValue> revits, List<Group> groups)
        {
            if (revits == null) return groups;
            string oldValue;
            foreach (var r in revits)
            {
                var gr = groups.FirstOrDefault(x => x.Params.Any(z => z.Desc == r.UIKey));
                if (gr != null)
                {
                  
                    var param = gr.Params.FirstOrDefault(z => z.Desc == r.UIKey);
                    if (!String.IsNullOrWhiteSpace(param.Units))
                        r.value = r.value.Replace(param.Units, "").Trim();

                    oldValue = r.value;

                    if (param.Units == "%")
                    {
                        r.value = Math.Round((Utilities.AccurateParse(r.value) / 100), 2).ToString().Replace(",", ".");
                    }
                    
                    if (!String.IsNullOrWhiteSpace(param.Units) && param.Units != "%")
                        r.value = GetNumbers(r.value.Trim());

                    if (param.Values != null)
                    {
                        if (param.Values.Count > 0)
                        {
                            if (!param.Values.Contains(r.value))
                            {
                                gr.Params.FirstOrDefault(z => z.Desc == r.UIKey).Values.Add(oldValue);
                            }
                        }
                    }

                    gr.Params.FirstOrDefault(z => z.Desc == r.UIKey).Value = r.value.Trim();

                }
            }
            return groups;
        }
        
        public static string GetNumbers(this string text)
        {
            text = text ?? string.Empty;
            if (!text.Any(p => char.IsDigit(p))) return text;
            return new string(text.Where(p => char.IsDigit(p) || p == ',').ToArray());
        }
    }
    public class RevitValue
    {
        public string UIKey;
        public string DBKey;
        public string value;
    }

    
}