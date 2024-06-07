using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MazeGenerator{

    //[SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;
    [SerializeField] List<int> winIndices;
    [SerializeField] List<EPipesButtonRotation> buttonRotations;
    private List<MazeNode> nodes;

    // Start is called before the first frame update
    void Start() {
        
    }
    public MazeGenerator() {
        winIndices = new List<int>();
        buttonRotations = new List<EPipesButtonRotation>();
        nodes = new List<MazeNode>();
        mazeSize = new Vector2Int(4,4);
        generateMaze(mazeSize);
    }
    public int[] getWinIndices() {
        int[] result = new int[winIndices.Count];
        for (int i = 0; i < winIndices.Count; i++) {
            result[i] = winIndices[i];
        }
        return result;
    }
    public EPipeButtonType getButton(int index) {
        return nodes[index].pipeType;
        

    }
    public EPipesButtonRotation getButtonRotation(int indexInWin) {
        int actualIndex = winIndices.IndexOf(indexInWin);
        if (actualIndex != -1) {
            return buttonRotations[actualIndex];
        } else {
            return EPipesButtonRotation.Rotation0;
        }
    }
    public EPipeButtonType getCurrentNodeType(Vector2 prevPos, Vector2 nextPos) {
        // compare rows
        float dx = nextPos.x - prevPos.x;

        // compare columns
        float dy = nextPos.y - prevPos.y;

        if (dx != 0 && dy != 0) {
            return EPipeButtonType.Angled;
        };

        return EPipeButtonType.Straight;
    }

    public Side getPipeInput(Vector2 position1, Vector2 position2) {
        float dy = position2.y - position1.y;

        float dx = position2.x - position1.x;

        if (dy == 0) {
            if (dx > 0) {
                return Side.Top;
            } else {
                return Side.Bottom;
            }
        } else {
            if (dy > 0) {
                return Side.Left;
            } else {
                return Side.Right;
            }
        }
    }

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

    public void generateMaze(Vector2Int size) {

        //Debug.Log("size.x : " + size.x);
        // Create Nodes
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                //Debug.Log(x + " | " + y);
                //Vector3 nodePos = new Vector3(x - (size.x / 2f), 0, y - (size.y / 2f));
                //MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                MazeNode newNode = new MazeNode();
                newNode.position = new Vector2(x, y);
                nodes.Add(newNode);
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        // Choose starting node
        //Debug.Log(nodes.Count);
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
