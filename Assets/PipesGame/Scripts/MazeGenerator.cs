using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
public enum Side {
    None,
    Left,
    Right,
    Top,
    Bottom
}
/// <summary>
/// objects contained in the maze. Each of these stores the information for an individual pipe.
/// </summary>
public class MazeNode {
    [SerializeField] public Vector2 position; // row & col coordinates for the pipe, represented as a vector2
    [SerializeField] public EPipeButtonType pipeType; // the type of pipe; straight, angled, or empty
    [SerializeField] public Side input; // what side -- left, right, top, or bottom -- of the tile of the pipe is the side that the previous pipe touches
    [SerializeField] public Side output; // same sorta thing as above, but it's where the pipe after this one connects
}
public class MazeGenerator{

    //[SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize; // the length in tiles of one side of the square that represents the entirety of the board
    [SerializeField] List<int> winIndices; // a list on indexes of the pipes, in the order that they need to be traversed in order ti
    // traverse from start to end of the puzzle.
    [SerializeField] List<EPipesButtonRotation> buttonRotations; // corresponds to the winIndices but instead holds the rotations that
    // those pipes need to be.
    private List<MazeNode> nodes; // list of the MazeNodes that make up the entirety of the board. In order of left->right, top->bottom.

    /// <summary>
    /// Constructor for MazeGenerator. Instantiates the winIndices list, the buttonRotations list, the nodes list, and sets the mazeSize.
    /// After that it calss the generateMaze() function to get a random maze.
    /// </summary>
    public MazeGenerator() {
        winIndices = new List<int>();
        buttonRotations = new List<EPipesButtonRotation>();
        nodes = new List<MazeNode>();
        mazeSize = new Vector2Int(4,4);
        generateMaze(mazeSize);
    }
    /// <summary>
    /// returns the list of winIndices int int[] form.
    /// </summary>
    public int[] getWinIndices() {
        int[] result = new int[winIndices.Count];
        for (int i = 0; i < winIndices.Count; i++) {
            result[i] = winIndices[i];
        }
        return result;
    }
    /// <summary>
    /// takes the index of the button in the nodesList and returns what type of pipe it is; straight, angled, or empty.
    /// </summary>
    public EPipeButtonType getButton(int index) {
        return nodes[index].pipeType;
    }
    /*public Side getInput(int index) {
        return nodes[index].input;
    }
    public Side getOutput(int index) {
        return nodes[index].output;
    }*/
    
