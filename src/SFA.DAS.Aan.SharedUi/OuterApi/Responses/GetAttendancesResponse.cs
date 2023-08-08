using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.Aan.SharedUi.OuterApi.Responses;

public record GetAttendancesResponse(List<Attendance> Attendances);

public record Attendance(Guid CalendarEventId, EventFormat EventFormat, DateTime EventStartDate, string EventTitle);

