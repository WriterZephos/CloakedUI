namespace ClkdUI.Assets.Interfaces
{
    /// <summary>
    /// This interface defines the API for statefull subcomponents
    /// that have properties that can be dynamically overriden.
    ///
    /// Any class that implements this interface should store the ICollapsableState
    /// argument passed to Collapse and forward property getter calls that is can't
    /// or chooses not to fulfill to that object. The expectation would
    /// be that that object is the same type or at least has the same
    /// interface, and therefore can respond to the same getter calls.
    /// A type check will probably be necessary as part of the Collapse
    /// method's implementation. Setter calls can likewise be handled
    /// or forwarded.
    /// </summary>
    public interface ICollapsableState
    {
        /// <summary>
        /// Collapses this ICollapsableState into the provided
        /// ICollapsableState, so that any gets for null values
        /// are forwarded and all sets are forwarded.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        ICollapsableState Collapse(ICollapsableState other);

        /// <summary>
        /// Returns this ICollapsableState uncollapsed,
        /// so that all property gets and sets will be its
        /// own.
        /// </summary>
        /// <returns></returns>
        ICollapsableState UnCollapse();
    }
}