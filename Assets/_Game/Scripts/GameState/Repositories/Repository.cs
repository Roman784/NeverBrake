namespace GameState
{
    public class Repository
    {
        public readonly AudioRepository Audio;
        public readonly LevelsRepository Levels;
        public readonly CarsRepository Cars;

        public Repository(IGameStateProvider gameStateProvider)
        {
            Audio = new AudioRepository(gameStateProvider);
            Levels = new LevelsRepository(gameStateProvider);
            Cars = new CarsRepository(gameStateProvider);
        }
    }
}
