using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Camera : MonoBehaviour
{
    public Controller2D target;
    public Vector2 focusAreaSize;

    FocusArea focusArea;

    void Start()
    {
        focusArea = new FocusArea(target.collider.bounds, focusAreaSize);
    }
    void LateUpdate()
    {
        focusArea.Update(target.colliders.bounds);
    }

    struct FocusArea
    {
        public Vector2 middle;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            middle = new Vector2((left + right) / 2, (top + bottom) / 2);

        }
        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftX = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftX = targetBounds.max.y - top;
            }
            bottom += shiftY;
            top += shiftY;

            middle = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }

}*/
public class Camera : MonoBehaviour {

    public GameObject followTarget;
    private Vector3 targetPos;
    public float moveSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
		
	}
}
