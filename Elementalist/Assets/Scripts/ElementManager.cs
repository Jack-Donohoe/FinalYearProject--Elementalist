using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    private static ElementManager _instance;
    public static ElementManager Instance { get { return _instance; } }
    
    public List<Element> elements;

    private Dictionary<(string, string), Element> combinations;
    
    private Dictionary<(string, string), float> damage_multipliers;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        combinations = new Dictionary<(string, string), Element>()
        {
            { ("Fire", "Water"), FindElement("Steam") },
            { ("Water", "Fire"), FindElement("Steam") },
            { ("Fire", "Earth"), FindElement("Magma") },
            { ("Earth", "Fire"), FindElement("Magma") },
            { ("Fire", "Air"), FindElement("Lightning") },
            { ("Air", "Fire"), FindElement("Lightning") },
            { ("Water", "Earth"), FindElement("Wood") },
            { ("Earth", "Water"), FindElement("Wood") },
            { ("Water", "Air"), FindElement("Ice") },
            { ("Air", "Water"), FindElement("Ice") },
            { ("Earth", "Air"), FindElement("Sand") },
            { ("Air", "Earth"), FindElement("Sand") },
        };

        damage_multipliers = new Dictionary<(string, string), float>()
        {
            { ("Fire", "Fire"), 0.5f }, { ("Fire", "Water"), 0.5f }, { ("Fire", "Earth"), 2f }, { ("Fire", "Air"), 1f },
            { ("Water", "Water"), 0.5f }, { ("Water", "Fire"), 2f }, { ("Water", "Earth"), 1f }, { ("Water", "Air"), 1f },
            { ("Earth", "Earth"), 0.5f }, { ("Earth", "Fire"), 0.5f }, { ("Earth", "Water"), 1f }, { ("Earth", "Air"), 2f },
            { ("Air", "Air"), 0.5f }, { ("Air", "Fire"), 1f }, { ("Air", "Water"), 1f }, { ("Air", "Earth"), 0.5f },
            { ("Steam", "Fire"), 2f }, { ("Steam", "Water"), 1f }, { ("Steam", "Earth"), 2f }, { ("Steam", "Air"), 0.5f },
            { ("Magma", "Fire"), 1f }, { ("Magma", "Water"), 0.5f }, { ("Magma", "Earth"), 2f }, { ("Magma", "Air"), 2f },
            { ("Lightning", "Fire"), 1f }, { ("Lightning", "Water"), 2f }, { ("Lightning", "Earth"), 0.5f }, { ("Lightning", "Air"), 2f },
            { ("Wood", "Fire"), 0.5f }, { ("Wood", "Water"), 2f }, { ("Wood", "Earth"), 2f }, { ("Wood", "Air"), 1f },
            { ("Ice", "Fire"), 0.5f }, { ("Ice", "Water"), 2f }, { ("Ice", "Earth"), 1f }, { ("Ice", "Air"), 2f },
            { ("Sand", "Fire"), 2f }, { ("Sand", "Water"), 2f }, { ("Sand", "Earth"), 0.5f }, { ("Sand", "Air"), 1f },
        };
    }

    public Element CombineElements((string, string) names)
    {
        return combinations[names];
    }

    public Element FindElement(string name)
    {
        foreach (var element in elements)
        {
            if (element.GetName() == name)
            {
                return element;
            }
        }

        return null;
    }

    public float GetDamageMultiplier((string, string) names)
    {
        return damage_multipliers[names];
    }
}
