using Microsoft.EntityFrameworkCore;
using TECin2.ClassLibrary.Entities;

namespace TECin2.API.Database
{
    public class TECinContext : DbContext
    {
        public TECinContext() { }
        public TECinContext(DbContextOptions<TECinContext> options) : base(options) { }

        public DbSet<ApprovedComputer> ApprovedComputers { get; set; }
        public DbSet<ComputerLocation> ComputerLocation { get; set; }
        public DbSet<CheckInStatus> CheckInStatus { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<WorkHoursInDay> WorkHoursInDay { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<User> User { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                                .HasMany(u => u.Groups)
                .WithMany(g => g.Users)
                .UsingEntity(j => j.ToTable("GroupUser"));
            base.OnModelCreating(modelBuilder);
        }
    }

    public class TECinContext2 : DbContext
    {
        public TECinContext2() { }
        public TECinContext2(DbContextOptions<TECinContext2> options) : base(options) { }

        public DbSet<Password> Password { get; set; }
        public DbSet<SecurityNumb> SecurityNumb { get; set; }
    }
}
