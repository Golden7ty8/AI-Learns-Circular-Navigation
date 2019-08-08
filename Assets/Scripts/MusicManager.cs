using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;

    AudioClip[] clipOrder;
    bool[] pickedClips;
    //int pickedClipsCount;
    int currentPlayIndex;
    int prevLastIndex;

    // Start is called before the first frame update
    void Start()
    {
        //pickedClipsCount = 0;
        clipOrder = new AudioClip[clips.Length];
        pickedClips = new bool[clips.Length];

        CreateNewOrder(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            //Play new clip.
            PlayNext();
        }
    }

    void PlayNext()
    {   
        //If list has finished, re-shuffle.
        if(currentPlayIndex == clipOrder.Length)
        {
            CreateNewOrder();
        }

        currentPlayIndex++;

        audioSource.clip = clipOrder[currentPlayIndex];
        audioSource.Play();
    }

    void CreateNewOrder(bool start = false)
    {
        for (int i = 0; i < pickedClips.Length; i++)
        {
            pickedClips[i] = false;
        }

        int picked;
        for(int i = pickedClips.Length; i > 0; i--)
        {
            //If we are starting over for a second time, shrink the range by one compared to usual, and mark the prevLast as already picked for the first loop
            //to insure it is not picked first.
            if (!start && i == pickedClips.Length)
            {
                picked = Random.Range(0, i - 1);
                pickedClips[prevLastIndex] = true;
            }
            else
            {
                //Start by picking a random number within the acceptable range...
                picked = Random.Range(0, i);
            }

            //Then go through the pickedClips array so we can pick the index specified by picked (but skipping any clips already picked).
            for(int j = 0; j < pickedClips.Length; j++)
            {
                //If this clip has not yet been picked...
                if(!pickedClips[j])
                {
                    //Either count picked down or if at zero, take this clip.
                    if(picked == 0)
                    {
                        //Add to clip order.
                        clipOrder[pickedClips.Length - i] = clips[j];
                        pickedClips[j] = true;

                        //If this is the last in the order, mark the prevLastIndex.
                        if(i == 1)
                        {
                            prevLastIndex = j;
                        }

                        break;
                    }
                    else
                    {
                        picked--;
                    }

                }
            }

            //Now set prevLast back to unpicked.
            if (!start && i == pickedClips.Length)
            {
                pickedClips[prevLastIndex] = false;
            }
        }

        currentPlayIndex = -1;
    }
}
