using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourfirm
{
    internal class Person
    {
        public int id;
        public string surname;
        public string name;
        public string father;
        public string birthday;
        public string email;
        public string phone;
        public string login;
        public string password;

        public void Clear()
        {
            id = 0;
            surname = string.Empty;
            name = string.Empty;
            father = string.Empty;
            birthday = string.Empty;
            email = string.Empty;
            phone = string.Empty;
            login = string.Empty;
            password = string.Empty;

        }
    }
}
