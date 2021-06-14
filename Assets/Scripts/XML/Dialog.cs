using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Dialog
{
    [XmlAttribute("Id")]
    public int id;

    [XmlElement("Teller")]
    public string teller;

    [XmlElement("Content")]
    public string content;
}
