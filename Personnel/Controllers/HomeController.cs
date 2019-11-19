using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personnel.Models;

namespace Personnel.Controllers
{
    public class HomeController : Controller
    {

        private readonly DatabaseContext _context;
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //var CheckLastUpdates = _context.CheckLastUpdates.FromSql(queryData).ToList();
            return View();
        }




        public IActionResult Gendata()
        {
            IActionResult response = Unauthorized();
            //var queryData = "SELECT b.Site, " +
            //    "CONCAT(SUBSTRING(b.LastUpdate,5,2),'/',SUBSTRING(b.LastUpdate,3,2),'/',SUBSTRING(b.LastUpdate,1,2),' ',SUBSTRING(b.LastUpdate,8,2),':',SUBSTRING(b.LastUpdate,10,2),':',SUBSTRING(b.LastUpdate,12,2)) as LastUpdate, " +
            //    "b.RowData  FROM " +
            //    "(SELECT a.Site as Site, " +
            // "(SELECT TOP 1 Data2 FROM checkinouts  WHERE Site=a.Site ORDER BY Data2 DESC ) as LastUpdate,  " +
            //  "(SELECT Count(Site) FROM checkinouts  WHERE Site=a.Site) as RowData " +
            //  " FROM ( " +
            //  "SELECT " +
            //  "checkinouts.Site " +
            //  "FROM checkinouts " +
            //  "GROUP BY checkinouts.Site " +
            //  ")as a" +
            //  ")as b ";



            var queryData = "SELECT b.Site, " +
    "CONCAT(SUBSTRING(b.LastUpdate,5,2),'/',SUBSTRING(b.LastUpdate,3,2),'/',SUBSTRING(b.LastUpdate,1,2),' ',SUBSTRING(b.LastUpdate,8,2),':',SUBSTRING(b.LastUpdate,10,2),':',SUBSTRING(b.LastUpdate,12,2)) as LastUpdate, " +
    "b.RowData  FROM " +
    "(SELECT a.Site as Site, " +
 "(SELECT  Data2 FROM CheckInOuts  WHERE Site=a.Site ORDER BY Data2 DESC limit 1 ) as LastUpdate,  " +
  "(SELECT Count(Site) FROM CheckInOuts  WHERE Site=a.Site) as RowData " +
  " FROM ( " +
  "SELECT " +
  "CheckInOuts.Site " +
  "FROM CheckInOuts " +
  "GROUP BY CheckInOuts.Site " +
  ")as a" +
  ")as b ";


            var CheckLastUpdates = _context.CheckLastUpdates.FromSql(queryData).ToList();
                response = Ok(new { data = CheckLastUpdates });
            return response;

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ExportData()
        {

            ViewBag.StartDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            //var queryData = "SELECT DISTINCT Site as code,Site as name FROM dbo.CheckInOuts";
            var queryData = "SELECT DISTINCT Site as code,Site as name FROM CheckInOuts";
            var sourceAutoCompletes = _context.SourceAutoCompletes.FromSql(queryData).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            return View();
        }



        public FileStreamResult CreateFile(string site,string date1,string date2)
        {

            var StartDate = date1.Substring(8, 2) + date1.Substring(3, 2)  + date1.Substring(0, 2) + " 00:00";
            var EndDate = date2.Substring(8, 2)  + date2.Substring(3, 2) + date2.Substring(0, 2) + " 23:59";
            var StartDate1 = date1.Substring(8, 2) + date1.Substring(3, 2) + date1.Substring(0, 2);
            var EndDate1 = date2.Substring(8, 2) + date2.Substring(3, 2) + date2.Substring(0, 2);



            var CheckInOut =_context.CheckInOuts.Where(p=>p.Site==site && p.Data2.CompareTo(StartDate)>=0 && p.Data2.CompareTo(EndDate)<=0).ToList();
            var stringdata = "";
            foreach (var std in CheckInOut as List<CheckInOut>)
            {
                stringdata += std.Data1 + "            " +std.Data2.Substring(0,11)+ "\n";
            }
            var byteArray = Encoding.ASCII.GetBytes(stringdata);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", site+ StartDate1+"_"+ EndDate1+".txt");
        }
    }
}
