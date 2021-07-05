using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPoint : MonoBehaviour
{
    public bool isAutoStart;
    public bool isMiniDialog;
    public bool isOneOff;

    public TextAsset storyFile;
    public TextAsset effectFile;

    private IXmlRoot dialog;
    private IXmlRoot dialogEffect;
    private bool oneOffExecuted;

    private void Start()
    {
        if (isMiniDialog)
        {
            dialog = XmlParser<MiniDialogRoot>.DeserializeXml(storyFile);
            dialogEffect = XmlParser<MiniDialogEffectRoot>.DeserializeXml(effectFile);
        }
        else
        {
            dialog = XmlParser<DialogRoot>.DeserializeXml(storyFile);
            dialogEffect = XmlParser<DialogEffectRoot>.DeserializeXml(effectFile);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isOneOff && !oneOffExecuted)
        {
            if (!isMiniDialog)
            {
                if (isAutoStart)
                {
                    DialogController.instance.ShowDialog(dialog as DialogRoot, dialogEffect as DialogEffectRoot);
                    oneOffExecuted = true;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        DialogController.instance.ShowDialog(dialog as DialogRoot, dialogEffect as DialogEffectRoot);
                        oneOffExecuted = true;
                    }
                }
            }
            else
            {
                if (isAutoStart)
                {
                    DialogController.instance.ShowMiniDialog(dialog as MiniDialogRoot, dialogEffect as MiniDialogEffectRoot);
                    oneOffExecuted = true;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        DialogController.instance.ShowMiniDialog(dialog as MiniDialogRoot, dialogEffect as MiniDialogEffectRoot);
                        oneOffExecuted = true;
                    }
                }
            }
        }
        else if (!isOneOff)
        {
            if (!isMiniDialog)
            {
                if (isAutoStart)
                {
                    DialogController.instance.ShowDialog(dialog as DialogRoot, dialogEffect as DialogEffectRoot);
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        DialogController.instance.ShowDialog(dialog as DialogRoot, dialogEffect as DialogEffectRoot);
                    }
                }
            }
            else
            {
                if (isAutoStart)
                {
                    DialogController.instance.ShowMiniDialog(dialog as MiniDialogRoot, dialogEffect as MiniDialogEffectRoot);
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        DialogController.instance.ShowMiniDialog(dialog as MiniDialogRoot, dialogEffect as MiniDialogEffectRoot);
                    }
                }
            }
        }
    }
}