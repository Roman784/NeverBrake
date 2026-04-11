using UnityEngine;

namespace _
{
    public class Car : MonoBehaviour
    {
        public float acceleration = 8f;
        public float maxSpeed = 12f;

        public float turnSpeed = 200f;

        [Range(0f, 1f)]
        public float grip = 0.9f; // сцепление (меньше = больше дрифта)

        public float driftFactor = 0.95f; // дополнительное скольжение

        private Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            float moveInput = Input.GetAxis("Vertical");
            float turnInput = Input.GetAxis("Horizontal");

            // Движение вперёд
            Vector2 forward = transform.up;
            rb.AddForce(forward * moveInput * acceleration);

            // Ограничение скорости
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }

            // Поворот
            float speedFactor = rb.linearVelocity.magnitude / maxSpeed;
            float turn = turnInput * turnSpeed * speedFactor * Time.fixedDeltaTime;

            rb.MoveRotation(rb.rotation - turn);

            // --- САМОЕ ВАЖНОЕ: СКОЛЬЖЕНИЕ ---

            Vector2 linearVelocity = rb.linearVelocity;

            // разложение скорости на компоненты
            Vector2 forwardVel = transform.up * Vector2.Dot(linearVelocity, transform.up);
            Vector2 sideVel = transform.right * Vector2.Dot(linearVelocity, transform.right);

            // уменьшаем боковую скорость (контроль сцепления)
            rb.linearVelocity = forwardVel + sideVel * (1f - grip);

            // дополнительный "ленивый" возврат — создаёт overshoot
            rb.linearVelocity *= driftFactor;
        }
    }
}
