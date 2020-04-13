using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    private static int iterationDuration, iterationSteps;
    private static bool stopWhenNoHumans;
    private static float aggression, endurance, health, moveSpeed, stamina, strength, hunger, thirst, age;
    public static int IterationDuration {
        get {
            return iterationDuration;
        }
        set {
            iterationDuration = value;
        }
    }
    public static int IterationSteps {
        get {
            return iterationSteps;
        }
        set {
            iterationSteps = value;
        }
    }
    public static bool StopWhenNoHumans {
        get {
            return stopWhenNoHumans;
        }
        set {
            stopWhenNoHumans = value;
        }
    }
    public static float Aggression {
        get {
            return aggression;
        }
        set {
            aggression = value;
        }
    }
    public static float Endurance {
        get {
            return endurance;
        }
        set {
            endurance = value;
        }
    }
    public static float Health {
        get {
            return health;
        }
        set {
            health = value;
        }
    }
    public static float MoveSpeed {
        get {
            return moveSpeed;
        }
        set {
            moveSpeed = value;
        }
    }
    public static float Stamina {
        get {
            return stamina;
        }
        set {
            stamina = value;
        }
    }
    public static float Strength {
        get {
            return strength;
        }
        set {
            strength = value;
        }
    }
    public static float Hunger {
        get {
            return hunger;
        }
        set {
            hunger = value;
        }
    }
    public static float Thirst {
        get {
            return thirst;
        }
        set {
            thirst = value;
        }
    }
    public static float Age {
        get {
            return age;
        }
        set {
            age = value;
        }
    }
}
