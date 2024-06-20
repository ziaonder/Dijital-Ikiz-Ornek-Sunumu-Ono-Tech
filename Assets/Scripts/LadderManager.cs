using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ladders;
    [Range(0, 1)]
    public int selectedLadder;

    private void OnValidate()
    {
        for(int i = 0; i < ladders.Length; i++)
        {
            if (i == selectedLadder)
            {
                ladders[i].SetActive(true);
            }
            else
            {
                ladders[i].SetActive(false);
            }
        }
    }
}
