using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinalProj.Models
{
    public class Seller
    {
        public int SellerId { get; set; }
        [Required(ErrorMessage ="Please input the name")]
        public string Name { get; set; }

        public List<Item> Items { get; set; }

        public Seller()
        {
            Items = new List<Item>();
        }
    }
}
