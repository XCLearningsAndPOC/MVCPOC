using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        public ProductManagerController(IRepository<Product> productRepository, 
            IRepository<ProductCategory> productCategoryRepository)
        {
            context = productRepository;
            productCategories = productCategoryRepository;
        }


        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();

            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel productManagerViewModel = new ProductManagerViewModel();

            productManagerViewModel.Product = new Product();
            productManagerViewModel.ProductCategories = productCategories.Collection();

            return View(productManagerViewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase httpPostedFileBase)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (httpPostedFileBase != null)
                {
                    product.Image = product.Id + Path.GetExtension(httpPostedFileBase.FileName);
                    httpPostedFileBase.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel productManagerViewModel = new ProductManagerViewModel();

                productManagerViewModel.Product = product;
                productManagerViewModel.ProductCategories = productCategories.Collection();

                return View(productManagerViewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase httpPostedFileBase)
        {
            Product productToEdit = context.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                if (httpPostedFileBase != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(httpPostedFileBase.FileName);
                    httpPostedFileBase.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }

                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;                
                productToEdit.Price = product.Price;
                productToEdit.Name = product.Name;

                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);

                context.Commit();

                return RedirectToAction("Index");
            }
        }

    }
}