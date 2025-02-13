namespace SFA.DAS.ApprenticeAan.Web.Models;

public class EventTypeModel
{
    public string EventType { get; set; } = "";
    public bool IsSelected { get; set; }
    public int Ordering { get; set; }
}

public class EventTypeModelComparer : IEqualityComparer<EventTypeModel>
{
    public bool Equals(EventTypeModel? x, EventTypeModel? y)
    {
        if (x == null || y == null)
            return false;

        return x.EventType == y.EventType &&
               x.IsSelected == y.IsSelected &&
               x.Ordering == y.Ordering;
    }

    public int GetHashCode(EventTypeModel obj)
    {
        return HashCode.Combine(obj.EventType, obj.IsSelected, obj.Ordering);
    }
}