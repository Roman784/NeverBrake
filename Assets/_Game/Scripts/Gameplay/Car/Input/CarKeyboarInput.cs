using R3;
using UnityEngine;

namespace Gameplay
{
    public class CarKeyboarInput : CarInput
    {
        public override float GetHorizontalInput()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                return -1f;
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                return 1f;
            else return 0f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _jumpSignalSubj.OnNext(Unit.Default);
        }
    }
}
