using Microsoft.Extensions.Logging;

namespace ResumeTech.Common.Utility; 

public static class Logging {
    public static ILoggerFactory? LoggerFactory { get; set; } = new LoggerFactory();
    public static ILogger CreateLogger<T>() => LoggerFactory!.CreateLogger<T>();        
    public static ILogger CreateLogger(string categoryName) => LoggerFactory!.CreateLogger(categoryName);
}