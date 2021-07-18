using CollegeList.Models;
using Microsoft.EntityFrameworkCore;
using MVC_EF_Start.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeList.DataAccess
{
    public class Repository : IRepository
    {
		private readonly ApplicationDbContext _appDbContext;

		public Repository(ApplicationDbContext appDbContext)
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
						}) ;
					});
					await _appDbContext.AddAsync(newInstitution);
				}
				await _appDbContext.SaveChangesAsync();
			}

			return anyNewRcordsInserted;
		}

		public IEnumerable<Institution> GetAllInstitutions()
        {
			return  _appDbContext.Institution.ToList();
        }

		public Institution GetInstitutionById(Guid institutionId)
        {
			var university = _appDbContext.Institution.Include(x => x.FieldOfStudies)
													  .SingleOrDefault(x => x.Id == institutionId);
			return university;

		}

	}
}
