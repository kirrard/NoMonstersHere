using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MiniDialogEffectRoot : IXmlRoot
{
    [XmlElement("Dialog")]
    public List<MiniDialogEffect> dialogEffectList;
}
