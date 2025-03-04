using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectSL.Item;

namespace ProjectSL
{
    [CreateAssetMenu(menuName = "Potion")]
    public class Potion : ScriptableObject
    {
        public ItemType itemType;
        public enum PotionType
        {
            Lesser,
            Greater,
            Elixer
        }
        public PotionType potionType;
        public string potionName;
        public Sprite sprite;
        public int count;
        public bool isPercent;
        public int value;
    }
}
