using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CommandExecutor
{
    public static string ExecuteCommand(string commandInput)
    {
        if (string.IsNullOrWhiteSpace(commandInput))
        {
            return "Command cannot be empty.";
        }

        var parts = Regex.Matches(commandInput, @"[\""].+?[\""]|[^ ]+")
                         .Cast<Match>()
                         .Select(m => m.Value)
                         .ToArray();

        if (parts.Length == 0)
        {
            return "No command found.";
        }

        var commandName = parts[0].ToLower();
        var args = parts.Skip(1).ToArray();

        var method = CommandRegistry.GetCommand(commandName);
        if (method == null)
        {
            return $"Command '{commandName}' not found.";
        }

        var parameters = method.GetParameters();
        var parameterValues = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            var paramType = parameters[i].ParameterType;
            if (i >= args.Length)
            {
                parameterValues[i] = Type.Missing;
            }
            else
            {
                try
                {
                    var arg = args[i].Trim('"');
                    parameterValues[i] = CommandRegistry.ConvertArgument(arg, paramType);
                }
                catch (Exception e)
                {
                    return $"Error converting argument '{args[i]}' to type {paramType}: {e.Message}";
                }
            }
        }

        var instance = CommandRegistry.GetCommandInstance(commandName);
        try
        {
            var result = method.Invoke(instance, parameterValues);
            if (result != null)
            {
                return result.ToString();
            }
            return $"Command '{commandName}' executed successfully.";
        }
        catch (Exception e)
        {
            return $"Error executing command '{commandName}': {e.Message}";
        }
    }
}
