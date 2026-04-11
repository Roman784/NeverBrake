using R3;
using UnityEngine;

namespace Gameplay
{
    public class CarKeyboarInput : CarInput
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public override int GetHorizontalInput()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                return -1;
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                return 1;
            else
                return 0;

            //if (!Input.GetKey(KeyCode.Mouse0)) return 0;

            //var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            //return mousePosition.x > 0 ? 1 : -1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _jumpSignalSubj.OnNext(Unit.Default);
        }
    }
}
