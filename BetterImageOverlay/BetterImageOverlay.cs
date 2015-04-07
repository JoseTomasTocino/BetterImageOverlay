﻿using ICities;
using UnityEngine;
using System;
using System.IO;
using ColossalFramework;
using ColossalFramework.Plugins;

namespace BetterImageOverlay
{
    public class BetterImageOverlay : IUserMod
    {
        public string Description
        {
            get { return "Overlays an image (located at [Steam Skylines Folder]/Files/overlay.png). Image can be controlled using the keypad."; }
        }

        public string Name
        {
            get { return "BetterImageOverlay"; }
        }
    }

    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);

            Texture2D tex = new Texture2D(2, 2);

            try
            {
                byte[] bytes = File.ReadAllBytes("Files/overlay.png");
                tex.LoadImage(bytes);
            }
            catch
            {
                go.SetActive(false);
                return;
            }

            Material mat = new Material(" Shader \"OverlayShader\" { " +
                                        " Properties { " +
                                        " _MainTex (\"Texture to blend\", 2D) = \"black\" {} " +
                                        "}" +
                                        " SubShader { " +
                                        " Tags { \"Queue\" = \"Transparent+100\" } " +
                                        " Pass { " +
                                        " Blend SrcAlpha OneMinusSrcAlpha " +
                                        " SetTexture [_MainTex] { combine texture } " +
                                        " } " +
                                        " } " +
                                        " } ");



            mat.SetTexture("_MainTex", tex);

            go.GetComponent<Renderer>().material = mat;

            go.AddComponent<Movement>();

            go.transform.position = new Vector3(0f, 200f, 0f);
            go.transform.localScale = new Vector3(175f, 1f, 175f);
            go.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            go.transform.SetParent(Camera.main.transform.parent);
        }

    }

    public class Movement : MonoBehaviour
    {

        void Update()
        {
            // Image size (using keypad's plus and minus)
            Vector3 scaleDelta = new Vector3(2.5f, 0f, 2.5f);

            if (Input.GetKey(KeyCode.KeypadPlus))
            {
                transform.localScale += scaleDelta;
            }

            else if (Input.GetKey(KeyCode.KeypadMinus))
            {
                transform.localScale -= scaleDelta;
            }

            // Image rotation (using keypad 7 and 9)
            Vector3 rotationDelta = new Vector3(0f, 1f, 0f);

            if (Input.GetKey(KeyCode.Keypad7))
            {
                transform.eulerAngles -= rotationDelta;
            }

            else if (Input.GetKey(KeyCode.Keypad9))
            {
                transform.eulerAngles += rotationDelta;
            }

            // Image position (using keypad arrows)
            float positionDelta = 80f * Time.deltaTime;

            if (Input.GetKey(KeyCode.Keypad8)) // UP
            {
                transform.position += new Vector3(0f, 0f, positionDelta );
            }

            else if (Input.GetKey(KeyCode.Keypad2)) // DOWN
            {
                transform.position += new Vector3(0f, 0f, -positionDelta );
            }

            else if (Input.GetKey(KeyCode.Keypad4)) // LEFT
            {
                transform.position += new Vector3(-positionDelta , 0f, 0f);
            }

            else if (Input.GetKey(KeyCode.Keypad6)) // RIGHT
            {
                transform.position += new Vector3(positionDelta , 0f, 0f);
            }

            // Image toggle (using keypad's enter)
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
            }

            // Image height (using keypad's zero and period)
            if (Input.GetKey(KeyCode.KeypadPeriod))
            {
                transform.position += new Vector3(0f, 80f * Time.deltaTime, 0f);
            }

            else if (Input.GetKey(KeyCode.Keypad0))
            {
                transform.position -= new Vector3(0f, 80f * Time.deltaTime, 0f);
            }
        }
    }
}
