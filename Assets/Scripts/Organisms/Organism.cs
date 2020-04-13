using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Organism : MonoBehaviour
{
    public float enduranceMultiplier;
    public float healthMultiplier;
    public float hungerMultiplier;
    public float thirstMultiplier;
    public float endurance;
    public float baseHealth;
    public float baseHunger;
    public float baseThirst;
    public float currentHealth;
    public float currentHunger;
    public float currentThirst;
    public float age;
    public float reproductionAge;
    public float reproductionTimer;
    public float nutritionalValue;
    public bool canReproduce;
    public Organism reproductionPrefab;

    protected abstract List<KeyValuePair<string, float>> GetattributeDistributions();

    public virtual void Reproduce() {
        float randX = Random.Range(-1, 1);
        if (randX < 0)
            randX -= 2;
        else
            randX += 2;
        float randZ = Random.Range(-1, 1);
        if (randZ < 0)
            randZ -= 2;
        else
            randZ += 2;

        Organism temp = Instantiate(reproductionPrefab, new Vector3(transform.position.x + randX, .5f, transform.position.z + randZ), Quaternion.identity);
        
        reproductionTimer = 0;
        canReproduce = false;

        if (temp.transform.position.x > 21)
            temp.transform.position = new Vector3(Random.Range(17, 20), temp.transform.position.y, temp.transform.position.z);
        else if (temp.transform.position.x < -21)
            temp.transform.position = new Vector3(Random.Range(-17, -20), temp.transform.position.y, temp.transform.position.z);
        
        if (temp.transform.position.z > 24)
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, Random.Range(20, 23));
        else if (temp.transform.position.z < -24)
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, Random.Range(-20, -23));
    }

    public virtual void Die() {
        Destroy(gameObject);
    }
}
