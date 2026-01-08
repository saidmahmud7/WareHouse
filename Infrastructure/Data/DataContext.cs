using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<SubDepartment> SubDepartments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<FixedAsset> FixedAssets { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<AssetTransaction> AssetTransactions { get; set; }
        public DbSet<Asset> Assets { get; set; }

       
    }
}