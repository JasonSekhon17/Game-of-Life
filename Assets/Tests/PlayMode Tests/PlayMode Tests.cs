using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

namespace Tests
{
    public class PlayModeTests
    {
        [UnitySetUp]
        public IEnumerator LoadMainMenuScene() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForSeconds(.01f);
        }

        IEnumerator ProceedToSimulationWithSettings(int duration, int iterations, bool stopOnNohumansToggle) {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Slider durationSlider = settingsMenu.transform.Find("DurationSlider").GetComponent<Slider>();
            Slider iterationsSlider = settingsMenu.transform.Find("IterationsSlider").GetComponent<Slider>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            durationSlider.value = duration;
            yield return new WaitForFixedUpdate();
            iterationsSlider.value = iterations;
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = stopOnNohumansToggle;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);
        }

        [UnityTest]
        public IEnumerator CheckInitialMainMenuState() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;

            Assert.NotNull(mainMenu);
            Assert.NotNull(settingsMenu);

            Assert.IsTrue(mainMenu.activeSelf);
            Assert.IsFalse(settingsMenu.activeSelf);

            Assert.AreEqual(3, mainMenu.GetComponentsInChildren<Button>().Length);
            Assert.AreEqual(1, settingsMenu.GetComponentsInChildren<Button>().Length);
            Assert.AreEqual(2, settingsMenu.GetComponentsInChildren<Slider>().Length);
            Assert.AreEqual(1, settingsMenu.GetComponentsInChildren<Toggle>().Length);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckSettingsMenuTransition() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;

            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();

            Assert.NotNull(settingsButton);
            settingsButton.onClick.Invoke();

            Assert.IsFalse(mainMenu.activeSelf);
            Assert.IsTrue(settingsMenu.activeSelf);

            Assert.NotNull(backButton);
            backButton.onClick.Invoke();

            Assert.IsTrue(mainMenu.activeSelf);
            Assert.IsFalse(settingsMenu.activeSelf);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckSettingsMenuDurationSlider() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();

            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            
            //check default value
            Assert.AreEqual("60s", settingsMenu.transform.Find("DurationValue").GetComponent<TextMeshProUGUI>().text);

            //check values within range
            settingsMenu.transform.Find("DurationSlider").GetComponent<Slider>().value = 70;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual("70s", settingsMenu.transform.Find("DurationValue").GetComponent<TextMeshProUGUI>().text);

            settingsMenu.transform.Find("DurationSlider").GetComponent<Slider>().value = 20;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual("20s", settingsMenu.transform.Find("DurationValue").GetComponent<TextMeshProUGUI>().text);

            settingsMenu.transform.Find("DurationSlider").GetComponent<Slider>().value = 50;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual("50s", settingsMenu.transform.Find("DurationValue").GetComponent<TextMeshProUGUI>().text);

            //check values out of range
            settingsMenu.transform.Find("DurationSlider").GetComponent<Slider>().value = 19;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual("20s", settingsMenu.transform.Find("DurationValue").GetComponent<TextMeshProUGUI>().text);

            settingsMenu.transform.Find("DurationSlider").GetComponent<Slider>().value = 101;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual("100s", settingsMenu.transform.Find("DurationValue").GetComponent<TextMeshProUGUI>().text);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckSettingsMenuIterationsSlider() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();

            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            
            //check default value
            Assert.AreEqual("15", settingsMenu.transform.Find("IterationsValue").GetComponent<TextMeshProUGUI>().text);

            //check values within range
            settingsMenu.transform.Find("IterationsSlider").GetComponent<Slider>().value = 70;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual("70", settingsMenu.transform.Find("IterationsValue").GetComponent<TextMeshProUGUI>().text);

            settingsMenu.transform.Find("IterationsSlider").GetComponent<Slider>().value = 20;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual("20", settingsMenu.transform.Find("IterationsValue").GetComponent<TextMeshProUGUI>().text);

            settingsMenu.transform.Find("IterationsSlider").GetComponent<Slider>().value = 50;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual("50", settingsMenu.transform.Find("IterationsValue").GetComponent<TextMeshProUGUI>().text);

            //check values out of range
            settingsMenu.transform.Find("IterationsSlider").GetComponent<Slider>().value = 4;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual("5", settingsMenu.transform.Find("IterationsValue").GetComponent<TextMeshProUGUI>().text);

            settingsMenu.transform.Find("IterationsSlider").GetComponent<Slider>().value = 101;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual("100", settingsMenu.transform.Find("IterationsValue").GetComponent<TextMeshProUGUI>().text);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckSettingsMenuStopWhenNoHumansToggle() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;

            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            
            //default value
            Assert.True(humanToggle.isOn);
            
            //change value
            humanToggle.isOn = !humanToggle.isOn;
            yield return new WaitForFixedUpdate();
            Assert.False(humanToggle.isOn);

            humanToggle.isOn = !humanToggle.isOn;
            yield return new WaitForFixedUpdate();
            Assert.True(humanToggle.isOn);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckStartButtonTransition() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();

            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            Assert.AreEqual("Simulation", SceneManager.GetActiveScene().name);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckInitialDataTransferDefaults() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            Assert.AreEqual("Simulation", SceneManager.GetActiveScene().name);

            Assert.AreEqual(60, Data.IterationDuration);
            Assert.AreEqual(15, Data.IterationSteps);
            Assert.True(Data.StopWhenNoHumans);

            yield return null;
        }

        IEnumerator DataTransferHelper(int duration, int iterations, bool humans) {
            ProceedToSimulationWithSettings(duration, iterations, humans);

            //Check Data
            Assert.AreEqual(duration, Data.IterationDuration);
            Assert.AreEqual(iterations, Data.IterationSteps);
            Assert.AreEqual(humans, Data.StopWhenNoHumans);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckInitialDataTransferCustom_1() {
            DataTransferHelper(80, 70, false);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckInitialDataTransferCustom_2() {
            DataTransferHelper(30, 30, true);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckInitialDataTransferCustom_3() {
            DataTransferHelper(20, 50, false);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckInitialSimulationUI() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            Assert.AreEqual("Simulation", SceneManager.GetActiveScene().name);

            GameObject canvas = GameObject.Find("Canvas");
            Assert.NotNull(canvas);

            TextMeshProUGUI currentIterationText = canvas.transform.Find("CurrentIteration").GetComponent<TextMeshProUGUI>();
            Assert.NotNull(currentIterationText);
            Assert.True(currentIterationText.gameObject.activeSelf);
            Assert.AreEqual("Iteration: 1", currentIterationText.text);

            TextMeshProUGUI durationText = canvas.transform.Find("SimulationDuration").GetComponent<TextMeshProUGUI>();
            Assert.NotNull(durationText);
            Assert.True(durationText.gameObject.activeSelf);

            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            Assert.NotNull(startSimButton);
            Assert.True(startSimButton.gameObject.activeSelf);

            Button proceedButton = canvas.transform.Find("ProceedBtn").GetComponent<Button>();
            Assert.NotNull(proceedButton);
            Assert.False(proceedButton.gameObject.activeSelf);

            Button endButton = canvas.transform.Find("EndBtn").GetComponent<Button>();
            Assert.NotNull(endButton);
            Assert.False(endButton.gameObject.activeSelf);

            Button autoButton = canvas.transform.Find("AutoBtn").GetComponent<Button>();
            Assert.NotNull(autoButton);
            Assert.False(autoButton.gameObject.activeSelf);

            GameObject timeButtonGroup = canvas.transform.Find("TimeBtnGroup").gameObject;
            Assert.NotNull(timeButtonGroup);
            Assert.False(timeButtonGroup.activeSelf);

            Button pauseButton = timeButtonGroup.transform.Find("PauseBtn").GetComponent<Button>();
            Assert.NotNull(pauseButton);
            Assert.True(pauseButton.gameObject.activeSelf);

            Button resumeButton = timeButtonGroup.transform.Find("ResumeBtn").GetComponent<Button>();
            Assert.NotNull(resumeButton);
            Assert.True(resumeButton.gameObject.activeSelf);

            Button FastForwardButton = timeButtonGroup.transform.Find("FastForwardBtn").GetComponent<Button>();
            Assert.NotNull(FastForwardButton);
            Assert.True(FastForwardButton.gameObject.activeSelf);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckUIAfterStartSimulation() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();

            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            TextMeshProUGUI currentIterationText = canvas.transform.Find("CurrentIteration").GetComponent<TextMeshProUGUI>();
            Assert.NotNull(currentIterationText);
            Assert.True(currentIterationText.gameObject.activeSelf);
            Assert.AreEqual("Iteration: 1", currentIterationText.text);

            TextMeshProUGUI durationText = canvas.transform.Find("SimulationDuration").GetComponent<TextMeshProUGUI>();
            Assert.NotNull(durationText);
            Assert.True(durationText.gameObject.activeSelf);

            Button proceedButton = canvas.transform.Find("ProceedBtn").GetComponent<Button>();
            Assert.NotNull(proceedButton);
            Assert.False(proceedButton.gameObject.activeSelf);

            Button endButton = canvas.transform.Find("EndBtn").GetComponent<Button>();
            Assert.NotNull(endButton);
            Assert.False(endButton.gameObject.activeSelf);

            Button autoButton = canvas.transform.Find("AutoBtn").GetComponent<Button>();
            Assert.NotNull(autoButton);
            Assert.True(autoButton.gameObject.activeSelf);

            GameObject timeButtonGroup = canvas.transform.Find("TimeBtnGroup").gameObject;
            Assert.NotNull(timeButtonGroup);
            Assert.True(timeButtonGroup.activeSelf);

            Button pauseButton = timeButtonGroup.transform.Find("PauseBtn").GetComponent<Button>();
            Assert.NotNull(pauseButton);
            Assert.True(pauseButton.gameObject.activeSelf);

            Button resumeButton = timeButtonGroup.transform.Find("ResumeBtn").GetComponent<Button>();
            Assert.NotNull(resumeButton);
            Assert.True(resumeButton.gameObject.activeSelf);

            Button FastForwardButton = timeButtonGroup.transform.Find("FastForwardBtn").GetComponent<Button>();
            Assert.NotNull(FastForwardButton);
            Assert.True(FastForwardButton.gameObject.activeSelf);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckWorldSpawn() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();

            Assert.AreEqual(4, GameObject.Find("Water").GetComponentsInChildren<Transform>().Length - 1);
            Assert.AreEqual(15, GameObject.Find("Rocks").GetComponentsInChildren<Transform>().Length - 1);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckNumberOfHumansSpawned() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();

            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            Assert.AreEqual(10, GameObject.FindObjectsOfType<Human>().Length);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckNumberOfCarnivoresSpawned() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();

            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            Assert.AreEqual(10, GameObject.FindObjectsOfType<Carnivore>().Length);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckNumberOfHerbivoresSpawned() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();

            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            Assert.AreEqual(10, GameObject.FindObjectsOfType<Herbivore>().Length);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckNumberOfOmnivoresSpawned() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();

            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            Assert.AreEqual(10, GameObject.FindObjectsOfType<Omnivore>().Length);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckNumberOfPlantsSpawned() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();

            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            Assert.AreEqual(30, GameObject.FindObjectsOfType<Plant>().Length);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckIfHumansAreInBounds() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            foreach (Human human in GameObject.FindObjectsOfType<Human>()) {
                Assert.Greater(22, human.transform.position.x);
                Assert.Greater(25, human.transform.position.z);
                Assert.Less(-22, human.transform.position.x);
                Assert.Less(-25, human.transform.position.z);
            }
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckIfCarnivoresAreInBounds() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            foreach (Carnivore carnivore in GameObject.FindObjectsOfType<Carnivore>()) {
                Assert.Greater(22, carnivore.transform.position.x);
                Assert.Greater(25, carnivore.transform.position.z);
                Assert.Less(-22, carnivore.transform.position.x);
                Assert.Less(-25, carnivore.transform.position.z);
            }
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckIfHerbivoresAreInBounds() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            foreach (Herbivore herbivore in GameObject.FindObjectsOfType<Herbivore>()) {
                Assert.Greater(22, herbivore.transform.position.x);
                Assert.Greater(25, herbivore.transform.position.z);
                Assert.Less(-22, herbivore.transform.position.x);
                Assert.Less(-25, herbivore.transform.position.z);
            }
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckIfOmnivoreAreInBounds() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            foreach (Omnivore omnivore in GameObject.FindObjectsOfType<Omnivore>()) {
                Assert.Greater(22, omnivore.transform.position.x);
                Assert.Greater(25, omnivore.transform.position.z);
                Assert.Less(-22, omnivore.transform.position.x);
                Assert.Less(-25, omnivore.transform.position.z);
            }
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckIfPlantsAreInBounds() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            foreach (Plant plant in GameObject.FindObjectsOfType<Plant>()) {
                Assert.Greater(22, plant.transform.position.x);
                Assert.Greater(25, plant.transform.position.z);
                Assert.Less(-22, plant.transform.position.x);
                Assert.Less(-25, plant.transform.position.z);
            }
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckIfRocksAreInBounds() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            foreach (Transform rock in GameObject.Find("Rocks").GetComponentInChildren<Transform>()) {
                Assert.Greater(22, rock.transform.position.x);
                Assert.Greater(25, rock.transform.position.z);
                Assert.Less(-22, rock.transform.position.x);
                Assert.Less(-25, rock.transform.position.z);
            }
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckIfWaterAreInBounds() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            foreach (Transform water in GameObject.Find("Water").GetComponentInChildren<Transform>()) {
                Assert.Greater(22, water.transform.position.x);
                Assert.Greater(25, water.transform.position.z);
                Assert.Less(-22, water.transform.position.x);
                Assert.Less(-25, water.transform.position.z);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckTimeGroupButtons() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            Assert.AreEqual(1, Time.timeScale);

            GameObject timeButtonGroup = canvas.transform.Find("TimeBtnGroup").gameObject;
            Button pauseButton = timeButtonGroup.transform.Find("PauseBtn").GetComponent<Button>();
            Button resumeButton = timeButtonGroup.transform.Find("ResumeBtn").GetComponent<Button>();
            Button fastForwardButton = timeButtonGroup.transform.Find("FastForwardBtn").GetComponent<Button>();

            pauseButton.onClick.Invoke();
            Assert.AreEqual(0, Time.timeScale);
            yield return new WaitForSecondsRealtime(.1f);

            resumeButton.onClick.Invoke();
            Assert.AreEqual(1, Time.timeScale);
            yield return new WaitForSecondsRealtime(.1f);

            fastForwardButton.onClick.Invoke();
            Assert.AreEqual(4, Time.timeScale);
            yield return new WaitForSecondsRealtime(.1f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckAutoButton() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Slider durationSlider = settingsMenu.transform.Find("DurationSlider").GetComponent<Slider>();
            Slider iterationsSlider = settingsMenu.transform.Find("IterationsSlider").GetComponent<Slider>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            durationSlider.value = 0;
            yield return new WaitForFixedUpdate();
            iterationsSlider.value = 0;
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = true;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            TextMeshProUGUI durationText = canvas.transform.Find("SimulationDuration").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI currentIterationText = canvas.transform.Find("CurrentIteration").GetComponent<TextMeshProUGUI>();
            GameObject timeButtonGroup = canvas.transform.Find("TimeBtnGroup").gameObject;
            Button autoButton = canvas.transform.Find("AutoBtn").GetComponent<Button>();
            autoButton.onClick.Invoke();

            Assert.AreEqual(4, Time.timeScale);
            Assert.False(timeButtonGroup.activeSelf);

            yield return new WaitForSeconds(20);
            Assert.AreEqual("Iteration: 2", currentIterationText.text);
            Assert.AreEqual(4, Time.timeScale);
            Assert.False(timeButtonGroup.activeSelf);
            yield return new WaitForSeconds(5);
            Assert.AreNotEqual("0s / 20s", durationText.text);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckHumanWander() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>())
                GameObject.Destroy(organism.gameObject);
                
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Human"), new Vector3(0, 0, 0), Quaternion.identity);
            yield return new WaitForSecondsRealtime(.1f);
            Human human = temp.GetComponent<Human>();
            Vector3 initialPos = human.transform.position;
            yield return new WaitForSecondsRealtime(1f);
            Assert.AreEqual("", human.currentPriority);
            Assert.AreNotSame(initialPos, human.transform.position);

            GameObject.Destroy(human.gameObject);
            yield return new WaitForSecondsRealtime(.1f);
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckCarnivoreWander() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>())
                GameObject.Destroy(organism.gameObject);

            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Carnivore"), new Vector3(0, 0, 0), Quaternion.identity);
            yield return new WaitForSecondsRealtime(.1f);
            Carnivore carnivore = temp.GetComponent<Carnivore>();
            Vector3 initialPos = carnivore.transform.position;
            yield return new WaitForSecondsRealtime(1f);
            Assert.AreEqual("", carnivore.currentPriority);
            Assert.AreNotSame(initialPos, carnivore.transform.position);
            
            GameObject.Destroy(carnivore.gameObject);
            yield return new WaitForSecondsRealtime(.1f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckHerbivoreWander() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>())
                GameObject.Destroy(organism.gameObject);

            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(0, 0, 0), Quaternion.identity);
            yield return new WaitForSecondsRealtime(.1f);
            Herbivore herbivore = temp.GetComponent<Herbivore>();
            Vector3 initialPos = herbivore.transform.position;
            yield return new WaitForSecondsRealtime(1f);
            Assert.AreEqual("", herbivore.currentPriority);
            Assert.AreNotSame(initialPos, herbivore.transform.position);
            
            GameObject.Destroy(herbivore.gameObject);
            yield return new WaitForSecondsRealtime(.1f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckOmnivoreWander() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>())
                GameObject.Destroy(organism.gameObject);

            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Omnivore"), new Vector3(0, 0, 0), Quaternion.identity);
            yield return new WaitForSecondsRealtime(.1f);
            Omnivore omnivore = temp.GetComponent<Omnivore>();
            Vector3 initialPos = omnivore.transform.position;
            yield return new WaitForSecondsRealtime(1f);
            Assert.AreEqual("", omnivore.currentPriority);
            Assert.AreNotSame(initialPos, omnivore.transform.position);
            
            GameObject.Destroy(omnivore.gameObject);
            yield return new WaitForSecondsRealtime(.1f);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckHumanSeekHerbivoreFlee() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }

            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Human"), new Vector3(0, 0, 0), Quaternion.identity);
            Human human = temp.GetComponent<Human>();
        
            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(5, 0, 0), Quaternion.identity);
            Herbivore herbivore = temp.GetComponent<Herbivore>();
            Vector3 initialHumanPos = human.transform.position;
            Vector3 initialHerbivorePos = herbivore.transform.position;

            yield return new WaitForSecondsRealtime(.01f);
            human.currentHunger = human.baseHunger / 3;

            yield return new WaitForSeconds(2f);
            Assert.AreEqual("Hunger", human.currentPriority);
            Assert.AreNotSame(initialHumanPos, human.transform.position);
            Assert.Greater(Vector3.Distance(initialHerbivorePos, initialHumanPos), Vector3.Distance(initialHerbivorePos, human.transform.position));
            Assert.AreEqual("Health", herbivore.currentPriority);
            Assert.Less(Vector3.Distance(initialHumanPos, initialHerbivorePos), Vector3.Distance(initialHumanPos, herbivore.transform.position));
            
            yield return new WaitForSecondsRealtime(.1f);

            GameObject.Destroy(human.gameObject);
            GameObject.Destroy(herbivore.gameObject);
            yield return new WaitForSecondsRealtime(.1f);
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckCarnivoreSeekHumanFlee() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }

            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Carnivore"), new Vector3(0, 0, 0), Quaternion.identity);
            Carnivore carnivore = temp.GetComponent<Carnivore>();
        
            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Human"), new Vector3(5, 0, 0), Quaternion.identity);
            Human human = temp.GetComponent<Human>();
            Vector3 initialCarnivorePos = carnivore.transform.position;
            Vector3 initialHumanPos = human.transform.position;

            yield return new WaitForSecondsRealtime(.01f);
            carnivore.currentHunger = carnivore.baseHunger / 3;

            yield return new WaitForSeconds(2f);
            Assert.AreEqual("Hunger", carnivore.currentPriority);
            Assert.AreNotSame(initialCarnivorePos, carnivore.transform.position);
            Assert.Greater(Vector3.Distance(initialHumanPos, initialCarnivorePos), Vector3.Distance(initialHumanPos, carnivore.transform.position));
            Assert.AreEqual("Health", human.currentPriority);
            Assert.Less(Vector3.Distance(initialCarnivorePos, initialHumanPos), Vector3.Distance(initialCarnivorePos, human.transform.position));
            
            yield return new WaitForSecondsRealtime(.1f);

            GameObject.Destroy(carnivore.gameObject);
            GameObject.Destroy(human.gameObject);
            yield return new WaitForSecondsRealtime(.1f);
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckOmnivoreSeekCarnivoreFlee() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }

            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Omnivore"), new Vector3(0, 0, 0), Quaternion.identity);
            Omnivore omnivore = temp.GetComponent<Omnivore>();
        
            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Carnivore"), new Vector3(5, 0, 0), Quaternion.identity);
            Carnivore carnivore = temp.GetComponent<Carnivore>();
            Vector3 initialOmnivorePos = omnivore.transform.position;
            Vector3 initialCarnivorePos = carnivore.transform.position;

            yield return new WaitForSecondsRealtime(.01f);
            omnivore.currentHunger = omnivore.baseHunger / 3;

            yield return new WaitForSeconds(2f);
            Assert.AreEqual("Hunger", omnivore.currentPriority);
            Assert.AreNotSame(initialOmnivorePos, omnivore.transform.position);
            Assert.Greater(Vector3.Distance(initialCarnivorePos, initialOmnivorePos), Vector3.Distance(initialCarnivorePos, omnivore.transform.position));
            Assert.AreEqual("Health", carnivore.currentPriority);
            Assert.Less(Vector3.Distance(initialOmnivorePos, initialCarnivorePos), Vector3.Distance(initialOmnivorePos, carnivore.transform.position));
            
            yield return new WaitForSecondsRealtime(.1f);

            GameObject.Destroy(omnivore.gameObject);
            GameObject.Destroy(carnivore.gameObject);
            yield return new WaitForSecondsRealtime(.1f);
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckHerbivoreSeek() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }

            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(0, 0, 0), Quaternion.identity);
            Herbivore herbivore = temp.GetComponent<Herbivore>();
        
            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Plant"), new Vector3(5, 0, 0), Quaternion.identity);
            Plant plant = temp.GetComponent<Plant>();
            Vector3 initialHerbivorePos = herbivore.transform.position;
            Vector3 initialPlantPos = plant.transform.position;

            yield return new WaitForSecondsRealtime(.01f);
            herbivore.currentHunger = herbivore.baseHunger / 3;
            yield return new WaitForSecondsRealtime(.01f);

            Assert.AreEqual("Hunger", herbivore.currentPriority);
            yield return new WaitForSeconds(2f);
            Assert.AreNotSame(initialHerbivorePos, herbivore.transform.position);
            Assert.Greater(Vector3.Distance(initialPlantPos, initialHerbivorePos), Vector3.Distance(initialHerbivorePos, herbivore.transform.position));
            
            yield return new WaitForSecondsRealtime(.1f);

            GameObject.Destroy(herbivore.gameObject);
            yield return new WaitForSecondsRealtime(.1f);
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckOmnivoreFlee() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }

            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Carnivore"), new Vector3(0, 0, 0), Quaternion.identity);
            Carnivore carnivore = temp.GetComponent<Carnivore>();
        
            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Omnivore"), new Vector3(5, 0, 0), Quaternion.identity);
            Omnivore omnivore = temp.GetComponent<Omnivore>();
            Vector3 initialCarnivorePos = carnivore.transform.position;
            Vector3 initialOmnivorePos = omnivore.transform.position;

            yield return new WaitForSecondsRealtime(.01f);
            carnivore.currentHunger = carnivore.baseHunger / 3;

            yield return new WaitForSeconds(2f);
            Assert.AreEqual("Health", omnivore.currentPriority);
            Assert.Less(Vector3.Distance(initialCarnivorePos, initialOmnivorePos), Vector3.Distance(initialCarnivorePos, omnivore.transform.position));
            
            yield return new WaitForSecondsRealtime(.1f);

            GameObject.Destroy(carnivore.gameObject);
            GameObject.Destroy(carnivore.gameObject);
            yield return new WaitForSecondsRealtime(.1f);
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckHumanThirst() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Human"), new Vector3(8, 0, 6), Quaternion.identity);
            Human human = temp.GetComponent<Human>();

            yield return new WaitForSecondsRealtime(.1f);
            human.currentThirst = human.baseThirst / 3;
            
            float initialThirst = human.currentThirst;
            Vector3 initialHumanPos = human.transform.position;
            Vector3 waterPos = human.closestWaterSource.transform.position;
            
            yield return new WaitForSecondsRealtime(.1f);
            Assert.AreEqual("Thirst", human.currentPriority);
            Assert.Greater(initialThirst, human.currentThirst);
            yield return new WaitForSecondsRealtime(1f);
            Assert.Greater(Vector3.Distance(initialHumanPos, waterPos), Vector3.Distance(human.transform.position, waterPos));
            Assert.Less(initialThirst, human.currentThirst);
            
            human.transform.position = new Vector3(0, 1, 0);
            human.currentThirst = 0;
            yield return new WaitForSecondsRealtime(.1f);
            Assert.True(human == null);
        }

        [UnityTest]
        public IEnumerator CheckCarnivoreThirst() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Carnivore"), new Vector3(8, 0, 6), Quaternion.identity);
            Carnivore carnivore = temp.GetComponent<Carnivore>();

            yield return new WaitForSecondsRealtime(.1f);
            carnivore.currentThirst = carnivore.baseThirst / 3;
            
            float initialThirst = carnivore.currentThirst;
            Vector3 initialCarnivorePos = carnivore.transform.position;
            Vector3 waterPos = carnivore.closestWaterSource.transform.position;
            
            yield return new WaitForSecondsRealtime(.1f);
            Assert.AreEqual("Thirst", carnivore.currentPriority);
            Assert.Greater(initialThirst, carnivore.currentThirst);
            yield return new WaitForSecondsRealtime(1f);
            Assert.Greater(Vector3.Distance(initialCarnivorePos, waterPos), Vector3.Distance(carnivore.transform.position, waterPos));
            Assert.Less(initialThirst, carnivore.currentThirst);
            
            carnivore.transform.position = new Vector3(0, 1, 0);
            carnivore.currentThirst = 0;
            yield return new WaitForSecondsRealtime(.1f);
            Assert.True(carnivore == null);
        }

        [UnityTest]
        public IEnumerator CheckOmnivoreThirst() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Omnivore"), new Vector3(8, 0, 6), Quaternion.identity);
            Omnivore omnivore = temp.GetComponent<Omnivore>();

            yield return new WaitForSecondsRealtime(.1f);
            omnivore.currentThirst = omnivore.baseThirst / 3;
            
            float initialThirst = omnivore.currentThirst;
            Vector3 initialOmnivorePos = omnivore.transform.position;
            Vector3 waterPos = omnivore.closestWaterSource.transform.position;
            
            yield return new WaitForSecondsRealtime(.1f);
            Assert.AreEqual("Thirst", omnivore.currentPriority);
            Assert.Greater(initialThirst, omnivore.currentThirst);
            yield return new WaitForSecondsRealtime(1f);
            Assert.Greater(Vector3.Distance(initialOmnivorePos, waterPos), Vector3.Distance(omnivore.transform.position, waterPos));
            Assert.Less(initialThirst, omnivore.currentThirst);
            
            omnivore.transform.position = new Vector3(0, 1, 0);
            omnivore.currentThirst = 0;
            yield return new WaitForSecondsRealtime(.1f);
            Assert.True(omnivore == null);
        }

        [UnityTest]
        public IEnumerator CheckHerbivoreThirst() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(8, 0, 6), Quaternion.identity);
            Herbivore herbivore = temp.GetComponent<Herbivore>();

            yield return new WaitForSecondsRealtime(.1f);
            herbivore.currentThirst = herbivore.baseThirst / 3;
            
            float initialThirst = herbivore.currentThirst;
            Vector3 initialHerbivorePos = herbivore.transform.position;
            Vector3 waterPos = herbivore.closestWaterSource.transform.position;
            
            yield return new WaitForSecondsRealtime(.1f);
            Assert.AreEqual("Thirst", herbivore.currentPriority);
            Assert.Greater(initialThirst, herbivore.currentThirst);
            yield return new WaitForSecondsRealtime(1f);
            Assert.Greater(Vector3.Distance(initialHerbivorePos, waterPos), Vector3.Distance(herbivore.transform.position, waterPos));
            Assert.Less(initialThirst, herbivore.currentThirst);
            
            herbivore.transform.position = new Vector3(0, 1, 0);
            herbivore.currentThirst = 0;
            yield return new WaitForSecondsRealtime(.1f);
            Assert.True(herbivore == null);
        }

        [UnityTest]
        public IEnumerator CheckHumanHungerAndAttack() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Human"), new Vector3(0, 0, 0), Quaternion.identity);
            Human human = temp.GetComponent<Human>();

            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(5, 0, 0), Quaternion.identity);
            Herbivore herbivore = temp.GetComponent<Herbivore>();

            yield return new WaitForSecondsRealtime(.01f);
            human.currentHunger = human.baseHunger / 2;
            human.aggression = 100;
            herbivore.baseMoveSpeed = .5f;
            yield return new WaitForSecondsRealtime(.01f);

            float initialHunger = human.currentHunger;
            Vector3 initialHumanPos = human.transform.position;
            Vector3 initialHerbivorePos = herbivore.transform.position;
            yield return new WaitForSecondsRealtime(.01f);
            Assert.AreEqual("Hunger", human.currentPriority);
            Assert.Greater(initialHunger, human.currentHunger);
            yield return new WaitForSecondsRealtime(1.5f);
            Assert.Greater(herbivore.baseHealth, herbivore.currentHealth);
            Assert.Greater(Vector3.Distance(initialHerbivorePos, initialHumanPos), Vector3.Distance(initialHerbivorePos, human.transform.position));
            Assert.False(human.canInteract);
            initialHumanPos = human.transform.position;
            initialHerbivorePos = herbivore.transform.position;
            yield return new WaitForSecondsRealtime(1f);
            Assert.True(human.canInteract);
            yield return new WaitForSecondsRealtime(1f);
            Assert.False(human.canInteract);
            Assert.Greater(Vector3.Distance(initialHerbivorePos, initialHumanPos), Vector3.Distance(initialHerbivorePos, human.transform.position));
            Assert.Less(initialHunger, human.currentHunger);
            Assert.Greater(human.baseHunger, human.currentHunger);
            Assert.True(herbivore == null);

            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Plant"), new Vector3(5, 0, 0), Quaternion.identity);
            Plant plant = temp.GetComponent<Plant>();
            yield return new WaitForSecondsRealtime(.01f);
            human.currentHunger = human.baseHunger / 2;
            human.transform.position = new Vector3(0, 0, 0);
            yield return new WaitForSecondsRealtime(.01f);
            initialHumanPos = human.transform.position;
            Vector3 initialPlantPos = plant.transform.position;
            initialHunger = human.currentHunger;
            yield return new WaitForSecondsRealtime(.01f);
            Assert.AreEqual("Hunger", human.currentPriority);
            Assert.Greater(initialHunger, human.currentHunger);
            yield return new WaitForSecondsRealtime(1.5f);
            Assert.False(human.canInteract);
            Assert.Greater(Vector3.Distance(initialPlantPos, initialHumanPos), Vector3.Distance(initialPlantPos, human.transform.position));
            Assert.Less(initialHunger, human.currentHunger);
            Assert.Greater(human.baseHunger, human.currentHunger);
            Assert.True(plant == null);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckOmnivoreHungerAndAttack() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Omnivore"), new Vector3(0, 0, 0), Quaternion.identity);
            Omnivore omnivore = temp.GetComponent<Omnivore>();

            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(5, 0, 0), Quaternion.identity);
            Herbivore herbivore = temp.GetComponent<Herbivore>();

            yield return new WaitForSecondsRealtime(.01f);
            omnivore.currentHunger = omnivore.baseHunger / 2;
            omnivore.aggression = 100;
            herbivore.baseMoveSpeed = .5f;
            yield return new WaitForSecondsRealtime(.01f);

            float initialHunger = omnivore.currentHunger;
            Vector3 initialOmnivorePos = omnivore.transform.position;
            Vector3 initialHerbivorePos = herbivore.transform.position;
            yield return new WaitForSecondsRealtime(.01f);
            Assert.AreEqual("Hunger", omnivore.currentPriority);
            Assert.Greater(initialHunger, omnivore.currentHunger);
            yield return new WaitForSecondsRealtime(1.5f);
            Assert.Greater(herbivore.baseHealth, herbivore.currentHealth);
            Assert.Greater(Vector3.Distance(initialHerbivorePos, initialOmnivorePos), Vector3.Distance(initialHerbivorePos, omnivore.transform.position));
            Assert.False(omnivore.canInteract);
            initialOmnivorePos = omnivore.transform.position;
            initialHerbivorePos = herbivore.transform.position;
            yield return new WaitForSecondsRealtime(1f);
            Assert.True(omnivore.canInteract);
            yield return new WaitForSecondsRealtime(1f);
            Assert.False(omnivore.canInteract);
            Assert.Greater(Vector3.Distance(initialHerbivorePos, initialOmnivorePos), Vector3.Distance(initialHerbivorePos, omnivore.transform.position));
            Assert.Less(initialHunger, omnivore.currentHunger);
            Assert.Greater(omnivore.baseHunger, omnivore.currentHunger);
            Assert.True(herbivore == null);

            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Plant"), new Vector3(5, 0, 0), Quaternion.identity);
            Plant plant = temp.GetComponent<Plant>();
            yield return new WaitForSecondsRealtime(.01f);
            omnivore.currentHunger = omnivore.baseHunger / 2;
            omnivore.transform.position = new Vector3(0, 0, 0);
            yield return new WaitForSecondsRealtime(.01f);
            initialOmnivorePos = omnivore.transform.position;
            Vector3 initialPlantPos = plant.transform.position;
            initialHunger = omnivore.currentHunger;
            yield return new WaitForSecondsRealtime(.01f);
            Assert.AreEqual("Hunger", omnivore.currentPriority);
            Assert.Greater(initialHunger, omnivore.currentHunger);
            yield return new WaitForSecondsRealtime(1.5f);
            Assert.False(omnivore.canInteract);
            Assert.Greater(Vector3.Distance(initialPlantPos, initialOmnivorePos), Vector3.Distance(initialPlantPos, omnivore.transform.position));
            Assert.Less(initialHunger, omnivore.currentHunger);
            Assert.Greater(omnivore.baseHunger, omnivore.currentHunger);
            Assert.True(plant == null);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckCarnivoreHungerAndAttack() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Carnivore"), new Vector3(0, 0, 0), Quaternion.identity);
            Carnivore carnviore = temp.GetComponent<Carnivore>();

            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(5, 0, 0), Quaternion.identity);
            Herbivore herbivore = temp.GetComponent<Herbivore>();

            yield return new WaitForSecondsRealtime(.01f);
            carnviore.currentHunger = carnviore.baseHunger / 2;
            carnviore.aggression = 100;
            herbivore.baseMoveSpeed = .5f;
            yield return new WaitForSecondsRealtime(.01f);

            float initialHunger = carnviore.currentHunger;
            Vector3 initialOmnivorePos = carnviore.transform.position;
            Vector3 initialHerbivorePos = herbivore.transform.position;
            yield return new WaitForSecondsRealtime(.01f);
            Assert.AreEqual("Hunger", carnviore.currentPriority);
            Assert.Greater(initialHunger, carnviore.currentHunger);
            yield return new WaitForSecondsRealtime(1f);
            Assert.Greater(herbivore.baseHealth, herbivore.currentHealth);
            Assert.Greater(Vector3.Distance(initialHerbivorePos, initialOmnivorePos), Vector3.Distance(initialHerbivorePos, carnviore.transform.position));
            Assert.False(carnviore.canInteract);
            yield return new WaitForSecondsRealtime(1f);
            Assert.True(carnviore.canInteract);
            Assert.Less(initialHunger, carnviore.currentHunger);
            Assert.Greater(carnviore.baseHunger, carnviore.currentHunger);
            Assert.True(herbivore == null);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckHerbivoreHunger() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(0, 0, 0), Quaternion.identity);
            Herbivore herbivore = temp.GetComponent<Herbivore>();

            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Plant"), new Vector3(5, 0, 0), Quaternion.identity);
            Plant plant = temp.GetComponent<Plant>();

            yield return new WaitForSecondsRealtime(.01f);
            herbivore.currentHunger = herbivore.baseHunger / 2;
            yield return new WaitForSecondsRealtime(.01f);

            float initialHunger = herbivore.currentHunger;
            Vector3 initialOmnivorePos = herbivore.transform.position;
            Vector3 initialPlantPos = plant.transform.position;
            initialHunger = herbivore.currentHunger;
            yield return new WaitForSecondsRealtime(.01f);
            Assert.AreEqual("Hunger", herbivore.currentPriority);
            Assert.Greater(initialHunger, herbivore.currentHunger);
            yield return new WaitForSecondsRealtime(1.5f);
            Assert.False(herbivore.canInteract);
            Assert.Greater(Vector3.Distance(initialPlantPos, initialOmnivorePos), Vector3.Distance(initialPlantPos, herbivore.transform.position));
            Assert.Less(initialHunger, herbivore.currentHunger);
            Assert.Greater(herbivore.baseHunger, herbivore.currentHunger);
            Assert.True(plant == null);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckHumanAndHerbivoreStamina() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Human"), new Vector3(0, 0, 0), Quaternion.identity);
            Human human = temp.GetComponent<Human>();

            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(4, 0, 0), Quaternion.identity);
            Herbivore herbivore = temp.GetComponent<Herbivore>();

            yield return new WaitForSecondsRealtime(.01f);
            human.aggression = 100;
            human.currentHunger = human.baseHunger / 2;
            human.baseMoveSpeed = 3.5f;
            herbivore.baseMoveSpeed = 4f;
            yield return new WaitForSecondsRealtime(.01f);

            float initialHumanStamina = human.currentStamina;
            float initialHerbivoreStamina = herbivore.currentStamina;
            yield return new WaitForSecondsRealtime(2f);
            Assert.AreEqual(Animal.ActivityState.Sprinting, human.activityState);
            Assert.AreEqual(Animal.ActivityState.Sprinting, herbivore.activityState);
            Assert.Greater(initialHumanStamina, human.currentStamina);
            Assert.Greater(initialHerbivoreStamina, herbivore.currentStamina);
            human.transform.position = new Vector3(-10, 0, 0);
            herbivore.transform.position = new Vector3(10, 0, 0);
            initialHumanStamina = human.currentStamina;
            initialHerbivoreStamina = herbivore.currentStamina;
            human.currentThirst += 50;
            herbivore.currentThirst += 50;
            yield return new WaitForSecondsRealtime(1f);
            Assert.AreEqual(Animal.ActivityState.Walking, human.activityState);
            Assert.AreEqual(Animal.ActivityState.Walking, herbivore.activityState);
            Assert.Less(initialHumanStamina, human.currentStamina);
            Assert.Less(initialHerbivoreStamina, herbivore.currentStamina);
            float humanDiff = Mathf.Abs(initialHumanStamina - human.currentStamina);
            float herbivoreDiff = Mathf.Abs(initialHerbivoreStamina - herbivore.currentStamina);
            human.currentStamina = -5;
            herbivore.currentStamina = -5;
            yield return new WaitForSecondsRealtime(1f);
            human.currentStamina += 5;
            herbivore.currentStamina += 5;
            Assert.AreEqual(Animal.ActivityState.Resting, human.activityState);
            Assert.AreEqual(Animal.ActivityState.Resting, herbivore.activityState);
            Assert.Less(0, human.currentStamina);
            Assert.Less(0, herbivore.currentStamina);
            Assert.Less(humanDiff, human.currentStamina);
            Assert.Less(herbivoreDiff, herbivore.currentStamina);
            human.currentStamina = human.baseStamina;
            herbivore.currentStamina = herbivore.baseStamina;
            yield return new WaitForSecondsRealtime(.1f);
            Assert.AreNotEqual(Animal.ActivityState.Resting, human.activityState);
            Assert.AreNotEqual(Animal.ActivityState.Resting, herbivore.activityState);
            Assert.GreaterOrEqual((int)human.baseStamina, (int)human.currentStamina);
            Assert.GreaterOrEqual((int)herbivore.baseStamina, (int)herbivore.currentStamina);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckCarnivoreStamina() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Carnivore"), new Vector3(0, 0, 0), Quaternion.identity);
            Carnivore carnivore = temp.GetComponent<Carnivore>();

            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(4, 0, 0), Quaternion.identity);
            Herbivore herbivore = temp.GetComponent<Herbivore>();

            yield return new WaitForSecondsRealtime(.01f);
            carnivore.aggression = 100;
            carnivore.currentHunger = carnivore.baseHunger / 2;
            carnivore.baseMoveSpeed = 3.5f;
            herbivore.baseMoveSpeed = 4f;
            yield return new WaitForSecondsRealtime(.01f);

            float initialHumanStamina = carnivore.currentStamina;
            yield return new WaitForSecondsRealtime(2f);
            Assert.AreEqual(Animal.ActivityState.Sprinting, carnivore.activityState);
            Assert.Greater(initialHumanStamina, carnivore.currentStamina);
            carnivore.transform.position = new Vector3(-10, 0, 0);
            GameObject.Destroy(herbivore);
            initialHumanStamina = carnivore.currentStamina;
            carnivore.currentThirst += 50;
            yield return new WaitForSecondsRealtime(1f);
            Assert.AreEqual(Animal.ActivityState.Walking, carnivore.activityState);
            Assert.Less(initialHumanStamina, carnivore.currentStamina);
            float carnivoreDiff = Mathf.Abs(initialHumanStamina - carnivore.currentStamina);
            carnivore.currentStamina = -5;
            yield return new WaitForSecondsRealtime(1f);
            carnivore.currentStamina += 5;
            herbivore.currentStamina += 5;
            Assert.AreEqual(Animal.ActivityState.Resting, carnivore.activityState);
            Assert.Less(0, carnivore.currentStamina);
            Assert.Less(carnivoreDiff, carnivore.currentStamina);
            carnivore.currentStamina = carnivore.baseStamina;
            yield return new WaitForSecondsRealtime(.1f);
            Assert.AreNotEqual(Animal.ActivityState.Resting, carnivore.activityState);
            Assert.GreaterOrEqual((int)carnivore.baseStamina, (int)carnivore.currentStamina);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckOmnivoreStamina() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = false;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);

            foreach (Organism organism in GameObject.FindObjectsOfType<Organism>()) {
                GameObject.Destroy(organism.gameObject);
            }
            yield return new WaitForSecondsRealtime(.1f);

            GameObject temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Omnivore"), new Vector3(0, 0, 0), Quaternion.identity);
            Omnivore omnivore = temp.GetComponent<Omnivore>();

            temp = MonoBehaviour.Instantiate((GameObject)Resources.Load("Herbivore"), new Vector3(4, 0, 0), Quaternion.identity);
            Herbivore herbivore = temp.GetComponent<Herbivore>();

            yield return new WaitForSecondsRealtime(.01f);
            omnivore.aggression = 100;
            omnivore.currentHunger = omnivore.baseHunger / 2;
            omnivore.baseMoveSpeed = 3.5f;
            herbivore.baseMoveSpeed = 4f;
            yield return new WaitForSecondsRealtime(.01f);

            float initialHumanStamina = omnivore.currentStamina;
            yield return new WaitForSecondsRealtime(2f);
            Assert.AreEqual(Animal.ActivityState.Sprinting, omnivore.activityState);
            Assert.Greater(initialHumanStamina, omnivore.currentStamina);
            omnivore.transform.position = new Vector3(-10, 0, 0);
            GameObject.Destroy(herbivore);
            initialHumanStamina = omnivore.currentStamina;
            omnivore.currentThirst += 50;
            yield return new WaitForSecondsRealtime(1f);
            Assert.AreEqual(Animal.ActivityState.Walking, omnivore.activityState);
            Assert.Less(initialHumanStamina, omnivore.currentStamina);
            float carnivoreDiff = Mathf.Abs(initialHumanStamina - omnivore.currentStamina);
            omnivore.currentStamina = -5;
            yield return new WaitForSecondsRealtime(1f);
            omnivore.currentStamina += 5;
            herbivore.currentStamina += 5;
            Assert.AreEqual(Animal.ActivityState.Resting, omnivore.activityState);
            Assert.Less(0, omnivore.currentStamina);
            Assert.Less(carnivoreDiff, omnivore.currentStamina);
            omnivore.currentStamina = omnivore.baseStamina;
            yield return new WaitForSecondsRealtime(.1f);
            Assert.AreNotEqual(Animal.ActivityState.Resting, omnivore.activityState);
            Assert.GreaterOrEqual((int)omnivore.baseStamina, (int)omnivore.currentStamina);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckSimulationOver() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Slider durationSlider = settingsMenu.transform.Find("DurationSlider").GetComponent<Slider>();
            Slider iterationsSlider = settingsMenu.transform.Find("IterationsSlider").GetComponent<Slider>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            durationSlider.value = 20;
            yield return new WaitForFixedUpdate();
            iterationsSlider.value = 5;
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = true;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);
            Button autoButton = canvas.transform.Find("AutoBtn").GetComponent<Button>();
            autoButton.onClick.Invoke();
            yield return new WaitForSeconds(21 * 5);
            Assert.AreEqual("Simulation OVer", SceneManager.GetActiveScene().name);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckSimulationEndButton() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Slider durationSlider = settingsMenu.transform.Find("DurationSlider").GetComponent<Slider>();
            Slider iterationsSlider = settingsMenu.transform.Find("IterationsSlider").GetComponent<Slider>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            durationSlider.value = 20;
            yield return new WaitForFixedUpdate();
            iterationsSlider.value = 5;
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = true;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            Button autoButton = canvas.transform.Find("AutoBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);
            autoButton.onClick.Invoke();
            Button endButton = canvas.transform.Find("EndBtn").GetComponent<Button>();
            Assert.False(endButton.gameObject.activeSelf);
            yield return new WaitForSeconds(20f);
            endButton.onClick.Invoke();
            yield return new WaitForSeconds(.1f);
            Assert.AreEqual("Simulation Over", SceneManager.GetActiveScene().name);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckSimulationDataTransferAndUI() {
            SceneManager.LoadScene("Home Menu");
            Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            GameObject mainMenu = GameObject.Find("Canvas").transform.Find("MainMenu").gameObject;
            GameObject settingsMenu = GameObject.Find("Canvas").transform.Find("SettingsMenu").gameObject;
            Button startButton = mainMenu.transform.Find("StartButton").GetComponent<Button>();
            Button settingsButton = mainMenu.transform.Find("SettingsButton").GetComponent<Button>();
            Button backButton = settingsMenu.transform.Find("SettingsBackButton").GetComponent<Button>();
            Slider durationSlider = settingsMenu.transform.Find("DurationSlider").GetComponent<Slider>();
            Slider iterationsSlider = settingsMenu.transform.Find("IterationsSlider").GetComponent<Slider>();
            Toggle humanToggle = settingsMenu.transform.Find("HumansToggle").GetComponent<Toggle>();

            //Define data
            settingsButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            durationSlider.value = 20;
            yield return new WaitForFixedUpdate();
            iterationsSlider.value = 5;
            yield return new WaitForFixedUpdate();
            humanToggle.isOn = true;
            yield return new WaitForFixedUpdate();

            //Start Simulation
            backButton.onClick.Invoke();
            yield return new WaitForFixedUpdate();
            startButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.01f);

            GameObject canvas = GameObject.Find("Canvas");
            Button startSimButton = canvas.transform.Find("StartBtn").GetComponent<Button>();
            Button autoButton = canvas.transform.Find("AutoBtn").GetComponent<Button>();
            startSimButton.onClick.Invoke();
            yield return new WaitForSecondsRealtime(.1f);
            autoButton.onClick.Invoke();
            Button endButton = canvas.transform.Find("EndBtn").GetComponent<Button>();
            Assert.False(endButton.gameObject.activeSelf);
            yield return new WaitForSeconds(20f);

            float aggression = float.Parse(canvas.transform.Find("Aggression").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            float endurance = float.Parse(canvas.transform.Find("Endurance").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            float moveSpeed = float.Parse(canvas.transform.Find("Move Speed").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            float stamina = float.Parse(canvas.transform.Find("Stamina").GetComponent<TextMeshProUGUI>().text.Split(new char[]{':'})[1].Trim());
            float strength = float.Parse(canvas.transform.Find("Strength").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            float health = float.Parse(canvas.transform.Find("Health").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            float hunger = float.Parse(canvas.transform.Find("Hunger").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            float thirst = float.Parse(canvas.transform.Find("Thirst").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            float survivalTime = float.Parse(canvas.transform.Find("Survival Time").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());

            endButton.onClick.Invoke();
            yield return new WaitForSeconds(.1f);
            Assert.AreEqual("Simulation Over", SceneManager.GetActiveScene().name);
            canvas = GameObject.Find("Canvas").transform.Find("SimulationOverMenu").gameObject;
            Button returnButton = canvas.transform.Find("ReturnButton").GetComponent<Button>();
            Assert.AreEqual(Data.Aggression.ToString("F2"), aggression.ToString("F2"));
            Assert.AreEqual(Data.Endurance.ToString("F2"), endurance.ToString("F2"));
            Assert.AreEqual(Data.MoveSpeed.ToString("F2"), moveSpeed.ToString("F2"));
            Assert.AreEqual(Data.Stamina.ToString("F2"), stamina.ToString("F2"));
            Assert.AreEqual(Data.Strength.ToString("F2"), strength.ToString("F2"));
            Assert.AreEqual(Data.Health.ToString("F2"), health.ToString("F2"));
            Assert.AreEqual(Data.Hunger.ToString("F2"), hunger.ToString("F2"));
            Assert.AreEqual(Data.Thirst.ToString("F2"), thirst.ToString("F2"));
            Assert.AreEqual(Data.Age.ToString("F2"), survivalTime.ToString("F2"));

            aggression = float.Parse(canvas.transform.Find("Aggression").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            endurance = float.Parse(canvas.transform.Find("Endurance").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            moveSpeed = float.Parse(canvas.transform.Find("Move Speed").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            stamina = float.Parse(canvas.transform.Find("Stamina").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            strength = float.Parse(canvas.transform.Find("Strength").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            health = float.Parse(canvas.transform.Find("Health").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            hunger = float.Parse(canvas.transform.Find("Hunger").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            thirst = float.Parse(canvas.transform.Find("Thirst").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            survivalTime = float.Parse(canvas.transform.Find("Survival Time").GetComponent<TextMeshProUGUI>().text.Split(':')[1].Trim());
            
            Assert.AreEqual(Data.Aggression.ToString("F2"), aggression.ToString("F2"));
            Assert.AreEqual(Data.Endurance.ToString("F2"), endurance.ToString("F2"));
            Assert.AreEqual(Data.MoveSpeed.ToString("F2"), moveSpeed.ToString("F2"));
            Assert.AreEqual(Data.Stamina.ToString("F2"), stamina.ToString("F2"));
            Assert.AreEqual(Data.Strength.ToString("F2"), strength.ToString("F2"));
            Assert.AreEqual(Data.Health.ToString("F2"), health.ToString("F2"));
            Assert.AreEqual(Data.Hunger.ToString("F2"), hunger.ToString("F2"));
            Assert.AreEqual(Data.Thirst.ToString("F2"), thirst.ToString("F2"));
            Assert.AreEqual(Data.Age.ToString("F2"), survivalTime.ToString("F2"));

            yield return null;
        }
    }
}
