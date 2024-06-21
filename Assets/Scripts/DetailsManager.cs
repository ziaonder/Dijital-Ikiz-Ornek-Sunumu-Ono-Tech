using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DetailsManager : MonoBehaviour
{
    // Namings are both English and Turkish that's because project is in Turkish.
    // Did this way not to get confused at assignments.
    private int siparisKodu = 2051920, malzemeKodu = 575, siparisMiktari = 300;
    [SerializeField] private TextMeshProUGUI siparisKoduText, malzemeKoduText, siparisMiktariIndicator;
    [SerializeField] private TextMeshProUGUI OEEValue;
    [SerializeField] private Transform siparisMiktariTransform;
    [SerializeField] private Image OEEImage;
    private float OEE_Timer = 60f, OEEValuef = 82f;
    private float secondTimer = 1f;
    [SerializeField] private Button button;
    private bool isAnimationPlaying = false;
    private struct PerSecondValues
    {
        public float baseValue;
        public float currentValue;

        public void Setup(float value)
        {
            baseValue = value;
            currentValue = value;
        }

        public void AssignNewValue(float rate)
        {
            double value = Math.Round(baseValue * rate / 100, 2);
            float valuef = (float)value;
            currentValue = baseValue + valuef;
        }
    }

    private PerSecondValues ocakSicakligi, ocakAgirligi, ustKalipSicakligi, altKalipSicakligi, ivme, basinc;
    [SerializeField] private TextMeshProUGUI ocakSicakTmPro, ocakAgirlikTmPro, 
        ustKalipTmPro, altKalipTmPro, ivmeTmPro, basincTmPro;
    
    void Start()
    {
        ocakSicakligi.Setup(500f);
        ocakAgirligi.Setup(800f);
        ustKalipSicakligi.Setup(25f);
        altKalipSicakligi.Setup(15f);
        ivme.Setup(.15f);
        basinc.Setup(.250f);
        UpdateEveryUI();
        button.onClick.AddListener(OpenURL);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            siparisKodu++;
            malzemeKodu++;
            if(siparisMiktari < 800)
                siparisMiktari += 50;
            UpdateXUI();
        }

        if (!isAnimationPlaying && Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(PlayAnimation());
        }

        CountOEETimerThenAct();

        if (secondTimer <= 0f)
        {
            UpdateValuesPerSecond();
            secondTimer = 1f; 
        }
        else
            secondTimer -= Time.deltaTime;
    }

    // Updates when the X button is pressed.
    private void UpdateXUI()
    {
        siparisKoduText.text = "O-" + siparisKodu.ToString();
        malzemeKoduText.text = "E-" + malzemeKodu.ToString();
        siparisMiktariTransform.localScale = new Vector3(siparisMiktari / 800f, 1f, 1f);
        siparisMiktariIndicator.text = $"{siparisMiktari} / 800";
    }

    private void UpdateEveryUI()
    {
        siparisKoduText.text = "O-" + siparisKodu.ToString();
        malzemeKoduText.text = "E-" + malzemeKodu.ToString();
        siparisMiktariTransform.localScale = new Vector3(siparisMiktari / 800f, 1f, 1f);
        siparisMiktariIndicator.text = $"{siparisMiktari} / 800";
        OEEImage.fillAmount = OEEValuef / 100f;
        OEEValue.text = "%" + OEEValuef.ToString();
    }

    private void CountOEETimerThenAct()
    {
        OEE_Timer -= Time.deltaTime;
        if (OEE_Timer <= 0)
        {
            OEE_Timer = 60f;
            // OEEValuef is float but assignment is int. This is to prevent displaying so many floating points.
            // I just assigned int values as it is not clarified. Here is a way of handling so many floating points;
            // string formattedNumber = $"{OEEValuef:F2}"; 
            OEEValuef =  UnityEngine.Random.Range(75, 86);
            OEEImage.fillAmount = OEEValuef / 100f;
            OEEValue.text = "%" + OEEValuef.ToString();
        }
    }

    private void UpdateValuesPerSecond()
    {
        float rate = UnityEngine.Random.Range(-10f, 10f);
        ocakSicakligi.AssignNewValue(rate);
        ocakAgirligi.AssignNewValue(rate);
        ustKalipSicakligi.AssignNewValue(rate);
        altKalipSicakligi.AssignNewValue(rate);
        ivme.AssignNewValue(rate);
        basinc.AssignNewValue(rate);

        UpdatePerSecondUI();
    }

    private void UpdatePerSecondUI()
    {
        ocakSicakTmPro.text = ocakSicakligi.currentValue.ToString() + "°C";
        ocakAgirlikTmPro.text = ocakAgirligi.currentValue.ToString() + " kg";
        ustKalipTmPro.text = ustKalipSicakligi.currentValue.ToString() + "°C";
        altKalipTmPro.text = altKalipSicakligi.currentValue.ToString() + "°C";
        ivmeTmPro.text = ivme.currentValue.ToString() + " mm/s<sup>2</sup>";
        basincTmPro.text = basinc.currentValue.ToString() + " bar";
    }

    private void OpenURL()
    {
        Application.OpenURL("https://www.google.com");
    }

    // Preferred a position and rotation change animation for the object. Like to make animations through scripts.
    private IEnumerator PlayAnimation()
    {
        isAnimationPlaying = true;
        Vector3 startPos = transform.position;
        Vector3 startDir = transform.localEulerAngles;
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * 2;
            if (timer <= .33f)
            {
                transform.position = Vector3.Lerp(startPos, startPos + Vector3.up / 4,
                    Normalize(0f, .33f, timer));

                transform.rotation = Quaternion.Lerp(Quaternion.Euler(startDir), 
                    Quaternion.Euler(startDir + Vector3.up * 15), Normalize(0f, .33f, timer));
            }
            else if (timer <= .66f)
            {
                transform.position = Vector3.Lerp(startPos + Vector3.up / 4, startPos - Vector3.up / 4,
                    Normalize(.33f, .66f, timer));

                transform.rotation = Quaternion.Lerp(Quaternion.Euler(startDir + Vector3.up * 15),
                    Quaternion.Euler(startDir + Vector3.up * -15), Normalize(.33f, .66f, timer));
            }
            else
            {
                transform.position = Vector3.Lerp(startPos - Vector3.up / 4, startPos,
                    Normalize(.66f, 1f, timer));

                transform.rotation = Quaternion.Lerp(Quaternion.Euler(startDir + Vector3.up * -15),
                    Quaternion.Euler(startDir), Normalize(.66f, 1f, timer));
            }

            yield return null;
        }

        isAnimationPlaying = false;
    }

    private float Normalize(float min, float max, float value)
    {
        return (value - min) / (max - min);
    }
}
