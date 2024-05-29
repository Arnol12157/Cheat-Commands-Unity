using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class CommandRegistry
{
    private static Dictionary<string, MethodInfo> _commandMethods = new Dictionary<string, MethodInfo>();
    private static Dictionary<string, object> _commandInstances = new Dictionary<string, object>();

    public static void RegisterCommands(object instance)
    {
        var type = instance.GetType();
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<CommandAttribute>();
            if (attribute != null)
            {
                var commandName = attribute.CommandName?.ToLower() ?? method.Name.ToLower();
                _commandMethods[commandName] = method;
                if (!method.IsStatic)
                {
                    _commandInstances[commandName] = instance;
                }
            }
        }
    }

    public static void RegisterCommandsFromAllAssemblies()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attribute = method.GetCustomAttribute<CommandAttribute>();
                    if (attribute != null)
                    {
                        var commandName = attribute.CommandName?.ToLower() ?? method.Name.ToLower();
                        _commandMethods[commandName] = method;
                        if (!method.IsStatic)
                        {
                            var instance = Activator.CreateInstance(type);
                            _commandInstances[commandName] = instance;
                        }
                    }
                }
            }
        }
    }

    public static MethodInfo GetCommand(string commandName)
    {
        _commandMethods.TryGetValue(commandName.ToLower(), out var method);
        return method;
    }

    public static object GetCommandInstance(string commandName)
    {
        _commandInstances.TryGetValue(commandName.ToLower(), out var instance);
        return instance;
    }

    public static IEnumerable<string> GetCommandNames()
    {
        return _commandMethods.Keys;
    }

    public static object ConvertArgument(string argument, Type targetType)
    {
        if (targetType == typeof(string))
        {
            return argument;
        }
        if (targetType == typeof(int))
        {
            return int.Parse(argument);
        }
        if (targetType == typeof(float))
        {
            return float.Parse(argument);
        }
        if (targetType == typeof(bool))
        {
            return bool.Parse(argument);
        }
        if (targetType == typeof(Vector3))
        {
            var parts = argument.Trim('(', ')').Split(',');
            return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
        }
        if (targetType == typeof(Vector2))
        {
            var parts = argument.Trim('(', ')').Split(',');
            return new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
        }
        // Add more types as needed
        return Convert.ChangeType(argument, targetType);
    }
}