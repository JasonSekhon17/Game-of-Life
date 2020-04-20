using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithmManager : MonoBehaviour
{
    public struct HumanDNA {
        public float aggressionMultiplier;
        public float enduranceMultiplier;
        public float healthMultiplier;
        public float hungerMultiplier;
        public float moveSpeedMultiplier;
        public float staminaMultiplier;
        public float strengthMultiplier;
        public float thirstMultiplier;
        public float endAge;

        public HumanDNA(float aggression, float endurance, float health, float hunger, float moveSpeed, float stamina, float strength, float thirst, float age) {
            aggressionMultiplier = aggression;
            enduranceMultiplier = endurance;
            healthMultiplier = health;
            hungerMultiplier = hunger;
            moveSpeedMultiplier = moveSpeed;
            staminaMultiplier = stamina;
            strengthMultiplier = strength;
            thirstMultiplier = thirst;
            endAge = age;
        }

        public HumanDNA(HumanDNA _humanDNA) {
            aggressionMultiplier = _humanDNA.aggressionMultiplier;
            enduranceMultiplier = _humanDNA.enduranceMultiplier;
            healthMultiplier = _humanDNA.healthMultiplier;
            hungerMultiplier = _humanDNA.hungerMultiplier;
            moveSpeedMultiplier = _humanDNA.moveSpeedMultiplier;
            staminaMultiplier = _humanDNA.staminaMultiplier;
            strengthMultiplier = _humanDNA.strengthMultiplier;
            thirstMultiplier = _humanDNA.thirstMultiplier;
            endAge = _humanDNA.endAge;
        }
    }

    public List<HumanDNA> matingPool;
    public DummyHuman idealHuman;

    void Start() {
        matingPool = new List<HumanDNA>();
    }

    public void RandomizeHumanDNA(Human human) {
        float[] randVals = {Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f),
                            Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f),
                            Random.Range(0f, 1f), Random.Range(0f, 1f)};
        float sum = 0;
        foreach (float val in randVals)
            sum += val;
        human.aggressionMultiplier = (randVals[0]/sum) * 4f;
        human.enduranceMultiplier = (randVals[1]/sum) * 4f;
        human.healthMultiplier = (randVals[2]/sum) * 4f;
        human.hungerMultiplier = (randVals[3]/sum) * 4f;
        human.moveSpeedMultiplier = (randVals[4]/sum) * 4f;
        human.staminaMultiplier = (randVals[5]/sum) * 4f;
        human.strengthMultiplier = (randVals[6]/sum) * 4f;
        human.thirstMultiplier = (randVals[7]/sum) * 4f;
    }

    public void BuildHumanDNAFromMatingPool(Human human) {
        HumanDNA parent_1 = matingPool[Random.Range(0, matingPool.Count - 1)];
        HumanDNA parent_2 = matingPool[Random.Range(0, matingPool.Count - 1)];
        float[] childVals = {
                            (parent_1.aggressionMultiplier + parent_2.aggressionMultiplier) / 2,
                            (parent_1.enduranceMultiplier + parent_2.enduranceMultiplier) / 2,
                            (parent_1.healthMultiplier + parent_2.healthMultiplier) / 2,
                            (parent_1.hungerMultiplier + parent_2.hungerMultiplier) / 2,
                            (parent_1.moveSpeedMultiplier + parent_2.moveSpeedMultiplier) / 2,
                            (parent_1.staminaMultiplier + parent_2.staminaMultiplier) / 2,
                            (parent_1.strengthMultiplier + parent_2.strengthMultiplier) / 2,
                            (parent_1.thirstMultiplier + parent_2.thirstMultiplier) / 2,
                            };
        float sum = 0;
        for (int i = 0; i < childVals.Length; i++) {
            if (Random.Range(0, 1) < .1)
                childVals[i] = Random.Range(.25f, childVals[i] * 1.5f);
            sum += childVals[i];
        }
        human.aggressionMultiplier = (childVals[0]/sum) * 4f;
        human.enduranceMultiplier = (childVals[1]/sum) * 4f;
        human.healthMultiplier = (childVals[2]/sum) * 4f;
        human.hungerMultiplier = (childVals[3]/sum) * 4f;
        human.moveSpeedMultiplier = (childVals[4]/sum) * 4f;
        human.staminaMultiplier = (childVals[5]/sum) * 4f;
        human.strengthMultiplier = (childVals[6]/sum) * 4f;
        human.thirstMultiplier = (childVals[7]/sum) * 4f;
    }

    public void SelectHumans() {
        List<HumanDNA> humanDNAs = new List<HumanDNA>(matingPool);
        humanDNAs.Sort((a, b) => a.endAge.CompareTo(b.endAge));

        if (humanDNAs[humanDNAs.Count - 1].endAge >= idealHuman.age - 5)
            idealHuman.SetAttributes(humanDNAs[humanDNAs.Count - 1]);
        
        matingPool.Clear();
        for (int i = 0; i < humanDNAs.Count; i++)
            for (int j = 0; j < i + 1; j++)
                matingPool.Add(humanDNAs[i]);
    }
}
