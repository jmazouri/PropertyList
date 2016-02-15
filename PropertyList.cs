using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jmazouri.PropertyList;
using UnityEngine.Events;
using UnityEngine.UI;

public class PropertyList : MonoBehaviour
{
    private List<PropertyListEntry> TrackedObjects = new List<PropertyListEntry>(0);

    public List<TypeTemplate> TypeTemplates = new List<TypeTemplate>(0); 

    [Serializable]
    public struct TypeTemplate
    {
        public string TypeName;
        public GameObject Template;
    }

    public bool DumpDebug;

    public static PropertyList Default
    {
        get
        {
            return FindObjectOfType<PropertyList>();
        }
    }

    void ClearChildren(Transform trans)
    {
        for (int i = trans.childCount - 1; i >= 0; --i)
        {
            var child = trans.GetChild(i).gameObject;
            if (child.name == "TitlePanel") { continue; }
            Destroy(child);
        }
    }

    void UpdatePropertyList()
    {
        ClearChildren(transform);

        for (int i = 0; i < TrackedObjects.Count; i++)
        {
            PropertyListEntry entry = TrackedObjects[i];

            for (int t = 0; t < TypeTemplates.Count; t++)
            {
                if (TypeTemplates[t].TypeName == entry.Type.Name)
                {
                    entry.GetGui(TypeTemplates[t].Template).transform.SetParent(transform, false);
                    break;
                }
            }

        }
    }

    // Use this for initialization
	void Start()
    {
	    UpdatePropertyList();
	}

    void OnGUI()
    {
        if (!DumpDebug)
        {
            return;
        }

        int y = 10;

        foreach (PropertyListEntry entry in TrackedObjects)
        {
            GUI.Label(new Rect(10, y, 300, 30), entry.Name + ": " + entry.ToString());
            y += 15;
        }

    }

    public void TrackProperty<T>(PropertyListEntry<T> newEntry)
    {
        bool continueOn = true;

        for (int i = 0; i < TrackedObjects.Count; i++)
        {
            PropertyListEntry entry = TrackedObjects[i];

            if (entry.Name == newEntry.Name)
            {
                continueOn = false;
                break;
            }
        }

        if (!continueOn)
        {
            return;
        }

        TrackedObjects.Add(newEntry);
        TrackedObjects = TrackedObjects.OrderBy(d => d.Type.Name).ThenBy(d=>d.Name).ToList();

        UpdatePropertyList();
    }

    public void StopTrackingProperty(string name)
    {
        if (TrackedObjects.Any(d => d.Name == name))
        {
            TrackedObjects.Remove(TrackedObjects.First(d => d.Name == name));
        }

        TrackedObjects = TrackedObjects.OrderBy(d => d.Type.Name).ThenBy(d => d.Name).ToList();

        UpdatePropertyList();
    }
	
	// Update is called once per frame
	void Update()
	{
	    float modifier = Mathf.Clamp(Input.mousePosition.x - GetComponent<RectTransform>().sizeDelta.x, 0, GetComponent<RectTransform>().sizeDelta.x - 15);

        transform.position = Vector3.Lerp(transform.position, new Vector3(20 - modifier, 20), Time.deltaTime * 8);
	}

    private static bool IsNumericType(Type t)
    {
        switch (Type.GetTypeCode(t))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }
}
