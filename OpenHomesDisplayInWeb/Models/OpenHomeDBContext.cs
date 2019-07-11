using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHomesDisplayInWeb
{
    class OpenHomeDBContext : DbContext
    {
        public DbSet<OpenHome> OpenHomeModels { get; set; }
        public DbSet<Suburb> SuburbModels { get; set; }
        public DbSet<District> DistrictModels { get; set; }
        public DbSet<Region> RegionModels { get; set; }
    }
}
