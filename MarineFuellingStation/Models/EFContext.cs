using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MFS.Models
{
#if DEBUG
    public class EFContextFactory : IDesignTimeDbContextFactory<EFContext>
    {
        const string connstr = "data source=120.24.88.129;initial catalog=MFS;persist security info=True;user id=yisuo;password=hr!2027055;MultipleActiveResultSets=True;";
        public EFContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFContext>();

            optionsBuilder.UseSqlServer(connstr);

            return new EFContext(optionsBuilder.Options);
        }
    }
#endif
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options) { }
        /// <summary>
        /// 销售计划
        /// </summary>
        public DbSet<SalesPlan> SalesPlans { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        /// <summary>
        /// 商品表
        /// </summary>
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreType> StoreTypes { get; set; }
        /// <summary>
        /// 采购计划
        /// </summary>
        public DbSet<Purchase> Purchases { get; set; }
        /// <summary>
        /// 化验单
        /// </summary>
        public DbSet<Assay> Assays { get; set; }
        /// <summary>
        /// 转仓单
        /// </summary>
        public DbSet<MoveStore> MoveStores { get; set; }
        /// <summary>
        /// 船舶清污单
        /// </summary>
        public DbSet<BoatClean> BoatCleans { get; set; }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is ITrackable trackable)
                {
                    var now = DateTime.Now;
                    var user = CurrentUser;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.LastUpdatedAt = now;
                            trackable.LastUpdatedBy = user;
                            break;

                        case EntityState.Added:
                            trackable.CreatedAt = now;
                            trackable.CreatedBy = user;
                            trackable.LastUpdatedAt = now;
                            trackable.LastUpdatedBy = user;
                            break;
                    }
                }
            }
        }
        public string CurrentUser { get; set; }
    }
}
