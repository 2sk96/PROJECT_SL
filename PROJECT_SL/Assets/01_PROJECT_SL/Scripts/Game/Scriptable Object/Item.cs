using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    [CreateAssetMenu(menuName = "Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public Sprite sprite;
        public int count;
        public enum ItemType
        {
            Potion,
            Bullet,
            Weapon,
            // 추후 추가 예정
        }
        public ItemType itemType;
    }
}
