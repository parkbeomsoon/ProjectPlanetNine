namespace DefineUtils
{
    public class CharacterInfo
    {
        public static string[] name = { "A-2", "E-4", "N-7" };
        public static int[] age = { 25, 28, 23 };
        public static string[] specialize = { "전투", "수리", "조사" };
        public static string[] explain = { "사거리가 짧은 대신 데미지가 강력한 2번 무기를 사용할 수 있다.", "수리임무 수행 시 미니게임 난이도가 하향된다.", "조사임무 수행 시 조사속도가 빠르다." };
    }
    
   
    public enum eEndingType
    {
        DeadEnding          = 0,
        ClearEnding,
    }
    public enum ePrefabs
    {
        MessageBox          = 0,
        NewCharacterWindow,
        SettingsWindow,
        StartWindow,
        FadeWindow,
        MissionWindow,
        MenuWindow,
        MinigameWindow,
        ResultWindow,
        LoadingWindow,
    }
    public enum eSceneType
    {
        TitleScene          = 0,
        TpvGameScene,
        FpvGameScene,
    }
    public enum eKeyOption
    {
        Interact            = 0,
        SwapWeapon1,
        SwapWeapon2,

        end
    }
    public enum eSoundOption
    {
        Background          = 0,
        Effect,

        end
    }
    public enum eMissionKind
    {
        SURVIVAL            = 0,
        REPAIR,
        RESEARCH,
        None
    }
    public enum eEnemyAnimation
    {
        IDLE                = 0,
        WALK,
        ATTACK,
        HIT,
        DEAD,
    }
    public enum eInteractKind
    {
        수리                 = 0,
        조사,
        None,
    }
    public enum eMapObjectKind
    {
        Enemys,
        Repairs,
        Research,
    }
    public enum eEnemyKind
    {
        Mutant,
    }
    public enum eRepairKind
    {
        BrokenDrone,
    }
    public enum eResearchKind
    {
        Blood,
    }
}
