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

        
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }
   

}
