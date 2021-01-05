using ClkdUI.Assets.SubComponents;
using Microsoft.Xna.Framework;

namespace ClkdUI.Assets.Interfaces
{
    public interface IBorderComponent : IEdgedComponent
    {
        Border Border { get; set; }
    }
}