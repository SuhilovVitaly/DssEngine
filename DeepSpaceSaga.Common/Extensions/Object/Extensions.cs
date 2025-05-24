using System.Text.Json;
using System.Text.Json.Serialization;

// Required for ReferenceHandler

namespace DeepSpaceSaga.Common.Extensions.Object;

public static class Extensions
{
    /// <summary>
    /// Performs a deep clone of the object using JSON serialization.
    /// </summary>
    /// <typeparam name="T">The type of the object being cloned.</typeparam>
    /// <param name="source">The object instance to clone.</param>
    /// <returns>A deep clone of the object.</returns>
    /// <remarks>
    /// This method relies on System.Text.Json serialization.
    /// Ensure that the object type T and all its nested types are serializable.
    /// Handles cyclic references by preserving them during serialization.
    /// Performance might be slower than manual cloning for complex objects.
    /// </remarks>
    public static T? DeepClone<T>(this T source)
    {
        // Don't serialize a null object, simply return the default for that object type
        if (source is null)
        {
            return default;
        }

        var options = new JsonSerializerOptions
        {
            // Preserve references to handle potential cyclic references in the object graph
            ReferenceHandler = ReferenceHandler.Preserve,
            // Include fields if necessary, though properties are more common
            // IncludeFields = true,
            // Include non-public members if necessary (use with caution)
            // PropertyNameCaseInsensitive = true, // If casing differs
        };

        // Serialize the object to a JSON string
        var json = JsonSerializer.Serialize(source, options);

        // Deserialize the JSON string back to an object of the same type
        return JsonSerializer.Deserialize<T>(json, options);
    }

    public static string ToJson(this object value)
    {
        return JsonSerializer.Serialize(value, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
} 