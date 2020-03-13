﻿namespace Menu.Utils
{
    using UnityEngine;
    using System;

    public class FadeController : MonoBehaviour
    {


        public float fadeSpeed = 1f;         // Speed that the screen fades to and from black.


        private bool sceneStarting = true;      // Whether or not the scene is still fading in.

        private Action nextAction;
        private bool sceneEnding = false;


        void Start()
        {
            GetComponent<Renderer>().enabled = true;
        }

        void Update()
        {
            // If the scene is starting...
            if (sceneStarting)
            {
                // ... call the StartScene function.
                DoStartScene();
            }
            else if (sceneEnding)
            {
                DoEndScene();
            }
        }

        void FadeToClear()
        {
            // Lerp the color of the texture between itself and transparent
            GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.clear, fadeSpeed * Time.deltaTime);
        }

        void FadeToBlack()
        {
            // Lerp the color of the texture between itself and black.
            GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.black, 4 * fadeSpeed * Time.deltaTime);
        }

        void DoStartScene()
        {
            // Fade the texture to clear.
            FadeToClear();

            // If the texture is almost clear...
            if (GetComponent<Renderer>().material.color.a <= 0.01f)
            {
                // ... set the colour to clear and disable the GUITexture.
                GetComponent<Renderer>().material.color = Color.clear;
                GetComponent<Renderer>().enabled = false;

                // The scene is no longer starting.
                sceneStarting = false;
            }
        }

        void DoEndScene()
        {

            FadeToBlack();
            // If the screen is almost black...
            if (GetComponent<Renderer>().material.color.a >= 0.95f)
            {
                sceneEnding = false;
                nextAction.Invoke();
            }
        }

        public void StartScene()
        {
            sceneStarting = true;
            sceneEnding = false;
        }

        public void FadeToLevel(Action action)
        {
            // Make sure the texture is enabled.
            GetComponent<Renderer>().enabled = true;
            nextAction = action;
            sceneEnding = true;

        }

    }
}