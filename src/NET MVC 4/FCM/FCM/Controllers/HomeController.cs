using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FCM.Controllers
{
    public class HomeController : Controller
    {
        private FileInfo TokensFile { get; } = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"fcm\tokens.txt"));

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StoreToken(string token)
        {
            StoreUserFcmToken(token);
            return new HttpStatusCodeResult(200);
        }

        private void StoreUserFcmToken(string token)
        {
            if (!TokensFile.Exists)
            {
                TokensFile.Directory?.Create();
                TokensFile.Create();
            }

            var tokens = System.IO.File.ReadAllLines(TokensFile.FullName, Encoding.UTF8).ToList();

            if (tokens.All(t => t != token))
                tokens.Add(token);
            
            System.IO.File.WriteAllLines(TokensFile.FullName, tokens);
        }
    }
}