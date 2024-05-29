using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatCommandsCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField _commandInputField;
    [SerializeField] private Button _cleanCommandButton;
    [SerializeField] private Button _executeButton;
    [SerializeField] private Transform _suggestionsContent;
    [SerializeField] private GameObject _suggestionItem;
    [SerializeField] private TMP_Text _commandInfoText;
    [SerializeField] private TMP_Text _logsText;
    
    private List<string> suggestions = new List<string>();
    private string _prevCommand = "";
    private string _fullLog = "";
    
    private void Awake()
    {
        CommandRegistry.RegisterCommandsFromAllAssemblies();
        _executeButton.onClick.AddListener(ExecuteCommand);
        _cleanCommandButton.onClick.AddListener(CleanCommandInputField);
    }

    private void ExecuteCommand()
    {
        var outputLog = CommandExecutor.ExecuteCommand(_commandInputField.text);
        _fullLog += $"{outputLog}\n";
    }

    private void Update()
    {
        UpdateSuggestions(_commandInputField.text);
        UpdateLogs();
        _cleanCommandButton.gameObject.SetActive(!string.IsNullOrEmpty(_commandInputField.text));
    }

    private void UpdateSuggestions(string input)
    {
        suggestions.Clear();
        if (!string.IsNullOrEmpty(input))
        {
            suggestions = CommandRegistry.GetCommandNames()
                .Where(cmd => cmd.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (input != _prevCommand)
        {
            _prevCommand = input;
            _commandInfoText.SetText("");
            CleanSuggestionsContent();
            DisplaySuggestions();
        }
    }

    private void CleanSuggestionsContent()
    {
        foreach (Transform suggestionItem in _suggestionsContent.transform)
        {
            Destroy(suggestionItem.gameObject);
        }
    }
    
    private void DisplaySuggestions()
    {
        if (suggestions.Count > 0)
        {
            foreach (var suggestion in suggestions)
            {
                var suggestionItem = Instantiate(_suggestionItem, _suggestionsContent.transform);
                suggestionItem.GetComponent<SuggestionItem>().Init(suggestion, () => DisplayCommandInfo(suggestion), () => SetCommand(suggestion));
            }
        }
    }
    
    private void DisplayCommandInfo(string commandName)
    {
        var method = CommandRegistry.GetCommand(commandName);
        if (method != null)
        {
            var commandInfo = $"<b>Command:</b> {commandName}\n"
                              + $"<b>Description:</b> {method.GetCustomAttribute<CommandAttribute>()?.Description ?? "No description available."}\n"
                              + "<b>Parameters:</b>\n";
            foreach (var param in method.GetParameters())
            {
                commandInfo += $"{param.Name} ({param.ParameterType.Name})\n";
            }
            _commandInfoText.SetText(commandInfo);
        }
    }

    private void SetCommand(string command)
    {
        _commandInputField.text = command;
    }

    private void UpdateLogs()
    {
        _logsText.SetText(_fullLog);
    }
    
    private void CleanCommandInputField()
    {
        _commandInputField.text = "";
    }
}
