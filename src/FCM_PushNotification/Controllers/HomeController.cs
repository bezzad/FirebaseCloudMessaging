using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using FCM_PushNotification.Models;

namespace FCM_PushNotification.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendMessage()
        {
            SendPushNotification();
            return Ok();
        }

        public static void SendPushNotification()
        {
            try
            {
                var serverKey = "AAAApXBVEZE:APA91bH56ib3KIqSrLy7h64y6kXjNU_KNvHGG365u3r-ii8TOjaf-jykU25Gm1o5XvWBRMgQIUm1KVtV4NQqkVBp2usA7NDL16aGp3Qq-5WhzxmPCCBTofXlItoCoFE9G_Ct-XoKjWMq";
                var messagingSenderId = "710554227089";
                var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                var pn = new PushNotificationModel()
                {
                    To = "ch6V0_mJ4vA:APA91bE-FkuphDLb0QGCyDZCKZoqUP8MxJuJh79CMKJO8bGIHWTuIpO20Kcx8Yc1VT7PyAFHnJ9mqBiHzRC7dtghYyyjX6qVhN-WK3M8FAEAvKD0f5I5F9vAJDqEMKeE5LVYmIO13ANS",
                    Notification = new Notification()
                    {
                        Title = "         طاقچه",
                        Body = "متن آزمایشی از سرور فایربیس",
                        Vibrate = new[] { 500, 110, 500, 110, 450, 110, 200, 110, 170, 40, 450, 110, 200, 110, 170, 40, 500 },
                        Icon = "https://taaghche.ir/assets/images/taaghchebrand.png",
                        ClickAction = "https://taaghche.ir",
                        Tag = Guid.NewGuid().ToString("N") // unique notify or update last noitfy on client
                    }
                };

                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(pn);
                var byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", messagingSenderId));
                tRequest.ContentLength = byteArray.Length;

                using (var dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (var tResponse = tRequest.GetResponse())
                    {
                        using (var dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (var tReader = new StreamReader(dataStreamResponse))
                            {
                                var sResponseFromServer = tReader.ReadToEnd();
                                var str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
