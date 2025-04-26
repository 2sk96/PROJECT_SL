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

    // itemName/sprite 같은 GameDataModel 에 있는 데이터의 경우 UserData에서는 필요없다
    // 사용할 때 itemID를 통해 GameDataModel을 참조해서 관련 데이터를 불러와 사용하는 방식으로 변경해 줘야 한다
    [System.Serializable]
    public class UserItemDTO : UserDataDTO
    {
        public string dtoID;        // 아이템 고유의 슬롯 ID 값 > 아이템 종류와 무관하게 아이템 고유의 ID값
        public string itemID;       // GameData에서의 ItemID > 아이템 종류에 따른 ID값
        public int count;
    }
}
