using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    [System.Serializable]
    public class GameDataDTO { }

    public enum ItemType
    {
        Potion,
        Material,
        Weapon,
        Bullet,
        Armor,
    }

    public enum InventoryType
    {
        Consumable,
        Equipment
    }

    public enum PotionType
    {
        Lesser,
        Greater,
        Elixer,
    }

    public enum WeaponType
    {
        Rifle=1,
        Pistol=2,
    }

    public enum BulletType
    {
        Normal,
        ArmorPiercing,
    }

    [System.Serializable]
    public class ItemDataDTO : GameDataDTO      // Game Data DTO => 1차 데이터
    {
        [System.Serializable]
        public class ItemData
        {
            [field: SerializeField] public string ItemID { get; set; }
            [field: SerializeField] public ItemType ItemCategory { get; set; }
            [field: SerializeField] public InventoryType InventoryCategory { get; set; }
            [field: SerializeField] public string ItemName { get; set; }
            [field: SerializeField] public bool IsStackable { get; set; }
            [field: SerializeField] public bool IsCraftable { get; set; }


            public ItemSO GetItemSO()
            {
                return Resources.Load<ItemSO>($"Game Data/ItemDataSO/ItemSO_{this.ItemID}");
            }
        }

        public List<ItemData> ItemDatas = new List<ItemData>();
    }

    // ItemDataDTO에서는 모든 Item에서 공용으로 필요한 데이터를 관리
    // 하단의 개별 DTO 에서 아이템 종류에 따라 따로 필요한 데이터를 관리
    // SO가 필요할 경우 위와 비슷하게 SO 세팅 후 Get{SO종류} 만들어서 사용
    [System.Serializable]
    public class PotionDataDTO : GameDataDTO
    {
        [System.Serializable]
        public class PotionData
        {
            [field: SerializeField] public string ItemID { get; set; }
            [field: SerializeField] public PotionType PotionCategory { get; set; }
            [field: SerializeField] public bool IsPercent {  get; set; }
            [field: SerializeField] public int HealingValue { get; set; }

        }

        public List<PotionData> PotionDatas = new List<PotionData>();

    }

    public class MaterialDataDTO : GameDataDTO
    {
        [field: SerializeField] public string ItemID { get; set; }
    }

    [System.Serializable]
    public class WeaponDataDTO : GameDataDTO
    {
        [System.Serializable]
        public class WeaponData
        {
            [field: SerializeField] public string ItemID { get; set; }
            [field: SerializeField] public WeaponType WeaponCategory { get; set; }
            [field: SerializeField] public float BaseDamage { get; set; }
            [field: SerializeField] public float FireRate { get; set; }
            [field: SerializeField] public float Accuracy { get; set; }
            [field: SerializeField] public int MagazineSize {  get; set; }

        }

        public List<WeaponData> WeaponDatas = new List<WeaponData>();
    }

    [System.Serializable]
    public class BulletDataDTO : GameDataDTO
    {
        [System.Serializable]
        public class BulletData
        {
            [field: SerializeField] public string ItemID { get; set; }
            [field: SerializeField] public BulletType BUlletCategory { get; set; }
            [field: SerializeField] public int BulletLevel { get; set; }
            [field: SerializeField] public float BulletDamage { get; set; }
            [field: SerializeField] public bool IgnoreArmor { get; set; }

        }

        public List<BulletData> BulletDatas = new List<BulletData>();
    }

    [System.Serializable]
    public class CraftingDataDTO : GameDataDTO
    {
        [System.Serializable]
        public class RecipeData
        {
            public string itemID;
            public int quantity;
        }

        [System.Serializable]
        public class CraftingData
        {
            public string TargetItemID;
            public List<RecipeData> RecipeList;
        }

        public List<CraftingData> CraftingDatas = new List<CraftingData>();
    }

}
