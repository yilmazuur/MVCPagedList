using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MVCPagedList.App.Data;
using MVCPagedList.App.Data.DataModel;
using MVCPagedList.App.Data.Helpers;
using MVCPagedList.App.Data.Interfaces;

namespace MVCPagedList
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UpdateCache();
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(new TimeSpan(0, 0, 10)); //belli aralıklarla cachei güncelleyecek bir thread oluşturuldu
                    UpdateCache();
                }
            });
           
        }

        private static void UpdateCache()
        {
            using (IGenericRepository<ProductModel2> m_Repository = new GenericRepository<ProductModel2>())
            {
                IEnumerable<ProductModel2> products = m_Repository.SelectAll();
                CacheHelper.PutCache("products", products);
            }
        }
    }
}
