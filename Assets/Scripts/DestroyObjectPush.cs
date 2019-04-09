using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectPush : MonoBehaviour
{
    public GameObject gameObject;
// Start is called before the first frame update
void Start()
{

}

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.tag == "Pushable")
    {
        Destroy(gameObject);
    }
}
// Update is called once per frame
void Update()
{

}
}
