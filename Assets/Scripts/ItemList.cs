using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    [SerializeField] Item item_1;

    public Item GetItem(int i)
    {
        if (i == 1) return item_1;
        return item_1;
    }
}
