using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using OOPLab.Attributes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace OOPLab.Items
{
    [Serializable]
    [CategoryName()]
    public abstract class Ammunition
    {
        public enum Size
        {
            XS, S, M, L, XL, XXL
        }
        [FieldName("Стоймость")]
        public int Price;
        [FieldName("Количество")]
        public int Count;
        [FieldName("Размер")]
        public Size ModelSize;
        [FieldName("Модель")]
        public string Name;
        [FieldName("Страна")]
        public string Country;
        public Ammunition()
        {
        }
        public Ammunition(int Price,int Count,Size size,string Name, string Country)
        {
            this.Price = Price;
            this.Count = Count;
            this.ModelSize = size;
            this.Name = Name;
            this.Country = Country;
        }
        public Ammunition(NameValueCollection list, Dictionary<string, object> objectList)
        {
            Price = Convert.ToInt32(list["Price"]);
            Count = Convert.ToInt32(list["Count"]);
            ModelSize = (Size)Enum.Parse(typeof(Size), list["ModelSize"]);
            Name = list["Name"];
            Country = list["Country"];
        }
    }
    [Serializable]
    [CategoryName("Штаны")]
    public class Pants : Ammunition
    {
        public enum TypeOfPockets
        {
            Inner, Outer
        }
        public enum Color
        {
            Blue, Black, Grey, Orange, Yellow
        }
        [FieldName("Цвет")]
        public Color color;
        [FieldName("Тип карманов")]
        public TypeOfPockets pocketType;
        public Pants()
        {
        }
        public Pants(Color color,TypeOfPockets typeOfPockets, int Price, int Count, Size size, string Name, string Country) : base(Price, Count, size, Name, Country)
        {
            this.color = color;
            this.pocketType = typeOfPockets;
        }
        public Pants(NameValueCollection list, Dictionary<string, object> objectList) : base(list, objectList)
        {
            pocketType = (TypeOfPockets)Enum.Parse(typeof(TypeOfPockets), list["pocketType"]);
            color = (Color)Enum.Parse(typeof(Color), list["color"]);
        }
    }
    [Serializable]
    [CategoryName("Куртка")]
    public class Jacket : Ammunition
    {
        public enum Material
        {
            Leather, Textile
        }
        [FieldName("Материал")]
        public Material material;
        
        public Jacket()
        {
        }
        public Jacket(Material material, int Price, int Count, Size size, string Name, string Country) : base(Price, Count, size, Name, Country)
        {
            this.material = material;   
        }
        public Jacket(NameValueCollection list, Dictionary<string, object> objectList) : base(list, objectList)
        {
            material = (Material)Enum.Parse(typeof(Material), list["material"]);
        }
    }
    [Serializable]
    [CategoryName("Шлем")]
    public class Helmet : Ammunition
    {
        public enum HelmetType
        {
            Integral, Modular, Open, Enduro, Cross
        }
        [FieldName("Тип шлема")]
        public HelmetType helmetType;

        [FieldName("Кл-во щитков")]
        public int numberOfShells;

        public Helmet()
        {
        }
        public Helmet(HelmetType helmetType ,int numberOfShells, int Price, int Count, Size size, string Name, string Country) : base(Price, Count, size, Name, Country)
        {
            this.helmetType = helmetType;
            this.numberOfShells = numberOfShells;
        }
        public Helmet(NameValueCollection list, Dictionary<string, object> objectList) : base(list, objectList)
        {
            helmetType = (HelmetType)Enum.Parse(typeof(HelmetType), list["helmetType"]);
            numberOfShells = Convert.ToInt32(list["numberOfShells"]);
        }
    }
    [Serializable]
    [CategoryName("Ботинки")]
    public class Boots : Ammunition
    {
        public enum BootsType
        {
            Biker, Timberland, Convers
        }
        [FieldName("Тип ботинок")]
        public BootsType bootsType;
        public Boots()
        {
        }
        public Boots(BootsType bootsType, int Price, int Count, Size size, string Name, string Country) : base(Price,Count,size,Name,Country)
        {
            this.bootsType = bootsType;
                
        }
        public Boots(NameValueCollection list, Dictionary<string, object> objectList) : base(list, objectList)
        {
            bootsType = (BootsType)Enum.Parse(typeof(BootsType), list["bootsType"]);
        }
    }
    [Serializable]
    [CategoryName("Набор")]
    public class MotorcyclеKit:Ammunition
    {
        [FieldName("Тип шлема")]
        public Helmet helmet;
        [FieldName("Тип куртки")]
        public Jacket jacket;
        [FieldName("Тип штанов")]
        public Pants pants;
        [FieldName("Тип ботинок")]
        public Boots boots;
        public MotorcyclеKit() { 
        }
        public MotorcyclеKit(Helmet helmet, Jacket jacket, Pants pants, Boots boots, int Price, int Count, Size size, string Name, string Country) : base(Price, Count, size, Name, Country)
        {
            this.helmet = helmet;
            this.jacket = jacket;
            this.pants = pants;
            this.boots = boots;
        }
        public MotorcyclеKit(NameValueCollection list, Dictionary<string, object> objectList) : base(list, objectList)
        {
            helmet = (Helmet)objectList["helmet"];
            jacket = (Jacket)objectList["jacket"];
            pants = (Pants)objectList["pants"];
            boots = (Boots)objectList["boots"];
        }
    }
}
