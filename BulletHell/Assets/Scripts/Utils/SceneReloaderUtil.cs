﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneReloaderUtil : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}