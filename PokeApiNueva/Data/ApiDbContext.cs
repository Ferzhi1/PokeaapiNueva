using Microsoft.EntityFrameworkCore;
using PokeApiNueva.Models;

namespace PokeApiNueva.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        public DbSet<Userpkm> Userpkmns { get; set; }

        public DbSet<FriendshipFM> FriendshipsFM { get; set; }

        public DbSet<CollectionUserPkm>CollectionUserPkms { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Userpkm>()
             .Property(u => u.Credits)
             .HasDefaultValue(5);
       
       
            
           
        }




    }
}

