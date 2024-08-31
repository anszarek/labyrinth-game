using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmotiosGameController : MonoBehaviour
{
    public TextMeshProUGUI HeartRateValue;

    void Update()
    {
        if (HeartRateValue != null) {
            int currentValue;
            if(int.TryParse(HeartRateValue.text, out currentValue)) {

                //if (currentValue > ) {

                //}
            }
        }
    }
}
