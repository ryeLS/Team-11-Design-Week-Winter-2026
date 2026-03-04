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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();



    }


    void Update()
    {
        //blockdestroy();
    }
    public void blockdestroy()
    {
        Destroy(gameObject);
    }
    //small blocks code



}
