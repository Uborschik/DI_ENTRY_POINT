using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
public sealed class InjectAttribute : Attribute { }
