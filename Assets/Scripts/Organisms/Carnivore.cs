using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : Animal
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
            if (priorities[index] == "Health" && (closestHuman != null || closestOmnivore != null)) {
                currentPriority = "Health";
                break;
            } else if (priorities[index] == "Hunger" && (closestHerbivore != null || closestHuman != null || closestOmnivore != null) && (currentHunger / baseHunger) < .5) {
                currentPriority = "Hunger";
                break;
            } else if (priorities[index] == "Thirst" && closestWaterSource != null && (currentThirst / baseThirst) < .5) {
                currentPriority = "Thirst";
                break;
            } else if (closestCarnivore != null && canReproduce && closestCarnivore.canReproduce && (currentThirst / baseThirst > .5) && (currentHunger / baseHunger > .5)) {
                currentPriority = "Reproduce";
                break;
            }
        }
    }

    protected override void ChooseTarget()
    {
        base.ChooseTarget();
        if (currentPriority == "Health") {
            if (closestHuman != null && closestOmnivore != null) {
                Transform temp = (Vector3.Distance(closestHuman.transform.position, transform.position) < Vector3.Distance(closestOmnivore.transform.position, transform.position)) ? closestHuman.transform : closestOmnivore.transform;
                movement.fleeTarget = temp;
                healthTarget = temp;
            } else if (closestHuman != null) {
                movement.fleeTarget = closestHuman.transform;
                healthTarget = closestHuman.transform;
            } else {
                movement.fleeTarget = closestOmnivore.transform;
                healthTarget = closestOmnivore.transform;
            }
        } else if (currentPriority == "Hunger") {
            if (closestHerbivore != null) {
                movement.pursueTarget = closestHerbivore.movement.rb;
                hungerTarget = closestHerbivore.movement.transform;
            } else if (closestHuman != null && closestOmnivore != null) {
                movement.pursueTarget = (Vector3.Distance(closestHuman.transform.position, transform.position) < Vector3.Distance(closestOmnivore.transform.position, transform.position)) ? closestHuman.movement.rb : closestOmnivore.movement.rb;
                hungerTarget = movement.pursueTarget.transform;
            } else if (closestHuman != null) {
                movement.pursueTarget = closestHuman.movement.rb;
                hungerTarget = movement.pursueTarget.transform;
            } else {
                movement.pursueTarget = closestOmnivore.movement.rb;
                hungerTarget = movement.pursueTarget.transform;
            }
        } else if (currentPriority == "Thirst") {
            movement.seekTarget = closestWaterSource.transform;
            thirstTarget = closestWaterSource.transform;
        } else if (currentPriority == "Reproduce") {
            movement.seekTarget = closestCarnivore.transform;
            reproductionTarget = closestCarnivore;
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
        if (currentPriority == "Hunger" && Vector3.Distance(hungerTarget.position, transform.position) < 3 && (Random.Range(0f, 1f) <= aggressionMultiplier)) {
            Attack(movement.pursueTarget.GetComponent<Animal>(), strength);
        } else if (currentPriority == "Thirst" && Vector3.Distance(thirstTarget.position, transform.position) < 3) {
            Drink();
        } else if (currentPriority == "Reproduce" && Vector3.Distance(reproductionTarget.transform.position, transform.position) < 2) {
            Reproduce();
        }
    }
}
