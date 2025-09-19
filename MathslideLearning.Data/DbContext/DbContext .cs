using MathslideLearning.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MathslideLearning.Data.DbContext
{
    public class MathslideLearningDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public MathslideLearningDbContext(DbContextOptions<MathslideLearningDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<SlidePage> SlidePages { get; set; }
        public DbSet<PurchasedSlide> PurchasedSlides { get; set; }
        public DbSet<SlideTag> SlideTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PurchasedSlide>()
                .HasKey(ps => new { ps.UserId, ps.SlideId });

            modelBuilder.Entity<PurchasedSlide>()
                .HasOne(ps => ps.User)
                .WithMany(u => u.PurchasedSlides)
                .HasForeignKey(ps => ps.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchasedSlide>()
                .HasOne(ps => ps.Slide)
                .WithMany(s => s.PurchasedByUsers)
                .HasForeignKey(ps => ps.SlideId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SlideTag>()
                .HasKey(st => new { st.SlideId, st.TagId });

            modelBuilder.Entity<SlideTag>()
                .HasOne(st => st.Slide)
                .WithMany(s => s.SlideTags)
                .HasForeignKey(st => st.SlideId);

            modelBuilder.Entity<SlideTag>()
                .HasOne(st => st.Tag)
                .WithMany(t => t.SlideTags)
                .HasForeignKey(st => st.TagId);
            modelBuilder.Entity<Slide>()
                .HasOne(s => s.Teacher)
                .WithMany(u => u.CreatedSlides)
                .HasForeignKey(s => s.TeacherId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
