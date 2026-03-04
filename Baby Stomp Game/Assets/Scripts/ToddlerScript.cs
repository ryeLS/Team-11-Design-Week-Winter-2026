using UnityEngine;

public class ToddlerScript : MonoBehaviour
{

    [SerializeField] AnimationCurve moveSpeedMult;
    [SerializeField] AnimationCurve footHeight;
    [SerializeField] AnimationCurve footMoveMultiplier;

    [SerializeField] float stompMod;

    [SerializeField] float camLookX;
    [SerializeField] float camLookY;
    [SerializeField] float camLookZ;
    [SerializeField] float mouseSensitivity = 1;

    [SerializeField] GameObject[] feet;
    [SerializeField] GameObject[] footDestinations;
    [SerializeField] ParticleSystem[] feetSplashParticleSystems;

    [SerializeField] Vector3 modifiedMouseDir;
    [SerializeField] GameObject pivotPoint;

    [HideInInspector] public bool leftFootStomped = false;
    [HideInInspector] public bool rightFootStomped = false;

    public LayerMask objectMask;
    public LayerMask waterMask;
    public LayerMask groundMask;
    Blocks block;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        block = FindObjectOfType<Blocks>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        modifiedMouseDir = Input.mousePositionDelta;
        modifiedMouseDir.z = modifiedMouseDir.y;
        modifiedMouseDir.y = 0;
        //Note: MovePosition for rigidBody objects.
        if (!Input.GetMouseButton(0))
        {
            
            feet[0].transform.position = UpdateFootHeight(feet[0].transform.position, 0, 0);

        }
        else if(Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {

            camLookZ = 3;
            feet[0].transform.position = UpdateFootHeight(feet[0].transform.position, .1f, stompMod);
            footDestinations[0].transform.position = UpdateDestinationPos(footDestinations[0].transform.position);
            feet[0].transform.position = UpdateFootPosition(feet[0].transform.position, footDestinations[0].transform.position);

        }

        if (!Input.GetMouseButton(1))
        {

            feet[1].transform.position = UpdateFootHeight(feet[1].transform.position, 0, 0);

        }
        else if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        {

            camLookZ = -3;
            feet[1].transform.position = UpdateFootHeight(feet[1].transform.position, .1f, stompMod);
            footDestinations[1].transform.position = UpdateDestinationPos(footDestinations[1].transform.position);
            feet[1].transform.position = UpdateFootPosition(feet[1].transform.position, footDestinations[1].transform.position);

        }      

        if (Input.GetMouseButton(2))
        {

            float camInputX = Input.mousePositionDelta.x * mouseSensitivity;
            float camInputY = Input.mousePositionDelta.y * mouseSensitivity;

            camLookY -= camInputY;
            camLookX -= camInputX;

            camLookY = Mathf.Clamp(camLookY, -60, 60);                     

        }

        pivotPoint.transform.localEulerAngles = new Vector3(0, -camLookX, camLookZ);
        Camera.main.transform.localEulerAngles = Vector3.right * camLookY;

        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {

            camLookZ = 0;

        }

        Debug.DrawLine(feet[0].transform.position, feet[0].transform.position + Vector3.down);
        Debug.DrawLine(feet[1].transform.position, feet[1].transform.position + Vector3.down);
        UpdateCameraPosition();

    }

    private void Update()
    {

        if (Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1))
        {

            //The left foot has hit the ground without stomping,
            //relevant functionality goes here.
            Debug.Log("Left Step");
            footDestinations[0].transform.position = feet[0].transform.position;

            Debug.Log("Left Step Hit Something: " + Physics.Raycast(feet[0].transform.position, Vector3.down, 100f, groundMask));

            if (Physics.Raycast(feet[0].transform.position, Vector3.down, 100f, waterMask))
            {

                Debug.Log("Splish.");
                feetSplashParticleSystems[0].Play();

            }

        }

        if (Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0))
        {

            //The right foot has hit the ground without stomping,
            //relevant functionality goes here.
            Debug.Log("Right Step");
            footDestinations[1].transform.position = feet[1].transform.position;

            Debug.Log("Right Step Hit Something: " + Physics.Raycast(feet[1].transform.position, Vector3.down, 100f, groundMask));

            if (Physics.Raycast(feet[1].transform.position, Vector3.down, 100f, waterMask))
            {

                Debug.Log("Splash.");
                feetSplashParticleSystems[1].Play();

            }

        }

        if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(1))
        {

            //The left foot has stomped,
            //relevant functionality goes here.

            Collider[] hitColliders = Physics.OverlapSphere(feet[0].transform.position, 2, objectMask);
            Debug.Log(hitColliders.Length);

            foreach (Collider collider in hitColliders)
            {                

                collider.attachedRigidbody.AddExplosionForce(5f, feet[0].transform.position, 20f, 3f, ForceMode.Impulse);
            }

            leftFootStomped = true;
            Debug.Log("Left Stomp");

        }

        if (Input.GetMouseButtonUp(1) && Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(0))
        {

            //The right foot has stomped,
            //relevant functionality goes here.

            Collider[] hitColliders = Physics.OverlapSphere(feet[1].transform.position, 2, objectMask);
            Debug.Log(hitColliders.Length);

            foreach (Collider collider in hitColliders)
            {

                collider.attachedRigidbody.AddExplosionForce(5f, feet[1].transform.position, 20f, 3f, ForceMode.Impulse);
            }
            
            rightFootStomped = true;
            Debug.Log("Right Stomp");

        }

    }

    void UpdateCameraPosition()
    {

        Vector3 feetMiddlePos = Vector3.Lerp(feet[0].transform.position, feet[1].transform.position, 0.5f);
        Vector3 camPos = Camera.main.transform.position;

        Vector3 camMoveDir = (feetMiddlePos - camPos).normalized;

        Vector3 newCamPos = camPos + (camMoveDir * moveSpeedMult.Evaluate(Vector3.Distance(camPos, feetMiddlePos)));
        pivotPoint.transform.position = newCamPos;

        pivotPoint.transform.position = new Vector3(newCamPos.x, 1, newCamPos.z);

    }

    Vector3 UpdateFootHeight(Vector3 footPos, float setHeight, float heightMod)
    {

        footPos.y = setHeight + heightMod;

        return footPos;

    }

    Vector3 UpdateDestinationPos(Vector3 destinationPos)
    {

        Vector3 newDestPos = destinationPos + (modifiedMouseDir/100);

        return newDestPos;

    }

    Vector3 UpdateFootPosition(Vector3 footPos, Vector3 destinationPos)
    {

        float currentFootY = footPos.y;
        //Debug.Log($"Distance to target foot from Cam: {Vector3.Distance(Camera.main.transform.position, footPos)}");

        Vector3 moveDir = (destinationPos - footPos).normalized * footMoveMultiplier.Evaluate(Vector3.Distance(Camera.main.transform.position, footPos));

        Vector3 newFootPos = footPos + (moveDir * moveSpeedMult.Evaluate(Vector3.Distance(footPos, destinationPos)));

        newFootPos.y = currentFootY;

        return newFootPos;

    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Bonk.");

    }

}
