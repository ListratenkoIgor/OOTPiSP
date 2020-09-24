using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Reflection;
using OOPLab.Items;
using OOPLab.Attributes;
using System.Xml.Serialization;

namespace OOPlab
{
    public abstract class FileCreator
    {
        public abstract byte[] SaveFile(List<Ammunition> catalog);
        public abstract List<Ammunition> OpenFile(byte[] data);
    }
    public class BinaryFileCreator : FileCreator
    {
        public override byte[] SaveFile(List<Ammunition> catalog)
        {
            BinaryFormatter Formater = new BinaryFormatter();
            MemoryStream MS = new MemoryStream();
            Formater.Serialize(MS, catalog);
            return MS.ToArray();
        }
        public override List<Ammunition> OpenFile(byte[] data)
        {
            List<Ammunition> catalog;
            BinaryFormatter Formater = new BinaryFormatter();
            MemoryStream MS = new MemoryStream(data);
            catalog = (List<Ammunition>)Formater.Deserialize(MS);
            return catalog;
        }
    }

    public class JsonFileCreator : FileCreator
    {
        public override byte[] SaveFile(List<Ammunition> Catalog)
        {
            JsonSerializerSettings Settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string Serialized = JsonConvert.SerializeObject(Catalog, Settings);
            byte[] ret = System.Text.Encoding.UTF8.GetBytes(Serialized);
            return ret;
        }
        public override List<Ammunition> OpenFile(byte[] data)
        {
            string Serialized = Encoding.UTF8.GetString(data);
            JsonSerializerSettings Settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            List<Ammunition> Catalog = JsonConvert.DeserializeObject<List<Ammunition>>(Serialized, Settings);
            return Catalog;
        }
    }

    public class AuthorFileCreator : FileCreator
    {
        public override byte[] SaveFile(List<Ammunition> Catalog)
        {
            string Serialized = null;
            foreach (Ammunition item in Catalog)
            {
                Serialize(ref Serialized, item);
                Serialized += "};";
            }
            Serialized += "$";
            byte[] ret = System.Text.Encoding.UTF8.GetBytes(Serialized);
            return ret;
        }

        public override List<Ammunition> OpenFile(byte[] data)
        {
            string Serialized = Encoding.UTF8.GetString(data);
            List<Ammunition> localCatalog = new List<Ammunition>();
            while (Serialized[0] != '$')
            {
                localCatalog.Add(Deserialize(ref Serialized));
            }
            return localCatalog;
        }


        private static void Serialize(ref string str, object obj)
        {
            Ammunition item = (Ammunition)obj;
            Type type = obj.GetType();
            str += type.ToString() + "{";
            foreach (FieldInfo member in type.GetFields())
            {       
                if (member.FieldType.ToString().IndexOf("OOPlab.Items.") > -1 && member.FieldType.ToString().IndexOf("+") <= -1)
                {
                    Serialize(ref str, member.GetValue(item));
                    str += "},";
                }
                else
                {
                    str += member.FieldType.ToString() + ":" + member.GetValue(item) + ",";
                }
            }
        }
        private static Type FieldType(ref string str, int ind)
        {
            Type objtype = Type.GetType(str.Substring(0, ind), false, true);
            str = str.Remove(0, ind + 1);
            return objtype;
        }

        private static string GetStringValue(ref string str)
        {
            int ind = str.IndexOf(',');
            string s = str.Substring(0, ind);
            str = str.Remove(0, ind + 1);
            return s;
        }

        private static Ammunition Deserialize(ref string str)
        {
            int index0 = str.IndexOf('{');
            Type objtype = FieldType(ref str, index0);
            FieldInfo[] fields = objtype.GetFields();
            Type[] types = new Type[fields.Length];
            Type t;
            object[] values = new object[fields.Length];
            int i = 0;
            foreach (var param in fields)
            {
                int index1 = str.IndexOf(':');
                int index2 = str.IndexOf('{');
                if (index1 < index2 || index2 == -1)
                {
                    t = FieldType(ref str, index1);
                    {
                        types[i] = t;
                        string value_str = GetStringValue(ref str);
                        switch (types[i].Name)
                        {
                            case "Integer":
                                {
                                    values[i] = Convert.ToInt32(value_str);
                                    break;
                                }
                            case "Int16":
                                {
                                    values[i] = Convert.ToInt16(value_str);
                                    break;
                                }
                            case "Int32":
                                {
                                    values[i] = Convert.ToInt32(value_str);
                                    break;
                                }
                            case "Int64":
                                {
                                    values[i] = Convert.ToInt64(value_str);
                                    break;
                                }
                            case "UInt16":
                                {
                                    values[i] = Convert.ToUInt16(value_str);
                                    break;
                                }
                            case "UInt32":
                                {
                                    values[i] = Convert.ToUInt32(value_str);
                                    break;
                                }
                            case "UInt64":
                                {
                                    values[i] = Convert.ToUInt64(value_str);
                                    break;
                                }
                            case "Double":
                                {
                                    values[i] = Convert.ToDouble(value_str);
                                    break;
                                }
                            case "Single":
                                {
                                    values[i] = Convert.ToSingle(value_str);
                                    break;
                                }
                            case "Data":
                                {
                                    values[i] = Convert.ToDateTime(value_str);
                                    break;
                                }
                            case "String":
                                {
                                    values[i] = Convert.ToString(value_str);
                                    break;
                                }
                            default:
                                if(t.IsEnum)
                                    values[i] = Enum.Parse(types[i], value_str);
                                else
                                    values[i] = Deserialize(ref str);
                                break;
                        }

                    }
                }
                else
                {
                    values[i] = Deserialize(ref str);
                }
                i++;

            }
            str = str.Remove(0, 2);
            object obj = Activator.CreateInstance(objtype, values);
            Ammunition Item = (Ammunition)obj;
            return Item;
        }
    }
}