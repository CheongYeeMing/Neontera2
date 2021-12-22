using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Transform respawnPointIntro;
    [SerializeField] private Transform respawnPointTown;
    [SerializeField] private Monologue respawnMonologueIntro;
    [SerializeField] private Monologue respawnMonologueTown;
    public void Respawn(Character character)
    {
        
        if (character.GetComponent<CharacterMovement>().location == "Intro")
        {
            character.GetComponent<ParallaxBackgroundManager>().Respawn("Intro", character.gameObject, respawnPointIntro.position);
            respawnMonologueIntro.gameObject.SetActive(true);
        }
        else
        {
            character.GetComponent<ParallaxBackgroundManager>().Respawn("Town", character.gameObject, respawnPointTown.position);
            respawnMonologueTown.gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
