using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    private static ElementManager _instance;
    public static ElementManager Instance { get { return _instance; } }
    
    public List<Element> elements;

    private Dictionary<(string, string), Element> combinations;
    
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
            { ("Fire", "Earth"), FindElement("Magma") },
        };
    }

    public Element CombineElements((string, string) names)
    {
        return combinations[names];
    }

    private Element FindElement(string name)
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
}
