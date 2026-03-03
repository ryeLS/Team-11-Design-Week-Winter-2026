using UnityEngine;

public class Blocks : MonoBehaviour
{
    public GameObject gameObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

        Debug.Log("ow");
        
        if (collision.gameObject.layer == 7)
        {
            blockdestroy();
        }

    }

}
