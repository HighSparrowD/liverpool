namespace Api.Models.Event;

public enum AttendeeStatus : byte
{
    None = 0, // Did not apply to attend
    Pending = 1,
    Approved = 2,
    Declined = 3,
    Removed = 4,
    Left = 5
}