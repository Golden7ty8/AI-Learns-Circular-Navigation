using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGroup : MonoBehaviour
{

    public ScoreManager scoreManager;
    public PlayerController playerController;

    public float pointValue;
    public float maxPickUpScore;

    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        maxPickUpScore = transform.childCount * pointValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isSimulating)
        {
            //float tmp = pointValue - GetClosestObjectDist();
            //scoreManager.distScore = tmp > 0 ? tmp : 0;
            scoreManager.distScore = pointValue - GetClosestObjectDist();
        }
    }

    public void UpdateDistScore()
    {
        //float tmp = pointValue - GetClosestObjectDist();
        scoreManager.distScore = pointValue - GetClosestObjectDist();
        //scoreManager.distScore = tmp > 0 ? tmp : 0;
    }

    public float GetClosestObjectDist()
    {
        //If no pickups are left...
        //if (transform.childCount == 0)
            //return 0;

        float bestDist = 9999;
        int bestIndex = 0;

        for (int i = 0; i < transform.childCount; i++) {
            //Skip any that are disabled, i.e picked up already.
            if(transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<PickUp>().distToPlayer < bestDist)
            {
                bestDist = transform.GetChild(i).GetComponent<PickUp>().distToPlayer;
                bestIndex = i;
            }
        }

        //Debug.Log("bestDist = " + bestDist.ToString());

        //If no pickups are left...
        if (bestDist == 9999)
            return pointValue;

        return transform.GetChild(bestIndex).GetComponent<PickUp>().distToPlayer;
    }

    public Vector3 GetClosestObjectDir()
    {
        float bestDist = 9999;
        int bestIndex = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            //Skip any that are disabled, i.e picked up already.
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<PickUp>().distToPlayer < bestDist)
            {
                bestDist = transform.GetChild(i).GetComponent<PickUp>().distToPlayer;
                bestIndex = i;
            }
        }

        //if (transform.childCount == 0)
        if (bestDist == 9999)
            return playerTransform.position;

        return transform.GetChild(bestIndex).position - playerTransform.position;
    }

    public void ResetPickUps()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        scoreManager.pickUpScore = 0;
    }
}
