using System.Collections.Generic;
using UnityEngine;

public class MonologueManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Monologue;
    // Start is called before the first frame update
    public void Start()
    {
        for (int i = 0; i < Data.Monologue.Count; i++)
        {
            if (Data.Monologue[i] == 0)
            {
                Monologue[i].gameObject.SetActive(false);
            }
        }
    }

    public void Save()
    {
        Data.Monologue.Clear();
        foreach(GameObject monologue in Monologue)
        {
            Data.Monologue.Add(monologue.activeSelf ? 1 : 0);
        }
    }
}
