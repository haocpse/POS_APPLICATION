using Microsoft.EntityFrameworkCore;
using ScannerQRCode.Entities;

public class QRContext : DbContext
{
   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Sử dụng kết nối từ appsettings.json hoặc chuỗi kết nối trực tiếp
        optionsBuilder.UseSqlServer("Server=DESKTOP-6JNVRRM\\SQLEXPRESS;Database=ScanQRCode;User Id=sa;Password=123456;TrustServerCertificate=True;");
    }
    public DbSet<QRCodeScan> QRCodeScans { get; set; }
    public  DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }



}
