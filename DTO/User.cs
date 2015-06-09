using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTO
{
    public class User
    {
        public User(int ID, string userName, string name, string surname,string password)
        {
            this.ID = ID;
            this.userName = userName;
            this.name = name;
            this.surName = surName;
            this.password = password;
        }

        public int ID { get; set; }
        public string userName { get; set; }
        public string name { get; set; }
        public string surName { get; set; }
        public string password { get; set; }
    }
}