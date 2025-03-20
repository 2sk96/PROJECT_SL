using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectSL.UserDataDTO;

namespace ProjectSL
{
    public class UserDataModel : SingletonBase<UserDataModel>
    {
        [field: SerializeField] public PlayerInventoryDTO PlayerInventoryData { get; private set; }


        public event System.Action<UserItemDTO> OnItemAdded;
        public event System.Action<UserItemDTO> OnItemUpdated;


        protected override void Awake()
        {
            base.Awake();
            PlayerInventoryData = new PlayerInventoryDTO();
        }

        public void Initialize()
        {
            // TODO : 세이브 파일을 로드한다
            // 세이브 파일에 있는 정보를 토대로 각 DTO들을 초기화한다
        }

        public void AddItemData(string itemID, string itemName, Sprite sprite, int count = 1)
        {
            UserItemDTO checkItemDTO = PlayerInventoryData.Items.Find(x => x.itemID == itemID);
            
            if(checkItemDTO == default(UserItemDTO))
            {
                UserItemDTO newItemDTO = new UserItemDTO();
                string uniqueID = Guid.NewGuid().ToString();
                newItemDTO.dtoID = uniqueID;
                // TODO : dtoID 만드는 방식은 추후 더 제대로된 방식으로 변경되어야 한다 (GUID 등등)

                newItemDTO.itemID = itemID;
                newItemDTO.itemName = itemName;
                newItemDTO.count = count;
                newItemDTO.sprite = sprite;

                // 실제 DTO에 삽입한다
                PlayerInventoryData.Items.Add(newItemDTO);

                // 아이템을 입수했다는 이벤트 호출
                OnItemAdded?.Invoke(newItemDTO);
            }
            else
            {
                Debug.Log("Update count!!!");
                checkItemDTO.count += count;

                OnItemUpdated?.Invoke(checkItemDTO);
            }


            // TODO : 기존에 먹은 아이템인가? > 그렇다면 카운트만 증가시킨다
            // TODO : 한번도 안먹은 아이템이면 새로 List에 담아주자
        }
    }
}
