using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateCmbModel(modelBuilder);
        }

        public DbSet<Cmb> Cmbs { get; set; }

        private static void CreateCmbModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cmb>().Property(p => p.Code).HasColumnName("CODE");
            modelBuilder.Entity<Cmb>().Property(p => p.Value).HasColumnName("VALUE");

        }
    }
}
