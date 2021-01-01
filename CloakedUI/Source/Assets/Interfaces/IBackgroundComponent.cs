using ClkdUI.Assets.SubComponents;

namespace ClkdUI.Assets.Interfaces
{
    public interface IBackgroundComponent : IEdgedComponent
    {
        Background Background { get; set; }
    }
}