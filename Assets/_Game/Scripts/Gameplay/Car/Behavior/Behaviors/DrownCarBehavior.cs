using DG.Tweening;
using GameRoot;

namespace Gameplay
{
    public class DrownCarBehavior : CarBehavior
    {
        private Tween _fallingIntoWaterTween;

        public DrownCarBehavior(CarBehaviorHandler handler, Car car) : base(handler, car)
        {
        }

        public override void Enter()
        {
            _car.View.SetActiveTireTracks(false);

            _fallingIntoWaterTween = 
                _car.View.PlayFallingIntoWaterAnimation()
                .OnComplete(() => G.SceneProvider.RestartScene());
            
            G.Camera.Zoom();
        }

        public override void Update(float deltaTime) { }

        public override void Exit()
        {
            base.Exit();

            _fallingIntoWaterTween?.Kill(true);
        }
    }
}
