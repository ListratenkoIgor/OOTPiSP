using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPLab.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ClassNameAttribute : Attribute
    {
        public ClassNameAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; set; }
    }
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class CategoryNameAttribute : Attribute
    {
        public CategoryNameAttribute()
        {
            
        }
        public CategoryNameAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; set; }
    }
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class FieldNameAttribute : Attribute
    {
        public FieldNameAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; set; }
    }
}
