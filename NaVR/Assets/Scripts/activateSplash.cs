using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateSplash : MonoBehaviour
{

    private bool activateEffect;
    private float deltaAltura;

    private float startpos;
    private bool lastState;
    private float targetpos;
    private bool direction;

    // Use this for initialization
    void Start()
    {
        activateEffect = false;
        deltaAltura = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (activateEffect == true)
        {
            if (lastState == false)
            {
                startpos = transform.position.y;
                targetpos = transform.position.y + 40;
                deltaAltura = 0.0f;
                direction = true;
            }
            lastState = true;
            if (targetpos - transform.position.y < 1.0f && direction == true)
            {
                targetpos = startpos;
                startpos = transform.position.y;
                deltaAltura = 0.0f;
                direction = false;
            }
            else if (transform.position.y - targetpos < 1.0f && direction == false)
            {
                activateEffect = false;
                transform.position = new Vector3(transform.position.x, targetpos, transform.position.z);
            }
            deltaAltura += 0.003f;
            float newPosition = Mathf.SmoothStep(transform.position.y, targetpos, deltaAltura);
            transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
        }
        else
        {
            lastState = false;
        }
    }

    public bool GetActivateEffect()
    {
        return activateEffect;
    }

    public void SetActivateEffect(bool activateEffect)
    {
        this.activateEffect = activateEffect;
    }
}
