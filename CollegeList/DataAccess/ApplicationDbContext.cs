using CollegeList.Models;
using Microsoft.EntityFrameworkCore;


namespace MVC_EF_Start.DataAccess
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Institution> Institution { get; set; }
    public DbSet<FieldOfStudy> FieldOfStudy { get; set; }
  }
}