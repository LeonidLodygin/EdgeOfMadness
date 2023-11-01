using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makarov_Attach : MonoBehaviour
{
    public Transform RightHandAttachment; // ������ �� ����� �������� �� ����� NPC.
    public GameObject Makarov; // ������ �� ������ ������.

    void Start()
    {
        // ����������� ������ � ����� ��������.
        if (RightHandAttachment != null && Makarov != null)
        {
            Makarov.transform.parent = RightHandAttachment;
            Makarov.transform.localPosition = Vector3.zero;
            Makarov.transform.localRotation = Quaternion.identity;
        }
    }
}
