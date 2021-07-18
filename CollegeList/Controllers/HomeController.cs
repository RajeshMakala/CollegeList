using CollegeList.DataAccess;
using CollegeList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MVC_EF_Start.DataAccess;

namespace CollegeList.Controllers
{
   
    public class HomeController : Controller
    {
        
        string BASE_URL = "https://api.data.gov/ed/collegescorecard/v1/schools?per_page=10";
        static string API_KEY = "P14k800OzswF9iR4VOg4HZIHxfqpp3UsdLseTcbn"; //Add your API key here inside ""

        HttpClient httpClient;
      
        
      
        private readonly ApplicationDbContext _appDbContext;

        public HomeController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _appDbContext.Database.EnsureCreated();
        }
        public async Task<bool> SaveCollegesListToDb(Rootobject rootObject)
        {
            bool anyNewRcordsInserted = false;
            var missingColleges = from newObject in rootObject.results
                                  join dbObject in _appDbContext.Institution
                                      on newObject.schoolname equals dbObject.SchoolName into pp
                                  from dbObject in pp.DefaultIfEmpty()
                                  where dbObject == null
                                  select newObject;
            if (missingColleges.Count() > 0)
            {
                foreach (var missingCollege in missingColleges)
                {
                    Institution newInstitution = new Institution();
                    newInstitution.SchoolName = missingCollege.schoolname;
                    newInstitution.SchoolSchool_url = missingCollege.schoolschool_url;
                    newInstitution.SchoolCity = missingCollege.schoolcity;
                    newInstitution.SchoolState = missingCollege.schoolstate;

                    missingCollege.latestprogramscip_4_digit.ForEach(x =>
                    {
                        newInstitution.FieldOfStudies.Add(new FieldOfStudy()
                        {
                            Description = x.title,
                            Degree = x.credential.title
                        });
                    });
                    await _appDbContext.AddAsync(newInstitution);
                }
                await _appDbContext.SaveChangesAsync();
            }

            return anyNewRcordsInserted;
        }

        public IEnumerable<Institution> GetAllInstitutions()
        {
            return _appDbContext.Institution.ToList();
        }

        public Institution GetInstitutionById(Guid institutionId)
        {
            var university = _appDbContext.Institution.Include(x => x.FieldOfStudies)
                                                      .SingleOrDefault(x => x.Id == institutionId);
            return university;

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
            return View(GetAllInstitutions());
        }

        public IActionResult Details(Guid id)
        {
            var val =  GetInstitutionById(id);
            
            return View(val);
        }


        public IActionResult GetCollegesInfo()
        {
            var detailsOfColleges = GetAllCollegesList();
            var a=SaveCollegesListToDb(detailsOfColleges);
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

        public async Task<IActionResult> Edit(Guid id)
        {
            Institution updatePark = _appDbContext.Institution.Where(p => p.Id == id).FirstOrDefault();
         
            

            UpdatePark updtpk = new UpdatePark()
            {
                ID = updatePark.Id,
                fullName = updatePark.SchoolName,
                parkCode = updatePark.SchoolState,
           
            };

            List<string> actnames = await _appDbContext.FieldOfStudy.Select(p => p.Degree).ToListAsync();

          
            ViewBag.actnames = actnames;

            return View(updtpk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("fullName,parkCode,statenames,activitynames")] UpdatePark updatedpk)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Institution ptobeupdated = _appDbContext.Institution
                        .Include(p => p.FieldOfStudies)
                       
                        .Where(p => p.Id == id)
                        .FirstOrDefault();


                    ptobeupdated.SchoolName = updatedpk.fullName;
                    ptobeupdated.SchoolCity = updatedpk.parkCode;


                    ptobeupdated.FieldOfStudies.Clear();
                  

                 

                    _appDbContext.Update(ptobeupdated);
                    await _appDbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Error Occured");
            }

            

            List<Guid> actnames = await _appDbContext.FieldOfStudy.Select(p => p.Id).ToListAsync();

         
            ViewBag.actnames = actnames;
            return View(updatedpk);
        }
        public async Task<IActionResult> Delete(Guid id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p = await _appDbContext.Institution


                .Where(p => p.Id.Equals(id))
                .FirstOrDefaultAsync();
            if (p == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete: " + p.Id;

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Error Occured";
            }

            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            Institution deletepark = await _appDbContext.Institution
                .Include(p => p.FieldOfStudies)

                .Where(p => p.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (deletepark == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _appDbContext.Institution.Remove(deletepark);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}

