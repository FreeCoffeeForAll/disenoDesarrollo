using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDiseño.Models.invoice;
using ProyectoFinalDiseño.Models.subscription;
using ProyectoFinalDiseño.Models.Training;
using ProyectoFinalDiseño.Models.user;
using System.Reflection.Emit;

namespace ProyectoFinalDiseño.Models
{
    public class ApplicationDbContext : IdentityDbContext<User_Application>
    {
        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<User_Visit> UserVisits { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<TrainingClass> TrainingClasses { get; set; }

        public DbSet<ClassReservation> ClassReservations { get; set; }


        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<noUse_Training> Trainings { get; set; }
        public DbSet<noUse_TrainingExercise> TrainingExercises { get; set; }
        public DbSet<noUse_Training_Client> ClientTrainings { get; set; }
        public DbSet<noUse_Training_Progress> ExerciseProgresses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Ensure identity tables are created
                                           // Define the relationship between Subscription and UserApplication
            builder.Entity<Subscription>()
                .HasOne(s => s.UserApplication) // A Subscription has one UserApplication
                .WithMany(u => u.Subscriptions) // A UserApplication can have many Subscriptions
                .HasForeignKey(s => s.UserId) // The foreign key in Subscription is UserId
                .OnDelete(DeleteBehavior.Cascade); // Optional: Define the delete behavior

            builder.Entity<noUse_TrainingExercise>()
        .HasKey(te => new { te.TrainingId, te.ExerciseId });

            builder.Entity<noUse_TrainingExercise>()
                .HasOne(te => te.Training)
                .WithMany(t => t.TrainingExercises)
                .HasForeignKey(te => te.TrainingId);

            builder.Entity<noUse_TrainingExercise>()
                .HasOne(te => te.Exercise)
                .WithMany()
                .HasForeignKey(te => te.ExerciseId);
        }
    }
}