using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;

[ExecuteAlways]
public class TimeManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private Camera mainCamera;

    public Text timeText;
    public Text colonText;
    private bool colonFlag;

    public int timeFlowMode;

    private float hour, minute, second;

    public bool lightFlag;

    void Start()
    {
        mainCamera = Camera.main;

        colonFlag = true;
        lightFlag = false;

        InvokeRepeating("TimePrinter", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            if (timeFlowMode == 0)
            {
                hour = float.Parse(DateTime.Now.ToString("HH"));
                minute = float.Parse(DateTime.Now.ToString("mm"));
                second = float.Parse(DateTime.Now.ToString("ss"));
            }
            else if (timeFlowMode == 1) 
            {
                minute += 10f;

                if (minute >= 60f) 
                {
                    minute = 0f;
                    hour = (++hour) % 24f;
                }
            }

            TimeOfDay = hour + ((minute + (second / 60f)) / 60f);
            
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }

        timeText.text = timeToString(hour) + "  " + timeToString(minute);
    }

    private void TimePrinter() 
    {
        colonFlag = !colonFlag;
        colonText.gameObject.SetActive(colonFlag);
    }

    private string timeToString(float value) 
    {
        if (value == 0f) 
        {
            return "00";
        }
        else if (value < 10f) 
        {
            return "0" + value.ToString();
        }
        else 
        {
            return value.ToString();
        }
    }

    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        // Change Camera background Color
        // mainCamera.backgroundColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
