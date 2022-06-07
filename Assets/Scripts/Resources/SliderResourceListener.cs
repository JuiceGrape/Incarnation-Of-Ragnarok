using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderResourceListener : MonoBehaviour
{
    public Resource resource;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        resource.OnValueChanged.AddListener(OnValueChanged);

        slider = this.GetComponent<Slider>();

        slider.maxValue = resource.maxValue;
        slider.minValue = resource.minValue;
        slider.value = resource.GetValue();
    }

    public void OnValueChanged()
    {
        slider.value = resource.GetValue();
    }
}
