using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class DialogEffect
{
    [XmlAttribute("Id")]
    public int id;

    [XmlElement("Bg")]
    public BackGround bg;

    [XmlElement("Sfx")]
    public Sfx sfx;

    [XmlArray("CharacterList")]
    [XmlArrayItem("Character")]
    public List<Character> characterList;
}
