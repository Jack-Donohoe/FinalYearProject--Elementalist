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
        };

        damage_multipliers = new Dictionary<(string, string), float>()
        {
            { ("Fire", "Fire"), 0.5f },
            { ("Fire", "Water"), 0.5f },
            { ("Fire", "Earth"), 2f },
            { ("Water", "Water"), 0.5f },
            { ("Water", "Fire"), 2f },
            { ("Water", "Earth"), 1f },
            { ("Earth", "Earth"), 0.5f },
            { ("Earth", "Fire"), 0.5f },
            { ("Earth", "Water"), 1f },
            { ("Steam", "Water"), 2f },
            { ("Steam", "Fire"), 2f },
            { ("Steam", "Earth"), 1f }
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
