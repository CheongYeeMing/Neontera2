using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Item;
    //[SerializeField] GameObject Intro_Item_1;
    //[SerializeField] GameObject Intro_Item_2;
    //[SerializeField] GameObject Intro_Item_3;
    //[SerializeField] GameObject Intro_Item_4;
    //[SerializeField] GameObject Intro_Item_5;
    //[SerializeField] GameObject Intro_Item_6;

    //[SerializeField] GameObject Town_Item_1;
    //[SerializeField] GameObject Town_Item_2;

    //[SerializeField] GameObject Forest_Item_1;
    //[SerializeField] GameObject Forest_Item_2;
    //[SerializeField] GameObject Forest_Item_3;
    //[SerializeField] GameObject Forest_Item_4;
    //[SerializeField] GameObject Forest_Item_5;
    //[SerializeField] GameObject Forest_Item_6;

    //[SerializeField] GameObject Cave_Item_1;
    //[SerializeField] GameObject Cave_Item_2;
    //[SerializeField] GameObject Cave_Item_3;
    //[SerializeField] GameObject Cave_Item_4;
    //[SerializeField] GameObject Cave_Item_5;
    //[SerializeField] GameObject Cave_Item_6;
    //[SerializeField] GameObject Cave_Item_7;

    //[SerializeField] GameObject BS_Item_1;
    //[SerializeField] GameObject BS_Item_2;
    //[SerializeField] GameObject BS_Item_3;
    //[SerializeField] GameObject BS_Item_4;
    //[SerializeField] GameObject BS_Item_5;

    //[SerializeField] GameObject SS_Item_1;
    //[SerializeField] GameObject SS_Item_2;
    //[SerializeField] GameObject SS_Item_3;
    //[SerializeField] GameObject SS_Item_4;

    //[SerializeField] GameObject SA1_Item_1;
    //[SerializeField] GameObject SA1_Item_2;

    //[SerializeField] GameObject SA2_Item_1;
    //[SerializeField] GameObject SA2_Item_2;
    //[SerializeField] GameObject SA2_Item_3;
    //[SerializeField] GameObject SA2_Item_4;
    //[SerializeField] GameObject SA2_Item_5;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Data.Item.Count; i++)
        {
            if (Data.Item[i] == 0)
                Item[i].gameObject.SetActive(false);
        }
        //if (Data.IntroItem1 == 0) Intro_Item_1.SetActive(false);
        //if (Data.IntroItem2 == 0) Intro_Item_2.SetActive(false);
        //if (Data.IntroItem3 == 0) Intro_Item_3.SetActive(false);
        //if (Data.IntroItem4 == 0) Intro_Item_4.SetActive(false);
        //if (Data.IntroItem5 == 0) Intro_Item_5.SetActive(false);
        //if (Data.IntroItem6 == 0) Intro_Item_6.SetActive(false);

        //if (Data.TownItem1 == 0) Town_Item_1.SetActive(false);
        //if (Data.TownItem2 == 0) Town_Item_2.SetActive(false);

        //if (Data.ForestItem1 == 0) Forest_Item_1.SetActive(false);
        //if (Data.ForestItem2 == 0) Forest_Item_2.SetActive(false);
        //if (Data.ForestItem3 == 0) Forest_Item_3.SetActive(false);
        //if (Data.ForestItem4 == 0) Forest_Item_4.SetActive(false);
        //if (Data.ForestItem5 == 0) Forest_Item_5.SetActive(false);
        //if (Data.ForestItem6 == 0) Forest_Item_6.SetActive(false);

        //if (Data.CaveItem1 == 0) Cave_Item_1.SetActive(false);
        //if (Data.CaveItem2 == 0) Cave_Item_2.SetActive(false);
        //if (Data.CaveItem3 == 0) Cave_Item_3.SetActive(false);
        //if (Data.CaveItem4 == 0) Cave_Item_4.SetActive(false);
        //if (Data.CaveItem5 == 0) Cave_Item_5.SetActive(false);
        //if (Data.CaveItem6 == 0) Cave_Item_6.SetActive(false);
        //if (Data.CaveItem7 == 0) Cave_Item_7.SetActive(false);

        //if (Data.BSItem1 == 0) BS_Item_1.SetActive(false);
        //if (Data.BSItem2 == 0) BS_Item_2.SetActive(false);
        //if (Data.BSItem3 == 0) BS_Item_3.SetActive(false);
        //if (Data.BSItem4 == 0) BS_Item_4.SetActive(false);
        //if (Data.BSItem5 == 0) BS_Item_5.SetActive(false);

        //if (Data.SSItem1 == 0) SS_Item_1.SetActive(false);
        //if (Data.SSItem2 == 0) SS_Item_2.SetActive(false);
        //if (Data.SSItem3 == 0) SS_Item_3.SetActive(false);
        //if (Data.SSItem4 == 0) SS_Item_4.SetActive(false);

        //if (Data.SA1Item1 == 0) SA1_Item_1.SetActive(false);
        //if (Data.SA1Item2 == 0) SA1_Item_2.SetActive(false);

        //if (Data.SA2Item1 == 0) SA2_Item_1.SetActive(false);
        //if (Data.SA2Item2 == 0) SA2_Item_2.SetActive(false);
        //if (Data.SA2Item3 == 0) SA2_Item_3.SetActive(false);
        //if (Data.SA2Item4 == 0) SA2_Item_4.SetActive(false);
        //if (Data.SA2Item5 == 0) SA2_Item_5.SetActive(false);
    }

    public void Save()
    {
        Data.Item.Clear();
        foreach (GameObject item in Item)
        {
            Data.Item.Add(item.activeSelf ? 1 : 0);
        }
        //Data.IntroItem1 = Intro_Item_1.activeSelf ? 1 : 0;
        //Data.IntroItem2 = Intro_Item_2.activeSelf ? 1 : 0;
        //Data.IntroItem3 = Intro_Item_3.activeSelf ? 1 : 0;
        //Data.IntroItem4 = Intro_Item_4.activeSelf ? 1 : 0;
        //Data.IntroItem5 = Intro_Item_5.activeSelf ? 1 : 0;
        //Data.IntroItem6 = Intro_Item_6.activeSelf ? 1 : 0;

        //Data.TownItem1 = Town_Item_1.activeSelf ? 1 : 0;
        //Data.TownItem2 = Town_Item_2.activeSelf ? 1 : 0;

        //Data.CaveItem1 = Cave_Item_1.activeSelf ? 1 : 0;
        //Data.CaveItem2 = Cave_Item_2.activeSelf ? 1 : 0;
        //Data.CaveItem3 = Cave_Item_3.activeSelf ? 1 : 0;
        //Data.CaveItem4 = Cave_Item_4.activeSelf ? 1 : 0;
        //Data.CaveItem5 = Cave_Item_5.activeSelf ? 1 : 0;
        //Data.CaveItem6 = Cave_Item_6.activeSelf ? 1 : 0;
        //Data.CaveItem7 = Cave_Item_7.activeSelf ? 1 : 0;

        //Data.BSItem1 = BS_Item_1.activeSelf ? 1 : 0;
        //Data.BSItem2 = BS_Item_2.activeSelf ? 1 : 0;
        //Data.BSItem3 = BS_Item_3.activeSelf ? 1 : 0;
        //Data.BSItem4 = BS_Item_4.activeSelf ? 1 : 0;
        //Data.BSItem5 = BS_Item_5.activeSelf ? 1 : 0;

        //Data.SSItem1 = SS_Item_1.activeSelf ? 1 : 0;
        //Data.SSItem2 = SS_Item_2.activeSelf ? 1 : 0;
        //Data.SSItem3 = SS_Item_3.activeSelf ? 1 : 0;
        //Data.SSItem4 = SS_Item_4.activeSelf ? 1 : 0;

        //Data.SA1Item1 = SA1_Item_1.activeSelf ? 1 : 0;
        //Data.SA1Item2 = SA1_Item_2.activeSelf ? 1 : 0;

        //Data.SA2Item1 = SA2_Item_1.activeSelf ? 1 : 0;
        //Data.SA2Item2 = SA2_Item_2.activeSelf ? 1 : 0;
        //Data.SA2Item3 = SA2_Item_3.activeSelf ? 1 : 0;
        //Data.SA2Item4 = SA2_Item_4.activeSelf ? 1 : 0;
        //Data.SA2Item5 = SA2_Item_5.activeSelf ? 1 : 0;
    }
}