    /// <summary>
    /// takes the index of a button in the nodesList.
    /// Makes sure it's a node required for the winList, then fetches the rotation of that button and returns that.
    /// Returns an arbitrary rotation if it's an empty button.
    /// </summary>
    public EPipesButtonRotation getButtonRotation(int indexInWin) {
        int actualIndex = winIndices.IndexOf(indexInWin);
        if (actualIndex != -1) {
            return buttonRotations[actualIndex];
        } else {
            return EPipesButtonRotation.Rotation0;
        }
    }
    /// <summary>
    /// takes two pipe positions and returns the type of pipe it must be according to how it needs to translate from
    /// the previous position to the next position.
    /// </summary>
    public EPipeButtonType getCurrentNodeType(Vector2 prevPos, Vector2 nextPos) {
        // compare rows
        float dx = nextPos.x - prevPos.x;

        // compare columns
        float dy = nextPos.y - prevPos.y;

        // if it changes in both x and y values, then we know the pipe is angled.
        if (dx != 0 && dy != 0) {
            return EPipeButtonType.Angled;
        };
        // else it must be a straight pipe. There will never be an empty pipe that comes from this because 
        // these are all pipes in the path.
        return EPipeButtonType.Straight;
    }
    /// <summary>
    /// takes two pipe positions (either the previous pipe and the current pipe or the next pipe and the current pipe)
    /// returns what side of the current pipe is being used. We use this information to calculate the rotation for the pipe.
    /// </summary>
    public Side getPipeInput(Vector2 position1, Vector2 position2) {
        // difference in x axis
        float dy = position2.y - position1.y;
        // difference in y axis
        float dx = position2.x - position1.x;
        // it will only translate in the x or y, never in both. So we check which one it is
        if (dy == 0) {
            if (dx > 0) { // if it translates in the x and that is a positive translation
                return Side.Top;
            } else {
                return Side.Bottom;
            }
        } else {
            if (dy > 0) { // if it translates in the y and that is a positive translation
                return Side.Left;
            } else {
                return Side.Right;
            }
        }
    }
    /// <summary>
    /// takes the two sides of the pipe that are being used and returns what rotation that pipe needs to be for the solution.
    /// </summary>
    public EPipesButtonRotation getPipeRotation(Side input, Side output) {
        switch (input) {
            case Side.Left:
                if (output == Side.Right) { return EPipesButtonRotation.Rotation90; }
                if (output == Side.Top) { return EPipesButtonRotation.Rotation270; }
                if (output == Side.Bottom) { return EPipesButtonRotation.Rotation0; }
                break;
            case Side.Right:
                if (output == Side.Left) return EPipesButtonRotation.Rotation90;
                if (output == Side.Top) return EPipesButtonRotation.Rotation180;
                if (output == Side.Bottom) return EPipesButtonRotation.Rotation90;
                break;
            case Side.Top:
                if (output == Side.Bottom) return EPipesButtonRotation.Rotation0;
                if (output == Side.Left) return EPipesButtonRotation.Rotation270;
                if (output == Side.Right) return EPipesButtonRotation.Rotation180;
                break;
            case Side.Bottom:
                if (output == Side.Top) return EPipesButtonRotation.Rotation0;
                if (output == Side.Left) return EPipesButtonRotation.Rotation0;
                if (output == Side.Right) return EPipesButtonRotation.Rotation90;
                break;
        }
        // Should never get here
        Debug.Log("SOMETHING WENT WRONG");
        return EPipesButtonRotation.Rotation0;
    }
    /// <summary>
    /// Code that generates the Maze. 
    /// </summary>
    public void generateMaze(Vector2Int size) {
        // Create the blank nodes that will be traversed later.
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) { 
                MazeNode newNode = new MazeNode();
                newNode.position = new Vector2(x, y);
                nodes.Add(newNode);
            }
        }
        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();
        // Choose starting node
        currentPath.Add(nodes[0]);

        while (!(currentPath[currentPath.Count - 1].Equals(nodes[15]))) {
            // Check nodes next to current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            // Check node to the right of the current node
            if (currentNodeX < size.x - 1) {
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y])) {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            // Check node to the left of the current node
            if (currentNodeX > 0) {
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y])) {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            // Check node above the current node
            if (currentNodeY < size.y - 1) {
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1])) {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            // Check the node below the current node
            if (currentNodeY > 0) {
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1])) {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // If there is an available neighbor, go to a random one
            if (possibleDirections.Count > 0) {
                // Choose Next Node
                //int chosenDirection = Random.Range(0, possibleDirections.Count);
                int chosenDirection = RandomNumberGenerator.GetInt32(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                MazeNode currentNode = currentPath[currentPath.Count - 1];

                // Top left node edge case
                //Debug.Log(currentNode.position);
                if (currentNode.position == (new Vector2(0, 0))) {
                    currentNode.pipeType = getCurrentNodeType(new Vector2(0, -1), chosenNode.position);
                    currentNode.input = getPipeInput(new Vector2(0, -1), currentNode.position);
                    currentNode.output = getPipeInput(chosenNode.position, currentNode.position);
                }
                // General case
                else {
                    currentNode.pipeType = getCurrentNodeType(currentPath[currentPath.Count - 2].position, chosenNode.position);
                    currentNode.input = getPipeInput(currentPath[currentPath.Count - 2].position, currentNode.position);
                    currentNode.output = getPipeInput(chosenNode.position, currentNode.position);
                }
                // Bottom Right node edge case
                if (chosenNode.position == new Vector2(3, 3)) {
                    chosenNode.pipeType = getCurrentNodeType(currentNode.position, new Vector2(3, 4));
                    chosenNode.input = getPipeInput(currentNode.position, chosenNode.position);
                    chosenNode.output = getPipeInput(new Vector2(3, 4), chosenNode.position);
                }



                currentPath.Add(chosenNode);
            }

            // No neighbors available. Backtrack to a space that has available neighbors
            else {
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath[currentPath.Count - 1].pipeType = EPipeButtonType.Empty;
                currentPath[currentPath.Count - 1].input = currentPath[currentPath.Count - 1].output = Side.None;
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }

        // Path is generated, map the path to winIndices
        foreach (MazeNode node in currentPath) {
            winIndices.Add(nodes.IndexOf(node));
            buttonRotations.Add(getPipeRotation(node.input, node.output));
        }
    }
}
