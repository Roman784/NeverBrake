using DG.Tweening;
using GameRoot;
using UnityEngine;

namespace Gameplay
{
    public class FinishCarBehavior : CarBehavior
    {
        private Collider2D _collider;
        private Tween _portalSuctionTween;

        public FinishCarBehavior(CarBehaviorHandler handler, Car car) : base(handler, car)
        {
        }

        public void SetParams(Collider2D collider)
        {
            _collider = collider;
        }

        public override void Enter()
        {
            if (_collider.TryGetComponent<FinishPortal>(out var portal))
            {
                _car.CollisionRegister.DisableRegistration();
                _car.Controller.Stop();
                _car.View.SetActiveTireTracks(false);
                _car.View.PlayPortalSuctionAnimation(portal.CenterPosition);
                
                G.Camera.Zoom();
            }
        }

        public override void Exit()
        {
            base.Exit();

            _portalSuctionTween?.Kill();
        }
    }
}
