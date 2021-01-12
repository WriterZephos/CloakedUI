using System;
using System.Collections.Generic;
using ClkdUI.Assets;
using ClkdUI.Assets.Interfaces;
using ClkdUI.Assets.SubComponents;
using ClkdUI.Main;

namespace ClkdUI.SubComponents
{
    /// <summary>
    /// Contains state for a gui component. This class
    /// is meant to be used in a hierarchy of GuiComponentState
    /// where each GuiComponentState can handle requests for
    /// state or forward the requests down the hierarchy. 
    /// </summary>
    public abstract class AbstractGuiComponentState
    {
        /// <summary>
        /// A Dictionary of ICollapsableState that is stored
        /// by this GuiComponentState.
        /// </summary>
        /// <value>Dictionary<string, ICollapsableState></value>
        internal Dictionary<string, ICollapsableState> State { get; set; }

        /// <summary>
        /// The next GuiComponentState in the state hierarchy.
        /// </summary>
        /// <value>AbstractGuiComponentState</value>
        internal AbstractGuiComponentState NextState { get; set; }

        /// <summary>
        /// Instantiates an AbstractGuiComponentState. The State
        /// dictionary of ICollapsableState objecst is initialized
        /// at the time of instantiation.
        /// </summary>
        protected AbstractGuiComponentState()
        {
            State = new Dictionary<string, ICollapsableState>();
        }

        /// <summary>
        /// Gets the ICollapsableState that corresponds to
        /// the provided key, or null if there is none.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected bool TryGetState(string key, out ICollapsableState state)
        {
            return State.TryGetValue(key, out state);
        }

        /// <summary>
        /// Returns an ICollapsableState object of type T corresponding to the
        /// provided key from the first GuiComponentState in the state hierarchy 
        /// that is able to fullfill the request, if any exists.
        /// 
        /// If no state is able to handle the request, a new ICollapsableState of type T
        /// is created and stored in this GuiComponentState, then returned.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal virtual T HandleStateRequest<T>(string key, AbstractStatefulGuiComponent component) where T : ICollapsableState, new()
        {
            ICollapsableState result;
            if (TryGetState(key, out result))
            {
                ICollapsableState nextResult = null;
                if (NextState != null)
                {
                    nextResult = NextState.HandleStateRequest<T>(key, component);
                }
                return (T)result.Collapse(nextResult);
            }
            else if (NextState != null)
            {
                return NextState.HandleStateRequest<T>(key, component);
            }
            T newState = new T();
            State[key] = newState;
            return newState;
        }

        /// <summary>
        /// Sets the next GuiComponentState in the hierarchy after this one, then
        /// returns it so that SetNext can be called on that one to build the
        /// hierarchy through method chaining.
        /// </summary>
        /// <param name="next"></param>
        /// <returns>AbstractGuiComponentState</returns>
        internal AbstractGuiComponentState SetNext(AbstractGuiComponentState next)
        {
            NextState = next;
            return next;
        }

        /// <summary>
        /// Sets this GuiComponentState as the next one after the provided
        /// GuiComponentState, then returns this GuiComponentState.
        /// </summary>
        /// <param name="before"></param>
        /// <returns>AbstractGuiComponentState</returns>
        internal AbstractGuiComponentState SetBefore(AbstractGuiComponentState before)
        {
            before.SetNext(this);
            return before;
        }

        /// <summary>
        /// Uncollapses all this GuiComponentState's ICollapsableState
        /// objects.
        /// </summary>
        /// <returns></returns>
        internal AbstractGuiComponentState Uncollapse()
        {
            foreach (ICollapsableState state in State.Values)
            {
                state.UnCollapse();
            }
            return this;
        }
    }
}