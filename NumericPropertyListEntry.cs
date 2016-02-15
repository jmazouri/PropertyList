using System;
using UnityEngine;
using UnityEngine.UI;

namespace Jmazouri.PropertyList
{
    public class NumericPropertyListEntry : PropertyListEntry<float>
    {
        public NumericPropertyListEntry(string name, Func<float> getter, Action<float> setter, float minimum, float maximum)
        : base(name, getter, setter, minimum, maximum)
        {
            DisplayFormat = "{0:0.##}";
        }

        public override GameObject GetGui(GameObject prefab)
        {
            var current = GameObject.Instantiate(prefab);

            current.name = Name;

            current.transform.Find("Title").GetComponent<Text>().text = Name;

            Slider currentSlider = current.GetComponentInChildren<Slider>();

            currentSlider.minValue = Minimum;
            currentSlider.maxValue = Maximum;

            currentSlider.value = Getter.Invoke();

            Text currentValue = current.transform.Find("CurrentValue").GetComponent<Text>();
            currentValue.text = String.Format(DisplayFormat, Getter.Invoke());

            currentSlider.onValueChanged.AddListener(delegate
            {
                Slider localSlider = currentSlider;
                Text localCurrentValue = currentValue;

                Setter.Invoke(localSlider.value);
                localCurrentValue.text = String.Format(DisplayFormat, Getter.Invoke());
            });

            Button resetButton = current.transform.Find("ResetButton").GetComponent<Button>();
            resetButton.onClick.AddListener(delegate
            {
                Slider localSlider = currentSlider;
                Text localCurrentValue = currentValue;

                localSlider.value = Default;
                localCurrentValue.text = Default.ToString();
                Setter.Invoke(Default);
            });

            return current;
        }
    }
}

