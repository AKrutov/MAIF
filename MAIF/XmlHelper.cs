using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MAIF
{
    public class Param
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
        [XmlAttribute(AttributeName = "units")]
        public string Units { get; set; }
        [XmlAttribute(AttributeName = "tooltip")]
        public string Tooltip { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
        [XmlElement(ElementName = "value")]
        public List<string> Values { get; set; }
        [XmlAttribute(AttributeName = "allow_add")]
        public string Allow_add { get; set; }
        [XmlAttribute(AttributeName = "allow_edit")]
        public string Allow_edit { get; set; }
        [XmlAttribute(AttributeName = "formula")]
        public string Formula { get; set; }
        [XmlAttribute(AttributeName = "is_hidden")]
        public string IsHidden { get; set; }
        [XmlAttribute(AttributeName = "is_required")]
        public string IsRequired { get; set; }
        [XmlAttribute(AttributeName = "scope")]
        public string Scope { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "Group")]
    public class Group
    {
        [XmlElement(ElementName = "Param")]
        public List<Param> Params { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
        [XmlAttribute(AttributeName = "h1")]
        public string H1 { get; set; }
        [XmlAttribute(AttributeName = "h2")]
        public string H2 { get; set; }
        [XmlAttribute(AttributeName = "h3")]
        public string H3 { get; set; }
        [XmlAttribute(AttributeName = "h3_formula")]
        public string H3_formula { get; set; }
    }


}