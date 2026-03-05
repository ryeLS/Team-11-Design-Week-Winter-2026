using UnityEngine;

public class Blocks : StompableScript
{
    Rigidbody rb;
    public ParticleSystem Puff;

    void Start()
    {
        Puff = ParticleSystem.FindObjectOfType<ParticleSystem>();
        rb = GetComponent<Rigidbody>();

    }
    public override void OnStomp()
    {
        throw new System.NotImplementedException();
    }


    //small blocks code

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.layer == 7 && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("collided");
            Instantiate(Puff, rb.position, Quaternion.identity);
            Destroy(gameObject);

        }

    }

}
