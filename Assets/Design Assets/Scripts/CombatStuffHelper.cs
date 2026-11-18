using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public static class CombatStuffHelper
{
    //convert the targeting of a move or token into if aim is needed or not 
    public static bool NeedAim(Target target)
    {
        if(target == Target.target)
        {
            return true;
        } else if (target == Target.entireSide)
        {
            return true;
        } else
        {
            return false;
        }
    }

    //return what a mouse is hovering over
    public static GameObject HoverScan()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        if(raysastResults.Count > 0){
            return raysastResults[0].gameObject;
        }
        else
        {
            return null;
        }
    }


}
