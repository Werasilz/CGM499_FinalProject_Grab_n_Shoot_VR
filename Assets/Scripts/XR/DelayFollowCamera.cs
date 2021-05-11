using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayFollowCamera : MonoBehaviour
{
    private struct PointInSpace
    {
        public Vector3 Rotation;
        public float Time;
    }

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offsetBetweenTarget;
    [HideInInspector] public float delay = 0.2f;
    [HideInInspector] public float speed = 100;

    ///<summary>
    /// Contains the positions of the target for the last X seconds
    ///</summary>
    private Queue<PointInSpace> pointsInSpace = new Queue<PointInSpace>();

    void LateUpdate()
    {
        // Add the current target position to the list of positions
        pointsInSpace.Enqueue(new PointInSpace() { Rotation = target.eulerAngles, Time = Time.time });

        // Move the camera to the position of the target X seconds ago 
        while (pointsInSpace.Count > 0 && pointsInSpace.Peek().Time <= Time.time - delay + Mathf.Epsilon)
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, pointsInSpace.Dequeue().Rotation + offsetBetweenTarget, Time.deltaTime * speed);
        }

        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
    }
}
