using ASP.NETCoreWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreWebApi.Data
{
    public partial class FinanceContext : DbContext
    {
        public FinanceContext() { }
        public FinanceContext(DbContextOptions<FinanceContext> options):base(options) { }

        public DbSet<OperationType> OperationsType { get; set; } = null!;
        public DbSet<FinancialTransaction> FinanciaTransactions { get; set; } = null!;
        public DbSet<DailyReport> DailyReports { get; set; } = null!;
        public DbSet<PeriodReport> PeriodReports { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinancialTransaction>().HasOne(f => f.Type).WithMany(t => t.FinancialTransactions).HasForeignKey(f => f.TypeId);

            modelBuilder.Entity<FinancialTransaction>()
            .HasOne(o => o.Type)
            .WithMany()
            .HasForeignKey(o => o.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DailyReport>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<PeriodReport>(entity =>
            {
                entity.HasNoKey();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}