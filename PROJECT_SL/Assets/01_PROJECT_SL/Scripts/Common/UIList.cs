// #1. UIList는 UI의 실제 파일(:프리펩 파일)의 이름과 동일해야 한다. 
// #2. PANEL: 패널은 게임 화면에서 고정적인 위치에 보여지는 Full Screen UI를 의미한다.
// #3. POPUP: 팝업은 게임 화면에서 패널 위에 띄워지는 UI들을 의미한다.
// UIList 안에서 순서는 딱히 의미가 없다. 중요한 건 PANEL 인지 POPUP 인지에 따른 위치

namespace ProjectSL
{
    public enum UIList
    {
        PANEL_START,

        MainHUDUI,
        PauseUI,
        LoadingUI,
        TitleUI,
        DeathUI,

        IngameMinimapUI,
        MinimapMarkerUI_Player,
        MinimapMarkerUI_Item,

        PANEL_END,
        POPUP_START,

        CrosshairUI,
        InteractionUI,

        POPUP_END,
    }
}
