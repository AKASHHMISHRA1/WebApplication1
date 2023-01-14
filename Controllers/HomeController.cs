using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using WebApplication1.Models;
using MongoDB.Driver;
using SharpCompress.Common;
using System.Linq;
using MongoDB.Bson;
using Microsoft.Extensions.Options;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        /*private MongoClient client = new MongoClient("mongodb://127.0.0.1:27017/");*/
        private readonly UsersService _userService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UsersService userService)
        {
            _logger = logger;
            _userService = userService;
        }


        
        public async Task<IActionResult> Index()
        {
            string date = DateTime.Now.ToString();
            Console.WriteLine(date);
            string ip = HttpContext.Connection.RemoteIpAddress.ToString() ;
            Console.WriteLine(ip);
            
            var user_identity = new BsonDocument
            {
                {"Date_time", date},
                {"IPAddress",ip}
            };

            /*var database = client.GetDatabase("users");
            var table = database.GetCollection<BsonDocument>("visite_table");
            table.InsertOne(user_identity);*/
            await _userService.Create(user_identity);

            var print_user_ipaddress = new user()
            {
                Date_time = date,
                IPAddress = ip,
            };
            return View(print_user_ipaddress);
        }

        public async Task<IActionResult> Privacy()
        {
            /*var database = client.GetDatabase("users");
            var table = database.GetCollection<BsonDocument>("visite_table");

            var list_of_users_ip=table.Find(new BsonDocument()).ToList();*/

            //var docs = table.Find(new BS).ToList();
            var listOfUsersIp = await _userService.Get();
            BsonDocument t=new BsonDocument();
            listOfUsersIp.ForEach(doc =>
            {
                Console.WriteLine(doc);
                t = doc;
            });
            Console.WriteLine(t["Date_time"]);
            
            
            var print_last_user_on_home = new user()
            {
                Date_time = t["Date_time"].ToString(),
                IPAddress = t["IPAddress"].ToString(),
            };

            return View(print_last_user_on_home);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}