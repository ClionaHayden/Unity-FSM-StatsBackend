using UnityEngine;

public static class JsonHelper
{
    /// <summary>
    /// Parses a JSON array string into an array of objects of type T.
    /// Unity's JsonUtility cannot directly parse JSON arrays, so this method
    /// wraps the array inside a dummy object with an "Items" field to work around that.
    /// </summary>
    /// <typeparam name="T">The type of objects in the JSON array.</typeparam>
    /// <param name="json">The JSON array string to parse.</param>
    /// <returns>An array of objects of type T parsed from the JSON.</returns>
    public static T[] FromJson<T>(string json)
    {
        // Wrap the original JSON array string inside an object with a property named "Items"
        string wrappedJson = "{\"Items\":" + json + "}";

        // Use JsonUtility to parse the wrapped JSON string into a Wrapper object
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);

        // Return the array stored in the Items property
        return wrapper.Items;
    }

    // Helper wrapper class used for deserializing the wrapped JSON
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
