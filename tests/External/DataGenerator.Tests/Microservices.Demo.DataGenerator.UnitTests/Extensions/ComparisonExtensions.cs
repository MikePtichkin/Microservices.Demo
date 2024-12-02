using System.Text.Json;

namespace Microservices.Demo.DataGenerator.UnitTests.Extensions;

public static class ComparisonExtensions
{
    public static bool JsonCompare<T>(this T object1, T object2)
    {
        return JsonSerializer.Serialize(object1) == JsonSerializer.Serialize(object2);
    }
}
