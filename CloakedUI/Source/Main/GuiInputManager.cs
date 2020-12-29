using System.Collections.Generic;
using Clkd.Assets;
using Clkd.Main;
using Clkd.Managers;
using ClkdUI.Assets;
using Microsoft.Xna.Framework;

namespace ClkdUI.Main
{
    public class GuiInputManager
    {

        public KeyboardInputManager KeyboardInputManager { get; set; }
        public MouseInputManager MouseInputManager { get; set; }
        public List<GuiInput> GuiInputs { get; set; }

        public GuiInputManager(KeyboardInputManager keyboardInputManager, MouseInputManager mouseInputManager)
        {
            KeyboardInputManager = keyboardInputManager;
            MouseInputManager = mouseInputManager;
            GuiInputs = new List<GuiInput>();
            InitializeTriggers();
        }

        public GuiInputManager(GameContext gameContext)
        {
            KeyboardInputManager = gameContext.DefaultKeyboardInput;
            MouseInputManager = gameContext.DefaultMouseInput;
            GuiInputs = new List<GuiInput>();
            InitializeTriggers();
        }

        public void AddGuiInput(GuiInput guiInput)
        {
            GuiInputs.Add(guiInput);
            GuiInputs.Sort();
        }

        internal void RemoveGuiInput(GuiInput guiInput)
        {
            GuiInputs.Remove(guiInput);
            GuiInputs.Sort();
        }

        private void InitializeTriggers()
        {
            Cloaked.Game.Window.TextInput += new System.EventHandler<TextInputEventArgs>(BroadCastTextEvent);
            KeyboardInputManager.RegisterKeyMapping(
                KeyMapping.GetMappingToAnyKey("GuiInputManager", priority: 0),
                new GenericInputActionTrigger<KeyStatus>((status) => true, (status) => { this.BroadcastKeyEvents(status); })
            );

            MouseInputManager.RegisterMouseMapping(
                MouseMapping.GetConstantMapping("GuiInputManager", priority: 0),
                new GenericInputActionTrigger<MouseStatus>((status) => true, (status) => { this.BroadcastMouseEvents(status); })
            );
        }

        private void BroadCastTextEvent(object sender, TextInputEventArgs textInputEventArgs)
        {
            foreach (GuiInput guiInput in GuiInputs)
            {
                if (guiInput.Subject.Focused)
                {
                    guiInput.PublishOnTextEntered(sender, textInputEventArgs);
                }
            }
        }

        private void BroadcastKeyEvents(KeyStatus status)
        {
            foreach (GuiInput guiInput in GuiInputs)
            {
                if (guiInput.Subject.Focused)
                {
                    if (status.IsKeyPressed())
                    {
                        guiInput.PublishOnKeyPressed(status);
                    }
                    else if (status.IsKeyHeld())
                    {
                        guiInput.PublishOnKeyHeld(status);
                    }
                    else if (status.IsKeyReleased())
                    {
                        guiInput.PublishOnKeyReleased(status);
                    }
                }
                if (status.StopPropogation) return;
            }
        }

        private void BroadcastMouseEvents(MouseStatus mouseStatus)
        {
            foreach (GuiInput guiInput in GuiInputs)
            {
                if (guiInput.IsHovered(mouseStatus.MouseState) && !guiInput.IsHovered(mouseStatus.PreviousMouseState))
                {
                    guiInput.PublishOnMouseEnter(mouseStatus);
                    guiInput.PublishOnMouseHover(mouseStatus);
                }
                else if (guiInput.IsHovered(mouseStatus.MouseState))
                {
                    guiInput.PublishOnMouseHover(mouseStatus);
                }
                else if (!guiInput.IsHovered(mouseStatus.MouseState) && guiInput.IsHovered(mouseStatus.PreviousMouseState))
                {
                    guiInput.PublishOnMouseExit(mouseStatus);
                }

                if (guiInput.IsHovered(mouseStatus.MouseState))
                {
                    if (mouseStatus.IsClicked())
                    {
                        guiInput.PublishOnMouseClicked(mouseStatus);
                    }
                    else if (mouseStatus.IsHeld())
                    {
                        guiInput.PublishOnMouseHeld(mouseStatus);
                    }
                    else if (mouseStatus.IsReleased())
                    {
                        guiInput.PublishOnMouseReleased(mouseStatus);
                    }

                    //TODO add scroll support
                    // if (guiInput.Subject.Focused && mouseStatus.MouseScrolled())
                    // {
                    //     guiInput.PublishOnMouseScrolled(status);
                    // } else
                }

                if (mouseStatus.StopPropogation) return;
            }
        }
    }
}
