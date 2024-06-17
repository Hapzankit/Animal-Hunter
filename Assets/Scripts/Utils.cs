using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
public static class Utils 
{
    public static void PunchOnButtonClick(GameObject ButtonObject, Action taskAfterPunch = null)
    {

        RectTransform rectTransform = ButtonObject.GetComponent<RectTransform>();
        rectTransform.DOKill(true);
        rectTransform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.3f, 1, 1).OnComplete(() =>
        {
            // Invoke the callback if it is set
            taskAfterPunch?.Invoke();
        });
    }
}
