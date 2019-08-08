using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainDisplacement : MonoBehaviour
{
    public Transform subject;

    public Vector3 originalDisplacement;

    // Start is called before the first frame update
    void Start()
    {
        originalDisplacement = transform.position - subject.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = originalDisplacement + subject.position;
    }
}
