using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

}

[Serializable]
public class ItemData
{
    #region Private
    [SerializeField]
    private string m_Name;
    [SerializeField]
    private int m_ID;
    [SerializeField]
    private GameObject m_RepObject;
    #endregion

    public string Name { get { return m_Name; } }

    public int ID { get { return m_ID; } }

    public GameObject RepObject { get { return m_RepObject; } }

}
