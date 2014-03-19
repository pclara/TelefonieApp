using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeViewDemo.Models
{
    public class Site
    {
        public long? id;
        public string denumire;
        public long? parinteJudet;
                        public List<Judet> lista;
                        public Site()
                        {
                            lista = new List<Judet>();
                        }
        public class Judet
        {
            public long? id;
            public string denumire;
            public Judet(long? i, string d)
            {
                id = i;
                denumire = d;
            }
        }
    }
}