using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.Dsl;
using FluentAssertions;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

public static class AutoFixtureBuilderExtensions
{
    public static IPostprocessComposer<T> WithValues<T, TProperty>(
        this IPostprocessComposer<T> composer,
        Expression<Func<T, TProperty>> propertyPicker,
        params TProperty[] values)
    {
        var queue = new Queue<TProperty>(values);

        return composer.With(propertyPicker, () => queue.Dequeue());
    }
}

public class AutoFixtureBuilderExtensionsTests
{
    [Test]
    public void CreatesManyWithSpecifiedIds()
    {
        Fixture fixture = new();
        int[] ids = new[] { 1, 5, 9 };

        var people = fixture.Build<Person>().WithValues(p => p.Id, ids).CreateMany(ids.Length);

        people.Any(p => p.Id == 1).Should().BeTrue();
        people.Any(p => p.Id == 5).Should().BeTrue();
        people.Any(p => p.Id == 9).Should().BeTrue();
    }

    public record Person(int Id, string Name);
}
