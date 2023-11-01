using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makarov_Attach : MonoBehaviour
{
    public Transform RightHandAttachment; // Ссылка на точку привязки на руках NPC.
    public GameObject Makarov; // Ссылка на модель оружия.

    void Start()
    {
        // Привязываем оружие к точке привязки.
        if (RightHandAttachment != null && Makarov != null)
        {
            Makarov.transform.parent = RightHandAttachment;
            Makarov.transform.localPosition = Vector3.zero;
            Makarov.transform.localRotation = Quaternion.identity;
        }
    }
}
