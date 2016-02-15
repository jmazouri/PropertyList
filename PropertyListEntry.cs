using System;
using UnityEngine;
using System.Collections;

namespace Jmazouri.PropertyList
{
    public abstract class PropertyListEntry
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public abstract GameObject GetGui(GameObject prefab);
    }

    public abstract class PropertyListEntry<T> : PropertyListEntry
    {
        public Func<T> Getter { get; set; }
        public Action<T> Setter { get; set; }

        public T Minimum { get; set; }
        public T Maximum { get; set; }

        public T Default { get; set; }

        public string DisplayFormat { get; set; }

        protected PropertyListEntry(string name, Func<T> getter, Action<T> setter, T minimum = default(T), T maximum = default(T))
        {
            Name = name;
            Getter = getter;
            Setter = setter;
            Minimum = minimum;
            Maximum = maximum;

            Default = getter.Invoke();
            Type = typeof(T);

            DisplayFormat = "{0}";
        }

        public override string ToString()
        {
            return Getter.Invoke().ToString();
        }
    }
}
