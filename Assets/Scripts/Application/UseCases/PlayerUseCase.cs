using System;
using UnityEngine;
using ColorClone.Domain.Interfaces;
using ColorClone.Domain.States;

namespace ColorClone.Application.UseCases
{
    public class PlayerUseCase : IPlayerInteractor, IPlayerContext
    {
        // Cambiar métodos y eventos a públicos según lo requerido por IPlayerContext
        public event Action OnDie;
        public event Action OnFinish;

        readonly Rigidbody2D _rb;
        readonly float _verticalForce;
        readonly SpriteRenderer _playerRenderer;
        readonly string[] _colorTags;
        readonly Color[] _colors;
        int _currentIndex;

        // Estados del jugador
        IPlayerState _currentState;
        readonly IPlayerState _aliveState;
        readonly IPlayerState _deadState;
        readonly IPlayerState _finishedState;

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

            // Instanciar estados
            _aliveState = new PlayerAliveState();
            _deadState = new PlayerDeadState();
            _finishedState = new PlayerFinishedState();
            _currentState = _aliveState;

            // Color inicial
            _playerRenderer.color = _colors[_currentIndex];
        }

        public void Jump()
        {
            _currentState.Jump(this);
        }

        public void HandleTrigger(Collider2D other)
        {
            // Solo procesar triggers si el jugador está en estado vivo
            if (_currentState != _aliveState)
                return;
            _currentState.HandleTrigger(this, other);
        }

        // Métodos auxiliares para los estados
        public void PerformJump()
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(Vector2.up * _verticalForce);
        }

        public void ProcessTriggerWhileAlive(Collider2D other)
        {
            // Solo procesar triggers si el jugador está en estado vivo
            if (_currentState != _aliveState)
                return;

            var tag = other.tag;
            Debug.Log($"Trigger detected with: {other.gameObject.name}, Tag: {tag}");
            Debug.Log($"Current player color tag: {_colorTags[_currentIndex]}");

            // 1) ColorChanger
            if (tag == "ColorChanger")
            {
                Debug.Log("ColorChanger triggered - changing color");
                ChangeColor();
                GameObject.Destroy(other.gameObject);
                return;
            }

            // 2) FinishLine (CORREGIDO: era "Finish" ahora es "FinishLine")
            if (tag == "FinishLine")
            {
                Debug.Log("FinishLine triggered - finishing level");
                ChangeState(_finishedState);
                // IMPORTANTE: Cambiar el estado y salir inmediatamente para evitar cualquier otro trigger
                return;
            }

            // 3) Obstáculo de color incorrecto
            if (tag != _colorTags[_currentIndex])
            {
                Debug.Log($"Wrong color collision! Expected: {_colorTags[_currentIndex]}, Got: {tag} - Player dies");
                // Solo cambiar a muerto si aún está vivo (doble chequeo por seguridad)
                if (_currentState == _aliveState)
                    ChangeState(_deadState);
            }
            else
            {
                Debug.Log($"Correct color collision: {tag}");
            }
        }

        public void ChangeState(IPlayerState newState)
        {
            if (_currentState == newState) return;

            Debug.Log($"Changing state from {_currentState.GetType().Name} to {newState.GetType().Name}");

            _currentState.Exit(this);
            _currentState = newState;
            _currentState.Enter(this);
        }

        // Cambiar ChangeColor a public solo si es necesario por la interfaz IPlayerInteractor
        public void ChangeColor()
        {
            int newIndex;
            do
            {
                newIndex = UnityEngine.Random.Range(0, _colors.Length);
            } while (newIndex == _currentIndex && _colors.Length > 1); // Evitar bucle infinito si solo hay un color

            _currentIndex = newIndex;
            _playerRenderer.color = _colors[_currentIndex];

            Debug.Log($"Player color changed to: {_colorTags[_currentIndex]}");
        }

        public void Die()
        {
            Debug.Log("PlayerUseCase.Die() called - invoking OnDie event");
            OnDie?.Invoke();
        }

        public void Finish()
        {
            Debug.Log("PlayerUseCase.Finish() called - invoking OnFinish event");
            OnFinish?.Invoke();
        }
    }
}