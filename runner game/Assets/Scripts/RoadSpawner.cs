using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;

    private void SpawnRoad()
    {
        Vector3 position = new Vector3(0, 0, transform.parent.position.z + 120);
        Instantiate(roadPrefab, position, Quaternion.identity);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpawnRoad();
            Destroy(transform.parent.gameObject);
        }
    }
}
