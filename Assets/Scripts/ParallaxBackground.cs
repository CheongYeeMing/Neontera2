using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] protected Vector2 parallaxEffectMultiplier;
    [SerializeField] protected bool isInfiniteHorizontal;
    [SerializeField] protected bool isInfiniteVertical;

    protected Vector3 lastCameraPosition;
    protected Transform cameraTransform;
    protected float textureUnitSizeX;
    protected float textureUnitSizeY;

    // Start is called before the first frame update
    public virtual void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        transform.position = new Vector2(lastCameraPosition.x, lastCameraPosition.y);
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y /* parallaxEffectMultiplier.y*/);
        lastCameraPosition = cameraTransform.position;

        if (isInfiniteHorizontal)
        {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetPositionX =  (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, cameraTransform.position.y);
            }
        }
        if (isInfiniteVertical)
        {
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, offsetPositionY + cameraTransform.position.y);
            }
        } 
    }
}
