using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusica;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Slider sliderSensibilidad;
    public static event Action<float> OnSensibilidadCambiada;
    private void Start()
    {
        float masterGuardado = PlayerPrefs.GetFloat("VolumenMaster", 1f);
        float musicaGuardada = PlayerPrefs.GetFloat("VolumenMusica", 0.75f);
        float sfxGuardado = PlayerPrefs.GetFloat("VolumenSFX", 0.5f);
        float sensibilidadGuardada = PlayerPrefs.GetFloat("Sensibilidad", 1f);
        float sensibilidadGuardadaSlider = PlayerPrefs.GetFloat("SensibilidadSlider", 0.2f);

        sliderMaster.value = masterGuardado;
        sliderMusica.value = musicaGuardada;
        sliderSFX.value = sfxGuardado;
        sliderSensibilidad.value = sensibilidadGuardadaSlider;

        CambiarVolumenMaster(masterGuardado);
        CambiarVolumenMusica(musicaGuardada);
        CambiarVolumenSFX(sfxGuardado);
        CambiarSensibilidad(sensibilidadGuardadaSlider);
    }

    public void CambiarVolumenMaster(float valor)
    {
        float volumenSeguro = Mathf.Max(valor, 0.0001f);
        audioMixer.SetFloat("VolumenMaster", Mathf.Log10(volumenSeguro) * 20);
        PlayerPrefs.SetFloat("VolumenMaster", valor);
    }

    public void CambiarVolumenMusica(float valor)
    {
        float volumenSeguro = Mathf.Max(valor, 0.0001f);
        audioMixer.SetFloat("VolumenMusica", Mathf.Log10(volumenSeguro) * 20);
        PlayerPrefs.SetFloat("VolumenMusica", valor);
    }

    public void CambiarVolumenSFX(float valor)
    {
        float volumenSeguro = Mathf.Max(valor, 0.0001f);
        audioMixer.SetFloat("VolumenSFX", Mathf.Log10(volumenSeguro) * 20);
        PlayerPrefs.SetFloat("VolumenSFX", valor);
    }
    public void CambiarSensibilidad(float valor)
    {
        PlayerPrefs.SetFloat("SensibilidadSlider", valor);
        float v = Mathf.Max(0.1f, valor * 5f);
        PlayerPrefs.SetFloat("Sensibilidad", v);

        OnSensibilidadCambiada?.Invoke(v);
    }
}
