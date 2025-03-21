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

        public void AddItemData(string itemID, string itemName, Sprite sprite, bool isStackable, int count = 1)
        {
            // PlayerInventoryData 에서 추가된 아이템 데이터와 동일한 itemID를 가진 데이터를 찾는다
            UserItemDTO checkItemDTO = PlayerInventoryData.Items.Find(x => x.itemID == itemID);

            // 중첩 가능한 아이템일 경우
            // 1. checkItemDTO 가 없다면 새로 UserItemDTO 생성 후 PlayerInventoryData.Items 에 삽입
            // 2. checkItemDTO 가 있다면 기존의 데이터에 count 만 업데이트 해줌
            if (isStackable)
            {
                if(checkItemDTO == default(UserItemDTO))
                {
                    UserItemDTO newItemDTO = CreateNewItemDTO(itemID, itemName, sprite, count);

                    // 실제 DTO에 삽입한다
                    PlayerInventoryData.Items.Add(newItemDTO);

                    // 아이템을 입수했다는 이벤트 호출
                    OnItemAdded?.Invoke(newItemDTO);
                }
                else
                {
                    checkItemDTO.count += count;

                    OnItemUpdated?.Invoke(checkItemDTO);
                }
            }
            // 중첩 가능한 아이템이 아닐 경우 무조건 새로운 데이터 생성 후 PlayerInventoryData.Items 에 삽입
            else
            {
                UserItemDTO newItemDTO = CreateNewItemDTO(itemID, itemName, sprite, count);
                // 실제 DTO에 삽입한다
                PlayerInventoryData.Items.Add(newItemDTO);

                // 아이템을 입수했다는 이벤트 호출
                OnItemAdded?.Invoke(newItemDTO);
            }


            // TODO : 기존에 먹은 아이템인가? > 그렇다면 카운트만 증가시킨다
            // TODO : 한번도 안먹은 아이템이면 새로 List에 담아주자
        }

        public UserItemDTO CreateNewItemDTO(string itemID, string itemName, Sprite sprite, int count)
        {
            UserItemDTO newItemDTO = new UserItemDTO();
            string uniqueID = Guid.NewGuid().ToString();
            newItemDTO.dtoID = uniqueID;
            // TODO : dtoID 만드는 방식은 추후 더 제대로된 방식으로 변경되어야 한다 (GUID 등등)

            newItemDTO.itemID = itemID;
            newItemDTO.itemName = itemName;
            newItemDTO.count = count;
            newItemDTO.sprite = sprite;

            return newItemDTO;
        }
    }
}
