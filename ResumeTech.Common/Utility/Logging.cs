using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ResumeTech.Common.Utility; 

public static class Logging {
    public static ILoggerFactory? Factory { get; set; } = LoggerFactory.Create(logging => logging.AddSimpleConsole(options => {
        options.SingleLine = true;
        options.ColorBehavior = LoggerColorBehavior.Disabled;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
        options.UseUtcTimestamp = false;
    }));
    
    public static ILogger CreateLogger<T>() => Factory!.CreateLogger<T>();        
    public static ILogger CreateLogger(string categoryName) => Factory!.CreateLogger(categoryName);
}