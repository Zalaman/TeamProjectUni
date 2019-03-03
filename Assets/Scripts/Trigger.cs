using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    public Transform blockade;
    public Vector3 closedPosition = new Vector3(39.02f, 2.2f, 0);
    public Vector3 openPosition = new Vector3(39.02f, 0, 0);

    public float openSpeed = 1;

    private bool open = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (open == true)
        {
            blockade.position = Vector3.Lerp(blockade.position, openPosition, Time.deltaTime * openSpeed);

        }
        else
        {
            CloseDoor();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered");
        if(other.tag == "Pushable")
        {
            OpenDoor();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {

            
        
    }

    public void CloseDoor()
    {
        open = false;
    }
    public void OpenDoor()
    {
        open = true;
    }
}
