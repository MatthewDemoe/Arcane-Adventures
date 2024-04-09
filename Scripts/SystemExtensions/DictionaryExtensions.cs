using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions
{
    public static class DictionaryExtensions
    {
        public static void ReplaceWith<Key, Value>(this Dictionary<Key, Value> dictionaryToReplace, Dictionary<Key, Value> replacementDictionary)
        {
            dictionaryToReplace.Clear();
            dictionaryToReplace.AddRange(replacementDictionary);
        }

        public static void AddRange<Key, Value>(this Dictionary<Key, Value> dictionaryToAddTo, Dictionary<Key, Value> dictionaryToAddFrom)
        {
            foreach (var keyValuePair in dictionaryToAddFrom)
            {
                dictionaryToAddTo.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}