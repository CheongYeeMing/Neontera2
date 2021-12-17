using UnityEngine;

public class Portal : MonoBehaviour
{
    private const float PORTAL_NAME_TAG_POSITION_Y_OFFSET = -1.2f;

    [SerializeField] GameObject PortalNameTag;
    [SerializeField] public Portal destinationPortal;
    [SerializeField] public string location;
    [SerializeField] public bool isActivated;

    private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        PortalNameTag.transform.position = new Vector2(transform.position.x, transform.position.y + PORTAL_NAME_TAG_POSITION_Y_OFFSET);
        if (isActivated)
        {
            animator.SetTrigger("Activate");
        }
        else
        {
            animator.SetTrigger("Deactivate");

        }
    }

    public void Teleport(GameObject character)
    {
        if (isActivated && !character.GetComponent<ParallaxBackgroundManager>().isTeleporting)
        {
            character.GetComponent<ParallaxBackgroundManager>().isTeleporting = true;
            FindObjectOfType<AudioManager>().StopEffect("Portal");
            FindObjectOfType<AudioManager>().PlayEffect("Portal");
            StopAllCoroutines();
            StartCoroutine(character.GetComponent<ParallaxBackgroundManager>().ChangeBackground(destinationPortal.GetPortalLocation(), character, destinationPortal));
        }
    }

    public string GetPortalLocation()
    {
        return location;
    }
}
