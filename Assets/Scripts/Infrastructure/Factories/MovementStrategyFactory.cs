using ColorClone.Domain.Interfaces;
using UnityEngine;

namespace ColorClone.Infrastructure.Factories
{
    /// <summary>
    /// Creates movement strategies for entities.
    /// </summary>
    public static class MovementStrategyFactory
    {
        public static IMovementStrategy CreateDefault(Transform target, float speed)
        {
            return new WalkMovementStrategy(target, speed);
        }

        private class WalkMovementStrategy : IMovementStrategy
        {
            private readonly Transform _transform;
            private readonly float _speed;

            public WalkMovementStrategy(Transform transform, float speed)
            {
                _transform = transform;
                _speed = speed;
            }

            public void Move(float horizontal, float vertical)
            {
                Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
                _transform.Translate(dir * _speed * Time.deltaTime, Space.World);
            }
        }
    }
}