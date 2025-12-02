using UnityEngine;

public class CameraScaleIgonre : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Vector3 originalLocalPosition;
    private float slideCompensation = 2f;
    
    void Start()
    {
        originalLocalPosition = transform.localPosition;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }
    
    void LateUpdate()
    {
        if (playerMovement.isSliding)
        {
            transform.localPosition = new Vector3(
                originalLocalPosition.x,
                originalLocalPosition.y * slideCompensation,
                originalLocalPosition.z
            );
        }
        else
        {
            transform.localPosition = originalLocalPosition;
        }
    }
}
