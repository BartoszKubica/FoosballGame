using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FoosballGame.Domain
{
    public class FoosballGameDbContext : DbContext
    {
        public DbSet<GameDb> Game { get; set; }

        public FoosballGameDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<GameDb>(x =>
            {
                x.HasKey(s => s.Id);
                x.Property(x => x.StartDateTime)
                    .IsRequired();
                x.Property(e => e.State)
                    .IsRequired()
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include }),
                        v => JsonConvert.DeserializeObject<FoosballGameStateDb>(v,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include }));
            });
        }
    }
}
