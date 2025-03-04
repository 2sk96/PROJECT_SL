using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectSL.Item;

namespace ProjectSL
{
    [CreateAssetMenu(menuName = "Bullet")]
    public class Bullet : ScriptableObject
    {
        public ItemType itemType;
        public enum BulletType
        {
            Normal,
            ArmorPierce,
        }
        public enum BulletLevel
        {
            LV1,
            Lv2,
            LV3
        }
        public BulletLevel level;
        public BulletType type;
        public Sprite sprite;
        public string bulletName;
        public int count;
        public int value;
        public bool ignoreArmor;
    }
}
