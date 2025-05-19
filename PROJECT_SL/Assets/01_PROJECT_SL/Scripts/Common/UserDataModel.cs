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

        // itemID만 인자로 받고 추가적으로 item 정보가 필요하다면 GameDataModel 에 함수 추가해서 해당 값 가져올 수 있도록 사용
        public void AddItemData(string itemID, int count)
        {
            bool isStackable = GameDataModel.Singleton.GetIsStackableFromItemData(itemID);

            // PlayerInventoryData 에서 추가된 아이템 데이터와 동일한 itemID를 가진 데이터를 찾는다
            UserItemDTO checkItemDTO = PlayerInventoryData.Items.Find(x => x.itemID == itemID);

            // 중첩 가능한 아이템일 경우
            // 1. checkItemDTO 가 없다면 새로 UserItemDTO 생성 후 PlayerInventoryData.Items 에 삽입
            // 2. checkItemDTO 가 있다면 기존의 데이터에 count 만 업데이트 해줌
            if (isStackable)
            {
                if (checkItemDTO == default(UserItemDTO))
                {
                    UserItemDTO newItemDTO = CreateNewItemDTO(itemID, count);

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
                UserItemDTO newItemDTO = CreateNewItemDTO(itemID, count);
                // 실제 DTO에 삽입한다
                PlayerInventoryData.Items.Add(newItemDTO);

                // 아이템을 입수했다는 이벤트 호출
                OnItemAdded?.Invoke(newItemDTO);
            }
        }

        public UserItemDTO CreateNewItemDTO(string itemID, int count)
        {
            UserItemDTO newItemDTO = new UserItemDTO();
            string uniqueID = Guid.NewGuid().ToString();
            newItemDTO.dtoID = uniqueID;
            newItemDTO.itemID = itemID;
            newItemDTO.count = count;

            return newItemDTO;
        }
    }
}
