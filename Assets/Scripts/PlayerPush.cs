using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float distance = 1f;
    public LayerMask boxMask;


    GameObject box;
    GameObject box2;
    // Start is called before the first frame update
    void Start()
    {
        boxMask = LayerMask.GetMask("PuzzlePiece");
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance, boxMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, distance, boxMask);
        if (hitRight.collider != null && hitRight.collider.gameObject.tag == "Pushable" && Input.GetKeyDown(key: KeyCode.E))
        {
            box = hitRight.collider.gameObject;
            

            box.GetComponent<FixedJoint2D>().enabled = true;
            box.GetComponent<boxpull>().beingPushed = true;
            box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();


        }
        else if(hitLeft.collider != null && hitLeft.collider.gameObject.tag == "Pushable" && Input.GetKeyDown(key: KeyCode.E))
        {
            box2 = hitLeft.collider.gameObject;
            box2.GetComponent<FixedJoint2D>().enabled = true;
            box2.GetComponent<boxpull>().beingPushed = true;
            box2.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            box.GetComponent<FixedJoint2D>().enabled = false;
            box.GetComponent<boxpull>().beingPushed = false;

            box2.GetComponent<FixedJoint2D>().enabled = false;
            box2.GetComponent<boxpull>().beingPushed = false;

        }
        
    }

    public void OnDrawGixmosSelected()
    {



        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, (Vector3)transform.position + Vector3.right * transform.localScale.x * distance);
        Gizmos.DrawLine(transform.position, (Vector3)transform.position + Vector3.left * transform.localScale.x * distance);
    }
}
    


