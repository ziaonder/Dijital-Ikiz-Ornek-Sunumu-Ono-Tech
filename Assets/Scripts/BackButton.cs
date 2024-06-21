using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public static event Action OnBackButtonClicked;

    public void OnBackButtonClickedEvent()
    {
        OnBackButtonClicked?.Invoke();
    }
}
