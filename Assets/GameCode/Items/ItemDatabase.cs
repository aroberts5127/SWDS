using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/Items", order = 1)]
public class ItemDatabase : ScriptableObject {

    [SerializeField]
    private ItemData[] m_CItems;

    [SerializeField]
    private ItemData[] m_UCItems;

    [SerializeField]
    private ItemData[] m_RItems;

    [SerializeField]
    private ItemData[] m_URItems;

    public ItemData[] CItems { get { return m_CItems; } }

    public ItemData[] UCItems { get { return m_CItems; } }

    public ItemData[] RItems { get { return m_CItems; } }

    public ItemData[] URItems { get { return m_CItems; } }


}


