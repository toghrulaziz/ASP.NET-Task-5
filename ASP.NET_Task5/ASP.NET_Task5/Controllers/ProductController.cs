using ASP.NET_Task5.Data;
using ASP.NET_Task5.Helpers;
using ASP.NET_Task5.Models;
using ASP.NET_Task5.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ASP.NET_Task5.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ProductController(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        // get all 
        public IActionResult Index()
        {
            var products = _appDbContext.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        // add 
        public IActionResult Add()
        {
            var categories = _appDbContext.Categories.ToList();
            var tags = _appDbContext.Tags.ToList();
            ViewData["Categories"] = categories;
            ViewData["Tags"] = tags;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newProduct = _mapper.Map<Product>(product);

                    var tags = _appDbContext.Tags.Where(t => product.TagIds.Contains(t.Id)).ToList();
                    newProduct.Tags = tags;


                    if (product.ImageUrl != null)
                    {
                        newProduct.ImageUrl = await UploadFileHelper.UploadFile(product.ImageUrl);
                    }

                    _appDbContext.Products.Add(newProduct);
                    await _appDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(product);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "The file type is not accepted.";
                return View(product);
            }
        }



        // delete
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is not null)
            {
                _appDbContext.Products.Remove(product);
                await _appDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        // get
        public async Task<IActionResult> Get(int id)
        {
            var product = await _appDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        // update 
        public async Task<IActionResult> Update(int id)
        {
            var product = await _appDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            var categories = _appDbContext.Categories.ToList();
            var tags = _appDbContext.Tags.ToList();

            ViewData["Categories"] = categories;
            ViewData["Tags"] = tags;

            var editViewModel = _mapper.Map<EditProductViewModel>(product);
            

            return View(editViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Update(EditProductViewModel updatedProduct)
        {
            if (ModelState.IsValid)
            {
                var productToUpdate = await _appDbContext.Products
                    .Include(p => p.Tags)
                    .FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

                if (productToUpdate == null)
                {
                    return NotFound();
                }

                _mapper.Map(updatedProduct, productToUpdate);
                productToUpdate.ImageUrl = await UploadFileHelper.UploadFile(updatedProduct.ImageUrl);
               


                if (updatedProduct.TagIds != null && updatedProduct.TagIds.Any())
                {
                    productToUpdate.Tags.Clear();

                    var tags = await _appDbContext.Tags
                        .Where(tag => updatedProduct.TagIds.Contains(tag.Id))
                        .ToListAsync();

                    productToUpdate.Tags.AddRange(tags);
                }

                await _appDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return View(updatedProduct);
            }
        }



    }
}
