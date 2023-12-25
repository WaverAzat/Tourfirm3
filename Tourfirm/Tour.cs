using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourfirm
{
    internal class Tour
    {
        public int id;
        public string country;
        public string fly;
        public string data;
        public string night;
        public string tourist;
        public string food;
        public string star;

        public void Clear()
        {
            id = 0;
            country = string.Empty;
            fly = string.Empty;
            data = string.Empty;
            night = string.Empty;
            tourist = string.Empty;
            food = string.Empty;
            star = string.Empty;
        }
    }
}
