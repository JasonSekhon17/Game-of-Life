using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Animal
{

    public Game game;
    public GeneticAlgorithmManager.HumanDNA humanDNA;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        game = FindObjectOfType<Game>();
        humanDNA = new GeneticAlgorithmManager.HumanDNA(aggressionMultiplier, enduranceMultiplier,
                                                                                        healthMultiplier, hungerMultiplier, moveSpeedMultiplier,
                                                                                        staminaMultiplier, strengthMultiplier, thirstMultiplier, 0);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void ChoosePriority()
    {
        base.ChoosePriority();
        currentPriority = "";
        for(int index = 0; index < priorities.Count; index++) {
            if (priorities[index] == "Health" && (closestCarnivore != null || closestOmnivore != null)) {
                currentPriority = "Health";
                break;
            } else if (priorities[index] == "Hunger" && (closestPlant != null || closestHerbivore != null || closestCarnivore != null || closestOmnivore != null)
                        && (currentHunger / baseHunger) < .5) {
                currentPriority = "Hunger";
                break;
            } else if (priorities[index] == "Thirst" && closestWaterSource != null && (currentThirst / baseThirst) < .5) {
                currentPriority = "Thirst";
                break;
            } else if (closestHuman != null && canReproduce && closestHuman.canReproduce && (currentThirst / baseThirst > .5) && (currentHunger / baseHunger > .5)) {
                currentPriority = "Reproduce";
                break;
            }
        }
    }

    protected override void ChooseTarget()
    {
        base.ChooseTarget();
        if (currentPriority == "Health") {
            if (closestCarnivore != null && closestOmnivore != null) {
                Transform temp = (Vector3.Distance(closestCarnivore.transform.position, transform.position) < 
                                    Vector3.Distance(closestOmnivore.transform.position, transform.position)) ? 
                                    closestCarnivore.transform : closestOmnivore.transform;
                movement.fleeTarget = temp;
                healthTarget = temp;
            } else if (closestCarnivore != null) {
                movement.fleeTarget = closestCarnivore.transform;
                healthTarget = closestCarnivore.transform;
            } else {
                movement.fleeTarget = closestOmnivore.transform;
                healthTarget = closestOmnivore.transform;
            }
        } else if (currentPriority == "Hunger") {
            if (closestPlant != null && closestHerbivore != null) {
                bool plantIsCloser = Vector3.Distance(closestPlant.transform.position, transform.position) < 
                                        Vector3.Distance(closestHerbivore.transform.position, transform.position);
                if (plantIsCloser) {
                    movement.seekTarget = closestPlant.transform;
                    hungerTarget = closestPlant.transform;
                } else {
                    movement.pursueTarget = closestHerbivore.movement.rb;
                    hungerTarget = closestHerbivore.transform;
                }
            } else if (closestPlant != null) {
                movement.seekTarget = closestPlant.transform;
                hungerTarget = closestPlant.transform;
            } else if (closestHerbivore != null) {
                movement.pursueTarget = closestHerbivore.movement.rb;
                hungerTarget = closestHerbivore.transform;
            } else if (closestCarnivore != null && closestOmnivore != null) {
                movement.pursueTarget = (Vector3.Distance(closestCarnivore.transform.position, transform.position) < 
                                            Vector3.Distance(closestOmnivore.transform.position, transform.position)) ? 
                                            closestCarnivore.movement.rb : closestOmnivore.movement.rb;
                hungerTarget = movement.pursueTarget.transform;
            } else if (closestCarnivore != null) {
                movement.pursueTarget = closestCarnivore.movement.rb;
                hungerTarget = closestCarnivore.transform;
            } else {
                movement.pursueTarget = closestOmnivore.movement.rb;
                hungerTarget = closestOmnivore.transform;
            }
        } else if (currentPriority == "Thirst") {
            movement.seekTarget = closestWaterSource.transform;
            thirstTarget = closestWaterSource.transform;
        } else if (currentPriority == "Reproduce") {
            movement.seekTarget = closestHuman.transform;
            reproductionTarget = closestHuman;
        }
    }

    protected override void ChoosePriorityAction() {
        base.ChoosePriorityAction();
        if (currentPriority == "Health") {
            if (canInteract && !rechargingStamina)
                activityState = ActivityState.Sprinting;
            movement.movementState = AIMovement.MovementState.Flee;
        } else if (currentPriority == "Hunger") {
            if (canInteract && !rechargingStamina)
                activityState = ActivityState.Sprinting;
            if (hungerTarget.GetComponent<Plant>())
                movement.movementState = AIMovement.MovementState.Seek;
            else
                movement.movementState = AIMovement.MovementState.Pursue;
        } else if (currentPriority == "Thirst") {
            if (canInteract && !rechargingStamina)
                activityState = ActivityState.Walking;
            movement.movementState = AIMovement.MovementState.Seek;
        } else if (currentPriority == "Reproduce") {
            if (canInteract && !rechargingStamina)
                activityState = ActivityState.Walking;
            movement.movementState = AIMovement.MovementState.Seek;
        } else {
            if (canInteract && !rechargingStamina)
                activityState = ActivityState.Walking;
            else if (currentStamina < baseStamina)
                activityState = ActivityState.Resting;
            movement.movementState = AIMovement.MovementState.Wander;
        }
    }

    protected override void InteractWithTarget() {
        base.InteractWithTarget();
        if (currentPriority == "Hunger" && Vector3.Distance(hungerTarget.position, transform.position) < 2) {
            if (hungerTarget.GetComponent<Plant>() != null)
                Eat(hungerTarget.GetComponent<Plant>().nutritionalValue, hungerTarget.GetComponent<Plant>());
            else if (Random.Range(0f, 1f) <= aggressionMultiplier)
                Attack(movement.pursueTarget.GetComponent<Animal>(), strength);
        } else if (currentPriority == "Thirst" && Vector3.Distance(thirstTarget.position, transform.position) < 3) {
            Drink();
        } else if (currentPriority == "Reproduce" && Vector3.Distance(reproductionTarget.transform.position, transform.position) < 2) {
            Reproduce();
        }
    }

    public override void Die() {
        humanDNA.endAge = age;
        game.geneticAlgorithmManager.matingPool.Add(new GeneticAlgorithmManager.HumanDNA(humanDNA));
        base.Die();
    }
}
