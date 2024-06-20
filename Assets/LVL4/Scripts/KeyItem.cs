using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Lion_Statue,
        Lion_Head,
        Pergola,
        Anvil,
        Basket
    }

    [SerializeField]
    public ItemType itemType;
    [SerializeField]
    private GameObject itemObjectOverride;
    private bool hasOverride;

    [HideInInspector]
    public string[] itemTags = new string[3];

    public bool HasOverride { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (itemObjectOverride)
        {
            hasOverride = true;
        }
    }

    public void ShowItem(bool show)
    {
        if (hasOverride)
        {
            itemObjectOverride.SetActive(show);
        }
        else
        {
            this.gameObject.SetActive(show);
        }
    }

    public bool CompareItemTags(KeyItem other)
    {
        return itemTags.Contains(other.itemTags[0])
            && itemTags.Contains(other.itemTags[1])
            && itemTags.Contains(other.itemTags[2]);
    }

    public bool CanPickUp(){
        if (hasOverride)
        {
            return itemObjectOverride.activeSelf;
        }
        return true;
    }
}
