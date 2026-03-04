using UnityEngine;

public class mediumBlocks : MonoBehaviour
{
    GameObject gameObject;
    Foot foot;
    ToddlerScript toddler;
    float x;
    float y;
    float z;
    Vector3 position;
    Rigidbody rb;
    public Rigidbody leftFootRB;
    public Rigidbody rightFootRB;
    public ForceMode forceMode = ForceMode.Impulse;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        position = new Vector3(x, y, z);
        
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == leftFootRB ||
            other.attachedRigidbody == rightFootRB)
        {
            Vector3 explosionPosition = other.attachedRigidbody.position;
            rb.AddExplosionForce(500f, explosionPosition, 20f, 20f, ForceMode.Impulse);
        }
    }

}
