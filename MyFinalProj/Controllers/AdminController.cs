using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFinalProj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinalProj.Controllers
{
    //Uncomment it when you create a user with "Admin" access rights
    //[Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private StoreDbContext db;
        private IWebHostEnvironment _appEnvironment;
        private List<string> sellerNames = new List<string>();
        private List<string> categoryNames = new List<string>();
        public AdminController(StoreDbContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }
        /// <summary>
        /// Returns a list of category names from "Categories" table in the database
        /// </summary>
        /// <returns>List of category names</returns>
        public List<string> GetCategoryNames()
        {
            foreach (var cat in db.Categories.ToList())
            {
                categoryNames.Add(cat.CategoryName);
            }

            return categoryNames;
        }
        /// <summary>
        /// Returns a list of seller's names from "Sellers" table in the database
        /// </summary>
        /// <returns>List of seller's names</returns>
        public List<string> GetSellerNames()
        {
            foreach (var seller in db.Sellers.ToList())
            {
                sellerNames.Add(seller.Name);
            }

            return sellerNames;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddItem()
        {
            ViewBag.Sellers = new SelectList (GetSellerNames());
            ViewBag.Categories = new SelectList(GetCategoryNames());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(Item item, IFormFile uploadedFile, string category, string seller)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    //path to the file uploaded in the "AddItem" View
                    string path = "/files/" + uploadedFile.FileName;
                    using (var fs = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fs);
                    }
                    //path to the file to store in the database
                    item.PicturePath = path;
                }
                else
                {
                    //path to a placeholder picture in case of admin didn't upload picture in the view
                    item.PicturePath = "/files/nopic.jpg";
                }
                item.Category = db.Categories.FirstOrDefault(p => p.CategoryName == category);
                item.Seller = db.Sellers.FirstOrDefault(p => p.Name == seller);
                item.AmountToBuy = 0;
                db.Add(item);
                db.SaveChanges();
                return View("Index", item);
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult AddCat() => View();

        [HttpPost]
        public IActionResult AddCat(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Add(category);
                db.SaveChanges();
                return View("Index", category);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddSeller() => View();

        [HttpPost]
        public IActionResult AddSeller(Seller seller)
        {
            if (ModelState.IsValid)
            {
                db.Add(seller);
                db.SaveChanges();
                return View("Index", seller);
            }
            else
            {
                return View();
            }
        }

        public IActionResult ListItem() => View(db.Items.ToList());

        [HttpGet]
        public IActionResult EditItem(int id)
        {
            sellerNames = GetSellerNames();
            categoryNames = GetCategoryNames();
            Item item = db.Items.FirstOrDefault(x => x.ItemId == id);
            ViewBag.Sellers = new SelectList(sellerNames);
            ViewBag.Categories = new SelectList(categoryNames);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditItem(Item item, int id, IFormFile uploadedFile, string category, string seller)
        {
            Item itemToChange = db.Items.FirstOrDefault(x => x.ItemId == id);
            if (ModelState.IsValid)
            {
                if (item != null)
                {
                    itemToChange.Name = item.Name == null ? (itemToChange.Name) : (item.Name);
                    itemToChange.Model = item.Model == null ? (itemToChange.Model) : (item.Model);
                    itemToChange.Manufacturer = item.Manufacturer == null ? (itemToChange.Manufacturer) : (item.Manufacturer);
                    itemToChange.AmountAvailable = item.AmountAvailable == 0 ? (itemToChange.AmountAvailable) : (item.AmountAvailable);
                    itemToChange.Cost = item.Cost == 0 ? (itemToChange.Cost) : (item.Cost);
                }
                if (uploadedFile != null)
                {
                    string path = "/files/" + uploadedFile.FileName;
                    using (var fs = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fs);
                    }
                    itemToChange.PicturePath = path;
                }
                if (category != null)
                {
                    itemToChange.Category = db.Categories.FirstOrDefault(p => p.CategoryName == category);
                }
                if (seller != null)
                {
                    itemToChange.Seller = db.Sellers.FirstOrDefault(p => p.Name == seller);
                }
                db.Update(itemToChange);
                db.SaveChanges();
                return View("ListItem", db.Items.ToList());
            }
            else
            {
                return RedirectToAction("EditItem");
            }
            
        }

        [HttpPost]
        public IActionResult DeleteItem(int id)
        {
            Item item = db.Items.FirstOrDefault(x => x.ItemId == id);
            db.Remove(item);
            db.SaveChanges();
            return RedirectToAction("ListItem");
        }

        public IActionResult ListCat() => View(db.Categories.ToList());

        [HttpGet]
        public IActionResult EditCat(int id)
        {
            Category category = db.Categories.FirstOrDefault(x => x.CategoryId == id);
            return View(category);
        }
            
        [HttpPost]
        public IActionResult EditCat(int id, string newName)
        {
            if (!string.IsNullOrEmpty(newName))
            {
                Category category = db.Categories.FirstOrDefault(x => x.CategoryId == id);
                category.CategoryName = newName;
                db.Update(category);
                db.SaveChanges();
                return RedirectToAction("ListCat");
            }
            else
            {
                return RedirectToAction("EditCat");
            }
        }

        [HttpPost]
        public IActionResult DeleteCat(int id)
        {
            Category category = db.Categories.FirstOrDefault(x => x.CategoryId == id);
            List<Item> items = db.Items.Where(x => x.Category.CategoryId == id).ToList();
            foreach (var item in items)
            {
                item.Category = null;
            }
            db.UpdateRange(items);
            db.Remove(category);
            db.SaveChanges();
            return RedirectToAction("ListCat");
        }

        public IActionResult ListSeller() => View(db.Sellers.ToList());

        [HttpGet]
        public IActionResult EditSeller(int id)
        {
            Seller seller = db.Sellers.FirstOrDefault(x => x.SellerId == id);
            return View(seller);
        }

        [HttpPost]
        public IActionResult EditSeller(int id, string newName)
        {
            if (!string.IsNullOrEmpty(newName))
            {
                Seller seller = db.Sellers.FirstOrDefault(x => x.SellerId == id);
                seller.Name = newName;
                db.Update(seller);
                db.SaveChanges();
                return RedirectToAction("ListSeller");
            }
            else
            {
                return RedirectToAction("EditSeller");
            }   
        }

        [HttpPost]
        public IActionResult DeleteSeller(int id)
        {
            Seller seller= db.Sellers.FirstOrDefault(x => x.SellerId== id);
            List<Item> items = db.Items.Where(x => x.Seller.SellerId == id).ToList();
            foreach (var item in items)
            {
                item.Seller = null;
            }
            db.UpdateRange(items);
            db.Remove(seller);
            db.SaveChanges();
            return RedirectToAction("ListSeller");
        }


       
    }
}
