using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectSL
{
    public partial class GameDataModel : SingletonBase<GameDataModel>
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
            //CreateDBInitialize();

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

        // 데이터의 accuracy 값으로부터 해당 정확도일 때 생기는 오차 각도에 맞는 spreadAmount 계산 후 return
        public float GetWeaponAccuracy(float accuracy, float maxSpreadAngle = 10f)
        {
            float spreadAngle = (-maxSpreadAngle/100) * accuracy + maxSpreadAngle;
            float spreadAmount = Mathf.Sin(spreadAngle * Mathf.Deg2Rad);
            return spreadAmount;
        }

        public InventoryType GetItemInventoryCategory(string itemID)
        {
            ItemDataDTO.ItemData itemData = ItemData.ItemDatas.Find(x => x.ItemID == itemID);
            InventoryType inventoryCategory = itemData.InventoryCategory;
            return inventoryCategory;
        }

        //public float GetWeaponRecoilAmount(float recoil)
        //{

        //}

        //public float GetWeaponRecoilRecoverySpeed(float recoil)
        //{

        //}
    }
}
