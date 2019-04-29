using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class LightEstimate : MonoBehaviour

{
    public Light m_LightToEffect;
    private IlluminationManager m_IlluminationManager;

    private float? m_Temperature;
    private float? m_AmbientIntensity;

    Color color;
    float r, g, b;

    private void Start()
    {
        m_IlluminationManager = TrackerManager.Instance.GetStateManager().GetIlluminationManager();
    }

    private void Update()
    {
        if (m_IlluminationManager == null)
            return;

        if (m_LightToEffect != null)
        {

            m_Temperature = m_IlluminationManager.AmbientColorTemperature;
            if (m_Temperature != null)
            {

                color = Mathf.CorrelatedColorTemperatureToRGB((float)m_Temperature);

                //m_LightToEffect.colorTemperature = (float)m_Temperature;
            }

            m_AmbientIntensity = m_IlluminationManager.AmbientIntensity;
            if (m_AmbientIntensity != null && color != null)
            {
                color = color * ((float) m_AmbientIntensity / 1000);
                //m_LightToEffect.intensity = (float)m_AmbientIntensity;


                m_LightToEffect.color = color;
                m_LightToEffect.intensity = ((float)m_AmbientIntensity / 1000);
                RenderSettings.ambientSkyColor = color;
                RenderSettings.ambientEquatorColor = color;
                RenderSettings.ambientGroundColor = color;
            }

        }
    }
}
