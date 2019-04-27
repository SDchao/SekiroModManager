using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekiroModManager.Manager
{
    class ModInformation
    {
        public string name;
        public string time;
        public string size;
        public List<string> contain = new List<string>();

        public ModInformation(string n,string t,string s,List<string> c)
        {
            name = n;
            time = t;
            size = s;
            contain = c;
        }
    }
}
