using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private float OEE_Timer = 60f, OEEValuef = 82f, odaSicakligiValue = 500f, ocakAgirligiValue = 800f;
    private float ustKalipSicakligiValue = 25, altKalipSicakligi = 15f, ivme = .15f, basinc = .250f;
    
    void Start()
    {
        UpdateEveryUI();
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

        CountOEETimerThenAct();
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
            OEEValuef =  Random.Range(75, 86);
            OEEImage.fillAmount = OEEValuef / 100f;
            OEEValue.text = "%" + OEEValuef.ToString();
        }
    }
}
