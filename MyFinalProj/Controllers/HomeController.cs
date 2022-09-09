using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFinalProj.Data;
using MyFinalProj.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using RestSharp;
using System.ComponentModel;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace MyFinalProj.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private StoreDbContext db;
        private CookieOptions options = new CookieOptions();
        public HomeController (StoreDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            db = context;
            if (db.Items.Count()==0)
            {
                //Filling the database with random data in case if the database is empty
                Category myCat = new Category { CategoryName = "MyCat" };
                Seller seller = new Seller { Name = "Seller" };
                List<Item> items = new List<Item>()
                {
                    new Item { AmountAvailable = 100, AmountToBuy = 0, Cost = 2,
                    Manufacturer = "Some Factory", Description = "You've got to know someting about old memes", Model = "Natural",
                    Name = "Bananas", Seller = seller, Category = myCat, PicturePath="/files/bananas.jpg" },
                    new Item { AmountAvailable = 100, AmountToBuy = 0, Cost = 1,
                    Manufacturer = "Some Factory", Description = "You've got to know something about old memes", Model = "Some",
                    Name = "People", Seller = seller, Category = myCat,PicturePath="/files/people.jpg" },
                    new Item { AmountAvailable = 100, AmountToBuy = 0, Cost = 1,
                    Manufacturer = "Some Factory", Description = "You've got to know something about old memes", Model = "Some",
                    Name = "Just kidding, not bananas", Seller = seller, Category = myCat, PicturePath="/files/justkidding.jpg" },
                    new Item { AmountAvailable = 10, AmountToBuy = 0, Cost = 5,
                    Manufacturer = "Some Manufacturer", Description = "Lorem ipsum dolor sit amet, " +
                    "consectetur adipiscing elit. Donec a elit eu purus volutpat laoreet.", Model = "Some",
                    Name = "SomeItem", Seller = seller, Category = myCat, PicturePath="/files/nopic.jpg" },
                     new Item { AmountAvailable = 10, AmountToBuy = 0, Cost = 5,
                    Manufacturer = "Some Manufacturer", Description = "Lorem ipsum dolor sit amet, " +
                    "consectetur adipiscing elit. Donec a elit eu purus volutpat laoreet.", Model = "Some",
                    Name = "SomeItem", Seller = seller, Category = myCat, PicturePath="/files/nopic.jpg" },
                      new Item { AmountAvailable = 10, AmountToBuy = 0, Cost = 5,
                    Manufacturer = "Some Manufacturer", Description = "Lorem ipsum dolor sit amet, " +
                    "consectetur adipiscing elit. Donec a elit eu purus volutpat laoreet.", Model = "Some",
                    Name = "SomeItem", Seller = seller, Category = myCat, PicturePath="/files/nopic.jpg" },
                       new Item { AmountAvailable = 10, AmountToBuy = 0, Cost = 5,
                    Manufacturer = "Some Manufacturer", Description = "Lorem ipsum dolor sit amet, " +
                    "consectetur adipiscing elit. Donec a elit eu purus volutpat laoreet.", Model = "Some",
                    Name = "SomeItem", Seller = seller, Category = myCat, PicturePath="/files/nopic.jpg" },
                        new Item { AmountAvailable = 10, AmountToBuy = 0, Cost = 5,
                    Manufacturer = "Some Manufacturer", Description = "Lorem ipsum dolor sit amet, " +
                    "consectetur adipiscing elit. Donec a elit eu purus volutpat laoreet.", Model = "Some",
                    Name = "SomeItem", Seller = seller, Category = myCat, PicturePath="/files/nopic.jpg" },
            };

                db.Categories.Add(myCat);
                db.Sellers.Add(seller);
                db.Items.AddRange(items);
                db.SaveChanges();



            }
        }

        /// <summary>
        /// Returns ItemResponse object that contains list of items, current page, total amount of pages and name of action
        /// </summary>
        /// <param name="items">List of items to show in a page </param>
        /// <param name="currentPage">Current page from "_Pagination" Partial View</param>
        /// <param name="actionName">Name of the action  to call from "_Pagination" Partial View</param>
        /// <returns>ItemResponse object</returns>
        private ItemResponse GetResponse(List<Item> items, int currentPage, string actionName)
        {
            //Amount of items at one page
            var pageResults = 6f;
            //Total amount of pages
            var pagesAmount = Math.Ceiling(items.Count() / pageResults);
            // List of items to show in current page, skipped previous pages, and took amount of pages equal to pageResults (6)
            items = items
                .Skip((currentPage - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();

            var response = new ItemResponse
            {
                Items = items,
                CurrentPage = currentPage,
                Pages = (int)pagesAmount,
                ActionName = actionName
            };

            return response;
        }

        /// <summary>
        /// Returns list of Ids from cookie by splitting and parsing cookies accepted in parameter 
        /// </summary>
        /// <param name="cookie">Сookie to get ids</param>
        /// <returns>List of Ids</returns>
        public List<int> GetIdFromCookie (string cookie)
        {
            List<int> ids = new List<int>();

            string[] idsParsed = cookie.Split(',');

            foreach (var id in idsParsed)
            {
                int idInt;
                Int32.TryParse(id, out idInt);
                
                ids.Add(idInt);
            }
            IEnumerable<int> uniqueIds = ids.Distinct<int>();
            return uniqueIds.ToList();

        }

        /// <summary>
        /// Returns dictionary of Id-Quantity by splitting and parsing cookies accepted in parameter 
        /// </summary>
        /// <param name="cookie">cookie to get Id-Quantity dictionary from</param>
        /// <returns>Dictionary of id-quantity</returns>
        public Dictionary<int, int> GetIdQuantityByCookie(string cookie)
        {
            List<string> idQuantitiesParsed = cookie.Split(',').ToList();
            int id;
            int quantity;
            Dictionary<int, int> idQuantity = new Dictionary<int, int>();
            foreach (var item in idQuantitiesParsed)
            {
                if (item=="")
                {
                    break;
                }
                Int32.TryParse(item.Split('-')[0], out id);
                Int32.TryParse(item.Split('-')[1], out quantity);
                if (idQuantity.ContainsKey(id))
                {
                    idQuantity.Remove(id);
                }
                idQuantity.Add(id, quantity);
            }
            return idQuantity;
        }
        
        public IActionResult Index(int page)
        {
            List<Item> items = db.Items.ToList();
            ItemResponse response = GetResponse(items, page, "Index");
            return View("ShowItems", response);
        }

        public IActionResult ShowItems()
        {
             return View();
        }

        public IActionResult Search(string searchQuery, int page)
        {
            options.Expires = DateTime.Now.AddHours(1);
            ViewBag.EmptyMessage = "There are no products that match your search. We are sorry";
            if (searchQuery == null)
            {
                searchQuery = Request.Cookies["SearchQuery"];
            }
            else
            {
                Response.Cookies.Append($"SearchQuery", searchQuery, options);
            }
            List<Item> foundItems = db.Items.Where(x => x.Name.Contains(searchQuery) || x.Category.CategoryName.Contains(searchQuery)).ToList();
            ItemResponse response = GetResponse(foundItems, page, "Search");
            return View("ShowItems", response);
        }

        public IActionResult ItemsByCategory (int requiredCategoryId, int page)
        {
            options.Expires = DateTime.Now.AddHours(1);
            ViewBag.EmptyMessage = "There are no products in this category. We are sorry";
            if (requiredCategoryId == 0)
            {
                requiredCategoryId = int.Parse(Request.Cookies["CategoryId"]);
            }
            else
            {
                Response.Cookies.Append($"CategoryId", requiredCategoryId.ToString(), options);
            }
            List<Item> items = db.Items.Where(x => x.Category.CategoryId == requiredCategoryId).ToList();
            ItemResponse response = GetResponse(items, page, "ItemsByCategory");
            return View("ShowItems", response);
        }

        public IActionResult ItemPage(int id)
        {
            Item item = db.Items.FirstOrDefault(x => x.ItemId == id);
            item.Category=db.Items.FirstOrDefault(x=>x.ItemId==id).Category;
            return View(item);
        }
        
        public IActionResult Cart (int id, int quantity, int itemFromCartView)
        {
            Item item = new Item();
            Cart cart = new Cart();
            ViewBag.EmptyMessage = "Your cart is empty now. Please come back to shop and add something to cart";
            //Cookies will be expired after 1 hour or after log out of user
            options.Expires = DateTime.Now.AddHours(1);
            var itemIdsFromCookies = Request.Cookies["ItemIds"];
            //stores existing Ids and a new id of a chosen item in the shop
            string newItemIds = "";
            string cookieIdQuantity = "";
            Dictionary<int, int> idQuantityDict = new Dictionary<int, int>();
            int totalPayment = 0;
            if (itemIdsFromCookies != null)
            {
                newItemIds += itemIdsFromCookies.ToString() + $"{id}";
            }
            else
            {
                item = db.Items.FirstOrDefault(x => x.ItemId == id);
            }
            Response.Cookies.Append("ItemIds", $"{newItemIds},", options);
            List<int> ids = GetIdFromCookie(newItemIds);
            cookieIdQuantity = Request.Cookies[$"idQuantity"];
            if (cookieIdQuantity != null)
            {
                cookieIdQuantity += $"{itemFromCartView}-{quantity},";
            }
            else
            {
                quantity = 1;
                cookieIdQuantity = $"{itemFromCartView}-{quantity},";
            }

            Response.Cookies.Append($"idQuantity", cookieIdQuantity, options);
            // key=Id of item, value=Quantity (Amount to buy) of item
            idQuantityDict = GetIdQuantityByCookie(cookieIdQuantity);
            foreach (var idFromCookies in ids.Skip(1))
            {
                item = db.Items.FirstOrDefault(x => x.ItemId == idFromCookies);
              
                foreach (var dictItem in idQuantityDict)
                {
                    if (item.ItemId == dictItem.Key)
                    {
                        item.AmountToBuy = dictItem.Value;
                    }
                }
                totalPayment += item.Cost * (int)item.AmountToBuy;
                cart.AmountToPay = totalPayment;
                cart.Items.Add(item);
            }
            return View(cart);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
