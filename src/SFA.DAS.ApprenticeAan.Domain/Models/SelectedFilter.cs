namespace SFA.DAS.ApprenticeAan.Domain.Models;

public class SelectedFilter
{
    public string FieldName { get; set; } = null!;
    public int FieldOrder { get; set; }
    public List<FilterItem> Filters { get; set; } = new List<FilterItem>();
}