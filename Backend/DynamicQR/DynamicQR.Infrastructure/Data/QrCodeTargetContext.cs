//using DynamicQR.Infrastructure.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace DynamicQR.Infrastructure.Data;

//public class QrCodeTargetContext : DbContext
//{
//    public DbSet<QrCodeTarget> QrCodeTarget { get; init; }

//    public QrCodeTargetContext(DbContextOptions<QrCodeTargetContext> options)
//    : base(options)
//    {
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);

//        modelBuilder.Entity<QrCodeTarget>();
//    }
//}