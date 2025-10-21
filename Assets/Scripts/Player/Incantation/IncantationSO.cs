using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Incantation/NewIncantation", fileName = "Incantation")]
public class IncantationSO : ScriptableObject
{
    public string incantationName = "Repeater";         //�ֹ� �̸�
    public GameObject incantationPrefab;                //������

    public float damage = 5f;                           //���ط�
}
