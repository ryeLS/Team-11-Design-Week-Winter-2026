using UnityEngine;

public class mediumBlocks : MonoBehaviour
{
    GameObject gameObject;
    Foot foot;
    float x;
    float y;
    float z;
    Vector3 position;
    Rigidbody rb;
    public Rigidbody footRB;
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
        Debug.Log("boom");
        Vector3 explosionPosition = rb.position - footRB.position;
        rb.AddExplosionForce(500, explosionPosition, 20, 1f, forceMode);


    }

}
