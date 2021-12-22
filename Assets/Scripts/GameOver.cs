using UnityEngine;

public class GameOver : MonoBehaviour
{
    private const string LOCATION_INTRO = "Intro";
    private const string LOCATION_TOWN = "Town";

    [SerializeField] private Transform respawnPointIntro;
    [SerializeField] private Transform respawnPointTown;
    [SerializeField] private Monologue respawnMonologueIntro;
    [SerializeField] private Monologue respawnMonologueTown;

    public void Respawn(Character character)
    {
        
        if (character.GetComponent<CharacterMovement>().location == LOCATION_INTRO)
        {
            character.GetComponent<ParallaxBackgroundManager>().Respawn(LOCATION_INTRO, character.gameObject, respawnPointIntro.position);
            respawnMonologueIntro.gameObject.SetActive(true);
        }
        else
        {
            character.GetComponent<ParallaxBackgroundManager>().Respawn(LOCATION_TOWN, character.gameObject, respawnPointTown.position);
            respawnMonologueTown.gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
