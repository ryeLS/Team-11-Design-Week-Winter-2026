using UnityEngine;
using UnityEngine.SceneManagement;

public class ToddlerScript : MonoBehaviour
{

    [SerializeField] AnimationCurve moveSpeedMult;
    [SerializeField] AnimationCurve footHeight;
    [SerializeField] AnimationCurve footMoveMultiplier;

    [SerializeField] float stompMod;

    [SerializeField] float camLookX;
    [SerializeField] float camLookY;
    [SerializeField] float camLookZ;
    [SerializeField] float mouseSensitivity = 3;

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
    public AudioSource audioSource;
    public AudioClip stompSound;
    public AudioClip heavyStompSound;
    public AudioClip breakSound;
    public AudioClip puddleSound;
    Blocks block;
    CameraShake cameraShake;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        block = FindObjectOfType<Blocks>();
        cameraShake = FindObjectOfType<CameraShake>();
        Cursor.lockState = CursorLockMode.Locked;

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
            feet[0].transform.localEulerAngles = UpdateFootRotation();

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
            feet[1].transform.localEulerAngles = UpdateFootRotation();

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
            UpdateCameraPosition();

        }

        Debug.DrawLine(feet[0].transform.position, feet[0].transform.position + Vector3.down);
        Debug.DrawLine(feet[1].transform.position, feet[1].transform.position + Vector3.down);
        

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {

            //Go to main menu.
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Main Menu");

        }

        if (Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1))
        {

            //The left foot has hit the ground without stomping,
            //relevant functionality goes here.
            Debug.Log("Left Step");
            footDestinations[0].transform.position = feet[0].transform.position;

            audioSource.PlayOneShot(stompSound, 0.2f);
            Debug.Log("Left Step Hit Something: " + Physics.Raycast(feet[0].transform.position, Vector3.down, 100f, groundMask));

            if (Physics.Raycast(feet[0].transform.position, Vector3.down, 100f, waterMask))
            {

                Debug.Log("Splish.");
                feetSplashParticleSystems[0].Play();
                audioSource.PlayOneShot(puddleSound, 0.3f);

            }

        }

        if (Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0))
        {

            //The right foot has hit the ground without stomping,
            //relevant functionality goes here.
            Debug.Log("Right Step");
            footDestinations[1].transform.position = feet[1].transform.position;

            audioSource.PlayOneShot(stompSound, 0.2f);
            Debug.Log("Right Step Hit Something: " + Physics.Raycast(feet[1].transform.position, Vector3.down, 100f, groundMask));

            if (Physics.Raycast(feet[1].transform.position, Vector3.down, 100f, waterMask))
            {

                Debug.Log("Splash.");
                feetSplashParticleSystems[1].Play();
                audioSource.PlayOneShot(puddleSound, 0.3f);
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

            cameraShake.ShakeCamera(0.2f, 0.05f, true, true);
            leftFootStomped = true;
            Debug.Log("Left Stomp");
            audioSource.PlayOneShot(heavyStompSound, 0.3f);

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
            cameraShake.ShakeCamera(0.2f, 0.05f, true, true);
            rightFootStomped = true;
            Debug.Log("Right Stomp");
            audioSource.PlayOneShot(heavyStompSound, 0.3f);

        }

    }

    void UpdateCameraPosition()
    {

        Vector3 feetMiddlePos = Vector3.Lerp(feet[0].transform.position, feet[1].transform.position, 0.5f);
        Vector3 camPos = Camera.main.transform.position;

        Vector3 camMoveDir = (feetMiddlePos - camPos).normalized;

        Vector3 newCamPos = camPos + (camMoveDir * moveSpeedMult.Evaluate(Vector3.Distance(camPos, feetMiddlePos)));
        pivotPoint.transform.position = newCamPos;

        pivotPoint.transform.position = new Vector3(newCamPos.x, 1f, newCamPos.z);

    }

    Vector3 UpdateFootHeight(Vector3 footPos, float setHeight, float heightMod)
    {

        footPos.y = setHeight + heightMod;

        return footPos;

    }

    Vector3 UpdateDestinationPos(Vector3 destinationPos)
    {

        Vector3 alphaVect = Camera.main.transform.position + Camera.main.transform.forward * modifiedMouseDir.normalized.z;
        Vector3 betaVect = Camera.main.transform.position + Camera.main.transform.right * modifiedMouseDir.normalized.x;
        Vector3 alphaBetaLerp = Vector3.Lerp(alphaVect, betaVect, .5f);

        Debug.DrawLine(Camera.main.transform.position, alphaVect, Color.blue);
        Debug.DrawLine(Camera.main.transform.position, betaVect, Color.red);
        Debug.DrawLine(Camera.main.transform.position, alphaBetaLerp, Color.green);

        Vector3 newDestDir = (Camera.main.transform.position - alphaBetaLerp).normalized;
        newDestDir.y = 0;

        Vector3 newDestPos = destinationPos + (-newDestDir/15);
        return newDestPos;

    }

    Vector3 UpdateFootPosition(Vector3 footPos, Vector3 destinationPos)
    {

        float currentFootY = footPos.y;

        Vector3 moveDir = (destinationPos - footPos).normalized * footMoveMultiplier.Evaluate(Vector3.Distance(Camera.main.transform.position, footPos));

        Vector3 newFootPos = footPos + (moveDir * moveSpeedMult.Evaluate(Vector3.Distance(footPos, destinationPos)));

        newFootPos.y = currentFootY;

        return newFootPos;

    }

    Vector3 UpdateFootRotation()
    {
        Vector3 pivotRot = pivotPoint.transform.localEulerAngles;
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 10);

        Vector3 newFootRotate = new Vector3(0, pivotRot.y, 0);
        Debug.Log(newFootRotate);
        return newFootRotate;

    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Bonk.");

    }

    //"The Time for speeches is done, the first great test is here.
    //My order to you all is simple, yet heed it well, and exert yourselves to see it done.
    
    //They are coming. Kill them all."
    //- Rogal Dorn, Primarch of the Imperial Fists, 
    //Spoken in the wake of the siege of the imperial palace at the end of the Horus Heresy.

}
