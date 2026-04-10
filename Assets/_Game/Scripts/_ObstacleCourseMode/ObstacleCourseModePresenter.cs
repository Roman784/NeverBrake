using R3;
using System;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModePresenter
    {
        private readonly ObstacleCourseModeModel _model;

        private readonly CompositeDisposable _disposables = new();

        public ObstacleCourseModePresenter(ObstacleCourseModeModel model)
        {
            _model = model;
        }
    }
}
