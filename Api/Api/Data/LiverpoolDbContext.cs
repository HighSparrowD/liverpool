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
        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<User>().Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Event>().HasKey(x => x.Id);
        modelBuilder.Entity<Event>().Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Attendee>()
            .HasOne<Event>(x => x.Event)
            .WithMany(x => x.Attendees)
            .HasForeignKey(x => x.EventId);
        
        modelBuilder.Entity<EventTag>()
            .HasKey(e => new { e.TagId, e.EventId });
        
        PreAddTags(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void PreAddTags(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>().HasData(
            // Location
            new Tag { Id = 1, Text = "Indoor" },
            new Tag { Id = 2, Text = "Outdoor" },
            new Tag { Id = 3, Text = "Online" },
            new Tag { Id = 4, Text = "Hybrid" },

            // Activity level
            new Tag { Id = 5, Text = "Active" },
            new Tag { Id = 6, Text = "Relaxed" },
            new Tag { Id = 7, Text = "Casual" },
            new Tag { Id = 8, Text = "High Energy" },
            new Tag { Id = 9, Text = "Low Effort" },

            // Social type
            new Tag { Id = 10, Text = "Networking" },
            new Tag { Id = 11, Text = "Social" },
            new Tag { Id = 12, Text = "Friends" },
            new Tag { Id = 13, Text = "Professional" },
            new Tag { Id = 14, Text = "Community" },

            // Event purpose
            new Tag { Id = 15, Text = "Workshop" },
            new Tag { Id = 16, Text = "Meetup" },
            new Tag { Id = 17, Text = "Presentation" },
            new Tag { Id = 18, Text = "Discussion" },
            new Tag { Id = 19, Text = "Training" },
            new Tag { Id = 20, Text = "Brainstorming" },
            new Tag { Id = 21, Text = "Q&A" },

            // Lifestyle & interests
            new Tag { Id = 22, Text = "Sports" },
            new Tag { Id = 23, Text = "Wellness" },
            new Tag { Id = 24, Text = "Fitness" },
            new Tag { Id = 25, Text = "Food & Drinks" },
            new Tag { Id = 26, Text = "Music" },
            new Tag { Id = 27, Text = "Art & Culture" },
            new Tag { Id = 28, Text = "Tech" },
            new Tag { Id = 29, Text = "Gaming" },
            new Tag { Id = 30, Text = "Outdoors & Nature" },
            new Tag { Id = 31, Text = "Travel" },

            // Time & pace
            new Tag { Id = 32, Text = "Short Event" },
            new Tag { Id = 33, Text = "Long Event" },
            new Tag { Id = 34, Text = "Morning" },
            new Tag { Id = 35, Text = "Afternoon" },
            new Tag { Id = 36, Text = "Evening" },
            new Tag { Id = 37, Text = "Night" },
            new Tag { Id = 38, Text = "Weekend" },
            new Tag { Id = 39, Text = "Weekday" },

            // Audience & accessibility
            new Tag { Id = 40, Text = "Beginner Friendly" },
            new Tag { Id = 41, Text = "Intermediate" },
            new Tag { Id = 42, Text = "Advanced" },
            new Tag { Id = 43, Text = "Family Friendly" },
            new Tag { Id = 44, Text = "Adults Only" },
            new Tag { Id = 45, Text = "Inclusive" },

            // Group size & format
            new Tag { Id = 46, Text = "Small Group" },
            new Tag { Id = 47, Text = "Large Group" },
            new Tag { Id = 48, Text = "One-on-One" },
            new Tag { Id = 49, Text = "Open Event" },
            new Tag { Id = 50, Text = "Invite Only" },

            // Vibe & mood
            new Tag { Id = 51, Text = "Chill" },
            new Tag { Id = 52, Text = "Fun" },
            new Tag { Id = 53, Text = "Serious" },
            new Tag { Id = 54, Text = "Creative" },
            new Tag { Id = 55, Text = "Focused" },

            // Extras
            new Tag { Id = 56, Text = "Free" },
            new Tag { Id = 57, Text = "Paid" },
            new Tag { Id = 58, Text = "Bring Your Own" },
            new Tag { Id = 59, Text = "Beginner Welcome" },
            new Tag { Id = 60, Text = "Recurring Event" },
            new Tag { Id = 61, Text = "One-Time Event" },
            new Tag { Id = 62, Text = "Local" },
            new Tag { Id = 63, Text = "International" }
        );
    }
}