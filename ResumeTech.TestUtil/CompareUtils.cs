using System.Reflection;
using ResumeTech.Common.Utility;

namespace ResumeTech.TestUtil;

public static class CompareUtils {

   public static bool RecursiveEquals<T>(this T actual, T expected, params string[] ignore) where T : class {
      var unequalProperties = new List<UnequalProperty>();
      var ignoreList = ignore.ToList();
      actual.RecursiveEquals(expected, typeof(T), unequalProperties, ignoreList);
      Console.WriteLine(unequalProperties.ToExpandedString());
      return !unequalProperties.Any();
   }
   
   private static void RecursiveEquals(this object actual, object expected, Type type, IList<UnequalProperty> unequalProperties, IList<string> ignore) {
      var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
         .Where(p => !ignore.Contains(p.Name) && p.GetIndexParameters().Length == 0);

      foreach (var property in properties) {
         var propertyType = property.GetUnderlyingType();
         var actualValue = type.GetProperty(property.Name)!.GetValue(actual, null);
         var expectedValue = type.GetProperty(property.Name)!.GetValue(expected, null);
         if (propertyType.IsSimpleType()) {
            if (!Equals(actualValue, expectedValue)) {
               unequalProperties.Add(new (property.Name, expectedValue, actualValue));
            }
         }
         else {
            if (expectedValue == null && actualValue != null) {
               unequalProperties.Add(new (property.Name, expectedValue, actualValue));
            }
            else if (expectedValue != null && actualValue == null) {
               unequalProperties.Add(new (property.Name, expectedValue, actualValue));
            }
            else if (expectedValue != null && actualValue != null) {
               RecursiveEquals(actualValue, expectedValue, propertyType, unequalProperties, ignore);
            }
         }
      }
   }
   
   /// <summary>
   /// Determine whether a type is simple (String, Decimal, DateTime, etc) 
   /// or complex (i.e. custom class with public properties and methods).
   /// </summary>
   /// <see cref="http://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive"/>
   public static bool IsSimpleType(
      this Type type)
   {
      return
         type.IsValueType ||
         type.IsPrimitive ||
         new[]
         {
            typeof(String),
            typeof(Decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
         }.Contains(type) ||
         (Convert.GetTypeCode(type) != TypeCode.Object);
   }

   public static Type GetUnderlyingType(this MemberInfo member)
   {
      switch (member.MemberType)
      {
         case MemberTypes.Event:
            return ((EventInfo)member).EventHandlerType!;
         case MemberTypes.Field:
            return ((FieldInfo)member).FieldType;
         case MemberTypes.Method:
            return ((MethodInfo)member).ReturnType;
         case MemberTypes.Property:
            return ((PropertyInfo)member).PropertyType;
         default:
            throw new ArgumentException
            (
               "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
            );
      }
   }
}

public record UnequalProperty(string Name, object? Expected, object? Actual);