using UnityEngine;

public class CaveParallaxBackground : ParallaxBackground
{
    // Update is called once per frame
    public override void FixedUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y);
        lastCameraPosition = cameraTransform.position;

        if (isInfiniteHorizontal)
        {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, cameraTransform.position.y);
            }
        }
    }
}
