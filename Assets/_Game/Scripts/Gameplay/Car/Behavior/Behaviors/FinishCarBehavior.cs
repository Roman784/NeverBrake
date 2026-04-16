using DG.Tweening;
using GameRoot;
using UnityEngine;

namespace Gameplay
{
    public class FinishCarBehavior : CarBehavior
    {
        private Collider _collider;
        private Tween _portalSuctionTween;

        public FinishCarBehavior(CarBehaviorHandler handler, Car car) : base(handler, car)
        {
        }

        public void SetParams(Collider collider)
        {
            _collider = collider;
        }

        public override void Enter()
        {
            if (_collider.TryGetComponent<FinishPortal>(out var portal))
            {
                _car.CollisionRegister.Disable();
                _car.Controller.Stop();

                _car.View.SetActiveTireTracks(false);
                _portalSuctionTween =
                    _car.View.PlayPortalSuctionAnimation(portal.CenterPosition)
                        .OnComplete(() => G.SceneProvider.RestartScene());
                
                G.Camera.Zoom();
                return;
            }
        }

        public override void Exit()
        {
            base.Exit();

            _portalSuctionTween?.Kill();
        }
    }
}
