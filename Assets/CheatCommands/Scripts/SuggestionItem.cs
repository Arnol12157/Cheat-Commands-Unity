using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuggestionItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _suggestionText;
    [SerializeField] private Button _setCommandButton;
    [SerializeField] private Button _viewInfoButton;
    public Action OnViewInfoButton;
    public Action OnSetCommandButton;

    private void Awake()
    {
        _viewInfoButton.onClick.AddListener(ViewInfoButton);
        _setCommandButton.onClick.AddListener(SetCommandButton);
    }

    public void Init(string suggestionText, Action onViewInfoButton, Action onSetCommandButton)
    {
        _suggestionText.SetText(suggestionText);
        OnViewInfoButton = onViewInfoButton;
        OnSetCommandButton = onSetCommandButton;
    }

    private void ViewInfoButton()
    {
        OnViewInfoButton?.Invoke();
    }
    
    private void SetCommandButton()
    {
        OnSetCommandButton?.Invoke();
    }
}
