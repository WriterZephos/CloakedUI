using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Clkd.GUI.Interfaces;

[assembly: InternalsVisibleTo("CloakedUITests")]
namespace Clkd.GUI.Layouts
{
    public class GuiGridLayout : AbstractGuiLayout
    {
        public AbstractGuiComponent[,] Components { get; set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public float HorizontalGutter { get; set; }
        public float VerticalGutter { get; set; }

        public GuiGridLayout(int rows, int columns)
        {
            Dirty = true;
            Rows = rows;
            Columns = columns;
            Components = new AbstractGuiComponent[Rows, Columns];
        }

        public void AddComponent(int row, int column, AbstractGuiComponent component)
        {
            if (Components[row, column] == null)
            {
                Components[row, column] = component;
                Dirty = true;
            }
            else
            {
                throw new ArgumentException("A component already exists in the specified location.");
            }
        }

        public AbstractGuiComponent GetGuiComponent(int row, int column)
        {
            return Components[row, column];
        }
        public override IEnumerator<AbstractGuiComponent> GetEnumerator()
        {
            return Components.Cast<AbstractGuiComponent>().GetEnumerator();
        }

        internal override void RecalculateChildren(GuiContainer parent)
        {
            int row = 0;
            int column = 0;
            float yOffset = 0f;
            float xOffset = 0f;
            float maxWidth = 0f;
            foreach (AbstractGuiComponent component in Components)
            {
                if (row == 0)
                {
                    yOffset = parent.TopPadding;
                }
                else
                {
                    yOffset += HorizontalGutter;
                }

                if (column == 0)
                {
                    xOffset = parent.LeftPadding;
                    maxWidth = 0f;
                }

                if (component != null)
                {
                    SetChildPosition(
                        parent: parent,
                        childComponent: component,
                        xOffset: xOffset,
                        yOffset: yOffset);

                    if (component is GuiContainer c)
                    {
                        c.Layout.RecalculateChildren(c);
                    }

                    yOffset += component.RealHeight;
                    if (component.RealWidth > maxWidth)
                    {
                        maxWidth = component.RealWidth;
                    }
                }

                row++;
                if (row == Rows)
                {
                    column++;
                    row = 0;
                    xOffset += VerticalGutter;
                    xOffset += maxWidth;
                }
            }
            Dirty = false;
        }

        private void SetChildPosition(GuiContainer parent, AbstractGuiComponent childComponent, float xOffset, float yOffset)
        {
            childComponent.UpdatePosition(
                parent: parent,
                xOffset: xOffset,
                yOffset: yOffset);

        }

    }
}