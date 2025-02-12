using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chickincontroler : MonoBehaviour, interact
{
    [SerializeField] Dialog dialog;
    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }
}
