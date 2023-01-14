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
            string st = DateTime.Now.ToString("o");

            Console.WriteLine(st);
            

            string ip = HttpContext.Connection.RemoteIpAddress.ToString() ;
            Console.WriteLine(ip);
            
            var userIdentity = new BsonDocument
            {
                {"Date_time", date},
                {"IPAddress",ip},
                {"st",st },
            };

            /*var database = client.GetDatabase("users");
            var table = database.GetCollection<BsonDocument>("visite_table");
            table.InsertOne(user_identity);*/
            await _userService.Create(userIdentity);

            var printUserIpaddress = new User()
            {
                Date_time = date,
                IPAddress = ip,
            };
            return View(printUserIpaddress);
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
            
            
            var printLastUserOnHome = new User()
            {
                Date_time = t["Date_time"].ToString(),
                IPAddress = t["IPAddress"].ToString(),
            };

            return View(printLastUserOnHome);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}