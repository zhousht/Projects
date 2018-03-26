using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderDAL;
using OrderMng.Models.ProductsViewModels;

namespace OrderMng.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ModelDbContext _context;

        public ProductsController(ModelDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.Categories = GetCategories();
            Product product = new Product();
            product.Active = true;
            return View(product);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Discription,Price,CategoryId,Active")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = GetCategories();

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Discription,Price,CategoryId,Active")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductId == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        public IList<Category> GetCategories()
        {
            return _context.Categories.ToList();

        }

        [HttpPost]
        public ActionResult CheckoutShoppingCart(IEnumerable<OrderMng.Models.ProductsViewModels.ShoppingCartViewModel> id)
        {
            //List<ShoppingCartViewModel> list = ParseShoppingCartData(id);
            return null;
        }

        [HttpGet]
        public ActionResult GetShoppingCartList(string id)
        {
            List<ShoppingCartViewModel> list = ParseShoppingCartData(id);
            return View(list);
        }

        private List<ShoppingCartViewModel> ParseShoppingCartData (string json)
        {
            var cart = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
            List<int> proudctIds = GetProductIds(cart);
            var products = _context.Products.Where(p=> proudctIds.Contains(p.ProductId)).ToList<Product>();
            return GetShoppingCartList(cart, products);
        }

        private List<int> GetProductIds(Dictionary<string, string> list)
        {
            List<int> result = new List<int>();
            int num;
            foreach(string key in list.Keys)
            {
                num = GetProductIdFromKey(key);
                result.Add(num);
            }
            return result;
        }


        private List<ShoppingCartViewModel> GetShoppingCartList(Dictionary<string, string> cart, List<Product> products)
        {
            ShoppingCartViewModel item = null;
            Product product = null;
            List<ShoppingCartViewModel> list = new List<ShoppingCartViewModel>();
            foreach(string key in cart.Keys)
            {
                item = new ShoppingCartViewModel();
                product = GetProductByKey(key, products);
                if (product != null)
                {
                    item.ProductId = product.ProductId;
                    item.ProductName = product.Name;
                    item.Price = product.Price;
                    item.Count = GetProductCount(key, cart);
                    list.Add(item);
                }
            }

            return list;
        }

        private int GetProductCount(string key, Dictionary<string, string> cart)
        {
            string str = cart[key];
            int count = 0;
            try
            {
                count = int.Parse(str);
            }
            catch (Exception) { }
            return count;
        }

        private int GetProductIdFromKey(string key)
        {
            int id = 0;
            try
            {
                key = key.ToLower();
                if (key.StartsWith("product"))
                {
                    id=int.Parse(key.Substring(7));
                }
            }
            catch (Exception) { }
            return id;
        }

        private Product GetProductByKey(string key, List<Product> products)
        {
            int id = GetProductIdFromKey(key);
            foreach(Product p in products)
            {
                if(p.ProductId == id)
                {
                    return p;
                }
            }
            return null;
        }
    }
}
