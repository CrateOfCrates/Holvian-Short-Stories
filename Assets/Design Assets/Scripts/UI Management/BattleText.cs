using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using System.Text;

public class BattleText : MonoBehaviour
{
    [SerializeField] private TMP_Text textbox;
    [SerializeField] private int showlinesConfig;

    private Queue<string> messages = new Queue<string>();
    private readonly StringBuilder stringBuilder = new StringBuilder();

    public static BattleText Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void AddMessage(string message)
    {
        messages.Enqueue(message);

        while (messages.Count > showlinesConfig)
        {
            messages.Dequeue();
        }

        stringBuilder.Clear();
        foreach (string entry in messages)
        {
            stringBuilder.AppendLine(entry);
        }

        textbox.text = stringBuilder.ToString();
    }

    public void Clear()
    {
        textbox.text = "";
    }

    public void SetMessage(string message)
    {
        textbox.text = message;
    }
}
