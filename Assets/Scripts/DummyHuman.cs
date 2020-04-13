using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHuman : MonoBehaviour
{
    public float aggressionMultiplier;
    public float strengthMultiplier;
    public float moveSpeedMultiplier;
    public float staminaMultiplier;
    public float enduranceMultiplier;
    public float healthMultiplier;
    public float hungerMultiplier;
    public float thirstMultiplier;
    public float aggression;
    public float strength;
    public float moveSpeed;
    public float stamina;
    public float endurance;
    public float health;
    public float hunger;
    public float thirst;
    public float age;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, .25f, 0);
    }

    public void SetAttributes(GeneticAlgorithmManager.HumanDNA dna) {
        aggression = dna.aggressionMultiplier * 50;
        strength = dna.strengthMultiplier * 50;
        endurance = dna.enduranceMultiplier * 100;
        health = dna.healthMultiplier * 100;
        hunger = dna.hungerMultiplier * 100;
        thirst = dna.thirstMultiplier * 100;
        stamina = dna.staminaMultiplier * 100;
        moveSpeed = dna.moveSpeedMultiplier * 10;
        age = dna.endAge;
    }

}
