using UnityEngine;

public class Blocks : MonoBehaviour
{
    Foot foot;
    ToddlerScript toddler;
    float x;
    float y;
    float z;
    Vector3 position;
    Rigidbody rb;
    //public Rigidbody leftFootRB;
    //public Rigidbody rightFootRB;
    public ForceMode forceMode = ForceMode.Impulse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        position = new Vector3(x, y, z);
        toddler = FindObjectOfType<ToddlerScript>();

    }

    // Update is called once per frame
    void Update()
    {
        //blockdestroy();
    }
    public void blockdestroy()
    {
        Destroy(gameObject);
    }
    //small blocks code
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == 7 && toddler.leftFootStomped)
        {
            blockdestroy();
        }
        else if (collision.gameObject.layer == 7 && toddler.rightFootStomped)
        {
            blockdestroy();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody footRB = other.attachedRigidbody;
        Debug.Log(toddler.leftFootStomped);
        
        if (toddler.leftFootStomped)
        {
            Vector3 explosionPosition = footRB.position;
            rb.AddExplosionForce(500f, explosionPosition, 20f, 20f, ForceMode.Impulse);
            Debug.Log("Left foot stomped");
            toddler.leftFootStomped = false;
            Debug.Log(toddler.leftFootStomped);
        }
        if (toddler.rightFootStomped)
        {
            Vector3 explosionPosition = footRB.position;
            rb.AddExplosionForce(500f, explosionPosition, 20f, 20f, ForceMode.Impulse);
            Debug.Log("Right foot stomped");
            toddler.rightFootStomped = false;
            Debug.Log(toddler.rightFootStomped);
        }
    }

}
