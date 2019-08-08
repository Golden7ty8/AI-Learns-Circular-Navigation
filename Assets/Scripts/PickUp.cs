using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public ScoreManager scoreManager;
    public Transform playerTransform;
    public PickUpGroup pickUpGroup;

    public float speed;
    public float distToPlayer;

    void Awake()
    {
        //distToPlayer = -99999.0f;
        Vector3 posDif = transform.position - playerTransform.position;
        distToPlayer = Mathf.Sqrt(Mathf.Pow(posDif.x, 2) + Mathf.Pow(posDif.y, 2) + Mathf.Pow(posDif.z, 2));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0, Time.deltaTime * speed, 0);

        //Calculate distance to player.
        Vector3 posDif = transform.position - playerTransform.position;
        distToPlayer = Mathf.Sqrt(Mathf.Pow(posDif.x, 2) + Mathf.Pow(posDif.y, 2) + Mathf.Pow(posDif.z, 2));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
            pickUpGroup.UpdateDistScore();

            scoreManager.pickUpScore += transform.parent.GetComponent<PickUpGroup>().pointValue;
            scoreManager.UpdateDisplay();
        }
    }
}
