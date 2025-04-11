using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectSL
{
    public class GameDataModel : SingletonBase<GameDataModel>
    {
        [field: SerializeField] public ItemDataDTO ItemData { get; private set; } = new ItemDataDTO();
        [field: SerializeField] public PotionDataDTO PotionData { get; private set; } = new PotionDataDTO();
        [field: SerializeField] public MaterialDataDTO MaterialData { get; private set; } = new MaterialDataDTO();
        [field: SerializeField] public WeaponDataDTO WeaponData { get; private set; } = new WeaponDataDTO();
        [field: SerializeField] public BulletDataDTO BulletData { get; private set; } = new BulletDataDTO();
        [field: SerializeField] public CraftingDataDTO CraftingData { get; private set; } = new CraftingDataDTO();

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            // JSON 파일 생성을 위한 임시 코드
            //CreateItemData();
            //CreatePotionData();
            //CreateWeaponData();
            //CreateBulletData();
            //CreateCraftableData();

            // JSON 파일을 읽어서 유니티에서 사용할 형태로 변환
            if (FileManager.ReadFileData("Assets/01_PROJECT_SL/Resources/Game Data/ItemDatas.json", out string readItemJsonData))
            {
                ItemData = JsonUtility.FromJson<ItemDataDTO>(readItemJsonData);
            }
            if (FileManager.ReadFileData("Assets/01_PROJECT_SL/Resources/Game Data/PotionDatas.json", out string readPotionJsonData))
            {
                PotionData = JsonUtility.FromJson<PotionDataDTO>(readPotionJsonData);
            }
            if (FileManager.ReadFileData("Assets/01_PROJECT_SL/Resources/Game Data/WeaponDatas.json", out string readWeaponJsonData))
            {
                WeaponData = JsonUtility.FromJson<WeaponDataDTO>(readWeaponJsonData);
            }
            if (FileManager.ReadFileData("Assets/01_PROJECT_SL/Resources/Game Data/BulletDatas.json", out string readBulletJsonData))
            {
                BulletData = JsonUtility.FromJson<BulletDataDTO>(readBulletJsonData);
            }
            if (FileManager.ReadFileData("Assets/01_PROJECT_SL/Resources/Game Data/CraftingDatas.json", out string readCraftingData))
            {
                CraftingData = JsonUtility.FromJson<CraftingDataDTO>(readCraftingData);
            }
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
            WeaponData.WeaponDatas.Add(new WeaponDataDTO.WeaponData() { ItemID = "Item_Weapon_00001", WeaponCategory = WeaponType.Rifle, BaseDamage = 5, FireRate = 0.1f, Accuracy = 70, MagazineSize = 30 });
            WeaponData.WeaponDatas.Add(new WeaponDataDTO.WeaponData() { ItemID = "Item_Weapon_00002", WeaponCategory = WeaponType.Pistol, BaseDamage = 10, FireRate = 0.5f, Accuracy = 90, MagazineSize = 7 });

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
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData() { TargetItemID = "Item_Potion_00001", RecipeList = new List<CraftingDataDTO.RecipeData> {new CraftingDataDTO.RecipeData { itemID = "Item_Material_00001", quantity = 1 } } });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData() { 
                TargetItemID = "Item_Potion_00002", 
                RecipeList = new List<CraftingDataDTO.RecipeData> { 
                new CraftingDataDTO.RecipeData { itemID = "Item_Potion_00001", quantity = 1 }, 
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00002", quantity = 1 } 
                } 
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Potion_00003",
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Potion_00002", quantity = 1 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00003", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Bullet_00002",
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Bullet_00001", quantity = 1 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00004", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Bullet_00003",
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Bullet_00001", quantity = 1 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00005", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Bullet_00004",
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Bullet_00001", quantity = 1 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00006", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Bullet_00005",
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Bullet_00004", quantity = 1 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00004", quantity = 1 }
                }
            });
            CraftingData.CraftingDatas.Add(new CraftingDataDTO.CraftingData()
            {
                TargetItemID = "Item_Bullet_00006",
                RecipeList = new List<CraftingDataDTO.RecipeData> {
                new CraftingDataDTO.RecipeData { itemID = "Item_Bullet_00004", quantity = 1 },
                new CraftingDataDTO.RecipeData { itemID = "Item_Material_00005", quantity = 1 }
                }
            });

            string craftingDataToJson = JsonUtility.ToJson(CraftingData, true);

            FileManager.WriteFileFromString("Assets/01_PROJECT_SL/Resources/Game Data/CraftingDatas.json", craftingDataToJson);
        }

        public Dictionary<string, int> CreateRecipeList(string itemID, int count)
        {
            Dictionary<string, int> recipeInfo = new Dictionary<string, int>();
            recipeInfo.Add(itemID, count);
            return recipeInfo;
        }

        public bool GetItemData(string itemID, out ItemDataDTO.ItemData itemData)
        {
            itemData = ItemData.ItemDatas.Find(x => x.ItemID == itemID);
            return itemData != null;
        }

        public bool GetPotionItemData(string itemID, out PotionDataDTO.PotionData potionData)
        {
            potionData = PotionData.PotionDatas.Find(x => x.ItemID == itemID);
            return potionData != null;
        }

        public bool GetWeaponItemData(string itemID, out WeaponDataDTO.WeaponData weaponData)
        {
            weaponData = WeaponData.WeaponDatas.Find(x => x.ItemID == itemID);
            return weaponData != null;
        }

        public bool GetBulletItemData(string itemID, out BulletDataDTO.BulletData bulletData)
        {
            bulletData = BulletData.BulletDatas.Find(x => x.ItemID == itemID);
            return bulletData != null;
        }
    }
}
