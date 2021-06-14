using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlParser
{
    public static DialogRoot DeserializeDialog(TextAsset dialogFile)
    {
        DialogRoot dialog;

        using (StringReader stringReader = new StringReader(dialogFile.text))
        {
            dialog = (DialogRoot)new XmlSerializer(typeof(DialogRoot)).Deserialize(stringReader);
        }

        return dialog;
    }

    public static DialogEffectRoot DeserializeDialogEffect(TextAsset dialogFile)
    {
        DialogEffectRoot dialogEffect;

        using (StringReader stringReader = new StringReader(dialogFile.text))
        {
            dialogEffect = (DialogEffectRoot)new XmlSerializer(typeof(DialogEffectRoot)).Deserialize(stringReader);
        }

        return dialogEffect;
    }
}
