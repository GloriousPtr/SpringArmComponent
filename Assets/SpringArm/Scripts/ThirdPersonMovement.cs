using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    public Transform cam;
    public float speed = 6;
    public float turnSmoothTime = 0.1f;
    public Animator animator;

    private float turnSmoothVelocity;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        
        if (animator)
            animator.SetBool("Walking", direction.magnitude > 0.01f);
        
        if(direction.magnitude < 0.1)
            return;

        float targetAngle = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg) + cam.localRotation.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.localRotation.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.localRotation = Quaternion.Euler(0, angle, 0);

        Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        controller.Move(moveDir.normalized * (speed * Time.deltaTime));
    }
}
