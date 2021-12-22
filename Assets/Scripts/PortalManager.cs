using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Portal;

    private void Start()
    {
        for (int i = 0; i < Data.Portal.Count; i++)
        {
            if (Data.Portal[i] == 1)
                Portal[i].GetComponent<Portal>().isActivated = true;
        }
    }

    public void Save()
    {
        Data.Portal.Clear();
        foreach (GameObject portal in Portal)
        {
            Data.Portal.Add(portal.GetComponent<Portal>().isActivated ? 1 : 0);
        }
    }
}
