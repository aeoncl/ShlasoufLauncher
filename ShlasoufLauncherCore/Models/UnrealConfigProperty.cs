using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace ShlasoufLauncherCore.Models
{
    public class UnrealConfigProperty
    {

        public UnrealConfigProperty(){}

        public UnrealConfigProperty(string section, string key, string value)
        {
            Section = section;
            Key = key;
            Value = value;
        }
        public string Section { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Key}={Value}";
        }
    }
}
