using Microsoft.AspNetCore.Mvc;
using MVC_EF_Start.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeList.Controllers
{
    public class DatabaseController : Controller
    {
        public ApplicationDbContext dbContext;

        public DatabaseController(ApplicationDbContext context)
        {
            dbContext = context;
       
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
