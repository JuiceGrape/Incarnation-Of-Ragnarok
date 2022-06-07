using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextResourceListener : MonoBehaviour
{
    public Resource resource;

    public bool AddsName = false;

    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        OnResourceChanged();
        resource.OnValueChanged.AddListener(OnResourceChanged);
    }

    void OnResourceChanged()
    {
        text.text = resource.GetValue().ToString();

        if (AddsName)
        {
            text.text += " " + resource.name;
        }
    }
}
