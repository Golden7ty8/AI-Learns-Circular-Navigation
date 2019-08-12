using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   

    [System.Serializable]
    public struct Neuron
    {
        public float myValue;
        public List<float> weights;
    }

    [System.Serializable]
    public struct NeuronLine
    {
        //public List<Neuron> neurons;
        public Neuron[] neurons;
    }

    /*[System.Serializable]
    public struct NeuronBrain
    {
        public List<NeuronLine> neuronLines;

    }*/

    [System.Serializable]
    public struct Bloodline
    {
        public string name;
        public int count;
    }

    [System.Serializable]
    public struct DNA
    {
        public string name;

        //public List<float> weights;
        public int memberNum;
        public float[] weights;
        public float fitness;
        public float ratio;
        public bool evaluated;
        public int generation;
        public int level;
        //public string name;
        public int nameGen;
        public int bloodlineLevel;
        public Color avatarColor;
    }

    /*[System.Serializable]
    public struct Population
    {
        public List<DNA> members;
        //public DNA[] members;
    }*/

    public Rigidbody rb;
    public PickUpGroup pickUpGroup;

    public float speed;
    public bool AI;

    [Header("AI")]
    //public List<NeuronLine> brain;
    public bool loadPrevPopulation;
    public bool controlAI;
    public bool autoSave = true;
    [Range(0.0f, 1.0f)]
    public float mutationRate;
    public NeuronLine[] brain;
    public List<DNA> population;
    public List<Bloodline> bloodlines;
    [Tooltip("For randomly generated weights, what's the range (i.e 10 will result in a range of -10 to 10)?")]
    public float initialDataRange;
    [Tooltip("Should be a number that is divisible by 5.")]
    public int initialPopulationSize;
    [Tooltip("If the score reaches this number or lower, the simulation for this member is terminated.")]
    public float minPermittedScore;
    public int generation;
    public int memberCounter;
    public float timeLimit;
    public ScoreManager scoreManager;
    public float simulationDelayFrames;

    //[HideInInspector]
    public int activePopIndex;
    //[HideInInspector]
    public bool isSimulating;

    //[Header("AI Data - Inputs")]


    [Header("AI Data - Outputs")]
    public float horizontalAI;
    public float verticalAI;

    float startScore;

    Vector3 spawnPoint;

    void Awake()
    {
        isSimulating = false;
        //simulationDelay = -1;
        simulationDelayFrames = -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        activePopIndex = -1;
        memberCounter = 0;
        startScore = pickUpGroup.GetClosestObjectDist() * -1;

        if (AI)
        {
            /*if (loadPrevPopulation)
            {
                LoadGameData();
            }*/
            if (!loadPrevPopulation || !LoadGameData())
            {
                //Force populationSize to be divisible by 5.
                initialPopulationSize += (5 - initialPopulationSize % 5) % 5;
                generation = 1;
                GenerateStartingDNA(initialPopulationSize);
                //ResetSimulation(true);
            }
            ResetSimulation(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (AI)
        {
            if(simulationDelayFrames > 0)
            {
                //simulationDelay -= Time.deltaTime;
                simulationDelayFrames--;
                if(simulationDelayFrames <= 0)
                {
                    isSimulating = true;
                    simulationDelayFrames = -1;
                }
            }
            if (isSimulating)
            {
                UpdateAIBrain();
                if (controlAI)
                {
                    rb.AddForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * speed);
                }
                else
                {
                    rb.AddForce(new Vector3(horizontalAI, 0, verticalAI).normalized * speed);
                }
            }
        } else
        {
            rb.AddForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * speed);
        }
    }

    public void Kill() {
        transform.position = spawnPoint;
        rb.velocity = Vector3.zero;
    }

    /*void StartAIBrain()
    {

        brain = new List<NeuronLine>();

        //Define number of layers (min: 2).
        for (int i = 0; i < 2; i++)
        {
            brain.Add(new NeuronLine());
            //brain.neuronLines[i].neurons = new List<Neuron>();
        }

        //Define number of inputs.
        for (int i = 0; i < 7; i++)
        {
            brain[0].neurons.Add(new Neuron());
        }

        //Define number of outputs.
        for (int i = 0; i < 2; i++)
        {
            brain[brain.Count - 1].neurons.Add(new Neuron());
        }
    }*/

    void UpdateAIBrain()
    {
        
        //First check to see if the simulation for this member should be terminated.
        if (scoreManager.timeLeft <= 0/*Time.time - scoreManager.memberStartTime >= timeLimit*/ || scoreManager.totalScore < minPermittedScore || scoreManager.totalScore >= pickUpGroup.maxPickUpScore) {
            //Debug.Log("Before: -> totalScore = " + scoreManager.totalScore.ToString() + " memberStartTime = " + scoreManager.memberStartTime.ToString());
            ResetSimulation();
            //Debug.Log("After: -> totalScore = " + scoreManager.totalScore.ToString() + " memberStartTime = " + scoreManager.memberStartTime.ToString());
        }

        //Update Inputs

        //rb.velocity
        //distToNearestPickup
        //direction to nearest PickUp
        brain[0].neurons[0].myValue = rb.velocity.x;
        brain[0].neurons[1].myValue = rb.velocity.y;
        brain[0].neurons[2].myValue = rb.velocity.z;

        brain[0].neurons[3].myValue = pickUpGroup.GetClosestObjectDist();

        Vector3 tmp = pickUpGroup.GetClosestObjectDir();

        brain[0].neurons[4].myValue = tmp.x;
        brain[0].neurons[5].myValue = tmp.y;
        brain[0].neurons[6].myValue = tmp.z;



        //Set Outputs based on weights.
        for (int i = 0; i < 2; i++) {
            brain[1].neurons[i].myValue = 0;
            for (int j = 0; j < 7; j++) {
                //Debug.Log("brain size = " + brain.Length.ToString() + ", neurons sizes = (" + brain[0].neurons.Length.ToString() + ", " + brain[1].neurons.Length.ToString());
                //Debug.Log("brain[0].neurons[j].weights.Count = " + brain[0].neurons[j].weights.Count.ToString());
                //Debug.Log("i = " + i.ToString() + " j = " + j.ToString());
                brain[1].neurons[i].myValue += (brain[0].neurons[j].myValue * brain[0].neurons[j].weights[i]);
            }
            //Scale final value to a range of -1 to 1.
            //https://en.wikipedia.org/wiki/Sigmoid_function#/media/File:Gjl-t(x).svg
            float x = brain[1].neurons[i].myValue / initialDataRange;
            brain[1].neurons[i].myValue = x / (1 + Mathf.Abs(x));
        }

        //Project Outputs
        //horizontalMovement
        //verticalMovement
        horizontalAI = brain[1].neurons[0].myValue;
        verticalAI = brain[1].neurons[1].myValue;
    }

    void SaveGameData()
    {
        SaveSystem.SavePopulation(this, scoreManager);
    }

    bool LoadGameData()
    {
        GameData data = SaveSystem.LoadPopulation();
        if (data == null)
            return false;

        initialPopulationSize = data.initialPopulationSize;
        generation = data.curGeneration;
        scoreManager.globalHighScore = data.globalHighScore;
        memberCounter = data.memberCounter;

        int popCount = population.Count;

        /*memberNum = new int[popCount];
        weights = new float[popCount, playerController.population[0].weights.Length];
        fitness = new float[popCount];
        ratio = new float[popCount];
        evaluated = new bool[popCount];
        generation = new int[popCount];
        level = new int[popCount];*/

        bloodlines = new List<Bloodline>();

        for (int i = 0; i < data.bloodlineNames.Length; i++)
        {
            Bloodline tmp = new Bloodline();
            tmp.name = data.bloodlineNames[i];
            tmp.count = data.bloodlineCounts[i];
            //bloodlineCounts[i] = playerController.bloodlines[i].count;
            bloodlines.Add(tmp);
        }

        population = new List<DNA>();

        for (int i = 0; i < data.memberNum.Length; i++)
        {
            DNA loadedMember = new DNA();

            loadedMember.memberNum = data.memberNum[i];

            loadedMember.weights = new float[data.weights.GetLength(1)];
            for (int j = 0; j < data.weights.GetLength(1); j++)
            {
                loadedMember.weights[j] = data.weights[i, j];
            }

            loadedMember.fitness = data.fitness[i];
            loadedMember.ratio = data.ratio[i];
            loadedMember.evaluated = data.evaluated[i];
            loadedMember.generation = data.generation[i];
            loadedMember.level = data.level[i];
            loadedMember.name = data.name[i];
            loadedMember.nameGen = data.nameGen[i];
            loadedMember.bloodlineLevel = data.bloodlineLevel[i];
            loadedMember.avatarColor = new Color(data.avatarColor[i, 0], data.avatarColor[i, 1], data.avatarColor[i, 2]);

            population.Add(loadedMember);

        }

        return true;

    }

    void GenerateStartingDNA(int count)
    {
        //Simply run GenerateRandomDNA for as many times as requested and insert the DNA into the population.
        for(int i = 0; i < count; i++)
        {
            population.Add(GenerateRandomDNA());
        }

        if(autoSave)
            SaveGameData();
    }

    DNA GenerateRandomDNA()
    {
        DNA res = new DNA();
        res.level = 1;

        res.weights = new float[brain[0].neurons.Length * brain[1].neurons.Length];

        for(int i = 0; i < res.weights.Length; i++)
        {
            res.weights[i] = GenerateRandomWeight();
        }

        //res.fitness = -99999;
        res.fitness = 0;
        //memberCounter++;
        res.memberNum = ++memberCounter;
        res.name = NameBank.GetRandomName();
        res.nameGen = 1;
        res.bloodlineLevel = 1;
        res.avatarColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //res.avatarColor = new Color(Random.Range(0.0f, 255.0f), Random.Range(0.0f, 255.0f), Random.Range(0.0f, 255.0f));
        //Debug.Log("memberCounter = " + memberCounter.ToString());

        //Add entry to bloodlines array.
        Bloodline newBloodline = new Bloodline();
        newBloodline.name = res.name;
        newBloodline.count = 1;
        bloodlines.Add(newBloodline);

        return res;
    }

    float GenerateRandomWeight()
    {
        return Random.Range(-initialDataRange, initialDataRange);
    }

    void ResetSimulation(bool start = false)
    {
        //Debug.Log("ResetSimulation Ran");
        isSimulating = false;
        simulationDelayFrames = 2;
        if (start)
        {
            //Debug.Log("START!");
        } else
        {
            //Debug.Log("EVALUATE!");
            Evaluate();
            rb.velocity = Vector3.zero;
            transform.position = spawnPoint;
        }

        //Insure all scores are updated in time for the AI to use them. Waiting for the other scripts to update them
        //automatically may result in this AI taking/using the old values before they can be updated.
        pickUpGroup.ResetPickUps();
        //pickUpGroup.UpdateDistScore();
        scoreManager.distScore = 0;
        scoreManager.UpdateDisplay();
        //scoreManager.ResetScores();

        //scoreManager.memberStartTime = Time.time;

        //Debug.Log("Install next member!");
        InstallNextMember();
        scoreManager.ResetScores();
        //Debug.Log("ResetSimulation Complete!");
    } 

    //Take current score at give it to the current DNA's fitness.
    void Evaluate()
    {
        DNA tmp = population[activePopIndex];
        tmp.fitness = scoreManager.memberHighScore;
        tmp.evaluated = true;
        population[activePopIndex] = tmp;

        SaveGameData();
    }

    //Go to the next member in this generation (make sure to skip members from previous generations!!!),
    //if there are no more members to test, make sure to generate the new population with new members!
    void InstallNextMember()
    {
        //Error check
        if(population.Count == 0)
        {
            Debug.LogError("InstallNextMember() -> population.Count was 0!!!");
        }

        //Reset the bestScore value of scoreManager, so we can get the fitness for the next member!
        scoreManager.memberHighScore = 0;

        //Debug.Log("InstallNextMemberRan");

        while (true)
        {
            activePopIndex++;
            //Debug.Log("activePopIndex = " + activePopIndex.ToString());
            //If this index is larger then the index of the last value, then we need to generate a new population!
            if (activePopIndex >= population.Count)
            {
                CreateNextGeneration();
                //Debug.LogWarning("CreateNextGeneration(); is temporaraly disabled!");
                activePopIndex = 0;
            }

            //Now check to see if this DNA has already been evealuated, if so skip this DNA as it has already been tested!
            //if (population[activePopIndex].generation == generation)
            if (!population[activePopIndex].evaluated)
                break;
        }

        //We have now found the DNA we intend to test, send info over to next function!
        InstallDNA(population[activePopIndex]);
    }

    void InstallDNA(DNA current)
    {
        //Debug.Log(current.avatarColor);
        GetComponent<Renderer>().material.color = /*new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);*//*current.avatarColor;*/new Color(current.avatarColor.r, current.avatarColor.g, current.avatarColor.b, /*0.5f*/GetComponent<Renderer>().material.color.a);
        transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1 - current.avatarColor.r, 1 - current.avatarColor.g, 1 - current.avatarColor.b, transform.GetChild(0).GetComponent<Renderer>().material.color.a);
        InstallDNAWeights(current);
    }

    void InstallDNAWeights(DNA current)
    {
        int ijk = 0;
        for(int i = 0; i < 1; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                for(int k = 0; k < 2; k++)
                {
                    //brain[i].neurons[j].weights[k] = current.weights[i + j + k];
                    brain[i].neurons[j].weights[k] = current.weights[ijk++];
                    //Debug.Log("ijk = " + ijk.ToString());
                    //Debug.Log("weights.Length = " + current.weights.Length.ToString());
                }
            }
        }
    }

    //After everyone in the population has been evalulated/tested, we will now decide how the next generation will be created.
    //Strategy: Take top 20% (min: 1) of DNA, all other DNA will be discarded, the remaining DNA will be used to generate
    //the replacement DNA, for each new DNA, a weight will be taken from one of the parents, each parent will have a chance
    //to be selected, their chance are better the higher their percentage is.

    //REMOVED (Now only 80% and 20%): {We will also have 20% be newly generated, so the
    //new population will be: 20% from old population, 20% random DNA, and 60% new DNA generated from top 20%.}
    void CreateNextGeneration()
    {
        //Save temporary copy of current population.
        List<DNA> oldPopulation = new List<DNA>(population);
        float generationWorstFitness = pickUpGroup.maxPickUpScore;

        //Find the worst fitness score.
        for(int i = 0; i < oldPopulation.Count; i++)
        {
            if (generationWorstFitness > oldPopulation[i].fitness)
                generationWorstFitness = oldPopulation[i].fitness;
        }

        //Calculate size of top 20% (min: 1), i.e how many members will be in the top 20%?
        int top20size = (int)(oldPopulation.Count * 0.2f);
        if (top20size < 1)
            top20size = 1;

        //Find top 20 scores.
        List<float> top20Scores = new List<float>();
        for(int i = 0; i < oldPopulation.Count; i++)
        {
            //If the list already contains the correct number of scores, check this score with the scores in the list
            //from worst to best.
            if(top20Scores.Count >= top20size)
            {
                //Go from worst to best once this score is worst then the score in the list we are looking at, insert this score afterwards.
                //Then delete the last score in the list.
                for(int j = top20Scores.Count - 1; j >= 0; j--)
                {
                    if (oldPopulation[i].fitness < top20Scores[j])
                    {
                        if (j == top20Scores.Count - 1)
                            top20Scores.Add(oldPopulation[i].fitness);
                        else
                            top20Scores.Insert(j + 1, oldPopulation[i].fitness);

                        break;
                    } else if (j == 0)
                    {
                        //In the case where oldPopulation[i].fitness is not worse then any top20Scores[j],
                        //insert the score at the start of top20Scores[j].
                        top20Scores.Insert(0, oldPopulation[i].fitness);
                    }
                }

                //Remove last score.
                top20Scores.RemoveAt(top20Scores.Count - 1);

            } else //If the list is not yet full, simply insert this score into the list such that the list is
                //ordered from best to worst score.
            {
                for(int j = 0; j <= top20Scores.Count; j++)
                {
                    //If we reached j == top20Scores.Count, simply add the score to the end
                    //(usefull for when the list is empty).
                    if(j == top20Scores.Count)
                    {
                        top20Scores.Add(oldPopulation[i].fitness);
                        //The loop would break out at this point anyway, so break is technically not nessesary.
                        break;
                    } else
                    {
                        //Since we have not reached the end of this list (and the list is not empty),
                        //check to see if we should insert it here.
                        if(oldPopulation[i].fitness >= top20Scores[j])
                        {
                            top20Scores.Insert(j, oldPopulation[i].fitness);
                            break;
                        }
                    }
                }
            }
        }

        //Now top20Scores should contain the top20Scores from best to worst. Now remove any DNA from the oldPopulation list
        //that has a worse score then the worst of the top 20%.
        //Need to go backwards, so as to not confuse the indexing of our search when indexes change due to removals.
        for(int i = oldPopulation.Count - 1; i >= 0; i--)
        {
            //If this member's score is below the top 20%...
            if(oldPopulation[i].fitness < top20Scores[top20Scores.Count - 1])
            {
                oldPopulation.RemoveAt(i);
            }
        }

        //Now the oldPopulation should contain only the top 20%.
        //To breed DNA based on percentages, we will have the lowest premitted score (where ratio of 0.0 to 1.0 here is 0)
        //at the worst fitness of this generation and max permitted score as the best score of the generation.

        //Calculate and add all ratios together (also insert every ratio into their correcsponding DNA).
        float totalRatio = 0;
        for(int i = 0; i < oldPopulation.Count; i++)
        {
            DNA tmp = oldPopulation[i];
            tmp.ratio = ReturnRatio(oldPopulation[i], generationWorstFitness, top20Scores[0]);
            oldPopulation[i] = tmp;
            totalRatio += tmp.ratio;
        }

        population = new List<DNA>(oldPopulation);
        generation++;

        //Create random 20%!
        /*for(int i = 0; i < top20size; i++)
        {
            population.Add(GenerateRandomDNA());
        }*/

        //Now create new DNA, create new 60%! (It's 80% again now)
        for (int i = 0; i < top20size * 4; i++) {
            population.Add(GenerateNewDNA(oldPopulation, totalRatio));
        }

        //Population should now be updated for the new generation!

        if(autoSave)
            SaveGameData();
    }

    //Take the fitness of DNA and compute the ratio using a min and max and set the ratio of that DNA accordingly.
    float ReturnRatio(DNA member, float min, float max)
    {
        float /*newMin = 0, */newMax = max - min, tmpFitness = member.fitness - min;

        return tmpFitness / max;
    }

    DNA GenerateNewDNA(List<DNA> genePool, float totalRatio)
    {
        //Go through every weight and then grab one from the gene pool via probabilities!
        DNA newMember = new DNA();
        newMember.weights = new float[brain[0].neurons.Length * brain[1].neurons.Length];
        newMember.memberNum = ++memberCounter;
        newMember.generation = generation;

        //Default value
        newMember.level = 0;

        //We will use this array to pick with probability which parent will pass on their name and color to the newMember!
        DNA[] mainParentalInfluenceOptions = new DNA[newMember.weights.Length];

        //Go through each weight!
        for (int i = 0; i < brain[0].neurons.Length * brain[1].neurons.Length; i++)
        {
            //Start by generating a random number between 0 and the totalRatio!
            float rand = Random.Range(0.0f, totalRatio);

            //Find the cooresponding DNA member that cooresponds to the picked value of rand.

            //But wait!!! Perhaps there is a mutation and this weight will be random!!!
            if (mutationRate > Random.Range(0.0f, 1.0f) || mutationRate >= 1.0f)
            {
                //In this case there was a mutation!
                newMember.weights[i] = GenerateRandomWeight();
                Debug.Log("MUTATION!");
            }
            else
            {
                //In this case there was no mutation.
                float totalPrevRatios = 0;
                for (int j = 0; j < genePool.Count; j++)
                {
                    if (rand <= totalPrevRatios + genePool[j].ratio)
                    {
                        //At this point genePool[j] is the chosen DNA, and transfer the cooresponding weight!
                        newMember.weights[i] = genePool[j].weights[i];

                        //Also update newMember's level as nessesary.
                        if (newMember.level <= genePool[j].level)
                            newMember.level = genePool[j].level + 1;

                        //Record the parent that passed on this weight!
                        mainParentalInfluenceOptions[i] = genePool[j];

                        //No need to keep going through the genePool to find the right DNA for this particular weight
                        //cause it was just found!
                        break;
                    }
                    else
                    {
                        //If not, move on to the next DNA.
                        totalPrevRatios += genePool[j].ratio;

                        //If no DNA was chosen by the end of the genePool, the code is messed up somewhere :P .
                        if (j >= genePool.Count - 1)
                        {
                            Debug.LogError("No DNA was chosen for a weight!");
                        }
                    }
                }
            }
        }

        //Now that the weights are transfered, we can decide on the mainParentalInfluence!
        //Since paretns who passed on more weights wil be listed more times, the parents who passed on more weights
        //is more likely to be the mainInfluence to the child!
        DNA mainParentalInfluence = mainParentalInfluenceOptions[Random.Range(0, mainParentalInfluenceOptions.Length)];

        //Pass on the color, name, nameGen (sort of), and bloodlineLevel from the mainParentalInfluence to the newMember.
        newMember.name = mainParentalInfluence.name;
        newMember.nameGen = GetNextNameGen(newMember.name);
        newMember.bloodlineLevel = mainParentalInfluence.bloodlineLevel + 1;
        newMember.avatarColor = mainParentalInfluence.avatarColor;

        return newMember;
    }

    int GetNextNameGen(string bloodlineName)
    {
        int res = 1;
        for(int i = 0; i < bloodlines.Count; i++)
        {
            if(bloodlines[i].name == bloodlineName)
            {
                Bloodline tmp = bloodlines[i];
                res = ++tmp.count;
                bloodlines[i] = tmp;
                break;
            }
        }

        return res;
    }
}
