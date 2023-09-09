namespace ResumeTech.Common.Attributes;

public interface IConst {
    
}

[AttributeUsage(AttributeTargets.Property)]
public class ConstAttribute : Attribute, IConst {
    
}

[AttributeUsage(AttributeTargets.Property)]
public class ConstAttribute<T> : Attribute, IConst {
    
}