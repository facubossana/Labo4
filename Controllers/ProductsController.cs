using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalLaboIV.Data;
using FinalLaboIV.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace FinalLaboIV.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Home()
        {
            var applicationDbContext =
                _context.Product
                .Include(p => p.Brand)
                .Include(p => p.Category).Where(p => p.Favourite == true);

            var admin = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Administrator");
            if (!_context.Roles.Contains(admin))
            {
                _context.Roles.Add(new IdentityRole("Administrator"));
            }

            var everyone = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Everyone");
            if (!_context.Roles.Contains(everyone))
            {
                _context.Roles.Add(new IdentityRole("Everyone"));
            }

            _context.SaveChanges();

            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchByName, string searchByCategory)
        {
            var applicationDbContext =
                _context.Product
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.SupplierProduct).ThenInclude(sp => sp.Supplier);

            if (!String.IsNullOrEmpty(searchByName))
            {
                return View(applicationDbContext.Where(b => b.Name.Contains(searchByName)));
            }

            if (!String.IsNullOrEmpty(searchByCategory))
            {
                return View(applicationDbContext.Where(b => b.Category.Description.Contains(searchByCategory)));
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.SupplierProduct).ThenInclude(sp => sp.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Description");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description");

            PopulateSuppliersDropDownList();

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,Favourite,CategoryId,BrandId,Photo,SupplierProduct")] Product product, string[] selectedSuppliers)
        {
            if (ModelState.IsValid)
            {
                var imageFiles = HttpContext.Request.Form.Files;
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    var image = imageFiles[0];
                    if (image.Length > 0)
                    {
                        var rootPath = Path.Combine(env.WebRootPath, "images\\products");

                        // generar nombre aleatorio de fotografia
                        var imageFile = Guid.NewGuid().ToString();
                        imageFile = imageFile.Replace("-", "");
                        imageFile += Path.GetExtension(image.FileName);

                        var finalPath = Path.Combine(rootPath, imageFile);

                        using (var filestream = new FileStream(finalPath, FileMode.Create))
                        {
                            image.CopyTo(filestream);
                            product.Photo = imageFile;
                        };
                    }
                }

                CreateProductSupplier(selectedSuppliers, product);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Description", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);

            PopulateSuppliersDropDownList(product);

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.SupplierProduct).ThenInclude(sp => sp.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Description", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);

            PopulateSuppliersDropDownList(product);

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,Favourite,CategoryId,BrandId,Photo,SupplierProduct")] Product product, string[] selectedSuppliers)
        {
            var oldProduct = await _context.Product
                .Where(p => p.Id == product.Id)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.SupplierProduct).ThenInclude(sp => sp.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (id != product.Id)
            {
                return NotFound();
            }

            //string[] selectedSuppliers = Request.Form["selectedSuppliers"];

            if (product.Photo == null)
            {
                if (oldProduct.Photo != null)
                {
                    product.Photo = oldProduct.Photo;
                }
            }

            if (ModelState.IsValid)
            {
                var imageFiles = HttpContext.Request.Form.Files;
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    var image = imageFiles[0];
                    if (image.Length > 0)
                    {
                        var rootPath = Path.Combine(env.WebRootPath, "images\\products");

                        // generar nombre aleatorio de fotografia
                        var imageFile = Guid.NewGuid().ToString();
                        imageFile = imageFile.Replace("-", "");
                        imageFile += Path.GetExtension(image.FileName);

                        var finalPath = Path.Combine(rootPath, imageFile);

                        if (!String.IsNullOrEmpty(product.Photo))
                        {
                            string previousPicture = Path.Combine(rootPath, product.Photo);
                            if (System.IO.File.Exists(previousPicture))
                            {
                                System.IO.File.Delete(previousPicture);
                            }
                        }

                        using (var filestream = new FileStream(finalPath, FileMode.Create))
                        {
                            image.CopyTo(filestream);
                            product.Photo = imageFile;
                        };
                    }
                }

                try
                {
                    product.SupplierProduct.Clear();
                    CreateProductSupplier(selectedSuppliers, product);

                    _context.Remove(oldProduct);
                    _context.Add(product);

                    //_context.Update(product);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Description", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);

            CreateProductSupplier(selectedSuppliers, product);

            PopulateSuppliersDropDownList(product);

            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        private void PopulateSuppliersDropDownList(Product product = null)
        {
            var suppliersQuery = from d in _context.Supplier
                                 orderby d.Name
                                 select d;

            SelectList newSelectList = new SelectList(suppliersQuery.AsNoTracking(), "Id", "Name");

            if (product != null)
            {
                var productSuppliers = _context.SuppliersProducts.Where(sp => sp.ProductId == product.Id).Select(sp => sp.Supplier).ToList();

                foreach (var item in newSelectList)
                {
                    foreach (var supplier in productSuppliers)
                    {
                        if (supplier.Id.ToString() == item.Value)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }

            ViewBag.Suppliers = newSelectList;
        }

        private void CreateProductSupplier(string[] selectedSuppliers, Product product)
        {
            if (selectedSuppliers == null)
            {
                product.SupplierProduct = new List<SupplierProduct>();
            }

            var selectedSuppliersHS = new HashSet<string>(selectedSuppliers);

            foreach (var supplier in _context.Supplier)
            {
                if (selectedSuppliersHS.Contains(supplier.Id.ToString()))
                {
                    SupplierProduct newSupplierProduct = new SupplierProduct { Product = product, Supplier = supplier, ProductId = product.Id, SupplierId = supplier.Id };
                    product.SupplierProduct.Add(newSupplierProduct);
                }
            }
        }
    }
}
