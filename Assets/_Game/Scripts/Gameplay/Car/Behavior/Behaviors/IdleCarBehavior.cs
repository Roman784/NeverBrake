using GameRoot;
using UnityEngine;

namespace Gameplay
{
    public class IdleCarBehavior : CarBehavior
    {
        public IdleCarBehavior(CarBehaviorHandler handler, Car car) : base(handler, car)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _car.View.SetActiveTireTracks(false);
        }
    }
}