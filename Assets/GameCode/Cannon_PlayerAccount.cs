using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_PlayerAccount 
{
    //public static Cannon_PlayerAccount account;

    public string _name;
    public string _id;
    public int currency;

    public Cannon_PlayerAccount()
    {
        _name = "Test Account";
        _id = "000000";
        currency = 100000000;
    }

    public Cannon_PlayerAccount(string name, string id, int _currency)
    {
        _name = name;
        _id = id;
        currency = _currency;
        
    }

    public void UpdateCurrencyUp(int changeInCurrency) { currency += changeInCurrency; }
    public void ChangeCurrencyDown(int changeInCurrency) { currency -= changeInCurrency; }
}
