using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class UserDataDTO { }

    [System.Serializable]
    public class PlayerInventoryDTO : UserDataDTO
    {
        public List<UserItemDTO> Items = new List<UserItemDTO>();
    }

    [System.Serializable]
    public class UserItemDTO : UserDataDTO
    {
        public int dtoID;       // 아이템 고유의 슬롯 ID 값 > 아이템 종류와 무관하게 아이템 고유의 ID값
        public string itemID;   // GameData에서의 ItemID > 아이템 종류에 따른 ID값
        public int count;

    }
}
