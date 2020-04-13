using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Organism
{
    float reproductionRate;
    float deathAge;
    public GameObject closestWater;
    public PlantSensor plantSensor;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        plantSensor = GetComponentInChildren<PlantSensor>();
        GameObject[] waterSources = GameObject.FindGameObjectsWithTag("Water");
        if (waterSources != null) {
            closestWater = waterSources[0];
        for (int i = 1; i < waterSources.Length; i++)
            if (Vector3.Distance(waterSources[i].transform.position, transform.position) < Vector3.Distance(closestWater.transform.position, transform.position))
                closestWater = waterSources[i];
        }

        if (Vector3.Distance(closestWater.transform.position, transform.position) < closestWater.transform.localScale.x)
            transform.position += (transform.position - closestWater.transform.position).normalized * 3;

        reproductionRate = Random.Range(reproductionAge, reproductionAge + 1);
        deathAge = Random.Range(5, 8);
        age = 0;
        reproductionTimer = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        reproductionTimer += Time.deltaTime;
        age += Time.deltaTime;
        if (age >= deathAge)
            Die();
        if (reproductionTimer >= reproductionRate && plantSensor.targets.Count < 4) {
            Reproduce();
        }
    }

    protected override List<KeyValuePair<string, float>> GetattributeDistributions() {
        List<KeyValuePair<string, float>> distributions = new List<KeyValuePair<string, float>>();
        distributions.Add(new KeyValuePair<string, float>("Health", currentHealth / baseHealth));
        distributions.Add(new KeyValuePair<string, float>("Hunger", currentHunger / baseHunger));
        distributions.Add(new KeyValuePair<string, float>("Thirst", currentThirst / baseThirst));
        distributions.Sort((x, y) => (x.Value.CompareTo(y.Value)));
        return distributions;
    }
}
