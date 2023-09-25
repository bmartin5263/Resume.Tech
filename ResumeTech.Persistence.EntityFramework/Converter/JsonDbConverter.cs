using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResumeTech.Common.Json;

namespace ResumeTech.Persistence.EntityFramework.Converter; 

public class JsonDbConverter<T> : ValueConverter<T, string> {
    public JsonDbConverter()
        : base(
            v => IJsonParser.Default.Write(v).ToString(),
            v => IJsonParser.Default.ReadOrElse(v, default(T))!
        )
    {
    }
}