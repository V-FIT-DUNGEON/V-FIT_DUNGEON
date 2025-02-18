using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using EmeraldAI;
using UnityEngine.SceneManagement;

public class PlayerHitDeathBehavior : MonoBehaviour
{
    public ScreenFader screenFade;
    private EmeraldSystem EmeraldComponent;
    public Grabber rGrabber;
    public Grabber lGrabber;

    public float VibrateFrequency = 0.3f;
    public float VibrateAmplitude = 0.1f;
    public float VibrateDuration = 0.1f;

    void Start()
    {
        EmeraldComponent = GetComponent<EmeraldSystem>();
    }

    private EmeraldAPI.Health health;

    public void ScreenFadeDamage()
    {
        StartCoroutine(GotHit());
    }

    public void OnDeath()
    {
        rGrabber.TryRelease();
        lGrabber.TryRelease();
        GetComponent<CharacterController>().enabled = false;//edit
        StartCoroutine(PlayerReset());
    }


    public IEnumerator GotHit()
    {
        InputBridge.Instance.VibrateController(VibrateFrequency, VibrateAmplitude, VibrateDuration, rGrabber.HandSide);
        InputBridge.Instance.VibrateController(VibrateFrequency, VibrateAmplitude, VibrateDuration, lGrabber.HandSide);

        screenFade.DoFadeIn();
        yield return new WaitForSeconds(0.2f);
        screenFade.DoFadeOut();
    }

    private IEnumerator PlayerReset()
    {
        StopCoroutine(GotHit());
        screenFade.DoFadeIn();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}