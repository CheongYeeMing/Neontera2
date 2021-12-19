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
        }
        else
        {
            character.GetComponent<ParallaxBackgroundManager>().Respawn("Town", character.gameObject, RespawnPointTown.position);
            RespawnMonologueTown.gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
