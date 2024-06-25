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

    #region Inspector
    [SerializeField]
    public ItemType itemType;
    [SerializeField]
    private GameObject itemObjectOverride;
    #endregion

    #region Private Variables
    private bool hasOverride;
    #endregion

    [HideInInspector]
    public string[] itemTags = new string[3];

    public bool HasOverride { get; set; }

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        if (itemObjectOverride)
        {
            hasOverride = true;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Shows or hides an item
    /// - The previously picked up item is shown
    /// - The new item being picked up is hidden
    /// </summary>
    /// <param name="show"> A boolean to set an item active or inactive </param>
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

    /// <summary>
    /// Returns true if the item has all tags the guard requested
    /// </summary>
    /// <param name="other"> The item the guard requested </param>
    public bool CompareItemTags(KeyItem other)
    {
        return itemTags.Contains(other.itemTags[0])
            && itemTags.Contains(other.itemTags[1])
            && itemTags.Contains(other.itemTags[2]);
    }

    /// <summary>
    /// Checks if an item can be picked up
    /// </summary>
    /// <returns></returns>
    public bool CanPickUp(){
        if (hasOverride)
        {
            return itemObjectOverride.activeSelf;
        }
        return true;
    }
    #endregion
}
