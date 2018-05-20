using System;
using System.Diagnostics;
using FCM_TEST.Models;
using Microsoft.AspNetCore.Mvc;

namespace FCM_TEST.Controllers
{
    public class HomeController : Controller
    {
        private readonly string storagePath = "D:\\UserTokens.txt";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StoreToken(string token)
        {
            StoreUserFcmTokenAsync(token);
            return Ok();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        private async void StoreUserFcmTokenAsync(string token)
        {
            if (System.IO.File.Exists(storagePath))
                await System.IO.File.AppendAllTextAsync(storagePath, token + Environment.NewLine);
            else
                await System.IO.File.WriteAllTextAsync(storagePath, token + Environment.NewLine);
        }
    }
}