namespace JustGame.SubwaySurfer
{
    public enum Lanes
    {
        Left,
        Middle,
        Right,
    }

    public enum PoolObjectType
    {
        BridgeBlock1,
        BridgeBlock2,
        CityBlock1,
        CityBlock2,
        CityBlock3,
        CrossingBlock1,
        CrossingBlock2,
        SubUrbanBlock1,
        SubUrbanBlock2,
        TunnelBlock,
        ObstacleSet1,
        ObstacleSet2,
        Coin,
    }


    public enum PanelStates
    {
        None,
        Loading,
        Instruction,
        Intro,
        GamePlay,
        GameOver,
    }

    public enum GamePlayState
    {
        None,
        Play,
        Pause,
        OnCollision
    }

    public enum AudioType
    {
        BackgroundMusic,
        Intro,
        Hurt,
        CoinCollect,
        ButtonClick,
        GameOver
    }

    public enum TagType
    {
        Obstacle,
        Ground,
        Coin
    }
}