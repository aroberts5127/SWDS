using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_EventHandler : MonoBehaviour
{
    public static Cannon_EventHandler instance;

    public static event Action<int> gainPointsEvent;
    public static event Action playerHitEvent;
    public static event Action collectBombEvent;
    public static event Action useBombEvent;

    void Start()
    {
        if (instance != null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    public void gainPointsHandler(int score)
    {
        gainPointsEvent?.Invoke(score);
    }
    public void playerHitHandler()
    {
        playerHitEvent?.Invoke();
    }
    public void collectBombHandler()
    {
        collectBombEvent?.Invoke();
    }
    public void usetBombHandler()
    {
        useBombEvent?.Invoke();
    }

}
