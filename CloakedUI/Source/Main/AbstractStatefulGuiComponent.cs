using System;
using Clkd.Main;
using ClkdUI.Assets.Interfaces;
using ClkdUI.SubComponents;

namespace ClkdUI.Main
{
    /// <summary>
    /// AbstractStatefulGuiComponent is an AbstractComponent
    /// with an InternalState property of type
    /// AbstractGuiComponentState.
    /// 
    /// This class also contains internal and protected methods
    /// for use by subclasses.
    /// </summary>
    public abstract class AbstractStatefulGuiComponent : AbstractComponent
    {
        /// <summary>
        /// This GuiComponent's state. This property references the top of
        /// the state hierarchy, which is a chain of GuiComponentState objects
        /// that form a chain of responsibility. When any program needs to
        /// access this component's state, the top GuiComponentState in
        /// the hierarchy gets the chance to fulfill the the request or
        /// or pass it down the chain to a GuiComponentState that it holds
        /// a reference to. This allows state properties to be overridden
        /// conditionally without defining every property for every possible
        /// scenario, as any requests for properties in a GuiComponentState
        /// that are not defined can be forwarded down the hierarchy, while
        /// properties that are defined can be returned.
        /// </summary>
        /// <value></value>
        protected AbstractGuiComponentState InternalState { get; set; }

        /// <summary>
        /// The constructor for AbstractStatefulGuiComponent. The constructor callse SetState
        /// and calls the constructor for the base class AbstractComponent with
        /// canGetRenderables: true, canUpdate: true.
        /// </summary>
        /// <returns></returns>
        public AbstractStatefulGuiComponent() : base(canGetRenderables: true, canUpdate: true)
        {
            SetState();
        }

        /// <summary>
        /// Sets the InternalState for this GuiComponent. This method is called from
        /// the constructor and should be overriden by by concrete types that inherit
        /// from AbstractStatefulGuiComponents to add any state they will use.
        /// </summary>
        /// <returns></returns>
        protected abstract AbstractGuiComponentState SetState();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal T GetInternalState<T>(string key, AbstractStatefulGuiComponent component) where T : ICollapsableState, new()
        {
            return InternalState.HandleStateRequest<T>(key, component);
        }
    }
}