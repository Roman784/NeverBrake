using UnityEngine;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeView : MonoBehaviour
    {
        private ObstacleCourseModePresenter _presenter;

        public void Init(ObstacleCourseModePresenter presenter)
        {
            _presenter = presenter;
        }

        public void DisplaySomething() { }
    }
}
