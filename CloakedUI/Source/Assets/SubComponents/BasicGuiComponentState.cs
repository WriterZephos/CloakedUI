using System.Collections.Generic;
using ClkdUI.Assets.Interfaces;
using ClkdUI.Main;
using ClkdUI.SubComponents;

namespace ClkdUI.Assets.SubComponents
{
    /// <summary>
    /// BasicGuiComponentState is a basic implementation of AbstractGuiComponent.
    /// 
    /// BasicGuiComponentState state for a gui component. This class
    /// is meant to be used in a hierarchy of GuiComponentState
    /// where each GuiComponentState can handle requests for
    /// state or forward the requests down the hierarchy.
    /// </summary>
    public class BasicGuiComponentState : AbstractGuiComponentState { }
}