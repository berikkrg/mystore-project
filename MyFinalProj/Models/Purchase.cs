using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MyFinalProj.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public DateTime? PurchaseStart { get; set; }

        public DateTime? PurchaseEnd { get; set; }
        //добавить в класс юзера покупки
    }
}
