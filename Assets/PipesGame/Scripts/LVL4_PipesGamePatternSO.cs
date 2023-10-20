using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPipesButtonRotation
{
    Rotation0, 
    Rotation90,
    Rotation180,
    Rotation270
}

[Serializable]
public class LVL4_PipesGameSpawnableItem 
{
    public GameObject button;
    public EPipesButtonRotation rotation;
}
[CreateAssetMenu(fileName = "New LVL4_PipesGamePattern", menuName = "IMW/PipesGamePattern", order = 1)]


public class LVL4_PipesGamePatternSO : ScriptableObject
{
    [SerializeField]
    List<LVL4_PipesGameSpawnableItem> pattern;

    public List<LVL4_PipesGameSpawnableItem> Pattern
    {
        get => pattern;
    }
    [SerializeField]
    int[] winIndexes;
     public int[] WinIndexes { 
        get => winIndexes;
     }
}