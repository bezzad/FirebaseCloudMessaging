using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dapper;

namespace FCM.Controllers
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
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> StoreToken(string token)
        {
            await StoreUserFcmTokenAsync(token);
            return new HttpStatusCodeResult(200);
        }

        private async Task StoreUserFcmTokenAsync(string token)
        {
            using (var conn = new SqlConnection(GetConnStr()))
            {
                var res = await conn.ExecuteAsync(@"if not exists (select 1 from Tokens as t where @Token = t.Token) 
                                                        Insert into Tokens values(@Username, @Token)", 
                                                    new { Username = Guid.NewGuid().ToString("N"), Token = token });
            }
        }
    }
}