using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{
    public LayerMask passengerMask;
    public Vector3[] localWayPoints;
    Vector3[] globalWayPoints;

    public float speed;
    public bool cycle;
    public float timeWait;

    //clamps the range for slowdown amount
    [Range (0,2)]
    public float slowDownAmount;
    int fromWayPointIndex;
    float percentBetweenWayPoints;
    float nextMoveTime;

    List<PassengerMovement> passengerMovements;
    Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

    public override void Start()
    {
        base.Start();
        globalWayPoints = new Vector3[localWayPoints.Length];
        for (int i = 0; i < localWayPoints.Length; i++)
        {
            globalWayPoints[i] = localWayPoints[i] + transform.position;
        }
    }

    void Update()
    {
        UpdateRaycastOrigins();

        Vector3 velocity = CalcPlatformMovement();

        CalcPassMovement(velocity);
        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    // y = x*pow.a/ x*pow.a +(1-x)*pow.a this equation is for the speed slowing down when it reaches its target
    float SlowDown(float x)
    {
        float a = slowDownAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector3 CalcPlatformMovement()
    {
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }
        fromWayPointIndex %= globalWayPoints.Length;
        int toWayPointIndex = (fromWayPointIndex + 1) %globalWayPoints.Length;
        float distanceBetweenWayPoints = Vector3.Distance(globalWayPoints[fromWayPointIndex], globalWayPoints[toWayPointIndex]);
        percentBetweenWayPoints += Time.deltaTime * speed / distanceBetweenWayPoints;
        percentBetweenWayPoints = Mathf.Clamp01(percentBetweenWayPoints);
        float slowDownPercentBetweenWayPoints = SlowDown(percentBetweenWayPoints);

        Vector3 newPos = Vector3.Lerp(globalWayPoints[fromWayPointIndex], globalWayPoints[toWayPointIndex], slowDownPercentBetweenWayPoints);

        if (percentBetweenWayPoints >= 1)
        {
            percentBetweenWayPoints = 0;
            fromWayPointIndex++;
            if (!cycle)
            {
                if (fromWayPointIndex >= globalWayPoints.Length - 1)
                {
                    fromWayPointIndex = 0;
                    System.Array.Reverse(globalWayPoints);
                }
            }
            nextMoveTime = Time.time + timeWait;
        }

        return newPos - transform.position;
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in passengerMovements)
        {
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
            }
            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
               passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    void CalcPassMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovements = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //Moving vertical platform
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;


            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }

            }
        }

        //Moving horizontal platform
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }
            }

        }
        //If passenger is standing on top it'll detect
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y ;

                        passengerMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));

                    }
                }
            }
        }
    }
    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

   void OnDrawGizmos()
    {
        if (localWayPoints != null)
        {
            Gizmos.color = Color.cyan;
            float size = .3f;
            for (int i = 0; i < localWayPoints.Length; i++)
            {
                Vector3 globalWayPointPos = (Application.isPlaying)?globalWayPoints[i]: localWayPoints[i] + transform.position;
                Gizmos.DrawLine(globalWayPointPos - Vector3.up * size, globalWayPointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWayPointPos - Vector3.left * size, globalWayPointPos + Vector3.left * size);
            }
        }
    }
}