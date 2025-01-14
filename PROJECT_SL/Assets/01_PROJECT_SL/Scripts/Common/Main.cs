using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectSL
{
    public enum SceneType
    {
        None,
        Empty,

        // 실제 게임에 사용 되어지는 컨텐츠 씬들 추가
        Title,
        Ingame,
    }

    public class Main : SingletonBase<Main>
    {

        // 인스펙터창에서 보이게 하기 위해 [field: SerializeField] 추가
        // property로 선언해서 { get; private set; }를 통해 외부에서 변경할 수 없기 때문에 인스펙터 창에서 확인하기 위해 [field: SerializeField] 필요
        [field: SerializeField] public SceneType CurrentSceneType { get; private set; } = SceneType.None;
        [field: SerializeField] public SceneBase CurrentSceneController { get; private set; } = null;
        [field: SerializeField] public bool IsProgressSceneChanging { get; private set; } = false;

        private bool isInitialized = false;

        // Start 함수만 IEnumerator 로 사용 가능, 유니티 사이클에 있는 다른 함수들은 불가능
        private void Start()
        {
            Initialize();
#if UNITY_EDITOR
            Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            if (activeScene.name.Equals("Main"))
            {
                ChangeScene(SceneType.Title);
            }
#else
            ChangeScene(SceneType.Title);
#endif
        }

        public void Initialize()
        {
            if (isInitialized) return;

            isInitialized = true;
            
            // 시스템 초기화를 수행
            UIManager.Singleton.Initialize();
            InputSystem.Singleton.Initialize();
            
            // EventSystem Prefab 복제 후 Even.Main의 자식으로 추가
            GameObject eventSystemPrefab = Resources.Load<GameObject>("EventSystem/EventSystem");
            GameObject esInstance = UnityEngine.Object.Instantiate(eventSystemPrefab);
            esInstance.transform.SetParent(transform);
        }

        public void ChangeScene(SceneType sceneType, bool isForceLoad = false, System.Action onSceneChangeCompletedCallback = null)
        {
            // 이미 씬이 변경중일 경우 return
            if (IsProgressSceneChanging)
            {
                Debug.Log($"Scene is changing already. {CurrentSceneType}");
                return;
            }

            // 변경하고자 하는 씬이 지금과 같고 강제로 로드하지 않을 경우 return
            // 반대로 지금과 같은 씬을 불러오는데 isForceLoad=true 일 경우 강제로 씬을 다시 불러온다
            if (CurrentSceneType == sceneType && !isForceLoad)
            {
                return;
            }

            CurrentSceneType = sceneType;
            switch (sceneType)
            {
                case SceneType.Title:
                    StartCoroutine(ChangeScene<TitleScene>());
                    break;
                case SceneType.Ingame:
                    StartCoroutine(ChangeScene<IngameScene>());
                    break;
            }
        }

        private IEnumerator ChangeScene<T>(System.Action onSceneChangeCompletedCallback = null) where T : SceneBase
        {
            IsProgressSceneChanging = true;
            // 씬 변경할 때 모든 UI 비활성화
            UIManager.Singleton.HideAllUIs();

            // Loading UI 활성화
            UIManager.Show<LoadingUI>(UIList.LoadingUI);

            yield return new WaitForSeconds(1f);

            // 기존의 Current Scene Controlller [SceneBase] 가 있다면, OnEnd() 를 호출 후 삭제
            if (CurrentSceneController != null)
            {
                yield return StartCoroutine(CurrentSceneController.OnEnd());
                Destroy(CurrentSceneController.gameObject);
                CurrentSceneController = null;
            }

            // A > B 로 씬을 변경할 때 A씬과 B씬이 동시에 메모리를 점유하는 상황이 발생한다
            // PC/콘솔은 문제가 없는 경우가 많지만 모바일 환경일 경우 메모리 폭등으로 게임이 꺼지는 현상 발생
            // 위 현상을 방지하기 위해 중간에 Empty 씬을 불러서 A/B 씬이 동시에 올라가 있지 않도록 한다
            AsyncOperation emptyLoadAsync = SceneManager.LoadSceneAsync(SceneType.Empty.ToString(), LoadSceneMode.Single);
            while (!emptyLoadAsync.isDone)
            {
                yield return null;
            }

            // 새로운 명령을 받기 위한 SceneController를 생성 및 OnStart() 호출
            GameObject newSceneController = new GameObject(typeof(T).Name);
            newSceneController.transform.SetParent(transform);
            CurrentSceneController = newSceneController.AddComponent<T>();

            yield return StartCoroutine(CurrentSceneController.OnStart());

            onSceneChangeCompletedCallback?.Invoke();
            IsProgressSceneChanging = false;

            // Loading UI 비활성화
            UIManager.Hide<LoadingUI>(UIList.LoadingUI);
        }

        public void SystemQuit()
        {
            // TODO : 게임 종료 전 처리할 것들을 수행


            // 게임 종료
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}