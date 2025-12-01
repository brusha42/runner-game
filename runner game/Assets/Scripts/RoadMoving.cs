using UnityEngine;

public class RoadMoving : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(-transform.forward * speed * Time.fixedDeltaTime);
    }
}
