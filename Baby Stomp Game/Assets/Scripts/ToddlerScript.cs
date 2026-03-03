using UnityEngine;

public class ToddlerScript : MonoBehaviour
{

    [SerializeField] AnimationCurve moveSpeedMult;
    [SerializeField] AnimationCurve footHeight;

    [SerializeField] float stompMod;

    [SerializeField] float camLookX;
    [SerializeField] float camLookY;
    [SerializeField] float mouseSensitivity = 1;

    [SerializeField] GameObject[] feet;
    [SerializeField] GameObject[] footDestinations;

    [SerializeField] Vector3 modifiedMouseDir;
    [SerializeField] GameObject pivotPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

            feet[0].transform.position = UpdateFootHeight(feet[0].transform.position, .5f, stompMod);
            footDestinations[0].transform.position = UpdateDestinationPos(footDestinations[0].transform.position);
            feet[0].transform.position = UpdateFootPosition(feet[0].transform.position, footDestinations[0].transform.position);

        }

        if (!Input.GetMouseButton(1))
        {

            feet[1].transform.position = UpdateFootHeight(feet[1].transform.position, 0, 0);

        }
        else if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        {

            feet[1].transform.position = UpdateFootHeight(feet[1].transform.position, .5f, stompMod);
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

            Camera.main.transform.localEulerAngles = Vector3.right * camLookY;

            pivotPoint.transform.localEulerAngles = Vector3.up * -camLookX;

        }

        if(!Input.GetMouseButton(0) & !Input.GetMouseButton(1))
        {

            UpdateCameraPosition();

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

        Vector3 moveDir = (destinationPos - footPos).normalized;

        Vector3 newFootPos = footPos + (moveDir * moveSpeedMult.Evaluate(Vector3.Distance(footPos, destinationPos)))/10;

        newFootPos.y = currentFootY;

        return newFootPos;

    }

}
