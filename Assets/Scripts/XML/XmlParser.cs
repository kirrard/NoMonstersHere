using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlParser<T>
{
    public static T DeserializeXml(TextAsset dialogFile)
    {
        T dialog;

        using (StringReader stringReader = new StringReader(dialogFile.text))
        {
            dialog = (T)new XmlSerializer(typeof(T)).Deserialize(stringReader);
        }

        return dialog;
    }
}
