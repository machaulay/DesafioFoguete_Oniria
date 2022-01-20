using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gasolina : MonoBehaviour
{
    public Slider slider;

    public void SetMaxGas(int gas)
    {
        slider.maxValue = gas;
        slider.value = gas;
    }
    public void SetGas(int gas)
    {
        slider.value = gas;
    }
}
