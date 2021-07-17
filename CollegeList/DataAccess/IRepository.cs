using CollegeList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeList.DataAccess
{
    public interface IRepository
    {
       Task<bool> SaveCollegesListToDb(Rootobject rootObject);

        IEnumerable<Institution> GetAllInstitutions();

        Institution GetInstitutionById(Guid institutionId);
    }
}
