using E4UsersMVCWebApp.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml;

namespace E4UsersMVCWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        /*

        public ActionResult Blogs()
        {
            List<blogObj> blogList = GetBlogList(Server.MapPath("~/configfiles/homepage.xml"));

            ViewBag.BlogList = blogList;
            return View();
        }

        public static List<blogObj> GetBlogList(string xmlpath)
        {
            List<blogObj> obList = new List<blogObj>();
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlpath);
            XmlNodeList elemList = doc.GetElementsByTagName("blog");
            blogObj _obj = null;
            foreach (XmlNode chldNode in elemList)
            {
                _obj = new blogObj();
                _obj.title = chldNode.Attributes["title"].Value;
                _obj.imagePath = chldNode.Attributes["imageUrl"].Value;
                _obj.shortinfo = chldNode.InnerText;
                obList.Add(_obj);
            }
            return obList;
        }
        */

    }
}