using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

public record GetAttendancesResponse(List<Attendance> Attendances);

public record Attendance(Guid CalendarEventId, EventFormat EventFormat, DateTime EventStartDate, string EventTitle);

