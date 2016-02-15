using System;
using UnityEngine;
using UnityEngine.UI;

namespace Jmazouri.PropertyList
{
    public class BooleanPropertyListEntry : PropertyListEntry<bool>
    {
        public BooleanPropertyListEntry(string name, Func<bool> getter, Action<bool> setter)
        : base(name, getter, setter)
        {

        }

        public override GameObject GetGui(GameObject prefab)
        {
            var current = GameObject.Instantiate(prefab);

            current.name = Name;

            current.transform.Find("Toggle/Label").GetComponent<Text>().text = Name;
            Toggle currentToggle = current.transform.Find("Toggle").GetComponent<Toggle>();
            currentToggle.isOn = Getter();

            currentToggle.onValueChanged.AddListener(delegate
            {
                Setter.Invoke(currentToggle.isOn);
            });

            return current;
        }
    }
}
