using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinalProj.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage="Please input a Category Name")]
        public string CategoryName { get; set; }
        public List<Item> Items { get; set; }

        public Category()
        {
            Items = new List<Item>();
        }


    }
}
