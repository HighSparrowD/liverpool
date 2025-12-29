using Api.Data;
using Api.Models.Event;
using Api.Entities.Event;
using Microsoft.EntityFrameworkCore;
using Attendee = Api.Entities.Event.Attendee;
using AttendeeStatus = Api.Entities.Event.AttendeeStatus;

namespace Api.Services.Event;

public class AttendeeService(LiverpoolDbContext dbContext) : IAttendeeService
{
    public async Task<Models.Event.Attendee?> GetAttendee(long userId, long eventId)
    {
        var attendee = await dbContext.Attendee
            .FirstOrDefaultAsync(x => x.UserId == userId && x.EventId == eventId);

        return attendee?.ToDto();
    }

    public async Task<Models.Event.Attendee> ApplyToAttend(ParticipationModel model)
    {
        var attendee = await dbContext.Attendee
            .FirstOrDefaultAsync(x => x.UserId == model.UserId && x.EventId == model.EventId);

        if (attendee != null)
        {
            if (attendee.Status is AttendeeStatus.Declined or AttendeeStatus.Removed 
                or AttendeeStatus.Approved or AttendeeStatus.Pending)
                return attendee.ToDto();
            
            attendee.Status = AttendeeStatus.Pending;
            await dbContext.SaveChangesAsync();
            return attendee.ToDto();
        }

        attendee = new Attendee()
        {
            UserId = model.UserId,
            EventId = model.EventId,
            Status = AttendeeStatus.Pending,
        };
        
        dbContext.Attendee.Add(attendee);
        await dbContext.SaveChangesAsync();
        
        return attendee.ToDto();
    }
}