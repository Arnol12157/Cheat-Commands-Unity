using System;

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string CommandName { get; }
    public string Description { get; }

    public CommandAttribute(string commandName = null, string description = null)
    {
        CommandName = commandName;
        Description = description;
    }
}