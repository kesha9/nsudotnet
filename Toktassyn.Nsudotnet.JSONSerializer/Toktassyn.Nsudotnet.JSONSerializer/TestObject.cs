using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Toktassyn.Nsudotnet.JSONSerializer
{
    [Serializable]
    class TestObject
    {
        public int a;
        public string s;

        [NonSerialized]
        public string ignore; // это поле не должно сериализоваться

        public string[] arrayMember;
        public bool b;

        public TestObject()
        {
            a = 5;
            b = true;
            s = "Hahaha";
            ignore = "IgnoreMe";
            arrayMember = new string[] { "1", "3", "5", "7", "9" };
         
        }
    }
}
