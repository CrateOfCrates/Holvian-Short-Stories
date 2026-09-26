using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
    
public class DisplayButtonUI : MonoBehaviour
{

    public void Display(bool status)
    {
        gameObject.SetActive(status);
    }

}