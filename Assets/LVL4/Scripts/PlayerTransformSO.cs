using UnityEngine;

[CreateAssetMenu(fileName = "Player Transform", menuName = "IMW/Player Transform")]
public class PlayerTransformSO : ScriptableObject
{
    private Transform playerTransform;

    public void Set(Transform transform)
    {
        playerTransform = transform;
    }

    public Vector3 Position
    {
        get { return playerTransform.position; }
    }
}
