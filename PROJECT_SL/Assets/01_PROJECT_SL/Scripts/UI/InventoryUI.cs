using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class InventoryUI : UIBase
    {
        public override bool IsCursorVisible => true;

        [field: SerializeField] public InfiniteScroll infiniteScroll;

        public override void Show()
        {
            base.Show();
            RefreshInventory();
            UserDataModel.Singleton.OnItemAdded += AddItemToInventory;
            UserDataModel.Singleton.OnItemUpdated += UpdateItemAtInventory;
        }

        public override void Hide()
        {
            base.Hide();

            UserDataModel.Singleton.OnItemAdded -= AddItemToInventory;
            UserDataModel.Singleton.OnItemUpdated -= UpdateItemAtInventory;
        }

        

        // 인벤토리가 열고 닫힐 때 호출
        public void RefreshInventory()
        {
            // 일단은 싹 다 비우고 다시 삽입하는 방식
            // 추후 제대로 된 방식으로 변경 
            // 비우는 대신 검사를 통해 내용물을 비교하고 차이나는 부분을 추가/제거할 수 있도록

            //var infinteScrollData = infiniteScroll.GetDataList();
            //var inventoryItemsData = UserDataModel.Singleton.PlayerInventoryData.Items;

            //foreach(var item in inventoryItemsData)
            //{

            //}


            infiniteScroll.ClearData();
            var inventoryItemDatas = UserDataModel.Singleton.PlayerInventoryData.Items;

            for (int i = 0; i < inventoryItemDatas.Count; i++)
            {
                AddItem(inventoryItemDatas[i].itemID, inventoryItemDatas[i].itemName, inventoryItemDatas[i].sprite, inventoryItemDatas[i].count);
            }
            
        }

        // 더미데이터 추가를 위한 임시 코드
        //private void Start()
        //{
        //    for (int i = 0; i < 100; i++)
        //    {
        //        AddItem($"Item_{i}", null, $"Item_Name_{i}", Random.Range(1, 99));
        //    }
        //}

        public void AddItem(string itemID, string itemName, Sprite sprite, int count)
        {
            var newInfiniteData = new InventoryUI_InfiniteScrollData();
            newInfiniteData.itemID = itemID;
            newInfiniteData.sprite = sprite;
            newInfiniteData.itemName = itemName;
            newInfiniteData.itemCount = count;
            
            infiniteScroll.InsertData(newInfiniteData);
        }

        public void UpdateItem(string itemId, Sprite sprite, string itemName, int count)
        {
            
        }

        public void RemoveItem(string itemID)
        {
            var allData = infiniteScroll.GetDataList();
            for (int i = 0; i < allData.Count; i++)
            {
                var castingData = allData[i] as InventoryUI_InfiniteScrollData;
                if (castingData.itemID.Equals(itemID))
                {
                    infiniteScroll.RemoveData(castingData);
                }
            }
        }

        public void UpdateItemAtInventory(UserItemDTO checkItemDTO)
        {
            Debug.Log("UpdateItemAtInventory");
            List<InfiniteScrollData> currentInfiniteScrollData = infiniteScroll.GetDataList();
            for (int i = 0; i < currentInfiniteScrollData.Count; i++)
            {
                var data = currentInfiniteScrollData[i] as InventoryUI_InfiniteScrollData;
                if (data.itemID == checkItemDTO.itemID)
                {
                    Debug.Log("itemName: " + data.itemID);

                    data.itemCount = checkItemDTO.count;
                    infiniteScroll.UpdateData(data);
                }
            }
        }

        // 1. 인벤토리가 열려있는 상태에서
        // 2. 아이템 추가가 발생했을 때 호출
        private void AddItemToInventory(UserItemDTO newItemDTO)
        {
            Debug.Log("AddItemToInventory" + newItemDTO.itemName);
            
            AddItem(newItemDTO.itemID, newItemDTO.itemName, newItemDTO.sprite, newItemDTO.count);
        }
    }
}
