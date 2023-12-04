using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapModuleContact
{
    [SerializeField] string _contactType;
    [SerializeField] List<string> _unsuitableContactTypes;

    public string ContactType => _contactType;
    public List<string> UnsuitableContactTypes => _unsuitableContactTypes;

    public bool MatchingContacts(MapModuleContact otherContact)
    {
        return !otherContact.ContactType.Contains(ContactType) && !UnsuitableContactTypes.Contains(otherContact.ContactType);
    }
}

public static class ContactDirectionInMap
{
    public static Vector2 Forward => new Vector2(0, 1);
    public static Vector2 Back => new Vector2(0,-1);
    public static Vector2 Right => new Vector2(1, 0);
    public static Vector2 Left => new Vector2(-1, 0);
}
