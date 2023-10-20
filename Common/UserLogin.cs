using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyShop
{
    [Serializable]
    public class UserLogin
    {
        public int UserId { get; set; }
        public string UserName { get; set;}
        public string Name { get; set;}

    }
}