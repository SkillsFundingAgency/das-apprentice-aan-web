﻿using SFA.DAS.ApprenticeAan.Domain.Constants;

namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class EventFilterChoices
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public List<EventFormat> EventFormats { get; set; } = new List<EventFormat>();
    public List<int> CalendarIds { get; set; } = new List<int>();
    public List<int> RegionIds { get; set; } = new List<int>();
    public List<ChecklistLookup> EventFormatsLookup { get; set; } = new List<ChecklistLookup>();


    public List<ChecklistLookup> EventTypesLookup { get; set; } = new List<ChecklistLookup>();
    public List<ChecklistLookup> RegionsLookup { get; set; } = new List<ChecklistLookup>();
}