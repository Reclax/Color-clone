namespace ColorClone.Domain.Interfaces
{
    public interface IPlayerFactory
    {
        IPlayerInteractor CreatePlayer(UnityEngine.Rigidbody2D rb, float verticalForce, UnityEngine.SpriteRenderer playerRenderer, string[] colorTags, UnityEngine.Color[] colors);
    }
}

