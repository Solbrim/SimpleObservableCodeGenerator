using System;
using System.Collections.Generic;
using System.Text;

namespace BenchSource.Models
{
    internal class FieldRecord
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string CamelName { get
        {
            var n = Name.Replace("_", "");
                return char.ToUpper(n[0]).ToString() + n.Substring(1, n.Length - 1);
        } }
        public string ChangedName { get => CamelName + "Changed"; }
        public string OnName { get => "On" + CamelName + "Change"; }
        public FieldRecord(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
