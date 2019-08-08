using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform cameraDest;
    public Transform cameraSubject;
    public Transform dummy;
    public float moveSpeed;
    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, cameraDest.position, Time.deltaTime * moveSpeed);
        dummy.position = transform.position;
        dummy.transform.LookAt(cameraSubject);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, dummy.eulerAngles, Time.deltaTime * rotSpeed);
    }
}
