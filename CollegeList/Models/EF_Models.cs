using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeList.Models
{
    public class Institution
    {
        public Guid Id { get; set; }
        public string SchoolName { get; set; }
        public string? SchoolCity { get; set; }
        public string? SchoolState { get; set; }
        public string? SchoolSchool_url { get; set; }
        public List<FieldOfStudy> FieldOfStudies { get; set; }

        public Institution()
        {
            Id = Guid.NewGuid();
            FieldOfStudies = new List<FieldOfStudy>();
        }

    }

    public class FieldOfStudy
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Degree { get; set; }
        public FieldOfStudy()
        {
            Id = Guid.NewGuid();
        }
    }

}
