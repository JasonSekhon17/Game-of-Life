using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBase))]
public class AIMovement : MonoBehaviour
{
    public enum MovementState {
        Wander,
        Arrive,
        Pursue,
        Seek,
        Flee
    }

    public MovementState movementState = MovementState.Wander;

    [Header("Wander")]

    public float wanderRadius = 1.2f;

    public float wanderDistance = 2f;
    
    public float wanderJitter = 40f;

    [Header("Pursue")]

    public float maxPursuePrediction = 1f;

    public MovementAIRigidbody pursueTarget;

    [Header("Seek")]

    public Transform seekTarget;

    [Header("Flee")]

    public Transform fleeTarget;

    public float fleeDist = 3.5f;

    public bool decelerateOnStop = true;

    public float maxAcceleration = 10f;

    public float timeToTarget = .1f;

    CollisionAvoidance colAvoid;

    NearSensor colAvoidSensor;

    Vector3 wanderTarget;

    SteeringBase steeringBase;

    MovementAIRigidbody rb;

    void Awake()
    {
        steeringBase = GetComponent<SteeringBase>();
        rb = GetComponent<MovementAIRigidbody>();
    }

    void Start()
    {
        float theta = Random.value * 2 * Mathf.PI;

        wanderTarget = new Vector3(wanderRadius * Mathf.Cos(theta), 0f, wanderRadius * Mathf.Sin(theta));
        
        colAvoid = GetComponent<CollisionAvoidance>();

        colAvoidSensor = transform.Find("ColAvoidSensor").GetComponent<NearSensor>();
    }

    void FixedUpdate()
    {
        Vector3 accel = colAvoid.GetSteering(colAvoidSensor.targets);
        if (accel.magnitude < .005f) {
            if (movementState == MovementState.Wander) {
                accel = Wander();

            } else if (movementState == MovementState.Pursue) {
                accel = Pursue(pursueTarget);

            } else if (movementState == MovementState.Seek) {
                accel = steeringBase.Seek(seekTarget.position);

            } else if (movementState == MovementState.Flee) {
                accel = Flee(fleeTarget.position);

            } else if (movementState == MovementState.Arrive) {

            }
        }

        steeringBase.Steer(accel);
        steeringBase.LookWhereYoureGoing();
    }

    //WANDER
    public Vector3 Wander()
    {
        float jitter = wanderJitter * Time.deltaTime;

        wanderTarget += new Vector3(Random.Range(-1f, 1f) * jitter, 0f, Random.Range(-1f, 1f) * jitter);
        
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetPosition = transform.position + transform.right * wanderDistance + wanderTarget;

        Debug.DrawLine(transform.position, targetPosition);

        return steeringBase.Seek(targetPosition);
    }

    //PURSUE
    public Vector3 Pursue(MovementAIRigidbody target)
    {
        Vector3 displacement = target.Position - transform.position;
        float distance = displacement.magnitude;

        float speed = rb.Velocity.magnitude;

        float prediction;
        if (speed <= distance / maxPursuePrediction) {
            prediction = maxPursuePrediction;
        } else {
            prediction = distance / speed;
        }

        Vector3 explicitTarget = target.Position + target.Velocity * prediction;

        Debug.DrawLine(transform.position, explicitTarget);

        return steeringBase.Seek(explicitTarget);
    }

    //Flee
    public Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 acceleration = transform.position - targetPosition;

        if (acceleration.magnitude > fleeDist) {
            if (decelerateOnStop && rb.Velocity.magnitude > .001f) {
                acceleration = -rb.Velocity / timeToTarget;

                if (acceleration.magnitude > maxAcceleration) {
                    acceleration.Normalize();
                    acceleration *= maxAcceleration;
                }

                return acceleration;
            } else {
                rb.Velocity = Vector3.zero;
                return Vector3.zero;
            }
        }

        acceleration.Normalize();
        acceleration *= maxAcceleration;

        return acceleration;
    }
}
