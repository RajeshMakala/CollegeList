using CollegeList.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeList.DataAccess
{
    class DbModelConfigurations : IEntityTypeConfiguration<Institution>
    {
        public void Configure(EntityTypeBuilder<Institution> institutionBuilder)
        {
            institutionBuilder.HasKey(x => x.Id);
            institutionBuilder.Property(x => x.SchoolName).IsRequired();
            institutionBuilder.Property(x => x.SchoolCity);
            institutionBuilder.Property(x => x.SchoolState);
            institutionBuilder.Property(x => x.SchoolSchool_url);

            institutionBuilder.OwnsMany(x => x.FieldOfStudies, fieldOfStudy =>

                {
                    fieldOfStudy.HasKey(x => x.Id);
                    fieldOfStudy.Property(x => x.Description);
                    fieldOfStudy.Property(x => x.Degree);
                }
            ) ;
        }
    }
}
