using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Sfx
{
    [XmlAttribute("Src")]
    public string src;

    [XmlAttribute("Loop")]
    public SfxLoop loop;
}
