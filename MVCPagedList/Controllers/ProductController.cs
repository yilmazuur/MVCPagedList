using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCPagedList.App.Data.DataModel;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Infrastructure;
using MVCPagedList.App.Data.Interfaces;
using MVCPagedList.App.Data;
using System.Web.Caching;
using System.Net;
using MVCPagedList.App.Data.Helpers;

namespace MVCPagedList.Controllers
{
    public class ProductController : Controller
    {
        private IEnumerable<ProductModel2> products = CacheHelper.GetCachedData<IEnumerable<ProductModel2>>("products");
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var productList = from s in products
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                productList = productList.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    productList = productList.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    productList = productList.OrderBy(s => s.LastUpdatedTime);
                    break;
                case "date_desc":
                    productList = productList.OrderByDescending(s => s.LastUpdatedTime);
                    break;
                default:  // Name ascending 
                    productList = productList.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(productList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, LastUpdatedTime")]ProductModel2 product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (IGenericRepository<ProductModel2> m_Repository = new GenericRepository<ProductModel2>())
                    {
                        m_Repository.Insert(product);
                        m_Repository.Save("products");
                        //IEnumerable<ProductModel> updatedProducts = m_Repository.SelectAll();
                        //CacheHelper.UpdateCache("products", updatedProducts);
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pr = products.Where(x => x.ID == id).FirstOrDefault();
            if (pr == null)
            {
                return HttpNotFound();
            }
            return View(pr);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "ID, Name")]ProductModel2 product)
        {
            var productToUpdate = products.Where(x => x.ID == product.ID).FirstOrDefault();
            productToUpdate.Name = product.Name;
            productToUpdate.LastUpdatedTime = DateTime.Now;
            try
            {
                using (IGenericRepository<ProductModel2> m_Repository = new GenericRepository<ProductModel2>())
                {
                    m_Repository.Update(productToUpdate);
                    m_Repository.Save("products");
                    //IEnumerable<ProductModel> updatedProducts = m_Repository.SelectAll();
                    //CacheHelper.UpdateCache("products", updatedProducts);
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(productToUpdate);
        }
    }
}