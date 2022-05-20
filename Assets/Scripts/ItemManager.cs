using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // List if In-Game Items on the ground
    [SerializeField] List<GameObject> Item;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < Data.Item.Count; i++)
        {
            if (Data.Item[i] == 0)
            {
                Item[i].gameObject.SetActive(false);
            }
        }
    }

    // Saves the state of Items on the ground
    // Whether they have been picked up before
    public void Save()
    {
        Data.Item.Clear();
        foreach (GameObject item in Item)
        {
            Data.Item.Add(item.activeSelf ? 1 : 0);
        }
    }
}
