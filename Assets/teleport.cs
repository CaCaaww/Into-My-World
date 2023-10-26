using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class teleport : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private TextMeshProUGUI value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void whenClicked()
    {
        player.transform.position = transform.position + Vector3.down * 70f;
        this.gameObject.SetActive(false);
        value.text = "risultato";
    }
}
