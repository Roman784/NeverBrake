using GameRoot;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeEnterParams : SceneEnterParams
    {
        public readonly int LevelNumber;
        public readonly int CarId;

        public ObstacleCourseModeEnterParams(int levelNumber, int carId) 
            : base(Scenes.OBSTACLE_COURSE_MODE)
        {
            LevelNumber = levelNumber;
            CarId = carId;
        }
    }
}
