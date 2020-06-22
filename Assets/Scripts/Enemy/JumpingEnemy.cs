using System.Collections;
using UnityEngine;

public class JumpingEnemy : Enemy
{
    public float MovementTime = 5.0f;
    public float JumpForce = 1750f;

    [Tooltip("Jump frequency in Hz")]
    public float JumpFrequency = 0.5f;
    private Rigidbody RigidBody;

    protected override void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
        base.Awake();
    }

    protected override IEnumerator MoveTo(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        while (transform.position != targetPosition)
        {
            this.Jump();
            yield return new WaitForSeconds(1/JumpFrequency);
        }
    }

    private void Jump()
    {
        Vector3 forceToAdd = (transform.forward + 2 * Vector3.up).normalized * JumpForce;
        RigidBody.AddForce(forceToAdd, ForceMode.Force);
    }
}