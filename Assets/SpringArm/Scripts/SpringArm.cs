using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class SpringArm : MonoBehaviour
{
    [Space]
    [Header("Follow Settings \n--------------------")]
    [Space]
    public Transform target;
    public float movementSmoothTime = 0.05f;
    public float targetArmLength = 3f;
    public Vector3 socketOffset;
    public Vector3 targetOffset;

    [Space]
    [Header("Collision Settings \n-----------------------")]
    [Space]
    public bool doCollisionTest = true;
    [Range(2, 20)]
    public int collisionTestResolution = 4;
    public float collisionProbeSize = 0.3f;
    public float collisionSmoothTime = 0.05f;
    public LayerMask collisionLayerMask = ~0;

    [Space]
    [Header("Rotation Settings \n-----------------------")]
    [Space]
    public bool useControlRotation = true;
    public float mouseSensitivity = 500f;

    [Space]
    [Header("Debugging \n--------------")]
    [Space]
    public bool visualDebugging = true;
    public Color springArmColor = new Color(0.75f, 0.2f, 0.2f, 0.75f);
    [Range(1f, 10f)]
    public float springArmLineWidth = 6f;
    public bool showRaycasts;
    public bool showCollisionProbe;

    #region Private Variables
    
    private Vector3 endPoint;
    private Vector3 socketPosition;
    private RaycastHit[] hits;
    private Vector3[] raycastPositions;

    private readonly Color collisionProbeColor = new Color(0.2f, 0.75f, 0.2f, 0.15f);

    // refs for SmoothDamping
    private Vector3 moveVelocity;
    private Vector3 collisionTestVelocity;

    // For mouse inputs
    private float pitch;
    private float yaw;
    
    #endregion

    private void Start()
    {
        raycastPositions = new Vector3[collisionTestResolution];
        hits = new RaycastHit[collisionTestResolution];
    }

    private void OnValidate()
    {
        raycastPositions = new Vector3[collisionTestResolution];
        hits = new RaycastHit[collisionTestResolution];
    }

    private void Update()
    {
        // if target is null, return from here: NullReference check
        if(!target)
            return;

        // Collision check
        if (doCollisionTest)
            CheckCollisions();
        
        // set the socketPosition
        SetSocketTransform();

        // handle mouse inputs for rotations
        if (useControlRotation && Application.isPlaying)
            Rotate();
        
        // follow the target applying targetOffset
        transform.position = Vector3.SmoothDamp(transform.position, target.position + targetOffset, ref moveVelocity, movementSmoothTime);
    }

    private void OnDrawGizmosSelected()
    {
        if(!visualDebugging)
            return;
     
        // Using Handles as they have MSAA, looks better than Gizmos
        
        // Draw main LineTrace or LineTraces of RaycastPositions, useful for debugging
        Handles.color = springArmColor;
        if(showRaycasts)
            foreach (Vector3 raycastPosition in raycastPositions)
                Handles.DrawAAPolyLine(springArmLineWidth, 2, transform.position, raycastPosition);
        else
            Handles.DrawAAPolyLine(springArmLineWidth, 2, transform.position, endPoint);
        
        // Draw collisionProbe, useful for debugging
        Handles.color = collisionProbeColor;
        if(showCollisionProbe)
            Handles.SphereHandleCap(0, socketPosition, Quaternion.identity, 2 * collisionProbeSize, EventType.Repaint);
    }

    /// <summary>
    /// Checks for collisions and fill the raycastPositions and hits array
    /// </summary>
    private void CheckCollisions()
    {
        // Cache transform as it is used quite often
        Transform trans = transform;
        
        // iterate through raycastPositions and hits and set the corresponding data
        for (int i = 0, angle = 0; i < collisionTestResolution; i++, angle += 360 / collisionTestResolution)
        {
            // Calculate the local position of a point w.r.t angle
            Vector3 raycastLocalEndPoint = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * collisionProbeSize;
            // Convert it to world space by offsetting it by origin: endPoint, and push in the array
            raycastPositions[i] = endPoint + trans.rotation * raycastLocalEndPoint;
            // Sets the hit struct if collision is detected between this gameobject's position and calculated raycastPosition
            Physics.Linecast(trans.position, raycastPositions[i], out hits[i], collisionLayerMask);
        }
    }

    /// <summary>
    /// Sets the translation of children according to filled raycastPositions and hits array data
    /// </summary>
    private void SetSocketTransform()
    {
        // Cache transform as it is used quite often
        Transform trans = transform;

        // offset a point in z direction of targetArmLength by socket offset and translating it into world space.
        Vector3 targetArmOffset = socketOffset - new Vector3(0, 0, targetArmLength);
        endPoint = trans.position + trans.rotation * targetArmOffset;
        
        // if collisionTest is enabled
        if (doCollisionTest)
        {
            // finds the minDistance
            float minDistance = targetArmLength;
            foreach (RaycastHit hit in hits)
            {
                if (!hit.collider)
                    continue;
                
                float distance = Vector3.Distance(hit.point, trans.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                }
            }
            
            // calculate the direction of children movement
            Vector3 dir = (endPoint - trans.position).normalized;
            // get vector for movement
            Vector3 armOffset = dir * (targetArmLength - minDistance);
            // offset it by endPoint and set the socketPositionValue
            socketPosition = endPoint - armOffset;
        }
        // if collision is disabled
        else
        {
            // set socketPosition value as endPoint
            socketPosition = endPoint;
        }

        // iterate through all children and set their position as socketPosition, using SmoothDamp to smoothly translate the vectors.
        foreach (Transform child in trans)
            child.position = Vector3.SmoothDamp(child.position, socketPosition, ref collisionTestVelocity, collisionSmoothTime);
    }
    
    /// <summary>
    /// Handle rotations
    /// </summary>
    private void Rotate()
    {
        // Increment yaw by Mouse X input
        yaw += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        // Decrement pitch by Mouse Y input
        pitch -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
        // Clamp pitch so that we can't invert the the gameobject by mistake
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        
        // Set the rotation to new rotation
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
