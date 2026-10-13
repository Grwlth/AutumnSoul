using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public GameObject[] trapPrefabs;

    private void Start()
    {
        if (trapPrefabs == null ||
            trapPrefabs.Length == 0)
        {
            return;
        }

        int randomIndex =
            Random.Range(0, trapPrefabs.Length);

        Instantiate(
            trapPrefabs[randomIndex],
            transform.position,
            transform.rotation
        );
    }
}