using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class QuestSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Text questName;

    public event Action<Quest> OnLeftClickEvent;

    private Quest _quest;

    public Quest Quest
    {
        get { return _quest; }
        set
        {
            _quest = value;
            if (_quest == null)
            {
                questName.enabled = false;
            }
            else
            {
                questName.text = _quest.title;
                questName.enabled = true;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Left)
        {
            if (Quest != null && OnLeftClickEvent != null)
            {
                OnLeftClickEvent(Quest);
            }
        }
    }

    protected virtual void OnValidate()
    {
        if (questName == null)
        {
            questName = GetComponent<Text>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
