using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Toktassyn.Nsudotnet.JSONSerializer
{
    class Serializator
    {
        private StringBuilder builder;
        public Object Obj { get; set; }

        public Serializator(Object o)
        {
            builder = new StringBuilder();
            Obj = o;
        }

        public void Serialize()
        {
            Type t = Obj.GetType();
            if (!(t.IsClass))
            {
                throw new Exception("The input parameter must be an object of class");
            }
            if (!(t.Attributes.ToString().Contains("Serializable")))
            {
                throw new Exception("The input parameter must be serializable");
            }
            FieldInfo[] fields = t.GetFields();
            builder.Append("{\n");
            foreach (FieldInfo fil in fields)
            {
                string attr = fil.Attributes.ToString();
                if (!(attr.Contains("NotSerialized")) && (attr.Contains("Public")))
                {
                    ConstructField(fil);
                }
            }
            builder.Remove(builder.Length - 2, 1);
            builder.Append("}\n");
        }

        private void ConstructField(FieldInfo fieldInfo)
        {
            string valueStr = null;
            if (fieldInfo.FieldType.IsArray)
            {
                var arrayBuilder = new StringBuilder();
                arrayBuilder.Append("[");
                var arr = fieldInfo.GetValue(Obj) as Array;
                foreach (var element in arr)
                {
                    arrayBuilder.Append(string.Format("{0}, ", ConstructValue(element)));
                }
                arrayBuilder.Remove(arrayBuilder.Length - 2, 2);
                arrayBuilder.Append("]");
                valueStr = arrayBuilder.ToString();
            }
            else
            {
                valueStr = ConstructValue(fieldInfo.GetValue(Obj));
            }
            if (valueStr == null)
            {
                throw new Exception("Unsupported data type");
            }
            builder.Append(string.Format("\"{0}\": {1},\n", fieldInfo.Name, valueStr));
        }

        private string ConstructValue(Object val)
        {
            if (typeof(string) == val.GetType())
            {
                return string.Format("\"{0}\"", val.ToString());
            }
            if (typeof(Boolean) == val.GetType())
            {
                return val.ToString().ToLower();
            }

            return val.ToString();
        }

        public String GetResult()
        {
            return builder.ToString();
        }
    }
}
