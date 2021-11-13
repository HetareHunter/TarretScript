using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCamera : MonoBehaviour
{
    GameObject[] cameras;
    [SerializeField] GameObject notDestroyCamera;

    // Start is called before the first frame update
    void Start()
    {
        cameras = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in cameras)
        {
            if (item != notDestroyCamera)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
