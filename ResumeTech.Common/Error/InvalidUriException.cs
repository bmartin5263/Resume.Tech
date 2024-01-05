namespace ResumeTech.Common.Error; 

public class InvalidUriException : Exception {
    public InvalidUriException(string? message) : base(message) { }

    public static InvalidUriException UriNotFound(string uri) {
        return new InvalidUriException($"Invalid URI: {uri}");
    }
}