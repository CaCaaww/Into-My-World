using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum Side {
    None,
    Left,
    Right,
    Top,
    Bottom
}

public class MazeNode {
    [SerializeField] public Vector2 position;
    [SerializeField] public EPipeButtonType pipeType;
    [SerializeField] public Side input;
    [SerializeField] public Side output;

    [SerializeField] GameObject[] walls;
    [SerializeField] MeshRenderer floor;
    public MazeNode() {
 
    }
}
