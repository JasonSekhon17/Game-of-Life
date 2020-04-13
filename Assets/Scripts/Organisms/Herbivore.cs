using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : Animal
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
            if (priorities[index] == "Health" && (closestCarnivore != null || closestHuman != null || closestOmnivore != null)) {
                currentPriority = "Health";
                break;
            } else if (priorities[index] == "Hunger" && closestPlant != null && (currentHunger / baseHunger) < .5) {
                currentPriority = "Hunger";
                break;
            } else if (priorities[index] == "Thirst" && closestWaterSource != null && (currentThirst / baseThirst) < .5) {
                currentPriority = "Thirst";
                break;
            } else if (closestHerbivore != null && canReproduce && closestHerbivore.canReproduce && (currentThirst / baseThirst > .5) && (currentHunger / baseHunger > .5)) {
                currentPriority = "Reproduce";
                break;
            }
        }
    }

    protected override void ChooseTarget()
    {
        base.ChooseTarget();
        float humanDist = 0;
        float omniDist = 0;
        float carnDist = 0;
        if (closestHuman != null)
            humanDist = Vector3.Distance(closestHuman.transform.position, transform.position);
        if (closestOmnivore != null)
            omniDist = Vector3.Distance(closestOmnivore.transform.position, transform.position);
        if (closestCarnivore != null)
            carnDist = Vector3.Distance(closestCarnivore.transform.position, transform.position);
        if (currentPriority == "Health") {
            if (closestHuman != null && closestOmnivore != null && closestCarnivore != null) {
                if (carnDist < humanDist && carnDist < omniDist) {
                    movement.fleeTarget = closestCarnivore.transform;
                    healthTarget = closestCarnivore.transform;
                } else if (humanDist < omniDist) {
                    movement.fleeTarget = closestHuman.transform;
                    healthTarget = closestHuman.transform;
                } else {
                    movement.fleeTarget = closestOmnivore.transform;
                    healthTarget = closestOmnivore.transform;
                }
            } else if (closestHuman != null && closestOmnivore != null) {
                if (humanDist < omniDist) {
                    movement.fleeTarget = closestHuman.transform;
                    healthTarget = closestHuman.transform;
                } else {
                    movement.fleeTarget = closestOmnivore.transform;
                    healthTarget = closestOmnivore.transform;
                }
            } else if (closestHuman != null && closestCarnivore != null) {
                if (humanDist < carnDist) {
                    movement.fleeTarget = closestHuman.transform;
                    healthTarget = closestHuman.transform;
                } else {
                    movement.fleeTarget = closestCarnivore.transform;
                    healthTarget = closestCarnivore.transform;
                }
            } else if (closestCarnivore != null && closestOmnivore != null) {
                if (carnDist < omniDist) {
                    movement.fleeTarget = closestCarnivore.transform;
                    healthTarget = closestCarnivore.transform;
                } else {
                    movement.fleeTarget = closestOmnivore.transform;
                    healthTarget = closestOmnivore.transform;
                }
            } else if (closestHuman != null) {
                movement.fleeTarget = closestHuman.transform;
                    healthTarget = closestHuman.transform;
            } else if (closestCarnivore != null) {
                movement.fleeTarget = closestCarnivore.transform;
                    healthTarget = closestCarnivore.transform;
            } else{
                movement.fleeTarget = closestOmnivore.transform;
                    healthTarget = closestOmnivore.transform;
            }
        } else if (currentPriority == "Hunger") {
            if (closestPlant != null) {
                movement.seekTarget = closestPlant.transform;
                hungerTarget = closestPlant.transform;
            }
        } else if (currentPriority == "Thirst") {
            movement.seekTarget = closestWaterSource.transform;
            thirstTarget = closestWaterSource.transform;
        } else if (currentPriority == "Reproduce") {
            movement.seekTarget = closestHerbivore.transform;
            reproductionTarget = closestHerbivore;
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
                activityState = ActivityState.Walking;
            movement.movementState = AIMovement.MovementState.Seek;
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
            Eat(closestPlant.nutritionalValue, closestPlant);
        } else if (currentPriority == "Thirst" && Vector3.Distance(thirstTarget.position, transform.position) < 3) {
            Drink();
        } else if (currentPriority == "Reproduce" && Vector3.Distance(reproductionTarget.transform.position, transform.position) < 2) {
            Reproduce();
        }
    }
}
