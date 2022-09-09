using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinalProj.Models
{
    public class ItemResponse
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public string ActionName { get; set; }
    }
}
