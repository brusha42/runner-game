using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> roadPrefabs;
    [SerializeField] private float bonusChance = 0.6f;

    private void Start()
    {
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("Prefabs");
        foreach (GameObject prefab in loadedPrefabs)
        {
            if (prefab.CompareTag("Road"))
            {
                roadPrefabs.Add(prefab);
            }
        }
    }

    private void SpawnRoad()
    {
        int index = Random.Range(0, roadPrefabs.Count);
        GameObject roadToSpawn = roadPrefabs[index];
        Vector3 position = new Vector3(0, 0, transform.parent.position.z + 120);
        Transform bonuses = roadToSpawn.transform.GetChild(0);
        for (int i = 0; i < bonuses.childCount; i++)
        {
            bonuses.transform.GetChild(i).gameObject.SetActive(true);
            if (Random.value > bonusChance)
            {
                bonuses.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        Instantiate(roadToSpawn, position, Quaternion.identity);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpawnRoad();
            UIManager.Instance.UpdateScore();
            Destroy(transform.parent.gameObject);
        }
    }
}
