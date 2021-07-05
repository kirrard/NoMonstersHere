using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MiniDialogRoot : IXmlRoot
{
    [XmlElement("Dialog")]
    public List<MiniDialog> dialogList;
}
