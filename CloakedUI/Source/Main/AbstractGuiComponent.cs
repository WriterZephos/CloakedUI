using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Clkd.Assets;
using Clkd.Main;
using ClkdUI.Assets;
using ClkdUI.Exceptions;
using System.Linq;
using ClkdUI.Assets.SubComponents;
using ClkdUI.SubComponents;
using ClkdUI.Assets.Interfaces;

namespace ClkdUI.Main
{
    public abstract class AbstractGuiComponent : AbstractStatefulGuiComponent
    {
        public GuiCoordinate Coordinate { get; set; }
        public GuiDirectionalVector4 Margin { get => GetInternalState<GuiDirectionalVector4>("Margin", this); }
        public GuiDirectionalVector4 Padding { get => GetInternalState<GuiDirectionalVector4>("Padding", this); }
        public GuiDimensions Dimensions { get => GetInternalState<GuiDimensions>("Dimensions", this); }
        public Background Background { get => GetInternalState<Background>("Background", this); }
        public Border Border { get => GetInternalState<Border>("Border", this); }
        public Edges Edges { get => GetInternalState<Edges>("Edges", this); }
        public Text Text { get => GetInternalState<Text>("Text", this); }

        protected bool _positionInitialized = false;

        public AbstractGuiComponent()
        {
            Coordinate = new GuiCoordinate(this);
            SetState();
        }

        protected override AbstractGuiComponentState SetState()
        {
            InternalState = new BasicGuiComponentState();
            return InternalState;
        }

        public abstract override List<IRenderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null);

        public abstract override void Update(GameTime gameTime);
        internal virtual void UpdateInternal(GameTime gameTime)
        {
            Update(gameTime);
        }

        public List<AbstractGuiComponent> GetHierarchy()
        {
            List<AbstractGuiComponent> components = new List<AbstractGuiComponent>();
            components.Add(this);
            if (!_positionInitialized)
            {
                GuiCoordinate currentCoordinate = Coordinate;
                while (currentCoordinate.Parent != null)
                {
                    components.Add(currentCoordinate.Parent);
                    currentCoordinate = currentCoordinate.Parent.Coordinate;
                }
            }
            return components;
        }

        internal virtual void UpdatePosition(AbstractGuiComponent parent, Vector2 offsets)
        {
            if (!_positionInitialized) _positionInitialized = true;
            Coordinate.UpdateCoordinate(
                parent: parent,
                offsets: offsets);
        }

        internal virtual void UpdatePosition(GuiCoordinate guiCoordinate, Vector2 offsets)
        {
            if (!_positionInitialized) _positionInitialized = true;
            Coordinate.UpdateCoordinate(
                guiCoordinate: guiCoordinate,
                offsets: offsets);
        }

        protected GuiPane GetRootPane()
        {
            if (!_positionInitialized) throw new OrphanedGuiComponentException("This component has not been positioned by a parent, and therefore has no reference to a parent.");
            GuiCoordinate currentCoordinate = Coordinate;
            while (currentCoordinate.Parent != null)
            {
                currentCoordinate = currentCoordinate.Parent.Coordinate;
            }

            if (currentCoordinate.Child is GuiPane gp)
            {
                return gp;
            }

            throw new OrphanedGuiComponentException("Could not reach a GuiPane instance by traversing up the hierarchy.");
        }
    }
}