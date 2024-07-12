using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField, Tooltip("The GameObject that this object will look at")]
    private GameObject objectToLookAt;
    [SerializeField]
    private bool flip;
    [SerializeField]
    private bool rotateX;

    void Update()
    {
        // Rotate current GameObject to look at the objectToLookAt
        this.gameObject.transform.LookAt(objectToLookAt.transform);

        // Rotate the GameObject 180 degrees around the y-axis
        if (flip)
        {
            this.gameObject.transform.Rotate(0, 180, 0);
        }

        // Restrict rotation around the x-axis
        if (!rotateX)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0, this.gameObject.transform.rotation.eulerAngles.y, this.gameObject.transform.rotation.eulerAngles.z);
        }
    }
}
