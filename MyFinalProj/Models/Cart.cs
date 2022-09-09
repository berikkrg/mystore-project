using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinalProj.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public List<Item> Items { get; set; }

        public int AmountOfItem { get; set; }

        public int AmountToPay { get; set; }
        public Cart ()
        {
            Items = new List<Item>();

        }
    }
}
