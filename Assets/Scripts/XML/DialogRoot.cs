using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class DialogRoot
{
    [XmlElement("Dialog")]
    public List<Dialog> dialogList;
}
