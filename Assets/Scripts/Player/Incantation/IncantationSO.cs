using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Incantation/NewIncantation", fileName = "Incantation")]
public class IncantationSO : ScriptableObject
{
    public string incantationName = "Repeater";         //주문 이름
    public GameObject incantationPrefab;                //프리팹

    public int level = 1;
    public float damage = 5f;                           //피해량
    public float fireRate = 0.5f;
}
