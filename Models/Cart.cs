using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyShop.Models
{
    [Serializable]
    public class Cart
    {
        public Product product { get; set; }
        public int Quantity { get; set; }

    }
}