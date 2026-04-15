using UnityEngine;

namespace Gameplay
{
    public class CarKeyboarInput : CarInput
    {
        public override int GetHorizontalInput()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                return -1;
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                return 1;
            else
                return 0;
        }

        public override bool ShouldJump()
        {
            return
                Input.GetKey(KeyCode.Space) ||
                Input.GetKey(KeyCode.W) || 
                Input.GetKey(KeyCode.UpArrow);
        }

        public override bool ShouldStartMoving()
        {
            return ShouldJump() || GetHorizontalInput() != 0;
        }
    }
}
