using UnityEngine;

public class Blocks : StompableScript
{
    public override void OnStomp()
    {
        throw new System.NotImplementedException();
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
