using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject platform;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
       // {
           // if (Input.GetKeyDown(key: KeyCode.E) == true)
            {

                platform.GetComponent<PlatformController>().enabled = true;
            }
       // }
        
    }   

}
