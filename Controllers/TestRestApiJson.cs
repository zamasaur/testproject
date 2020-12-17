using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Controllers
{
    public class TestRestApiJson : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
