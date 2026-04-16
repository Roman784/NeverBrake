using GameRoot;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeEnterParams : SceneEnterParams
    {
        public readonly int LevelNumber;

        public ObstacleCourseModeEnterParams(int levelNumber) : base(Scenes.OBSTACLE_COURSE_MODE)
        {
            LevelNumber = levelNumber;
        }
    }
}
