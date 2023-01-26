public class GameState
{
    public enum Stage
    {
        Preparing,
        Playing,
        Won,
        Lost
    }

    public bool IsInitState { get; set; }
    public Stage GameStage { get; set; }
    public float AverageMouseSpeed { get; set; }
    public float AverageMouseClicks { get; set; }
    public float LevelProgess { get; set; }
    public int DefendersCount { get; set; }
    public int SunflowersCount { get; set; }    // included in DefendersCount
    public int EnemiesCount { get; set; }

    public override string ToString()
    {
        return $"IsInitState: {IsInitState}, GameStage: {GameStage}, AverageMouseSpeed: {AverageMouseSpeed}, AverageMouseClicks: {AverageMouseClicks}" +
            $"LevelProgess: {LevelProgess}, ActDef-Att: {DefendersCount - SunflowersCount - EnemiesCount}";
    }
}
