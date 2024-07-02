using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToLookAt;
    [SerializeField]
    private bool flip;
    [SerializeField]
    private bool rotateX;

    void Update()
    {
        this.gameObject.transform.LookAt(objectToLookAt.transform);

        if (flip)
        {
            this.gameObject.transform.Rotate(0, 180, 0);
        }

        if (!rotateX)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0, this.gameObject.transform.rotation.eulerAngles.y, this.gameObject.transform.rotation.eulerAngles.z);
        }
    }
}
