using UnityEngine;

public class Blocks : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        
    }
    public void blockdestroy()
    {
        Destroy(gameObject);
    }
}
