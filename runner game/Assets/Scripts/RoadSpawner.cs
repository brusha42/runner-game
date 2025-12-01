using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;

    private void SpawnRoad()
    {
        Vector3 position = new Vector3(0, 0, 90);
        Instantiate(roadPrefab, position, Quaternion.identity);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpawnRoad();
        }
    }
}
