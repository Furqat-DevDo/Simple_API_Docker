using SimpleApi.Models;

namespace SimpleApi.Data
{
    public class ApplicationDBContext:DbContext
    {
        public DbSet<Media> Medias { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options){}
    }
}
