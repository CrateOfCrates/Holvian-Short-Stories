using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using JetBrains.Annotations;

public class BattleText : MonoBehaviour
{
    [SerializeField] private TMP_Text textPart;
    [SerializeField] private RectTransform logBoxRect;

    [SerializeField] private int gap = 0;

    private readonly Queue<string> logQueue = new Queue<string>();
    private readonly StringBuilder stringBuilder = new StringBuilder();

    public static BattleText Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    private void UpdateDisplay()
    {
        string separator = $"\n<size={gap}%>\n</size>";
        float maxHeight = logBoxRect.rect.height;

        textPart.text = string.Join(separator, logQueue);
        textPart.ForceMeshUpdate();

        while (textPart.preferredHeight > maxHeight && logQueue.Count > 1)
        {
            logQueue.Dequeue();
            textPart.text = string.Join(separator, logQueue);
            textPart.ForceMeshUpdate();
        }
    }

    public void AddMessage(string message)
    {
        logQueue.Enqueue(message);
        UpdateDisplay();
    }

    public void Clear()
    {
        logQueue.Clear();
        UpdateDisplay();
    }

    public void SetMessage(string message)
    {
        Clear();
        AddMessage(message);
    }
}
