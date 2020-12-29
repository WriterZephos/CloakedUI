using System.Collections.Generic;
using Clkd.Assets;
using Clkd.Main;
using Clkd.Managers;
using ClkdUI.Assets;

namespace ClkdUI.Main
{
    public class GuiInputManager
    {

        public KeyboardInputManager KeyboardInputManager { get; set; }
        public MouseInputManager MouseInputManager { get; set; }
        public List<GuiInput> GuiInputs { get; set; }

        public GuiInputManager(KeyboardInputManager keyboardInputManager = null, MouseInputManager mouseInputManager = null)
        {
            KeyboardInputManager = keyboardInputManager;
            MouseInputManager = mouseInputManager;
            GuiInputs = new List<GuiInput>();
        }

        public GuiInputManager(GameContext gameContext)
        {
            KeyboardInputManager = gameContext.DefaultKeyboardInput;
            MouseInputManager = gameContext.DefaultMouseInput;
            GuiInputs = new List<GuiInput>();
        }

        public void AddGuiInput(GuiInput guiInput)
        {
            GuiInputs.Add(guiInput);
            GuiInputs.Sort();
        }

        internal void RemoveGuiInput(GuiInput guiInput)
        {
            GuiInputs.Remove(guiInput);
        }

        private void InitializeTriggers()
        {
            KeyboardInputManager.RegisterKeyMapping(
                new KeyMapping(
                    actionName: "GuiInputManager",
                    priority: 0,
                    anyKey: true),
                new GenericInputActionTrigger<KeyStatus>((status) => true, (status) => { this.BroadcastKeyEvents(status); }));
        }

        private void BroadcastKeyEvents(KeyStatus status)
        {
            foreach (GuiInput guiInput in GuiInputs)
            {
                // TODO broadcast events if appropriate
            }

            // Iterate over GuiInputs
            // Do calculations to determine which events should be published
        }

        private bool IsFocused(GuiInput guiInput)
        {
            return guiInput.Subject.Focused;
        }
    }
}