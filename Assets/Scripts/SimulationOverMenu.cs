﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationOverMenu : MonoBehaviour
{
    public void ReturnToMenu() {
        SceneManager.LoadScene(0);
    }
}
