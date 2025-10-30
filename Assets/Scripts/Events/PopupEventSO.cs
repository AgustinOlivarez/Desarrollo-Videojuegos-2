using System;
using System.Windows.Input;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/PopupEvent")]
public class PopupEventSO : ScriptableObject
{
    public Action<string, string, ICommand> OnShowPopup;
    public void Raise(string title, string message, ICommand buttonCommand)
    {
        OnShowPopup?.Invoke(title, message, buttonCommand);
    }
}