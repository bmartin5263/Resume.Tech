namespace ResumeTech.Common.Utility; 

public static class DictionaryUtils {

    public static V? Get<K, V>(this IDictionary<K, V> dict, K key, V? orElse) {
        return dict.TryGetValue(key, out var value) ? value : orElse;
    }

}