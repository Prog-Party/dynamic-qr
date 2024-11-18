//using DynamicQR.Infrastructure.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace DynamicQR.Infrastructure.Data;

//public class QrCodeContext : DbContext
//{
//    public DbSet<QrCode> QrCode { get; init; }

//    public QrCodeContext(DbContextOptions<QrCodeContext> options)
//    : base(options)
//    {
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);

//        modelBuilder.Entity<QrCode>();
//    }
//}