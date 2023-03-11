namespace DeliveryScheduler.Repository.Helpers;
public static class Extensions
{
    /// <summary>
    /// LINQ .Any() wrapper that includes a null check for list
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="collection">Collection</param>
    public static bool NotNullAndAny<T>(this IEnumerable<T>? collection)
    {
        return collection != null && collection.Any();
    }
}
