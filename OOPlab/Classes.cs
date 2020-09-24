using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPLab.MyClasses
{
    public class Classes
    {
        public enum TMode
        {
            create,
            read,
            update
        }
        public static List<Type> GetClassesFromNamespace(string nameSpace)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            List<string> namespacelist = new List<string>();
            List<Type> classlist = new List<Type>();
            foreach (Type type in asm.GetTypes())
            {
                if ((type.Namespace == nameSpace) && (type.IsClass) && (!type.IsAbstract))
                    namespacelist.Add(type.Name);
            }
            foreach (string classname in namespacelist)
                classlist.Add(Type.GetType(nameSpace+"."+classname,false,false));
            return classlist;
        }

    }
}
