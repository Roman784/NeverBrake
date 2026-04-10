namespace GameState
{
    public class Repository
    {
        public readonly AudioRepository Audio;

        public Repository(IGameStateProvider gameStateProvider)
        {
            Audio = new AudioRepository(gameStateProvider);
        }
    }
}
