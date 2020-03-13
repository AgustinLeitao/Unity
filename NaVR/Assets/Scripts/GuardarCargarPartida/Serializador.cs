using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
 
/// <summary>
/// Store objects in PlayerPrefs
/// </summary>
static class Serializador {
    /// <summary>
    /// Serialize the <paramref name="item"/> and save it to PlayerPrefs under the <paramref name="key"/>.
    /// <paramref name="item"/> class must have the [Serializable] attribute. Use the
    /// [NonSerialized] attribute on fields you do not want serialized with the class.
    /// </summary>
    /// <param name="item">The object</param>
    internal static string Serializar(object item) {
        using (var stream = new MemoryStream()) {
            formatter.Serialize(stream, item);
            var bytes = stream.ToArray();
            return Convert.ToBase64String(bytes);
        }
    }
 
    /// <summary>
    /// Load the <paramref name="key"/> from PlayerPrefs and deserialize it.
    /// </summary>
    /// <param name="key">The key</param>
    internal static T Deserializar<T>(string serialized) { 
        var bytes = Convert.FromBase64String(serialized);
 
        T deserialized;
        using (var stream = new MemoryStream(bytes)) {
            deserialized = (T)formatter.Deserialize(stream);
        }
 
        return deserialized;
    }
 
    static readonly BinaryFormatter formatter = new BinaryFormatter();
}