using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusica;
    [SerializeField] private Slider sliderSFX;

    private void Start()
    {
        float masterGuardado = PlayerPrefs.GetFloat("VolumenMaster", 0.75f);
        float musicaGuardada = PlayerPrefs.GetFloat("VolumenMusica", 0.75f);
        float sfxGuardado = PlayerPrefs.GetFloat("VolumenSFX", 0.75f);

        sliderMaster.value = masterGuardado;
        sliderMusica.value = musicaGuardada;
        sliderSFX.value = sfxGuardado;

        CambiarVolumenMaster(masterGuardado);
        CambiarVolumenMusica(musicaGuardada);
        CambiarVolumenSFX(sfxGuardado);
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
}
