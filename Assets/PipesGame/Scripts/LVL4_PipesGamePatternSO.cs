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
    [SerializeField] GameObject angledPrefab;
    [SerializeField] GameObject straightPrefab;
    [SerializeField] GameObject emptyPrefab;

    public List<LVL4_PipesGameSpawnableItem> Pattern
    {
        get => pattern;
    }
    [SerializeField]
    int[] winIndexes;
     public int[] WinIndexes { 
        get => winIndexes;
     }

    public LVL4_PipesGamePatternSO() {
        pattern = new List<LVL4_PipesGameSpawnableItem> ();
        for (int i = 0; i < 16; i++) {
            LVL4_PipesGameSpawnableItem adding = new LVL4_PipesGameSpawnableItem ();
            pattern.Add(adding);
        }
        MazeGenerator pipePattern = new MazeGenerator();
        winIndexes = pipePattern.getWinIndices();
        Debug.Log(pattern.Count);
        for (int i = 0; i < pattern.Count; i++) {
            pattern[i].rotation = pipePattern.getButtonRotation(i);
            EPipeButtonType buttonType = pipePattern.getButton(i);
            switch (buttonType) {
                case EPipeButtonType.Angled:
                    pattern[i].button = angledPrefab;
                    break;
                case EPipeButtonType.Straight:
                    pattern[i].button = straightPrefab;
                    break;
                case EPipeButtonType.Empty:
                    pattern[i].button = emptyPrefab;
                    break;
                default:
                    pattern[i].button = emptyPrefab;
                    break;
            }
        }

    }
}