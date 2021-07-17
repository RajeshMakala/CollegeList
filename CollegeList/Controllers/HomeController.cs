using CollegeList.DataAccess;
using CollegeList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CollegeList.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        string BASE_URL = "https://api.data.gov/ed/collegescorecard/v1/schools?per_page=10";
        static string API_KEY = "P14k800OzswF9iR4VOg4HZIHxfqpp3UsdLseTcbn"; //Add your API key here inside ""

        HttpClient httpClient;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            GetCollegesInfo();
            return View();
        }

        public IActionResult Model()
        {
            return View();
        }


        public IActionResult Explore()
        {
            return View(_repository.GetAllInstitutions());
        }

        public IActionResult Details(Guid id)
        {
            var val =  _repository.GetInstitutionById(id);
            
            return View(val);
        }


        public IActionResult GetCollegesInfo()
        {
            var detailsOfColleges = GetAllCollegesList();
            _repository.SaveCollegesListToDb(detailsOfColleges);
            return Ok();
        }


        public Rootobject GetAllCollegesList()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //string API_PATH = BASE_URL + "&_fields=school.name,school.city,school.state,id,school.school_url";
            string API_PATH = BASE_URL + "&_fields=school.name,school.city,school.state,id,school.school_url,programs.cip_4_digit.title,programs.cip_4_digit.credential.title";
            string CollegeList = "";

            Rootobject rootobject = null;
            httpClient.BaseAddress = new Uri(API_PATH);

            // It can take a few requests to get back a prompt response, if the API has not received
            //  calls in the recent past and the server has put the service on hibernation
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(API_PATH).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    CollegeList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!string.IsNullOrEmpty(CollegeList))
                    {
                        rootobject = JsonConvert.DeserializeObject<Rootobject>(CollegeList);
                        return rootobject;
                    }
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception("An Error Occured while fetching Colleges List", ex);
            }
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

