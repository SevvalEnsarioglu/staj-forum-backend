using Microsoft.EntityFrameworkCore;
using staj_forum_backend.Models;

namespace staj_forum_backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Topic> Topics { get; set; }
    public DbSet<Reply> Replies { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Topic configuration
        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_Topics_CreatedAt");
            entity.HasIndex(e => e.ViewCount).HasDatabaseName("IX_Topics_ViewCount");
            
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Content).HasColumnType("text").IsRequired();
            entity.Property(e => e.AuthorName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.ViewCount).HasDefaultValue(0);
        });

        // Reply configuration
        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TopicId).HasDatabaseName("IX_Replies_TopicId");
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_Replies_CreatedAt");
            
            entity.Property(e => e.Content).HasColumnType("text").IsRequired();
            entity.Property(e => e.AuthorName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            // Foreign key relationship
            entity.HasOne(e => e.Topic)
                .WithMany(t => t.Replies)
                .HasForeignKey(e => e.TopicId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ContactMessage configuration
        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_ContactMessages_CreatedAt");
            entity.HasIndex(e => e.IsRead).HasDatabaseName("IX_ContactMessages_IsRead");
            
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Subject).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Message).HasColumnType("text").IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsRead).HasDefaultValue(false);
        });
    }
}


