using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCamera : MonoBehaviour
{
    public Transform obj;
    public ConstantForce2D forceObj;
    public int speed = 2;
    public float cap = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(obj.localScale.x < cap)
        {
            float test = Time.deltaTime * speed;
            obj.localScale += new Vector3(test, test, test);
        }
        if (obj.localScale.x > 250)
        {
            forceObj.torque = 0f;
        }
    }
}
