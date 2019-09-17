using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Tameenk.Yakeen.DAL
{
    public class YakeenContext : DbContext
    {                   
        public YakeenContext() : base()
        {
           
        }

        public YakeenContext(DbContextOptions<YakeenContext> options) :
            base(options) { }
              
        public DbSet<Alien> Aliens { get; set; }        
        public DbSet<Occupation> Occupation { get; set; }
        public DbSet<ServiceRequestLog> ServiceRequestLog { get; set; }
        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<VehicleModel> VehicleModel { get; set; }
        public DbSet<VehicleMaker> VehicleMaker { get; set; }
        public DbSet<Company>Companies { get; set; }  
        public DbSet<LicenseType> LicenseType { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<CitizenRequestLog> CitizenRequestLogs { get; set; }
        public DbSet<AlienRequestLog> AlienRequestLogs { get; set; }
        public DbSet<VehicleRequestLog> VehicleRequestLogs { get; set; }
        public DbSet<CompanyRequestLog> CompanyRequestLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings["YakeenIntegration"]); 
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LicenseType>().Property(x => x.Id).UseSqlServerIdentityColumn();
            builder.Entity<CitizenRequestLog>().Property(x => x.ID).UseSqlServerIdentityColumn();
        }

        }
}
