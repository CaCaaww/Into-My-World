using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public enum ItemType
    {
        Cape,
        Crown,
        Helmet,
        Banner,
        Bowl,
        Brazier,
        Goblet,
        Jar,
        Loom,
        Lyre,
        Pillow,
        Plate,
        Sack,
        Scroll,
        Scythe,
        Chair,
        Censer,
        Sickle,
        Hammer,
        Axe,
        Shield,
        Spear,
        Sword,
        Scabbard,
        Flower,
        LionStatue,
        LionHead,
        Pergola,
        Anvil,
        Basket
    }

    [SerializeField]
    public ItemType itemType;
    [SerializeField]
    private GameObject itemObjectOverride;
    private List<String> itemTags;
    private bool hasOverride;

    public bool HasOverride{get; set;}

    // Start is called before the first frame update
    void Start()
    {
        if(itemObjectOverride){
            hasOverride = true;
        }
        itemTags = new List<string>();
        itemTags.Add("gold");
        itemTags.Add("sharp");
        itemTags.Add("clay");
    }

    public void ShowItem(bool show){
        if(hasOverride){
            itemObjectOverride.SetActive(show);
        } else{
            this.gameObject.SetActive(show);
        }
    }
}
