using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Miscellanious Global Variables.
    public int initialPopulationSize;
    public int curGeneration;
    public float globalHighScore;
    public int memberCounter;

    //Bloodline struct
    public string[] bloodlineNames;
    public int[] bloodlineCounts;

    //Population struct
    public int[] memberNum;
    public float[,] weights;
    public float[] fitness;
    public float[] ratio;
    public bool[] evaluated;
    public int[] generation;
    public int[] level;
    public string[] name;
    public int[] nameGen;
    public int[] bloodlineLevel;
    public float[,] avatarColor;

    public GameData(PlayerController playerController, ScoreManager scoreManager)
    {

        //ex: allWeights = playerController.weights;

        initialPopulationSize = playerController.initialPopulationSize;
        curGeneration = playerController.generation;
        globalHighScore = scoreManager.globalHighScore;
        memberCounter = playerController.memberCounter;

        int bloodCount = playerController.bloodlines.Count;
        int popCount = playerController.population.Count;

        bloodlineNames = new string[bloodCount];
        bloodlineCounts = new int[bloodCount];

        memberNum = new int[popCount];
        weights = new float[popCount, playerController.population[0].weights.Length];
        fitness = new float[popCount];
        ratio = new float[popCount];
        evaluated = new bool[popCount];
        generation = new int[popCount];
        level = new int[popCount];
        name = new string[popCount];
        nameGen = new int[popCount];
        bloodlineLevel = new int[popCount];
        avatarColor = new float[popCount,3];
        
        for(int i = 0; i < playerController.bloodlines.Count; i++)
        {
            bloodlineNames[i] = playerController.bloodlines[i].name;
            bloodlineCounts[i] = playerController.bloodlines[i].count;
        }

        for(int i = 0; i < playerController.population.Count; i++)
        {
            memberNum[i] = playerController.population[i].memberNum;

            for(int j = 0; j < playerController.population[i].weights.Length; j++)
            {
                weights[i, j] = playerController.population[i].weights[j];
            }

            fitness[i] = playerController.population[i].fitness;
            ratio[i] = playerController.population[i].ratio;
            evaluated[i] = playerController.population[i].evaluated;
            generation[i] = playerController.population[i].generation;
            level[i] = playerController.population[i].level;
            name[i] = playerController.population[i].name;
            nameGen[i] = playerController.population[i].nameGen;
            bloodlineLevel[i] = playerController.population[i].bloodlineLevel;

            avatarColor[i, 0] = playerController.population[i].avatarColor.r;
            avatarColor[i, 1] = playerController.population[i].avatarColor.g;
            avatarColor[i, 2] = playerController.population[i].avatarColor.b;

        }

    }

}
