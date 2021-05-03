using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private Animator animator = null;
    [SerializeField] private Rigidbody rigidBody = null;
    private float currentVelocity = 0;
    private float currentH = 0;
    private readonly float interpolation = 10;
    private readonly float walkScale = 0.33f;
    private bool wasGrounded;
    private Vector3 currentDirection = Vector3.zero;
    private bool isGrounded;
    private List<Collider> collisions = new List<Collider>();
    [HideInInspector] public bool walkSoundPlaying = false;

    private void Awake()
    {
        if (!animator) { gameObject.GetComponent<Animator>(); }
        if (!rigidBody) { gameObject.GetComponent<Animator>(); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!collisions.Contains(collision.collider))
                {
                    collisions.Add(collision.collider);
                }
                isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            isGrounded = true;
            if (!collisions.Contains(collision.collider))
            {
                collisions.Add(collision.collider);
            }
        }
        else
        {
            if (collisions.Contains(collision.collider))
            {
                collisions.Remove(collision.collider);
            }
            if (collisions.Count == 0) { isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collisions.Contains(collision.collider))
        {
            collisions.Remove(collision.collider);
        }
        if (collisions.Count == 0) { isGrounded = false; }
    }

    private void FixedUpdate()
    {
        animator.SetBool("Grounded", isGrounded);

        DirectUpdate();

        wasGrounded = isGrounded;
    }

    private void DirectUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            v *= walkScale;
            h *= walkScale;
            GetComponent<AudioSource>().Pause();
            walkSoundPlaying = false;
        }

        currentVelocity = Mathf.Lerp(currentVelocity, v, Time.deltaTime * interpolation);
        currentH = Mathf.Lerp(currentH, h, Time.deltaTime * interpolation);

        Vector3 direction = camera.forward * currentVelocity + camera.right * currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            currentDirection = Vector3.Slerp(currentDirection, direction, Time.deltaTime * interpolation);

            transform.rotation = Quaternion.LookRotation(currentDirection);
            transform.position += currentDirection * moveSpeed * Time.deltaTime;

            animator.SetFloat("MoveSpeed", direction.magnitude);
        }

        if (direction.magnitude < 0.2f)
        {
            GetComponent<AudioSource>().Pause();
            walkSoundPlaying = false;
        }
        else if (walkSoundPlaying == false && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<AudioSource>().Play();
            walkSoundPlaying = true;
        }
    }
}
