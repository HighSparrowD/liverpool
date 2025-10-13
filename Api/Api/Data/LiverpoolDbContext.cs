using Api.Entities.Common;
using Api.Entities.Event;
using Api.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class LiverpoolDbContext(DbContextOptions<LiverpoolDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    
    public DbSet<UserRating> UserRatings => Set<UserRating>();
    
    public DbSet<Event> Events => Set<Event>();
    
    public DbSet<Attendee> Attendee => Set<Attendee>();
    
    public DbSet<Tag> Tags => Set<Tag>();
    
    public DbSet<EventTag> EventTags => Set<EventTag>();
    
    // TODO: set up all entities
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventTag>()
            .HasKey(e => new { e.TagId, e.EventId });
        
        modelBuilder.Entity<EventTag>().HasOne<Event>().WithMany(e => e.Tags).
            HasForeignKey(e => e.EventId);
        
        base.OnModelCreating(modelBuilder);
    }
}