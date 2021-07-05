using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class DialogEffectRoot : IXmlRoot
{
    [XmlElement("Dialog")]
    public List<DialogEffect> dialogEffectList;
}
