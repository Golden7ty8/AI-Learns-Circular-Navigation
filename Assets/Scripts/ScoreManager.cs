using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public PickUpGroup pickUpGroup;

    public Gradient goodBadGradient;
    public Gradient goodBadBestGradient;

    public float pickUpScore;
    public float distScore;
    public float totalScore;
    public float memberHighScore;
    //public float memberTime;
    public float memberStartTime;
    public float speedCompletionBonus;
    public float memberTime;
    public float timeLeft;
    public float globalHighScore;

    public PlayerController playerController;

    float timeLimit;
    float prevProgressTime;
    float startScore;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        memberHighScore = 0;
        distScore = 0;
        memberStartTime = Time.time;
        timeLimit = playerController.timeLimit;
        startScore = pickUpGroup.pointValue - pickUpGroup.GetClosestObjectDist();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        memberTime = Time.time - memberStartTime;
        if (pickUpScore >= pickUpGroup.maxPickUpScore)
        {
            speedCompletionBonus = (pickUpGroup.maxPickUpScore / memberTime) * pickUpGroup.pointValue;
        } else
        {
            speedCompletionBonus = 0;
        }
        totalScore = pickUpScore + distScore + speedCompletionBonus;
        if (totalScore > memberHighScore)
        {
            prevProgressTime = Time.time;

            memberHighScore = totalScore;
            if(memberHighScore > globalHighScore)
            {
                globalHighScore = memberHighScore;
            }
            //Debug.Log("bestScore = " + bestScore.ToString());
        }
        timeLeft = timeLimit - (Time.time - prevProgressTime);

        int activePopIndex = playerController.activePopIndex;
        string scoreColor = "#" + ColorUtility.ToHtmlStringRGBA(goodBadGradient.Evaluate(totalScore / memberHighScore))/*"#00ffffff"*/
            , memberHighScoreColor = "#" + ColorUtility.ToHtmlStringRGBA(goodBadGradient.Evaluate((memberHighScore - startScore) / (globalHighScore - startScore)))
            , timeLeftColor = "#" + ColorUtility.ToHtmlStringRGBA(goodBadGradient.Evaluate(timeLeft / timeLimit))
            , memberTimeColor = "#" + ColorUtility.ToHtmlStringRGBA(goodBadBestGradient.Evaluate(Mathf.Clamp01(1.0f - memberTime) / 2.0f + Mathf.Clamp01((pickUpGroup.maxPickUpScore - memberTime) / (pickUpGroup.maxPickUpScore - 1)) / 2.0f))
            , globalHighScoreColor = "#" + ColorUtility.ToHtmlStringRGBA(goodBadBestGradient.Evaluate(Mathf.Clamp01((globalHighScore - startScore) / (pickUpGroup.maxPickUpScore - startScore)) / 2.0f + Mathf.Clamp01((globalHighScore - pickUpGroup.maxPickUpScore) / (pickUpGroup.maxPickUpScore)) / 2.0f));
        //Debug.Log("globalHighScoreColor =" + globalHighScoreColor);
        //Debug.Log((memberHighScore - startScore) / (globalHighScore - startScore));
        GetComponent<Text>().text = "<b>Member #:</b> " + (playerController.activePopIndex >= 0 ? playerController.population[playerController.activePopIndex].memberNum.ToString() : "N/A")
            + "\n<b>Member Name:</b> " + (playerController.activePopIndex >= 0 ? (playerController.population[activePopIndex].name + (playerController.population[activePopIndex].nameGen > 1 ? " " + NameBank.GetRomanNumeral(playerController.population[activePopIndex].nameGen) : "")) : "N/A")
            + "\n<b>Score:</b><color=" + scoreColor + "> " + totalScore.ToString() + "</color>"
            + "\n<b>Member's High Score:</b><color=" + memberHighScoreColor +"> " + memberHighScore.ToString() + "</color>"
            + "\n<b>Time Left:</b><color=" + timeLeftColor + "> " + (Mathf.Round(timeLeft * 10) / 10).ToString() + (Mathf.Round(timeLeft * 10) / 10 % 1 == 0 ? ".0" : "") + "</color>"
            + "\n<b>Member Time:</b><color=" + memberTimeColor + "> " + (Mathf.Round(memberTime * 10) / 10).ToString() + (Mathf.Round(memberTime * 10) / 10 % 1 == 0 ? ".0" : "") + "</color>"
            + "\n<b>Generation #:</b> " + playerController.generation.ToString()
            + "\n<b>Member Level:</b> " + (playerController.activePopIndex >= 0 ? playerController.population[playerController.activePopIndex].level.ToString() : "N/A")
            + "\n<b>Member Bloodline Level:</b> " + (playerController.activePopIndex >= 0 ? playerController.population[playerController.activePopIndex].bloodlineLevel.ToString() : "N/A")
            + "\n<b>Global High Score:</b><color=" + globalHighScoreColor + "> " + globalHighScore.ToString() + "</color>";
    }

    public void ResetScores()
    {
        distScore = 0;
        prevProgressTime = memberStartTime = Time.time;
    }
}
