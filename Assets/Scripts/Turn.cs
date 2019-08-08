using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.eulerAngles += new Vector3(0, Time.deltaTime * speed, 0);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + Time.deltaTime * speed, 0);
    }
}
