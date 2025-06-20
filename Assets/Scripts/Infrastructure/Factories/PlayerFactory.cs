using ColorClone.Domain.Interfaces;
using ColorClone.Application.UseCases;
using UnityEngine;

namespace ColorClone.Infrastructure.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayerInteractor CreatePlayer(Rigidbody2D rb, float verticalForce, SpriteRenderer playerRenderer, string[] colorTags, Color[] colors)
        {
            return new PlayerUseCase(rb, verticalForce, playerRenderer, colorTags, colors);
        }
    }
}

