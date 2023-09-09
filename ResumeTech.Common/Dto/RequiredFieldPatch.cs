namespace ResumeTech.Common.Dto; 

public readonly record struct RequiredFieldPatch<T>(T NewValue);