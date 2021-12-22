using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Transform RespawnPointIntro;
    [SerializeField] private Transform RespawnPointTown;
    [SerializeField] private Monologue RespawnMonologueIntro;
    [SerializeField] private Monologue RespawnMonologueTown;
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
