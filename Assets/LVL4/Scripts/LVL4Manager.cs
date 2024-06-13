using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LVL4Manager : MonoBehaviour
{
    [HideInInspector]
    public static LVL4Manager instance;
    [SerializeField]
    private TMP_Text itemText;
    [SerializeField]
    private CanvasRenderer itemPanel;
    private KeyItem currentlyHeldItem;

    void Start()
    {
        instance = this;
    }

    void Update()
    {

    }

    public void RetryButton()
    {
        SceneManager.LoadSceneAsync("LVL4");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void PickUpItem(KeyItem item)
    {
        if (currentlyHeldItem)
        {
            currentlyHeldItem.ShowItem(true);
        }
        currentlyHeldItem = item;
        item.ShowItem(false);
        itemPanel.gameObject.SetActive(true);
        itemText.text = item.itemType.ToString();
        itemPanel.SetAlpha(1);
        DOTween.To(()=>itemPanel.GetAlpha(), x=>itemPanel.SetAlpha(x), 0, 4);
        // foreach(TMP_Text i in itemPanel.GetComponentsInChildren<TMP_Text>())
        // {
        //     DOTween.To(()=>i.color.a, x=>i.color.a, 0, 4);
        // }
    }

}
