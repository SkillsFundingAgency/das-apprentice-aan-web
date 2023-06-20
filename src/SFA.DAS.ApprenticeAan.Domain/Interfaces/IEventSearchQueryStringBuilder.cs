using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Models;


namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface IEventSearchQueryStringBuilder
{
    List<SelectedFilter> BuildEventSearchFilters(EventFilterChoices eventFilterChoices, List<ChecklistLookup> eventFormatsLookup, List<ChecklistLookup> eventTypesLookup, IUrlHelper url);
}