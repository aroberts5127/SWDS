using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/Items", order = 1)]
public class ItemDatabase : ScriptableObject {

    [SerializeField]
    private ItemData[] m_CItems = null;

    [SerializeField]
    private ItemData[] m_UCItems = null;

    [SerializeField]
    private ItemData[] m_RItems = null;

    [SerializeField]
    private ItemData[] m_URItems = null;

    public ItemData[] CItems { get { return m_CItems; } }

    public ItemData[] UCItems { get { return m_CItems; } }

    public ItemData[] RItems { get { return m_CItems; } }

    public ItemData[] URItems { get { return m_CItems; } }


    public ItemDatabase()
    {
        m_CItems = new ItemData[0];
        m_UCItems = new ItemData[0];
        m_RItems = new ItemData[0];
        m_URItems = new ItemData[0];
    }
}


