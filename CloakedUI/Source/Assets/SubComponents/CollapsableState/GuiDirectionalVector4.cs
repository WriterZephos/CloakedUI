using ClkdUI.Assets.Interfaces;

namespace ClkdUI.Assets.SubComponents
{
    public class GuiDirectionalVector4 : ICollapsableState
    {
        private float? _top;
        public float Top
        {
            get => _top ?? CollapseTo?.Top ?? 0f;
            set
            {
                if (CollapseTo == null) _top = value;
                else CollapseTo.Top = value;
            }
        }
        private float? _bottom;
        public float Bottom
        {
            get => _bottom ?? CollapseTo?.Bottom ?? 0f;
            set
            {
                if (CollapseTo == null) _bottom = value;
                else CollapseTo.Bottom = value;
            }
        }
        private float? _left;
        public float Left
        {
            get => _left ?? CollapseTo?.Left ?? 0f;
            set
            {
                if (CollapseTo == null) _left = value;
                else CollapseTo.Left = value;
            }
        }
        private float? _right;
        public float Right
        {
            get => _right ?? CollapseTo?.Right ?? 0f;
            set
            {
                if (CollapseTo == null) _right = value;
                else CollapseTo.Right = value;
            }
        }

        private GuiDirectionalVector4 CollapseTo { get; set; }

        public ICollapsableState Collapse(ICollapsableState other)
        {
            if (other is GuiDirectionalVector4 temp) CollapseTo = temp;
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