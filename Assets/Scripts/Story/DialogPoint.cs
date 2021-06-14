using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPoint : MonoBehaviour
{
    public bool isAutoStart;
    public bool isMiniDialog;
    public TextAsset storyFile;
    public TextAsset effectFile;

    private DialogRoot dialog;
    private DialogEffectRoot dialogEffect;

    private void Start()
    {
        dialog = XmlParser.DeserializeDialog(storyFile);
        dialogEffect = XmlParser.DeserializeDialogEffect(effectFile);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DialogController.instance.ShowDialog(dialog, dialogEffect);
        }
    }
}