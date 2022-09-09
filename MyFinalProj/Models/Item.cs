using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinalProj.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        [Required(ErrorMessage = "Please input a name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please input a cost")]
        public int Cost { get; set; }
        [Required(ErrorMessage = "Please input a model")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Please input a manufacturer's name")]
        public string Manufacturer { get; set; }
        [Required(ErrorMessage = "Please input a description")]
        public string  Description { get; set; }
        public int? AmountToBuy { get; set; }
        [Required(ErrorMessage = "Please input an amount available")]
        public int AmountAvailable { get; set; }
        public string PicturePath { get; set; }
       // [Required(ErrorMessage = "Please choose the category")]
        public Category Category { get; set; }

        public Cart Cart { get; set; }

        public Purchase Purchase { get; set; }
        public Seller Seller { get; set; }

    }
}
