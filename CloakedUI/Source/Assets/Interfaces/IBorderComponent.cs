using Clkd.Assets.SubComponents;

namespace ClkdUI.Assets.Interfaces
{
    public interface IBorderComponent : IEdgedComponent
    {
        Border Border { get; set; }
    }
}