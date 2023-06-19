using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Models;


namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface IEventSearchQueryStringBuilder
{
    List<SelectedFilter> BuildEventSearchFilters(EventFilters eventFilters, IUrlHelper url);
}