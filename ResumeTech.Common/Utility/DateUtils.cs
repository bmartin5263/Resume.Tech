using System.Globalization;

namespace ResumeTech.Common.Utility; 

public static class DateUtils {

    public static DateOnly SlashedDate(this string str) {
        return DateOnly.Parse(str, new CultureInfo("en-US"));
    }
    
}