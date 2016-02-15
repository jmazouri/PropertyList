using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Jmazouri.PropertyList
{
    public class ColorPropertyListEntry : PropertyListEntry<Color>
    {
        public ColorPropertyListEntry(string name, Func<Color> getter, Action<Color> setter)
        : base(name, getter, setter)
        {

        }

        public override GameObject GetGui(GameObject prefab)
        {
            var current = GameObject.Instantiate(prefab);

            current.name = Name;

            current.transform.Find("Title").GetComponent<Text>().text = Name;

            Slider redSlider = current.transform.Find("Colors/Red/RedSlider").GetComponent<Slider>();
            Slider greenSlider = current.transform.Find("Colors/Green/GreenSlider").GetComponent<Slider>();
            Slider blueSlider = current.transform.Find("Colors/Blue/BlueSlider").GetComponent<Slider>();

            Text redValue = current.transform.Find("Colors/Red/RedValue").GetComponent<Text>();
            Text greenValue = current.transform.Find("Colors/Green/GreenValue").GetComponent<Text>();
            Text blueValue = current.transform.Find("Colors/Blue/BlueValue").GetComponent<Text>();

            Image colorPreview = current.transform.Find("ColorPreview").GetComponent<Image>();

            UnityAction<float> sliderUpdate = delegate
            {
                Color toSet = new Color(redSlider.value, greenSlider.value, blueSlider.value);

                Setter.Invoke(toSet);

                redValue.text = Mathf.FloorToInt(redSlider.value * 255).ToString();
                greenValue.text = Mathf.FloorToInt(greenSlider.value * 255).ToString();
                blueValue.text = Mathf.FloorToInt(blueSlider.value * 255).ToString();

                colorPreview.color = toSet;
            };

            redSlider.onValueChanged.AddListener(sliderUpdate);
            greenSlider.onValueChanged.AddListener(sliderUpdate);
            blueSlider.onValueChanged.AddListener(sliderUpdate);

            redSlider.value = Getter.Invoke().r;
            greenSlider.value = Getter.Invoke().g;
            blueSlider.value = Getter.Invoke().b;

            redValue.text = Mathf.FloorToInt(redSlider.value * 255).ToString();
            greenValue.text = Mathf.FloorToInt(greenSlider.value * 255).ToString();
            blueValue.text = Mathf.FloorToInt(blueSlider.value * 255).ToString();

            colorPreview.color = Getter.Invoke();

            Button resetButton = current.transform.Find("ResetButton").GetComponent<Button>();
            resetButton.onClick.AddListener(delegate
            {
                redSlider.value = Default.r;
                greenSlider.value = Default.g;
                blueSlider.value = Default.b;

                redValue.text = Default.r.ToString();
                greenValue.text = Default.g.ToString();
                blueValue.text = Default.b.ToString();

                colorPreview.color = Default;

                Setter.Invoke(Default);
            });

            return current;
        }
    }
}
