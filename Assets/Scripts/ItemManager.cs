using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Item;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Data.Item.Count; i++)
        {
            if (Data.Item[i] == 0)
                Item[i].gameObject.SetActive(false);
        }
    }

    public void Save()
    {
        Data.Item.Clear();
        foreach (GameObject item in Item)
        {
            Data.Item.Add(item.activeSelf ? 1 : 0);
        }
    }
}
