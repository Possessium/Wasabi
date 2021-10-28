using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;

public class WSB_Ban : MonoBehaviour
{
    public static WSB_Ban I { get; private set; }


    [SerializeField] WSB_PlayerMovable player = null;
    public WSB_PlayerMovable Player { get { return player; } }

    [SerializeField] WSB_PlayerInteraction playerInteraction = null;
    public WSB_PlayerInteraction PlayerInteraction { get { return playerInteraction; } }


    private void Awake()
    {
        I = this;
    }
}
