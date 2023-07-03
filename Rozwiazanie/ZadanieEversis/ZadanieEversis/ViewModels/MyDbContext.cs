using System.Data.Entity;
using System.Threading.Tasks;

namespace MVVMExample
{
    public class MyDbContext : DbContext
    {
        public DbSet<DataItem> DataItems { get; set; }
    }
}
