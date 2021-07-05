using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MiniDialogEffect
{
    [XmlAttribute("Id")]
    public int id;

    [XmlElement("Sfx")]
    public Sfx sfx;

    [XmlElement("Portrait")]
    public string portrait;

    [XmlElement("Pos")]
    public MiniDlgPanelPos pos;
}
