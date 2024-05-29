using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CheatCommandsWindow : EditorWindow
{
    private string _input;
    private List<string> _commandHistory = new List<string>();
    private int _historyIndex = -1;
    private Vector2 _scrollPosition;
    private List<string> _outputLog = new List<string>();
    private List<string> _suggestions = new List<string>();
    private string _selectedCommand;

    [MenuItem("TG Tools/Cheat Commands")]
    public static void ShowWindow()
    {
        GetWindow<CheatCommandsWindow>("Cheat Commands");
        CommandRegistry.RegisterCommandsFromAllAssemblies();
    }

    private void OnGUI()
    {
        _input = EditorGUILayout.TextField("Command", _input);

        // Update suggestions based on current input
        UpdateSuggestions(_input);

        if (GUILayout.Button("Execute"))
        {
            var log = CommandExecutor.ExecuteCommand(_input);
            _outputLog.Add(log);
            _commandHistory.Add(_input);
            _historyIndex = _commandHistory.Count;
            _input = ""; // Clear the input field after executing
        }

        // Display the command history and allow navigation with up/down arrow keys
        HandleHistoryNavigation();

        // Display suggestions
        DisplaySuggestions();

        // Display the output log
        GUILayout.Label("Output Log", EditorStyles.boldLabel);
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        foreach (var log in _outputLog)
        {
            GUILayout.Label(log);
        }
        GUILayout.EndScrollView();
    }

    private void UpdateSuggestions(string input)
    {
        _suggestions.Clear();
        if (!string.IsNullOrEmpty(input))
        {
            _suggestions = CommandRegistry.GetCommandNames()
                                         .Where(cmd => cmd.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                                         .ToList();
        }
    }

    private void DisplaySuggestions()
    {
        if (_suggestions.Count > 0)
        {
            GUILayout.Label("Suggestions", EditorStyles.boldLabel);
            foreach (var suggestion in _suggestions)
            {
                if (GUILayout.Button(suggestion))
                {
                    _input = suggestion;
                    _selectedCommand = suggestion;
                    Repaint();
                }
            }

            if (!string.IsNullOrEmpty(_selectedCommand))
            {
                DisplayCommandInfo(_selectedCommand);
            }
        }
    }

    private void DisplayCommandInfo(string commandName)
    {
        var method = CommandRegistry.GetCommand(commandName);
        if (method != null)
        {
            GUILayout.Label($"Command: {commandName}", EditorStyles.boldLabel);
            GUILayout.Label($"Description: {method.GetCustomAttribute<CommandAttribute>()?.Description ?? "No description available."}");
            GUILayout.Label("Parameters:", EditorStyles.boldLabel);

            foreach (var param in method.GetParameters())
            {
                GUILayout.Label($"{param.Name} ({param.ParameterType.Name})");
            }
        }
    }

    private void HandleHistoryNavigation()
    {
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.UpArrow)
            {
                if (_historyIndex > 0)
                {
                    _historyIndex--;
                    _input = _commandHistory[_historyIndex];
                    Repaint();
                }
            }
            else if (Event.current.keyCode == KeyCode.DownArrow)
            {
                if (_historyIndex < _commandHistory.Count - 1)
                {
                    _historyIndex++;
                    _input = _commandHistory[_historyIndex];
                    Repaint();
                }
            }
        }
    }
}
