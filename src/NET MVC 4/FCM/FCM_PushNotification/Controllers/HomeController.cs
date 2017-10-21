using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dapper;
using FCM_PushNotification.Models;

namespace FCM_PushNotification.Controllers
{
    public class HomeController : Controller
    {
        public static string GetConnStr()
        {
#if DEBUG
            var conn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["debug"];
#else
            var conn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["release"];
#endif
            return conn.ToString();
        }

        public ActionResult Index()
        {
            var model = new Notification()
            {
                Title = "         طاقچه",
                Body = "متن آزمایشی از سرور فایربیس",
                Vibrate = new[] { 500, 110, 500, 110, 450, 110, 200, 110, 170, 40, 450, 110, 200, 110, 170, 40, 500 },
                Icon = "https://taaghche.ir/assets/images/taaghchebrand.png",
                ClickAction = "https://taaghche.ir",
                Tag = Guid.NewGuid().ToString("N") // unique notify or update last noitfy on client
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SendMessage(Notification model)
        {
            await SendPushNotificationAsync(model);
            return new HttpStatusCodeResult(200);
        }

        public async Task SendPushNotificationAsync(Notification model)
        {
            try
            {
                var serverKey = "AAAApXBVEZE:APA91bH56ib3KIqSrLy7h64y6kXjNU_KNvHGG365u3r-ii8TOjaf-jykU25Gm1o5XvWBRMgQIUm1KVtV4NQqkVBp2usA7NDL16aGp3Qq-5WhzxmPCCBTofXlItoCoFE9G_Ct-XoKjWMq";
                var messagingSenderId = "710554227089";
                var tokens = await GetTokensAsync();
                foreach (var userToken in tokens)
                {
                    var pn = new MessageModel()
                    {
                        To = userToken,
                        Notification = model
                    };
                    var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(pn);
                    var byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.Headers.Add($"Authorization: key={serverKey}");
                    tRequest.Headers.Add($"Sender: id={messagingSenderId}");
                    tRequest.ContentLength = byteArray.Length;

                    tRequest.GetRequestStream();
                    //using (var dataStream = tRequest.GetRequestStream())
                    //{
                    //    dataStream.Write(byteArray, 0, byteArray.Length);
                    //    using (var tResponse = tRequest.GetResponse())
                    //    {
                    //        using (var dataStreamResponse = tResponse.GetResponseStream())
                    //        {
                    //            using (var tReader = new StreamReader(dataStreamResponse))
                    //            {
                    //                var sResponseFromServer = tReader.ReadToEnd();
                    //                var str = sResponseFromServer;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }
        }

        public async Task<IEnumerable<string>> GetTokensAsync()
        {
            IEnumerable<string> res;
            using (var conn = new SqlConnection(GetConnStr()))
            {
                res = await conn.QueryAsync<string>("select token from Tokens");
            }

            return res;
        }

    }
}