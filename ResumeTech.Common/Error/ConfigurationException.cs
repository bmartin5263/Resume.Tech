namespace ResumeTech.Common.Error; 

/**
 * Indicates a problem with how the application was setup that could not be determined until runtime
 */
public class ConfigurationException : Exception {
 public ConfigurationException(string? message) : base(message) { }
 public ConfigurationException(string? message, Exception? innerException) : base(message, innerException) { }
}