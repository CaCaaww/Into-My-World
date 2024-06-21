using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyItemTags", menuName = "IMW/KeyItemTags")]
public class KeyItemTagsSO : ScriptableObject
{
    private Dictionary<KeyItem.ItemType, TagsHolder> itemDictionary;

    [SerializeField]
    private TagsHolder cape = new TagsHolder(),
    crown = new TagsHolder(),
    helmet = new TagsHolder(),
    banner = new TagsHolder(),
    bowl = new TagsHolder(),
    brazier = new TagsHolder(),
    goblet = new TagsHolder(),
    jar = new TagsHolder(),
    loom = new TagsHolder(),
    lyre = new TagsHolder(),
    pillow = new TagsHolder(),
    plate = new TagsHolder(),
    sack = new TagsHolder(),
    scroll = new TagsHolder(),
    scythe = new TagsHolder(),
    chair = new TagsHolder(),
    censer = new TagsHolder(),
    sickle = new TagsHolder(),
    hammer = new TagsHolder(),
    axe = new TagsHolder(),
    shield = new TagsHolder(),
    spear = new TagsHolder(),
    sword = new TagsHolder(),
    scabbard = new TagsHolder(),
    flower = new TagsHolder(),
    lionStatue = new TagsHolder(),
    lionHead = new TagsHolder(),
    pergola = new TagsHolder(),
    anvil = new TagsHolder(),
    basket = new TagsHolder();

    public void Init()
    {
        itemDictionary = new Dictionary<KeyItem.ItemType, TagsHolder>
        {
            {KeyItem.ItemType.Cape, cape},
            {KeyItem.ItemType.Crown, crown},
            {KeyItem.ItemType.Helmet, helmet},
            {KeyItem.ItemType.Banner, banner},
            {KeyItem.ItemType.Bowl, bowl},
            {KeyItem.ItemType.Brazier, brazier},
            {KeyItem.ItemType.Goblet, goblet},
            {KeyItem.ItemType.Jar, jar},
            {KeyItem.ItemType.Loom, loom},
            {KeyItem.ItemType.Lyre, lyre},
            {KeyItem.ItemType.Pillow, pillow},
            {KeyItem.ItemType.Plate, plate},
            {KeyItem.ItemType.Sack, sack},
            {KeyItem.ItemType.Scroll, scroll},
            {KeyItem.ItemType.Scythe, scythe},
            {KeyItem.ItemType.Chair, chair},
            {KeyItem.ItemType.Censer, censer},
            {KeyItem.ItemType.Sickle, sickle},
            {KeyItem.ItemType.Hammer, hammer},
            {KeyItem.ItemType.Axe, axe},
            {KeyItem.ItemType.Shield, shield},
            {KeyItem.ItemType.Spear, spear},
            {KeyItem.ItemType.Sword, sword},
            {KeyItem.ItemType.Scabbard, scabbard},
            {KeyItem.ItemType.Flower, flower},
            {KeyItem.ItemType.Lion_Head, lionHead},
            {KeyItem.ItemType.Lion_Statue, lionStatue},
            {KeyItem.ItemType.Pergola, pergola},
            {KeyItem.ItemType.Anvil, anvil},
            {KeyItem.ItemType.Basket, basket}
        };
    }

    public bool SetTagsOfItem(KeyItem item)
    {
        TagsHolder holder;
        itemDictionary.TryGetValue(item.itemType, out holder);
        if (holder.tag1 != "" && holder.tag2 != "" &&  holder.tag3 != "")
        {
            string[] tags = new string[] { holder.tag1, holder.tag2, holder.tag3 };
            item.itemTags = tags;
        }
        else
        {
            string badTag = item.itemType.ToString();
            string[] tags = new string[] { badTag, badTag, badTag };
            item.itemTags = tags;
            return false;
        }
        return true;
    }

    [System.Serializable]
    private struct TagsHolder
    {
        public string tag1, tag2, tag3;
    }

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TagsHolder))]
    private class ItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect tag1Rect = new Rect(position.x, position.y, position.width * 1 / 3, position.height);
            Rect tag2Rect = new Rect(position.x + position.width * 1 / 3, position.y, position.width * 1 / 3, position.height);
            Rect tag3Rect = new Rect(position.x + position.width * 2 / 3, position.y, position.width * 1 / 3, position.height);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(tag1Rect, property.FindPropertyRelative("tag1"), GUIContent.none);
            EditorGUI.PropertyField(tag2Rect, property.FindPropertyRelative("tag2"), GUIContent.none);
            EditorGUI.PropertyField(tag3Rect, property.FindPropertyRelative("tag3"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
    #endif  
}