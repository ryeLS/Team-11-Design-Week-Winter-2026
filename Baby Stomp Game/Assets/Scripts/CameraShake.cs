using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isShaking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShakeCamera(float duration, float severity, bool vertical , bool horizontal)
    {
        if (isShaking)
        {
            return;
        }
        StartCoroutine(Shake(duration, severity, vertical, horizontal));
    }
    private IEnumerator Shake(float duration, float severity, bool vertical, bool horizontal)
    {
        originalPosition = transform.localPosition;
        isShaking = true;
        
        while(duration > 0)
        {
            Vector3 shakeOffset = Vector3.zero;
            if (vertical)
            {
                shakeOffset.x = Random.Range(-1f, 1f) * severity;
            }
            if(horizontal)
            {
                shakeOffset.y = Random.Range(-1f, 1f) * severity;
            }
            transform.localPosition = originalPosition + shakeOffset;
            duration -= Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPosition;
        isShaking = false;
    }
}
