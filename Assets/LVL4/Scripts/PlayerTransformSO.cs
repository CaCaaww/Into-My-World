using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "IMW/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    private Transform transform;
    private GameObject lookingAt;

    public Transform Transform { get; set; }

    public GameObject LookingAt { get; set; }
}
