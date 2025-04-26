using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectSL
{
    public class InventoryUI_ItemSlot : InfiniteScrollItem
    {
        public Image itemIcon;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemCountText;

        public void SetData(Sprite itemImage, string itemName, int count)
        {
            itemIcon.sprite = itemImage;
            itemNameText.text = itemName;
            itemCountText.text = count.ToString();
        }

        public override void UpdateData(InfiniteScrollData scrollData)
        {
            var data = scrollData as InventoryUI_InfiniteScrollData;
            string itemName = "";
            Sprite InventorySprite = null;
            if (GameDataModel.Singleton.GetItemData(data.itemID, out ItemDataDTO.ItemData result))
            {
                itemName = result.ItemName;
                ItemSO itemSO = result.GetItemSO();
                InventorySprite = itemSO.sprite;
            }
            SetData(InventorySprite, itemName, data.itemCount);
        }
    }
}
