using R3;
using System;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModePresenter : IDisposable
    {
        private readonly ObstacleCourseModeView _view;
        private readonly ObstacleCourseModeModel _model;

        private readonly CompositeDisposable _disposables = new();

        public ObstacleCourseModePresenter(
            ObstacleCourseModeView view, 
            ObstacleCourseModeModel model)
        {
            _view = view;
            _model = model;
        
            SetUpSubscriptions();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void SetUpSubscriptions()
        {

        }
    }
}
