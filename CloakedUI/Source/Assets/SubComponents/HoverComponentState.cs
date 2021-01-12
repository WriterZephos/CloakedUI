using System.Collections.Generic;
using ClkdUI.Assets.Interfaces;
using ClkdUI.Assets.SubComponents;
using ClkdUI.SubComponents;

namespace ClkdUI.Main
{
    /// <summary>
    /// HoverComponentState stores the state for a component
    /// when it is hovered. This class only responds to HandleStateRequests
    /// when the component is hovered, otherwise it forwards the request
    /// down the state hierarchy.
    /// </summary>
    public class HoverComponentState : AbstractGuiComponentState
    {
        /// <summary>
        /// The Margins around this GuiComponent.
        /// </summary>
        /// <typeparam name="GuiDirectionalVector4"></typeparam>
        /// <returns>GuiDirectionalVector4</returns>
        public GuiDirectionalVector4 Margin { get => HandleStateRequest<GuiDirectionalVector4>("Margin"); }

        /// <summary>
        /// The Padding inside this GuiComponent.
        /// </summary>
        /// <typeparam name="GuiDirectionalVector4"></typeparam>
        /// <returns>GuiDirectionalVector4</returns>
        public GuiDirectionalVector4 Padding { get => HandleStateRequest<GuiDirectionalVector4>("Padding"); }

        /// <summary>
        /// The Dimensions of this GuiComponent.
        /// </summary>
        /// <typeparam name="GuiDimensions"></typeparam>
        /// <returns>Dimensions</returns>
        public GuiDimensions Dimensions { get => HandleStateRequest<GuiDimensions>("Dimensions"); }

        /// <summary>
        /// The Border of this GuiComponent.
        /// </summary>
        /// <typeparam name="Border"></typeparam>
        /// <returns>Border</returns>
        public Border Border { get => HandleStateRequest<Border>("Border"); }

        /// <summary>
        /// The Background of this GuiComponent.
        /// </summary>
        /// <value></value>
        public Background Background { get => HandleStateRequest<Background>("Background"); }

        /// <summary>
        /// The Text of this GuiComponent;
        /// </summary>
        /// <typeparam name="Text"></typeparam>
        /// <returns></returns>
        public Text Text { get => HandleStateRequest<Text>("Text"); }

        /// <summary>
        /// Overrides base.HandleStateRequest to inject this state if the component
        /// is hovered, but otherwise forwards the request to the next lower
        /// component state.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>T</returns>
        internal override T HandleStateRequest<T>(string key, AbstractStatefulGuiComponent component)
        {
            if (component is AbstractInputGuiComponent temp && temp.Input.Hovered)
            {
                return base.HandleStateRequest<T>(key, component);
            }
            else
            {
                return NextState.HandleStateRequest<T>(key, component);
            }
        }

        /// <summary>
        /// Retrieves a collapsable state object, creating if it necessary.
        /// This is called when a user wants to assign state specifically to this 
        /// ComponenteState and not to another ComponenteState, so the call for 
        /// retrival is not passed on to lower states.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>T</returns>
        private T HandleStateRequest<T>(string key) where T : ICollapsableState, new()
        {
            ICollapsableState result;
            if (TryGetState(key, out result))
            {
                return (T)result.UnCollapse();
            }
            T newState = new T();
            State[key] = newState;
            return newState;
        }
    }
}