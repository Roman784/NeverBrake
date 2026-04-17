namespace GameState
{
    public class Repository
    {
        public readonly AudioRepository Audio;
        public readonly LevelsRepository Levels;

        public Repository(IGameStateProvider gameStateProvider)
        {
            Audio = new AudioRepository(gameStateProvider);
            Levels = new LevelsRepository(gameStateProvider);
        }
    }
}
