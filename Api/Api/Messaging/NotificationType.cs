using System.Text.Json.Serialization;

namespace Api.Messaging;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationType
{
    EventUserAccepted,
    EventUserDenied,
    EventUserRemoved,
    EventCancelled,
    EventUpcoming,
    EventChanged,
    NewAttendee
}