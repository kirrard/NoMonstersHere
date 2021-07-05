using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MiniDialog
{
    [XmlAttribute("Id")]
    public int id;

    [XmlElement("Content")]
    public string content;
}
