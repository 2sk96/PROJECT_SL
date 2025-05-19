using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectSL
{
    public class InventoryUI_CraftingItemSlot : InfiniteScrollItem
    {
        public Image targetitemIcon;
        public TextMeshProUGUI targetitemNameText;
        public TextMeshProUGUI targetitemCountText;

        public void SetData(Sprite itemImage, string itemName, int count)
        {
            targetitemIcon.sprite = itemImage;
            targetitemNameText.text = itemName;
            targetitemCountText.text = count.ToString();
        }

        public override void UpdateData(InfiniteScrollData scrollData)
        {
            var data = scrollData as InventoryUI_CraftingInfiniteScrollData;
            string itemName = "";
            Sprite InventorySprite = null;
            if (GameDataModel.Singleton.GetItemData(data.targetItemID, out ItemDataDTO.ItemData result))
            {
                itemName = result.ItemName;
                ItemSO itemSO = result.GetItemSO();
                InventorySprite = itemSO.sprite;
            }
            SetData(InventorySprite, itemName, data.targetItemCount);
        }
    }
}
