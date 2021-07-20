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
        
        if (character.GetComponent<CharacterMovement>().location == "Intro")
        {
            character.GetComponent<ParallaxBackgroundManager>().Respawn("Intro", character.gameObject, RespawnPointIntro.position);
            RespawnMonologueIntro.gameObject.SetActive(true);
            //character.transform.position = RespawnPointIntro.position;
        }
        else
        {

            //character.GetComponent<ParallaxBackgroundManager>().OffBackground();
            //character.GetComponent<ParallaxBackgroundManager>().SetBackground("Town");
            //character.GetComponent<ParallaxBackgroundManager>().Teleport("Town", character.gameObject, RespawnPointTown.position);'
            character.GetComponent<ParallaxBackgroundManager>().Respawn("Town", character.gameObject, RespawnPointTown.position);
            RespawnMonologueTown.gameObject.SetActive(true);
            //character.transform.position = RespawnPointTown.position;
        }
        //character.GetComponent<CharacterHealth>().Revive();
        //character.GetComponent<CharacterWallet>().MinusGold(character.GetComponent<CharacterWallet>().GetGoldAmount() / 10);
        gameObject.SetActive(false);
    }
}
