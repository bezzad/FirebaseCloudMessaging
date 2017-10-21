using System;
using System.Web.Mvc;

namespace FCM.Controllers
{
    public class HomeController : Controller
    {
        string storagePath = "D:\\UserTokens.txt";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StoreToken(string token)
        {
            StoreUserFcmTokenAsync(token);
            return new HttpStatusCodeResult(200);
        }

        private void StoreUserFcmTokenAsync(string token)
        {
            if (System.IO.File.Exists(storagePath))
            {
                System.IO.File.AppendAllText(storagePath, token + Environment.NewLine);
            }
            else
            {
                System.IO.File.WriteAllText(storagePath, token + Environment.NewLine);
            }
        }
    }
}