using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Captain : MonoBehaviour
{
    private enum CaptainState
    {
        Sitting,
        Searching
    }

    #region Inspector
    [SerializeField, Tooltip("The range around the captain that the player cannot be in")]
    private float aggroRange;
    [SerializeField, Tooltip("The time it takes for the captain to cause the player to lose")]
    private float aggroTime;
    [SerializeField]
    private PlayerTransformSO playerTransform;
    [SerializeField]
    private GameOverEventChannel gameOverEventChannel;
    [SerializeField, Tooltip("Face textures for the captain")]
    private Texture2D angryFace, neutralFace;
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

        this.gameObject.GetComponent<Animator>().Rebind();

        // Set the timer to zero
        playerAggroTimer = 0.0f;

        this.gameObject.GetComponent<Animator>().SetBool("Sitting", true);
    }

    void Update()
    {
        switch (state)
        {
            case CaptainState.Sitting:
                this.gameObject.GetComponent<Animator>().SetBool("Sitting", true);
                facePlate.material.mainTexture = neutralFace;
                playerAggroTimer = 0.0f;
                guardAggroCooldownTimer += Time.deltaTime;

                if (Vector3.Distance(this.transform.position, playerTransform.Position) < aggroRange && guardAggroCooldownTimer >= 0.2f)
                {
                    state = CaptainState.Searching;
                    prevState = CaptainState.Sitting;

                    Debug.Log("Play guard audio");
                    this.GetComponent<AudioSource>().Play();

                    // warningSprite.SetActive(true);
                }
                break;
            case CaptainState.Searching:
                this.gameObject.GetComponent<Animator>().SetBool("Sitting", true);
                facePlate.material.mainTexture = angryFace;
                playerAggroTimer += Time.deltaTime;
                if (playerAggroTimer >= aggroTime)
                {
                    gameOverEventChannel.RaiseEvent(new GameOverEvent());
                }

                if (Vector3.Distance(this.transform.position, playerTransform.Position) > aggroRange)
                {
                    state = prevState;
                    guardAggroCooldownTimer = 0.0f;

                    // warningSprite.SetActive(false);
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
