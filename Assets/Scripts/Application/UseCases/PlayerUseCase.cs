using System;
using UnityEngine;
using ColorClone.Domain.Interfaces;

namespace ColorClone.Application.UseCases
{
    public class PlayerUseCase : IPlayerInteractor
    {
        public event Action OnDie;
        public event Action OnFinish;

        readonly Rigidbody2D _rb;
        readonly float _verticalForce;
        readonly SpriteRenderer _playerRenderer;
        readonly string[] _colorTags;
        readonly Color[] _colors;
        int _currentIndex;

        public PlayerUseCase(
            Rigidbody2D rb,
            float verticalForce,
            SpriteRenderer playerRenderer,
            string[] colorTags,
            Color[] colors)
        {
            _rb = rb;
            _verticalForce = verticalForce;
            _playerRenderer = playerRenderer;
            _colorTags = colorTags;
            _colors = colors;
            _currentIndex = 0;

            // Color inicial
            _playerRenderer.color = _colors[_currentIndex];
        }

        public void Jump()
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(Vector2.up * _verticalForce);
        }

        public void HandleTrigger(Collider2D other)
        {
            var tag = other.tag;

            // 1) ColorChanger
            if (tag == "ColorChanger")
            {
                ChangeColor();
                UnityEngine.Object.Destroy(other.gameObject);
                return;
            }

            // 2) FinishLine → evento Finish
            if (tag == "FinishLine")
            {
                OnFinish?.Invoke();
                return;
            }

            // 3) Cualquier otro trigger → si no coincide con el color actual, evento Die
            if (tag != _colorTags[_currentIndex])
            {
                OnDie?.Invoke();
            }
        }

        public void ChangeColor()
        {
            // Cambio aleatorio de color (como en el código original)
            _currentIndex = UnityEngine.Random.Range(0, _colors.Length);
            _playerRenderer.color = _colors[_currentIndex];
        }
    }
}