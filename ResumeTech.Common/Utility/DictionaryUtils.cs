namespace ResumeTech.Common.Utility; 

public static class DictionaryUtils {

    public static V? Find<K, V>(this IDictionary<K, V> dict, K key) {
        return dict.TryGetValue(key, out var value) ? value : default;
    }

    public static V GetOrInsert<D, K, V>(this D self, K key, Func<V> defaultValue) where D : IDictionary<K, V> {
        if (self.TryGetValue(key, out var value)) {
            return value;
        }
        value = defaultValue();
        self[key] = value;
        return value;
    }
    
}