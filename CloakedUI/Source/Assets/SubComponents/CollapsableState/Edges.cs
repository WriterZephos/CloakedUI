using ClkdUI.Assets.Interfaces;

namespace ClkdUI.Assets.SubComponents
{
    public class Edges : ICollapsableState
    {
        private int? _radius;
        public int Radius
        {
            get => _radius ?? CollapseTo?.Radius ?? 0;
            set
            {
                if (CollapseTo != null) CollapseTo.Radius = value;
                else _radius = value;
            }
        }
        private float? _edgeBlurr;
        public float EdgeBlurr
        {
            get => _edgeBlurr ?? CollapseTo?.EdgeBlurr ?? 0f;
            set
            {
                if (CollapseTo != null) CollapseTo.EdgeBlurr = value;
                else _edgeBlurr = value;
            }
        }
        private float? _innerEdgetBlurr;
        public float InnerEdgeBlurr
        {
            get => _innerEdgetBlurr ?? CollapseTo?.InnerEdgeBlurr ?? 0f;
            set
            {
                if (CollapseTo != null) CollapseTo.InnerEdgeBlurr = value;
                else _innerEdgetBlurr = value;
            }
        }

        private Edges CollapseTo;
        public ICollapsableState Collapse(ICollapsableState other)
        {
            if (other is Edges temp) CollapseTo = temp;
            else CollapseTo = null;
            return this;
        }

        public ICollapsableState UnCollapse()
        {
            CollapseTo = null;
            return this;
        }
    }
}