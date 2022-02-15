using UnityEngine;

namespace Assets.Scripts.Components
{
    public class MoveComponent : MonoBehaviour
    {

        private float moveX;
        private float moveY;
        private Vector2 movementDirection;
        private float movementSpeed;

        public Vector2 MoveBehaviour(Vector2 movementInput, Animator animator)
        {
            moveX = movementInput.x;
            moveY = movementInput.y;
            movementDirection = new Vector2(moveX, moveY);

            animator.SetFloat("moveX", moveX);
            animator.SetFloat("moveY", moveY);

            movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
            movementDirection.Normalize();

            return movementInput;
        }

        public Vector2 Move(Rigidbody2D rb2d , float movementBaseSpeed)
        {
            return movementDirection * movementSpeed * movementBaseSpeed;
        }
    }
}
