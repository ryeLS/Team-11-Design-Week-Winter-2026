using UnityEngine;

public class Blocks : MonoBehaviour
{

    //public Rigidbody leftFootRB;
    //public Rigidbody rightFootRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    void Update()
    {
        //blockdestroy();
    }

    //small blocks code

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.layer == 7 && Input.GetKey(KeyCode.LeftShift))
        {

            Destroy(gameObject);

        }

    }

}
