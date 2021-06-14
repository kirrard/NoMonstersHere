using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Character
{
    [XmlAttribute("Name")]
    public string name;

    [XmlAttribute("Sprite")]
    public string sprite;

    [XmlAttribute("Pos")]
    public CharacterPos pos;

    [XmlAttribute("Telling")]
    public bool telling;
}
