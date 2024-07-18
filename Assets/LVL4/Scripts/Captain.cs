using UnityEngine;

public class Captain : MonoBehaviour
{
    private enum CaptainState
    {
        Sitting, // Captain is sitting idle
        Searching // Captain is actively searching for the player
    }

    #region Inspector
    [SerializeField, Tooltip("The range around the captain that the player cannot be in")]
    private float aggroRange;
    [SerializeField, Tooltip("The time it takes for the captain to cause the player to lose")]
    private float aggroTime;
    [SerializeField]
    private PlayerDataSO playerData;
    [SerializeField]
    private GameOverEventChannel gameOverEventChannel;
    [SerializeField, Tooltip("Face textures for the captain")]
    private Texture2D angryFace, neutralFace;
    [SerializeField, Tooltip("Image to display above head when player is in aggro range")]
    private GameObject warningSprite;
    #endregion

    #region Private Variables
    private CaptainState state = CaptainState.Sitting;
    private CaptainState prevState = CaptainState.Sitting;
    private float playerAggroTimer;
    private float guardAggroCooldownTimer;
    private MeshRenderer facePlate;
    #endregion

    void Start()
    {
        // Assign captain body
        foreach (MeshRenderer i in this.gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            if (i.gameObject.name.Contains("FacePlate"))
            {
                facePlate = i;
                break;
            }
        }

        // Reset any animation states
        this.gameObject.GetComponent<Animator>().Rebind();

        // Set the timer to zero
        playerAggroTimer = 0.0f;
    }

    void Update()
    {
        switch (state)
        {
            case CaptainState.Sitting:
                facePlate.material.mainTexture = neutralFace;
                playerAggroTimer = 0.0f;
                guardAggroCooldownTimer += Time.deltaTime;

                // Check if player is in aggro range
                if (Vector3.Distance(this.transform.position, playerData.Transform.position) < aggroRange && guardAggroCooldownTimer >= 0.2f)
                {
                    state = CaptainState.Searching;
                    prevState = CaptainState.Sitting;

                    Debug.Log("Play guard audio");
                    this.GetComponent<AudioSource>().Play();

                    warningSprite.SetActive(true);
                }
                break;
            case CaptainState.Searching:
                facePlate.material.mainTexture = angryFace;
                GetComponentInChildren<LVL4_GuardLookAt>().lookAt = true;
                playerAggroTimer += Time.deltaTime;
                // Check if player has been in aggro range too long
                if (playerAggroTimer >= aggroTime)
                {
                    gameOverEventChannel.RaiseEvent();

                    /* ========================== SEND DATA TO SERVER HERE ==============================*/
                }

                // Check if player moves out of aggro range
                if (Vector3.Distance(this.transform.position, playerData.Transform.position) > aggroRange)
                {
                    state = prevState;
                    GetComponentInChildren<LVL4_GuardLookAt>().lookAt = false;
                    guardAggroCooldownTimer = 0.0f;

                    warningSprite.SetActive(false);
                }
                break;
        }
    }

    void OnDrawGizmos()
    {
        // Draws the visual representation of the aggro zone
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, aggroRange);
    }
}
