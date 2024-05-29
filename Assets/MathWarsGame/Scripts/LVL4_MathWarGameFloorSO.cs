using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New LVL4_MathWarGameFloor", menuName = "IMW/MathWarGameFloor", order = 1)]
public class LVL4_MathWarGameFloorSO : ScriptableObject
{
    public NumberStruct[] stages;
}
[System.Serializable]
public struct NumberStruct
{
    public int[] floors;
}