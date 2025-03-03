using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class InventoryUI : UIBase
    {
        public InfiniteScroll infiniteScroll;

        public override void Show()
        {
            base.Show();

            RefreshInventory();
            UserDataModel.Singleton.OnItemAdded += RefreshInventory;
        }

        public override void Hide()
        {
            base.Hide();

            UserDataModel.Singleton.OnItemAdded -= RefreshInventory;
        }

        public void RefreshInventory()
        {
            // 일단은 싹 다 비우고 다시 삽입하는 방식
            // 추후 제대로 된 방식으로 변경 
            // 비우는 대신 검사를 통해 내용물을 비교하고 차이나는 부분을 추가/제거할 수 있도록
            infiniteScroll.ClearData();
            var inventoryItemDatas = UserDataModel.Singleton.PlayerInventoryData.Items;

            for (int i = 0; i < inventoryItemDatas.Count; i++)
            {
                AddItem(inventoryItemDatas[i].itemID, null, inventoryItemDatas[i].itemID, 1);
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

        public void AddItem(string itemId, Sprite sprite, string itemName, int count)
        {
            var newInfiniteData = new InventoryUI_InfiniteScrollData();
            newInfiniteData.itemId = itemId;
            newInfiniteData.sprite = sprite;
            newInfiniteData.itemName = itemName;
            newInfiniteData.itemCount = count;
            
            infiniteScroll.InsertData(newInfiniteData);
        }

        public void RemoveItem(string itemId)
        {
            var allData = infiniteScroll.GetDataList();
            for (int i = 0; i < allData.Count; i++)
            {
                var castingData = allData[i] as InventoryUI_InfiniteScrollData;
                if (castingData.itemId.Equals(itemId))
                {
                    infiniteScroll.RemoveData(castingData);
                }
            }
        }

        public void UpdateItem()
        {

        }

        private void RefreshInventory(UserItemDTO newItemDTO)
        {
            AddItem(newItemDTO.itemID, null, newItemDTO.itemID, newItemDTO.count);
        }
    }
}
