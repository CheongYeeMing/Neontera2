using UnityEngine;

public class Portal : MonoBehaviour
{
    private const float PORTAL_NAME_TAG_POSITION_Y_OFFSET = -1.2f;
    private const string ACTIVATE_PORTAL = "Activate";
    private const string DEACTIVATE_PORTAL = "Deactivate";

    [SerializeField] GameObject portalNameTag;
    [SerializeField] private Portal destinationPortal;
    [SerializeField] private string location;
    [SerializeField] private bool isActivated;

    private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        portalNameTag.transform.position = new Vector2(transform.position.x, transform.position.y + PORTAL_NAME_TAG_POSITION_Y_OFFSET);
        if (isActivated)
        {
            animator.SetTrigger(ACTIVATE_PORTAL);
        }
        else
        {
            animator.SetTrigger(DEACTIVATE_PORTAL);

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

    public bool IsActivated()
    {
        return isActivated;
    }
}
