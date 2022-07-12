using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField] private Button[] _buttons;

    public UnityEvent OnAnyClicked = new UnityEvent();

    private Dictionary<Button, UnityAction> _onClickActions = new Dictionary<Button, UnityAction>();

    private void OnEnable()
    {
        foreach (var button in _buttons)
        {
            _onClickActions.Add(button, () => SelectButton(button));
            button.onClick.AddListener(_onClickActions[button]);
        }
    }

    private void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.onClick.RemoveListener(_onClickActions[button]);
            _onClickActions.Remove(button);
        }
    }

    private void Start()
    {
        if (_buttons.Length == 0) return;

        _buttons[0].interactable = false;
    }

    private void SelectButton(Button selectedButton)
    {
        foreach (var button in _buttons)
        {
            button.interactable = true;
        }

        selectedButton.interactable = false;
        OnAnyClicked?.Invoke();
    }
}
