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
        public DbSet<Slide> Slides { get; set; }
        public DbSet<SlidePage> SlidePages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SlideTag> SlideTags { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserExamHistory> UserExamHistories { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SlideTag>().HasKey(st => new { st.SlideId, st.TagId });
            modelBuilder.Entity<ReceiptDetail>().HasKey(rd => new { rd.ReceiptId, rd.SlideId });
            modelBuilder.Entity<ExamQuestion>().HasKey(eq => new { eq.ExamId, eq.QuestionId });
            modelBuilder.Entity<QuestionTag>().HasKey(qt => new { qt.QuestionId, qt.TagId });

            modelBuilder.Entity<Slide>()
                .HasOne(s => s.Teacher)
                .WithMany(u => u.CreatedSlides)
                .HasForeignKey(s => s.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Teacher)
                .WithMany(u => u.CreatedExams)
                .HasForeignKey(e => e.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.User)
                .WithMany(u => u.Receipts)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserExamHistory>()
                .HasOne(h => h.User)
                .WithMany(u => u.ExamHistories)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReceiptDetail>()
                .HasOne(rd => rd.Receipt)
                .WithMany(r => r.ReceiptDetails)
                .HasForeignKey(rd => rd.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReceiptDetail>()
                .HasOne(rd => rd.Slide)
                .WithMany(s => s.ReceiptDetails)
                .HasForeignKey(rd => rd.SlideId);

            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Exam)
                .WithMany(e => e.ExamQuestions)
                .HasForeignKey(eq => eq.ExamId);

            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Question)
                .WithMany(q => q.ExamQuestions)
                .HasForeignKey(eq => eq.QuestionId);

            modelBuilder.Entity<QuestionTag>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionTags)
                .HasForeignKey(qt => qt.QuestionId);

            modelBuilder.Entity<QuestionTag>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuestionTags)
                .HasForeignKey(qt => qt.TagId);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Teacher)
                .WithMany(u => u.CreatedQuestions)
                .HasForeignKey(q => q.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
