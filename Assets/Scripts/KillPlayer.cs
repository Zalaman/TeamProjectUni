using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;


   private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.transform.CompareTag("Player"))
            col.collider.transform.position = spawnPoint.position;
        print("collision");
    }
}