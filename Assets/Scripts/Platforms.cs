using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    public GameObject platform;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pushable")
        // {
        // if (Input.GetKeyDown(key: KeyCode.E) == true)
        {

            platform.GetComponent<PlatformController>().enabled = true;
        }
        // }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
