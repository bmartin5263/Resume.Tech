namespace ResumeTech.Common.Dto; 

public readonly record struct FieldPatch<T>(T? NewValue);