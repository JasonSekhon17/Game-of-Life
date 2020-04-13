using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    // simulation states
    public enum SimulationState {
        Initial,
        Running,
        Ended
    }
    
    SimulationState simulationState;

    // simulation settings
    public float simulationDuration;
    public float simulationCurrentTime;
    public int simulationTotalIteration;
    public int simulationCurrentIteration;
    public bool stopWhenNoHumansRemaining;
    public bool auto;

    // organism spawn numbers
    public int startingPlantsCount;
    public int startingCarnivoreCount;
    public int startingHerbivoreCount;
    public int startingOmnivoreCount;
    public int startingHumanCount;

    // prefabs
    public Plant plantPrefab;
    public Carnivore carnivorePrefab;
    public Herbivore herbivorePrefab;
    public Omnivore omnivorePrefab;
    public Human humanPrefab;

    // game components
    public GeneticAlgorithmManager geneticAlgorithmManager;

    // UI components
    public Button startBtn;
    public Button proceedBtn;
    public GameObject timeButtonGrp;
    public Button pauseBtn;
    public Button resumeBtn;
    public Button fastForwardBtn;
    public Button autoBtn;
    public Button endBtn;
    public TextMeshProUGUI simDurationText;
    public TextMeshProUGUI iterationText;

    // Ideal Human UI elements
    public TextMeshProUGUI aggressionText;
    public TextMeshProUGUI enduranceText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI moveSpeedText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI thirstText;
    public TextMeshProUGUI survivalText;

    // Start is called before the first frame update
    void Start()
    {
        simulationDuration = Data.IterationDuration;
        simulationTotalIteration = Data.IterationSteps;
        stopWhenNoHumansRemaining = Data.StopWhenNoHumans;

        simulationState = SimulationState.Initial;
        Time.timeScale = 0;
        simulationCurrentIteration = 1;
        iterationText.text = "Iteration: " + simulationCurrentIteration;
        geneticAlgorithmManager = GetComponent<GeneticAlgorithmManager>();

        startBtn.onClick.AddListener(StartSimulationIteration);
        proceedBtn.onClick.AddListener(InitializeSimulationIteration);
        pauseBtn.onClick.AddListener(PauseGame);
        resumeBtn.onClick.AddListener(ResumeGame);
        fastForwardBtn.onClick.AddListener(FastForwardGame);
        autoBtn.onClick.AddListener(ToggleAuto);
        endBtn.onClick.AddListener(EndSimulation);

        timeButtonGrp.SetActive(false);
        autoBtn.gameObject.SetActive(false);
        endBtn.gameObject.SetActive(false);
        auto = false;

        float max = Mathf.Max(startingPlantsCount, startingOmnivoreCount, startingHumanCount, startingHerbivoreCount, startingCarnivoreCount);
        for(int i = 0; i < max; i++) {
            if (i < startingCarnivoreCount) {
                Instantiate(carnivorePrefab, new Vector3(Random.Range(-19f, 19f), .5f, Random.Range(-21f, 21f)), Quaternion.Euler(new Vector3(0, Random.Range(0f,360f), 0)));
            }
            if (i < startingHerbivoreCount) {
                Instantiate(herbivorePrefab, new Vector3(Random.Range(19f, 19f), .5f, Random.Range(-21f, 21f)), Quaternion.Euler(new Vector3(0, Random.Range(0f,360f), 0)));
            }
            if (i < startingOmnivoreCount) {
                Instantiate(omnivorePrefab, new Vector3(Random.Range(-19f, 19f), .5f, Random.Range(-21f, 21f)), Quaternion.Euler(new Vector3(0, Random.Range(0f,360f), 0)));
            }
            if (i < startingPlantsCount) {
                Instantiate(plantPrefab, new Vector3(Random.Range(-19f, 19f), 0f, Random.Range(-21f, 21f)), Quaternion.Euler(new Vector3(0, Random.Range(0f,360f), 0)));
            }
            if (i < startingHumanCount) {
                Human temp = Instantiate(humanPrefab, new Vector3(Random.Range(-19f, 19f), .5f, Random.Range(-21f, 21f)), Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0)));
                geneticAlgorithmManager.RandomizeHumanDNA(temp);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (simulationCurrentIteration > simulationTotalIteration)
            EndSimulation();
        if (simulationState == SimulationState.Running) {
            simulationCurrentTime += Time.deltaTime;
            simDurationText.text = (int)simulationCurrentTime + "s / " + simulationDuration + "s";
            if (simulationCurrentTime >= simulationDuration || (GameObject.FindObjectsOfType(typeof(Human)).Length <= 0 && stopWhenNoHumansRemaining)) {
                EndSimulationIteration();
            }
        }
    }

    void InitializeSimulationIteration() {
        simulationState = SimulationState.Initial;
        startBtn.gameObject.SetActive(true);
        autoBtn.gameObject.SetActive(false);
        proceedBtn.gameObject.SetActive(false);
        RecreatePopulation();
        simulationCurrentTime = 0;
        endBtn.gameObject.SetActive(true);
        timeButtonGrp.SetActive(false);
        simulationCurrentIteration++;
        iterationText.text = "Iteration: " + simulationCurrentIteration;
        SetIdealHumanText();
        if(auto)
            StartSimulationIteration();
        else
            Time.timeScale = 0f;
    }

    void StartSimulationIteration() {
        if (!auto) {
            ResumeGame();
            timeButtonGrp.SetActive(true);
        }
        simulationState = SimulationState.Running;
        startBtn.gameObject.SetActive(false);
        autoBtn.gameObject.SetActive(true);
    }

    void EndSimulationIteration() {
        foreach(Human human in FindObjectsOfType<Human>()) {
            human.humanDNA.endAge = human.age;
            geneticAlgorithmManager.matingPool.Add(new GeneticAlgorithmManager.HumanDNA(human.humanDNA));
        }

        simulationState = SimulationState.Ended;
        proceedBtn.gameObject.SetActive(true);
        autoBtn.gameObject.SetActive(false);
        timeButtonGrp.SetActive(false);
        if (auto)
            InitializeSimulationIteration();
        else
            Time.timeScale = 0f;
    }

    void RecreatePopulation() {
        foreach(Organism organism in FindObjectsOfType<Organism>()) {
            Destroy(organism.gameObject);
        }
        geneticAlgorithmManager.SelectHumans();
        float max = Mathf.Max(startingPlantsCount, startingOmnivoreCount, startingHumanCount, startingHerbivoreCount, startingCarnivoreCount);
        for(int i = 0; i < max; i++) {
            if (i < startingCarnivoreCount) {
                Instantiate(carnivorePrefab, new Vector3(Random.Range(-19f, 19f), .5f, Random.Range(-21f, 21f)), Quaternion.Euler(new Vector3(0, Random.Range(0f,360f), 0)));
            }
            if (i < startingHerbivoreCount) {
                Instantiate(herbivorePrefab, new Vector3(Random.Range(-19f, 19f), .5f, Random.Range(-21f, 21f)), Quaternion.Euler(new Vector3(0, Random.Range(0f,360f), 0)));
            }
            if (i < startingOmnivoreCount) {
                Instantiate(omnivorePrefab, new Vector3(Random.Range(-19f, 19f), .5f, Random.Range(-21f, 21f)), Quaternion.Euler(new Vector3(0, Random.Range(0f,360f), 0)));
            }
            if (i < startingPlantsCount) {
                Instantiate(plantPrefab, new Vector3(Random.Range(-19f, 19f), 0f, Random.Range(-21f, 21f)), Quaternion.Euler(new Vector3(0, Random.Range(0f,360f), 0)));
            }
            if (i < startingHumanCount) {
                if (Random.Range(0f, 1f) > .25)
                    geneticAlgorithmManager.BuildHumanDNAFromMatingPool(humanPrefab);
                else
                    geneticAlgorithmManager.RandomizeHumanDNA(humanPrefab);
                Instantiate(humanPrefab, new Vector3(Random.Range(5f, 19f), .5f, Random.Range(6f, 21f)), Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0)));
            }
        }
        geneticAlgorithmManager.matingPool.Clear();
    }

    void PauseGame() {
        Time.timeScale = 0.0f;
        pauseBtn.GetComponent<Image>().color = new Color(1, 1, 1, .75f);
        resumeBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        fastForwardBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    void ResumeGame() {
        Time.timeScale = 1.0f;
        pauseBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        resumeBtn.GetComponent<Image>().color = new Color(1, 1, 1, .75f);
        fastForwardBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    void FastForwardGame() {
        Time.timeScale = 4.0f;
        pauseBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        resumeBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        fastForwardBtn.GetComponent<Image>().color = new Color(1, 1, 1, .75f);
    }

    void ToggleAuto() {
        auto = !auto;
        if (auto) {
            FastForwardGame();
            autoBtn.GetComponent<Image>().color = new Color(0, 1, 0, 1);
        } else {
            ResumeGame();
            autoBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        timeButtonGrp.SetActive(!auto);
    }

    void SetIdealHumanText() {
        aggressionText.text = "Aggression:\t" + geneticAlgorithmManager.idealHuman.aggression.ToString("F2");
        enduranceText.text = "Endurance:\t\t" + geneticAlgorithmManager.idealHuman.endurance.ToString("F2");
        healthText.text = "Health:\t\t" + geneticAlgorithmManager.idealHuman.health.ToString("F2");
        moveSpeedText.text = "Move Speed:\t" + geneticAlgorithmManager.idealHuman.moveSpeed.ToString("F2");
        staminaText.text = "Stamina:\t\t" + geneticAlgorithmManager.idealHuman.stamina.ToString("F2");
        strengthText.text = "Strength:\t\t" + geneticAlgorithmManager.idealHuman.strength.ToString("F2");
        hungerText.text = "Hunger:\t\t" + geneticAlgorithmManager.idealHuman.hunger.ToString("F2");
        thirstText.text = "Thirst:\t\t" + geneticAlgorithmManager.idealHuman.thirst.ToString("F2");
        survivalText.text = "Survival Time:\t" + geneticAlgorithmManager.idealHuman.age.ToString("F2");
    }

    void EndSimulation() {
        Data.Aggression = geneticAlgorithmManager.idealHuman.aggression;
        Data.Endurance = geneticAlgorithmManager.idealHuman.endurance;
        Data.Health = geneticAlgorithmManager.idealHuman.health;
        Data.MoveSpeed = geneticAlgorithmManager.idealHuman.moveSpeed;
        Data.Stamina = geneticAlgorithmManager.idealHuman.stamina;
        Data.Strength = geneticAlgorithmManager.idealHuman.strength;
        Data.Hunger = geneticAlgorithmManager.idealHuman.hunger;
        Data.Thirst = geneticAlgorithmManager.idealHuman.thirst;
        Data.Age = geneticAlgorithmManager.idealHuman.age;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
