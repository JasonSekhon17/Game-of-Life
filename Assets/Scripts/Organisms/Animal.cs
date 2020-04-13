using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Animal : Organism
{
    //Movement
    public enum ActivityState {
        Sprinting,
        Resting,
        Walking
    }
    public ActivityState activityState;
    public AIMovement movement;

    //Animal Attributes
    public float moveSpeedMultiplier;
    public float staminaMultiplier;
    public float strengthMultiplier;
    public float aggressionMultiplier;
    public float baseMoveSpeed;
    public float currentMoveSpeed;
    public float sprintModifier;
    public float baseStamina;
    public float currentStamina;
    public float strength;
    public float aggression;

    //Sight
    public SightSensor sightSensor;

    //Closest Targets
    public Carnivore closestCarnivore;
    public Herbivore closestHerbivore;
    public Omnivore closestOmnivore;
    public Human closestHuman;
    public Plant closestPlant;
    public WaterNode closestWaterSource;
    public Animal reproductionTarget;
    public Transform hungerTarget;
    public Transform thirstTarget;
    public Transform healthTarget;

    //Priority
    public List<string> priorities;
    public string currentPriority;

    public bool canInteract;
    public bool rechargingStamina;

    protected virtual void Start() {
        priorities = new List<string>();
        movement = GetComponent<AIMovement>();
        sightSensor = transform.Find("SightSensor").GetComponent<SightSensor>();
        canInteract = true;
        rechargingStamina = false;

        endurance = enduranceMultiplier * 100;
        baseHealth = healthMultiplier * 100;
        baseHunger = hungerMultiplier * 100;
        baseThirst = thirstMultiplier * 100;
        baseStamina = staminaMultiplier * 100;
        baseMoveSpeed = moveSpeedMultiplier * 10;

        currentHealth = baseHealth;
        currentHunger = baseHunger;
        currentThirst = baseThirst;
        currentStamina = baseStamina;
        currentMoveSpeed = baseMoveSpeed;

        strength = strengthMultiplier * 50;
        aggression = aggressionMultiplier * 50;

        age = 0;
        reproductionTimer = 0;
        canReproduce = false;
        sprintModifier = 1.5f;
    }

    protected virtual void Update()
    {
        UpdateAttributes();
        Observe();
        ChoosePriority();
        ChooseTarget();
        ChoosePriorityAction();
        if (canInteract)
            InteractWithTarget();
        DoMovement();
    }

    void UpdateAttributes () {
        currentHunger -= Time.deltaTime * 2f;
        if (activityState == ActivityState.Sprinting)
            currentThirst -= Time.deltaTime * 3f;
        else
            currentThirst -= Time.deltaTime * 2f;

        movement.steeringBase.maxVelocity = currentMoveSpeed;
        movement.steeringBase.maxAcceleration = currentMoveSpeed;

        age += Time.deltaTime;
        reproductionTimer += Time.deltaTime;
        if (reproductionTimer >= reproductionAge)
            canReproduce = true;
        
        if (currentThirst <= 0 || currentHunger <= 0 || currentHealth <= 0)
            Die();

        currentHealth += Time.deltaTime;
        if (currentHealth >= baseHealth)
            currentHealth = baseHealth;
    }

    void Observe() {
        if (closestCarnivore != null && !sightSensor.targets.Contains(closestCarnivore.gameObject))
            closestCarnivore = null;
        if (closestHerbivore != null && !sightSensor.targets.Contains(closestHerbivore.gameObject))
            closestHerbivore = null;
        if (closestOmnivore != null && !sightSensor.targets.Contains(closestOmnivore.gameObject))
            closestOmnivore = null;
        if (closestHuman != null && !sightSensor.targets.Contains(closestHuman.gameObject))
            closestHuman = null;
        if (closestPlant != null && !sightSensor.targets.Contains(closestPlant.gameObject))
            closestPlant = null;
        if (closestWaterSource != null && !sightSensor.targets.Contains(closestWaterSource.gameObject))
            closestWaterSource = null;
        
        foreach(GameObject target in sightSensor.targets) {
            if (target.GetComponent<Carnivore>()) {
                if (closestCarnivore == null)
                    closestCarnivore = target.GetComponent<Carnivore>();
                else if (Vector3.Distance(target.transform.position, transform.position) < Vector3.Distance(closestCarnivore.transform.position, transform.position))
                    closestCarnivore = target.GetComponent<Carnivore>();
            }
            else if (target.GetComponent<Herbivore>()) {
                if (closestHerbivore == null)
                    closestHerbivore = target.GetComponent<Herbivore>();
                else if (Vector3.Distance(target.transform.position, transform.position) < Vector3.Distance(closestHerbivore.transform.position, transform.position))
                    closestHerbivore = target.GetComponent<Herbivore>();
            }
            else if (target.GetComponent<Omnivore>()) {
                if (closestOmnivore == null)
                    closestOmnivore = target.GetComponent<Omnivore>();
                else if (Vector3.Distance(target.transform.position, transform.position) < Vector3.Distance(closestOmnivore.transform.position, transform.position))
                    closestOmnivore = target.GetComponent<Omnivore>();
            }
            else if (target.GetComponent<Human>()) {
                if (closestHuman == null)
                    closestHuman = target.GetComponent<Human>();
                else if (Vector3.Distance(target.transform.position, transform.position) < Vector3.Distance(closestHuman.transform.position, transform.position))
                    closestHuman = target.GetComponent<Human>();
            }
            else if (target.GetComponent<Plant>()) {
                if (closestPlant == null)
                    closestPlant = target.GetComponent<Plant>();
                else if (Vector3.Distance(target.transform.position, transform.position) < Vector3.Distance(closestPlant.transform.position, transform.position))
                    closestPlant = target.GetComponent<Plant>();
            }
            else if (target.GetComponent<WaterNode>())
                if (closestWaterSource == null)
                    closestWaterSource = target.GetComponent<WaterNode>();
                else if (Vector3.Distance(target.transform.position, transform.position) < Vector3.Distance(closestWaterSource.transform.position, transform.position))
                    closestWaterSource = target.GetComponent<WaterNode>();
        }
    }

    protected virtual void ChoosePriority() {
        List<KeyValuePair<string, float>> sortedDistributions = GetattributeDistributions();
        priorities.Clear();
        foreach(var prio in sortedDistributions)
            priorities.Add(prio.Key);
    }

    protected virtual void ChooseTarget() {}

    protected virtual void ChoosePriorityAction() {}

    protected virtual void InteractWithTarget() {}

    void DoMovement() {

        if (activityState == ActivityState.Sprinting) {
            currentMoveSpeed = baseMoveSpeed * sprintModifier;
            currentStamina -= Time.deltaTime;
        }
        
        if (currentStamina <= 0) {
            rechargingStamina = true;
            activityState = ActivityState.Resting;
            currentStamina = 0;
        }

        if (activityState == ActivityState.Resting) {
            currentMoveSpeed = .0f;
            currentStamina += (5f * Time.deltaTime);
            if (currentStamina >= baseStamina) {
                rechargingStamina = false;
                activityState = ActivityState.Walking;
                currentStamina = baseStamina;
            }
        }

        if (activityState == ActivityState.Walking) {
            currentMoveSpeed = baseMoveSpeed;
            currentStamina += (.5f * Time.deltaTime);
        }
    }

    protected override List<KeyValuePair<string, float>> GetattributeDistributions() {
        List<KeyValuePair<string, float>> distributions = new List<KeyValuePair<string, float>>();
        distributions.Add(new KeyValuePair<string, float>("Health", currentHealth / baseHealth));
        distributions.Add(new KeyValuePair<string, float>("Hunger", currentHunger / baseHunger));
        distributions.Add(new KeyValuePair<string, float>("Thirst", currentThirst / baseThirst));
        //distributions.Add(new KeyValuePair<string, float>("Stamina", currentStamina / baseStamina));
        distributions.Sort((x, y) => (x.Value.CompareTo(y.Value)));
        return distributions;
    }

    protected virtual void Attack(Animal target, float ammount) {
        if (currentStamina >= 10) {
            currentStamina -= 10;
            float nVal = target.nutritionalValue;
            bool died = target.TakeDamage(ammount);
            if (died)
                Eat(nVal);
            StartCoroutine(InteractionCooldown());
        }
    }

    protected virtual bool TakeDamage(float ammount) {
        currentHealth -= ammount - (enduranceMultiplier * ammount);
        if (currentHealth <= 0) {
            Die();
            return true;
        }
        return false;
    }

    protected virtual void Eat(float nutritionalValue, Organism organism) {
        if (currentStamina >= 5) {
            currentHunger += organism.nutritionalValue;
            currentStamina -= 5;
            if (currentHunger >= baseHunger)
                currentHunger = baseHunger;
            organism.Die();
            StartCoroutine(InteractionCooldown());
        }
    }

    protected virtual void Eat(float nutrionValue) {
        currentHunger += nutritionalValue;
        currentStamina -= 5;
        if (currentHunger >= baseHunger)
            currentHunger = baseHunger;
    }

    protected virtual void Drink() {
        if (currentStamina >= 5) {
            currentThirst += 20;
            currentStamina -= 5;
            if (currentThirst >= baseThirst)
                currentThirst = baseThirst;
            StartCoroutine(InteractionCooldown());
        }
    }

    protected IEnumerator InteractionCooldown(float waitValue = 1) {
        canInteract = false;
        activityState = ActivityState.Resting;
        yield return new WaitForSeconds(waitValue);
        canInteract = true;
    }
}
