using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class LightEstimate : MonoBehaviour
{
    private bool mAccessCameraImage = true;
    private Vuforia.PIXEL_FORMAT mPixelFormat; // or RGBA8888, RGB888, RGB565, YUV, GRAYSCALE
    private bool mFormatRegistered = false;

    public Text LightOutput1;
    public Text LightOutput2;

    public bool debugging;
    public Light m_LightToEffect;

    private Color lightColor = new Color(1, 1, 1, 1);
    private float ligtColorNum;
    public double intesityModifier = 10.0;
    public int temperatureModifier = 3000;

    public float? intensity { get; private set; }
    public float? colorTemperature { get; private set; }


    void Start()
    {
        mPixelFormat = Vuforia.PIXEL_FORMAT.UNKNOWN_FORMAT;
        GraphicsSettings.lightsUseLinearIntensity = true;
        GraphicsSettings.lightsUseColorTemperature = true;
        // set up pixel format
#if UNITY_EDITOR
        mPixelFormat = Vuforia.PIXEL_FORMAT.GRAYSCALE; // Need Grayscale for Editor
#else
        mPixelFormat = Vuforia.PIXEL_FORMAT.RGB888; // Use RGB888 for mobile
#endif

        // API for getting Vuforia Callbacks as of Unity 2018.1.0f2. 
        //The OnVuforiaStarted event is required for getting the camera pixel data for sure
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.RegisterOnPauseCallback(OnVuforiaPaused);
        VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);

        //text used for debugging
        if (debugging)
        {
        LightOutput1.text = "";
        LightOutput2.text = "";
        }

    }

    private void OnVuforiaStarted()
    {
        // Try register camera image format

        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            mFormatRegistered = true;
        }
        else
        {
            mFormatRegistered = false;
        }
    }

    // Called when app is paused / resumed
    private void OnVuforiaPaused(bool paused)
    {
        if (paused)
        {
            UnregisterFormat();
        }
        else
        {
            RegisterFormat();
        }
    }

    // Called each time the Vuforia state is updated
    private void OnTrackablesUpdated()
    { 
        if (mFormatRegistered)
        {
            if (mAccessCameraImage)
            {
                Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
                if (image != null)
                {
                    string imageInfo = mPixelFormat + " image: \n";
                    imageInfo += " size: " + image.Width + " x " + image.Height + "\n";
                    imageInfo += " bufferSize: " + image.BufferWidth + " x " + image.BufferHeight + "\n";
                    imageInfo += " stride: " + image.Stride;
                    Debug.Log(imageInfo);
                    byte[] pixels = image.Pixels;
                    if (pixels != null && pixels.Length > 0)
                    {
                        double totalLuminance = 0.0;
                        for (int p = 0; p < pixels.Length; p += 4)
                        {
                            totalLuminance += pixels[p] * 0.299 + pixels[p + 1] * 0.587 + pixels[p + 2] * 0.114;

                        }

                        totalLuminance /= (pixels.Length);
                        //this takes the totalLuminance in the line above and puts it in the decimal range
                        ligtColorNum = (float)totalLuminance * 0.0255f;
                        //ligtColorNum is put in for RGB. will change color along the gray scale
                        lightColor = new Color(ligtColorNum, ligtColorNum, ligtColorNum, 1.0f);
                        totalLuminance /= 255.0;
                        totalLuminance *= intesityModifier;
                        m_LightToEffect.intensity = (float)totalLuminance;

                        // This adjusts the Ambient light that's always present in a scene.
                        RenderSettings.ambientIntensity = m_LightToEffect.intensity;
                        RenderSettings.ambientLight = lightColor;

                        colorTemperature = (float?)(totalLuminance * temperatureModifier);
                        m_LightToEffect.colorTemperature = (float)colorTemperature;

                        //Used for debugging so you can see if light changes;
                        if (debugging == true)
                        {
                            if (m_LightToEffect.intensity != null)
                            {
                                LightOutput1.text = "light intensity = " + m_LightToEffect.intensity;
                            }
                            else
                            {
                                LightOutput1.text = "ambient intensity = " + RenderSettings.ambientIntensity;
                            }
                            LightOutput2.text = "color temp = " + m_LightToEffect.colorTemperature;
                        }
                    }
                }
            }
        }
    }

    // Unregister the camera pixel format (e.g. call this when app is paused)
    private void UnregisterFormat()
    {
        CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
        mFormatRegistered = false;
    }
    
    // Register the camera pixel format
    private void RegisterFormat()
    {
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            mFormatRegistered = true;
        }
        else
        {
            mFormatRegistered = false;
        }
    }
}
