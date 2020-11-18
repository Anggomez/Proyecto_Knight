using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;


public class SetVolume : MonoBehaviour
{

    public AudioMixer mixer;

    public void cambiarVolumen(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
}