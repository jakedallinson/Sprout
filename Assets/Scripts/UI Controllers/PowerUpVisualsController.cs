using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PowerUpVisualsController : MonoBehaviour
{ 
    public GameObject PowerUpUI;
    public GameObject CarrotsToPlant;
    public GameObject GardenGoodZone;
    public GameObject GardenBadZone;

    private float AlphaMax = 0.4f;
    private float AlphaSmooth = 0.02f;
    private float GoodZoneAlphaCounter;
    private float BadZoneAlphaCounter;

    void Start()
    {
        GoodZoneAlphaCounter = AlphaMax;
        BadZoneAlphaCounter = AlphaMax;
        GardenGoodZone.SetActive(true);
        GardenBadZone.SetActive(false);
        // event listeners
        GameEvents.current.OnPowerUpStarted += ShowPowerUpUI;
        GameEvents.current.OnPowerUpFinished += HidePowerUpUI;
        GameEvents.current.OnTouchOutsideGarden += StartBadZoneCounter;
    }

    void Update()
    {
        CarrotsToPlant.GetComponent<Text>().text = (PowerUpsController.CarrotsLeft()).ToString();
        FlashGoodZone();
        // flash the bad zone only if it was recently clicked
        if (BadZoneAlphaCounter < AlphaMax) {
            FlashBadZone();
        }
    }

    private void ShowPowerUpUI()
    {
        PowerUpUI.SetActive(true);
    }

    private void HidePowerUpUI()
    {
        PowerUpUI.SetActive(false);
    }

    private void StartBadZoneCounter()
    {
        // adjust the alpha to start the counter
        BadZoneAlphaCounter = AlphaMax;
        BadZoneAlphaCounter -= AlphaSmooth;
        GardenBadZone.SetActive(true);
    }

    private void FlashGoodZone()
    {
        // adjust the alpha
        GoodZoneAlphaCounter -= AlphaSmooth;
        if (GoodZoneAlphaCounter <= 0.0f) {
            GoodZoneAlphaCounter = AlphaMax;
        }
        // set alpha
        Image image = GardenGoodZone.GetComponent<Image>();
        Color c = image.color;
        c.a = GoodZoneAlphaCounter;
        image.color = c;
    }

    void FlashBadZone()
    {
        // adjust the alpha
        BadZoneAlphaCounter -= AlphaSmooth;
        if (BadZoneAlphaCounter <= 0.0f) {
            BadZoneAlphaCounter = AlphaMax;
            GardenBadZone.SetActive(false);
        }
        // set alpha
        Image image = GardenBadZone.GetComponent<Image>();
        Color c = image.color;
        c.a = BadZoneAlphaCounter;
        image.color = c;
    }
}
