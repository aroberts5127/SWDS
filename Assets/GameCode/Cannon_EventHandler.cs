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
    public static event Action userLoadedEvent;
    public static event Action userCurrencyUpdateEvent;
    public static event Action resetEnemyCountEvent;
    public static event Action<int> updateEnemyCountEvent;

    void Awake()
    {
        if (instance == null)
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
    public void useBombHandler()
    {
        useBombEvent?.Invoke();
    }

    public void userLoadedHandler()
    {
        userLoadedEvent?.Invoke();
    }

    public void currencyUpdateHandler()
    {
        userCurrencyUpdateEvent?.Invoke();
    }

    public void resetEnemyCountHandler()
    {
        resetEnemyCountEvent?.Invoke();
    }

    public void updateEnemyCountHandler(int i)
    {
        updateEnemyCountEvent?.Invoke(i);
    }

}
