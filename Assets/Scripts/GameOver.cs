using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Transform RespawnPointIntro;
    [SerializeField] Transform RespawnPointTown;
    [SerializeField] Monologue RespawnMonologueIntro;
    [SerializeField] Monologue RespawnMonologueTown;
    public void Respawn(Character character)
    {
        character.GetComponent<CharacterHealth>().Revive();
        character.GetComponent<CharacterWallet>().MinusGold(character.GetComponent<CharacterWallet>().GetGoldAmount() / 10);
        if (Data.location == "Intro")
        {
            RespawnMonologueIntro.gameObject.SetActive(true);
            character.transform.position = RespawnPointIntro.position;
        }
        else
        {
            RespawnMonologueTown.gameObject.SetActive(true);
            character.GetComponent<ParallaxBackgroundManager>().OffBackground();
            character.GetComponent<ParallaxBackgroundManager>().SetBackground("Town");
            //character.GetComponent<ParallaxBackgroundManager>().Teleport("Town", character.gameObject, RespawnPointTown.position);
            //character.GetComponent<ParallaxBackgroundManager>().OnValidate();
            character.transform.position = RespawnPointTown.position;
        }
        gameObject.SetActive(false);
    }
}
