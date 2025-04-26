using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectSL
{
    public partial class GameDataModel : SingletonBase<GameDataModel>
    {
        // JSON 파일 생성을 위한 임시 코드
        public void CreateDBInitialize()
        {
            CreateItemData();
            CreatePotionData();
            CreateWeaponData();
            CreateBulletData();
            CreateCraftableData();
        }

        private void CreateItemData()
        {
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Potion_00001", ItemCategory = ItemType.Potion, InventoryCategory = InventoryType.Consumable, ItemName = "하급 회복약", IsStackable = true, IsCraftable = true });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Potion_00002", ItemCategory = ItemType.Potion, InventoryCategory = InventoryType.Consumable, ItemName = "상급 회복약", IsStackable = true, IsCraftable = true });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Potion_00003", ItemCategory = ItemType.Potion, InventoryCategory = InventoryType.Consumable, ItemName = "엘릭서", IsStackable = true, IsCraftable = true });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Weapon_00001", ItemCategory = ItemType.Weapon, InventoryCategory = InventoryType.Equipment, ItemName = "라이플", IsStackable = false, IsCraftable = false });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Weapon_00002", ItemCategory = ItemType.Weapon, InventoryCategory = InventoryType.Equipment, ItemName = "권총", IsStackable = false, IsCraftable = false });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Bullet_00001", ItemCategory = ItemType.Weapon, InventoryCategory = InventoryType.Consumable, ItemName = "일반 탄환", IsStackable = true, IsCraftable = false });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Bullet_00002", ItemCategory = ItemType.Weapon, InventoryCategory = InventoryType.Consumable, ItemName = "상급 일반 탄환", IsStackable = true, IsCraftable = true });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Bullet_00003", ItemCategory = ItemType.Weapon, InventoryCategory = InventoryType.Consumable, ItemName = "최상급 일반 탄환", IsStackable = true, IsCraftable = true });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Bullet_00004", ItemCategory = ItemType.Weapon, InventoryCategory = InventoryType.Consumable, ItemName = "철갑탄", IsStackable = true, IsCraftable = true });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Bullet_00005", ItemCategory = ItemType.Weapon, InventoryCategory = InventoryType.Consumable, ItemName = "상급 철갑탄", IsStackable = true, IsCraftable = true });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Bullet_00006", ItemCategory = ItemType.Weapon, InventoryCategory = InventoryType.Consumable, ItemName = "최상급 철갑탄", IsStackable = true, IsCraftable = true });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Material_00001", ItemCategory = ItemType.Material, InventoryCategory = InventoryType.Consumable, ItemName = "약초", IsStackable = true, IsCraftable = false });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Material_00002", ItemCategory = ItemType.Material, InventoryCategory = InventoryType.Consumable, ItemName = "벌꿀", IsStackable = true, IsCraftable = false });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Material_00003", ItemCategory = ItemType.Material, InventoryCategory = InventoryType.Consumable, ItemName = "만드라고라", IsStackable = true, IsCraftable = false });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Material_00004", ItemCategory = ItemType.Material, InventoryCategory = InventoryType.Consumable, ItemName = "상급 화약", IsStackable = true, IsCraftable = false });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Material_00005", ItemCategory = ItemType.Material, InventoryCategory = InventoryType.Consumable, ItemName = "최상급 화약", IsStackable = true, IsCraftable = false });
            ItemData.ItemDatas.Add(new ItemDataDTO.ItemData() { ItemID = "Item_Material_00006", ItemCategory = ItemType.Material, InventoryCategory = InventoryType.Consumable, ItemName = "철갑탄 전용 화약", IsStackable = true, IsCraftable = false });

            string itemDataToJson = JsonUtility.ToJson(ItemData, true);

            FileManager.WriteFileFromString("Assets/01_PROJECT_SL/Resources/Game Data/ItemDatas.json", itemDataToJson);
        }

        private void CreatePotionData()
        {
            PotionData.PotionDatas.Add(new PotionDataDTO.PotionData() { ItemID = "Item_Potion_00001", PotionCategory = PotionType.Lesser, IsPercent = false, HealingValue = 20 });
            PotionData.PotionDatas.Add(new PotionDataDTO.PotionData() { ItemID = "Item_Potion_00002", PotionCategory = PotionType.Greater, IsPercent = false, HealingValue = 60 });
            PotionData.PotionDatas.Add(new PotionDataDTO.PotionData() { ItemID = "Item_Potion_00003", PotionCategory = PotionType.Elixer, IsPercent = true, HealingValue = 100 });

            string potionDataToJson = JsonUtility.ToJson(PotionData, true);

            FileManager.WriteFileFromString("Assets/01_PROJECT_SL/Resources/Game Data/PotionDatas.json", potionDataToJson);
        }

        private void CreateWeaponData()
        {
            WeaponData.WeaponDatas.Add(new WeaponDataDTO.WeaponData() { ItemID = "Item_Weapon_00001", WeaponCategory = WeaponType.Rifle, BaseDamage = 5, FireRate = 0.1f, Accuracy = 80, Recoil = 10, MagazineSize = 30 });
            WeaponData.WeaponDatas.Add(new WeaponDataDTO.WeaponData() { ItemID = "Item_Weapon_00002", WeaponCategory = WeaponType.Pistol, BaseDamage = 10, FireRate = 0.5f, Accuracy = 95, Recoil = 50, MagazineSize = 7 });

            string weaponDataToJson = JsonUtility.ToJson(WeaponData, true);

            FileManager.WriteFileFromString("Assets/01_PROJECT_SL/Resources/Game Data/WeaponDatas.json", weaponDataToJson);
        }

        private void CreateBulletData()
        {
            BulletData.BulletDatas.Add(new BulletDataDTO.BulletData() { ItemID = "Item_Bullet_00001", BUlletCategory = BulletType.Normal, BulletDamage = 7, BulletLevel = 1, IgnoreArmor = false });
            BulletData.BulletDatas.Add(new BulletDataDTO.BulletData() { ItemID = "Item_Bullet_00002", BUlletCategory = BulletType.Normal, BulletDamage = 20, BulletLevel = 2, IgnoreArmor = false });
            BulletData.BulletDatas.Add(new BulletDataDTO.BulletData() { ItemID = "Item_Bullet_00003", BUlletCategory = BulletType.Normal, BulletDamage = 30, BulletLevel = 3, IgnoreArmor = false });
            BulletData.BulletDatas.Add(new BulletDataDTO.BulletData() { ItemID = "Item_Bullet_00004", BUlletCategory = BulletType.ArmorPiercing, BulletDamage = 5, BulletLevel = 1, IgnoreArmor = true });
            BulletData.BulletDatas.Add(new BulletDataDTO.BulletData() { ItemID = "Item_Bullet_00005", BUlletCategory = BulletType.ArmorPiercing, BulletDamage = 15, BulletLevel = 2, IgnoreArmor = true });
            BulletData.BulletDatas.Add(new BulletDataDTO.BulletData() { ItemID = "Item_Bullet_00006", BUlletCategory = BulletType.ArmorPiercing, BulletDamage = 25, BulletLevel = 3, IgnoreArmor = true });

            string bulletDataToJson = JsonUtility.ToJson(BulletData, true);

            FileManager.WriteFileFromString("Assets/01_PROJECT_SL/Resources/Game Data/BulletDatas.json", bulletDataToJson);
        }

        private void CreateMaterialData()
        {

        }

        private void CreateCraftableData()
        {
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData() 
            { 
                TargetItemID = "Item_Potion_00001", 
                TargetItemCount = 1,
                RecipeList = new List<CraftingDataDTO.RecipeData> { 
                    new CraftingDataDTO.RecipeData { itemID = "Item_Material_00001", quantity = 1 } 
                } 
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Potion_00002",
                TargetItemCount = 1,
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Potion_00001", quantity = 1 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00002", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Potion_00003",
                TargetItemCount = 1,
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Potion_00002", quantity = 1 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00003", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Bullet_00002",
                TargetItemCount = 10,
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Bullet_00001", quantity = 10 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00004", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Bullet_00003",
                TargetItemCount = 10,
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Bullet_00001", quantity = 10 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00005", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Bullet_00004",
                TargetItemCount = 10,
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Bullet_00001", quantity = 10 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00006", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Bullet_00005",
                TargetItemCount = 10,
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Bullet_00004", quantity = 10 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00004", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Bullet_00006",
                TargetItemCount = 10,
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Bullet_00004", quantity = 10 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00005", quantity = 1 }
                }
            });

            string craftingDataToJson = JsonUtility.ToJson(CraftingData, true);

            FileManager.WriteFileFromString("Assets/01_PROJECT_SL/Resources/Game Data/CraftingDatas.json", craftingDataToJson);
        }
    }
}
