﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeanDotNetDocker.DataAccess;
using SeanDotNetDocker.Models;

namespace SeanDotNetDocker.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBContext _db;

        public HomeController(DBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var foo = await _db.WebSettings.Where(s => s.SettingKey.Equals("sean")).FirstOrDefaultAsync();
            if (foo != null)
                ViewBag.msg = foo.Value;
            else ViewBag.msg = "fuck you!";
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
    }
}
