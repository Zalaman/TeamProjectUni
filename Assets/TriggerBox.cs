using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    public GameObject triggerBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Pushable")
        {
            triggerBox.GetComponent<BoxCollider2D>().enabled = true;
            Debug.Log("working");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
