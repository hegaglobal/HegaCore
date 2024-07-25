using UnityEngine;
using TMPro;
using UnityEditor;

public class ObservableLocalizeTextMeshProUGUI : TextMeshProUGUI
{
    public event System.Action<string> OnTextChanged;
    public override string text
    {
        get => base.text;
        set
        {
            if (base.text != value)
            {
                base.text = value;
                OnTextChanged?.Invoke(value);
            }
        }
    }
}
