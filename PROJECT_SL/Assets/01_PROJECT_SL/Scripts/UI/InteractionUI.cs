using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSL
{
    public class InteractionUI : UIBase
    {
        public List<InteractionUI_Item> createdContents = new List<InteractionUI_Item>();
        private int selectedIndex = -1;

        [SerializeField] private InteractionUI_Item itemPrefab;
        [SerializeField] private Transform contentsGroup;
        
        private void Awake()
        {
            itemPrefab.gameObject.SetActive(false);
        }

        private void Start()
        {
            InputSystem.Singleton.OnMouseWheelUp += OnMouseScrollUp;
            InputSystem.Singleton.OnMouseWheelDown += OnMouseScrollDown;
        }

        private void OnDestroy()
        {
            InputSystem.Singleton.OnMouseWheelUp -= OnMouseScrollUp;
            InputSystem.Singleton.OnMouseWheelDown -= OnMouseScrollDown;
        }

        public override void Hide()
        {
            base.Hide();
            RemoveAllInteractionContent();
        }


        private void OnMouseScrollUp()
        {
            // createdContents 가 비어있다면 > 스크롤을 돌려도 아무 일도 일어나지 않는다
            if (createdContents.Count <= 0) return;

            if (selectedIndex >= 0 && selectedIndex < createdContents.Count)
            {
                createdContents[selectedIndex].isSelected = false;
            }
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = createdContents.Count - 1;
            }
            createdContents[selectedIndex].isSelected = true;
        }

        private void OnMouseScrollDown()
        {
            // createdContents 가 비어있다면 > 스크롤을 돌려도 아무 일도 일어나지 않는다
            if (createdContents.Count <= 0) return;
            // 기존 isSelected 되어있던 InteractionUI_Item 의 isSelected 해제
            if (selectedIndex >= 0 && selectedIndex < createdContents.Count)
            {
                createdContents[selectedIndex].isSelected = false;
            }
            // selectedIndex 값 변경, 범위가 아래로 벗어나면 가장 위로 초기화
            selectedIndex++;
            if (selectedIndex >= createdContents.Count)
            {
                selectedIndex = 0;
            }
            // selectedIndex 값에 맞게 IsSelected = true 를 새로운 InteracionUI_Item 에 부여
            createdContents[selectedIndex].isSelected = true;
        }

        public void AddInteractionContent(IInteractable interactable)
        {
            if (createdContents.Exists(x => x.InteractableData == interactable)) return;

            InteractionUI_Item newItem = Instantiate(itemPrefab, contentsGroup);
            newItem.gameObject.SetActive(true);
            newItem.ContentsText = interactable.Message;
            // createdContents가 새로 생길 경우에만 isSelected 및 selectedIndex 설정
            if (createdContents.Count == 0)
            {
                newItem.isSelected = true;
                selectedIndex = 0;
            }
            // 기존 createdContents가 있을 경우 isSelected 설정 X, selectedIndex도 변경 X
            else
            {
                newItem.isSelected = false;
            }
            newItem.InteractableData = interactable;

            createdContents.Add(newItem);
        }

        // 일단 여기까지만 조건 파악이 너무 헷갈린다
        public void RemoveInteractionContent(IInteractable interactable)
        {
            if (!createdContents.Exists(x => x.InteractableData == interactable)) return;

            int targetIndex = createdContents.FindIndex(x => x.InteractableData == interactable);
            if (targetIndex >= 0)
            {
                if (targetIndex == selectedIndex)
                {
                    if (targetIndex == 0)
                    {
                        // selectedIndex 유지, 기존 createdContents 가 1 이상일때만 작동하도록
                        if (createdContents.Count > 1)
                        {
                            createdContents[selectedIndex + 1].isSelected = true;
                        }
                    }
                    else if (targetIndex < createdContents.Count - 1)
                    {
                        // selectedIndex 유지..?
                        createdContents[selectedIndex + 1].isSelected = true;
                    }
                    else if (targetIndex == createdContents.Count - 1)
                    {
                        selectedIndex -= 1;
                        createdContents[selectedIndex].isSelected = true;

                    }
                }


                Destroy(createdContents[targetIndex].gameObject);
                createdContents.RemoveAt(targetIndex);
            }
        }


        public void RemoveAllInteractionContent()
        {
            for (int i = 0; i < createdContents.Count; i++)
            {
                Destroy(createdContents[i].gameObject);
            }
            createdContents.Clear();
        }

        public void ExecuteInteract()
        {
            if (selectedIndex >= 0 && selectedIndex < createdContents.Count)
            {
                createdContents[selectedIndex].InteractableData.Interact();
            }
        }
    }
}
