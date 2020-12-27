using System.Collections.Generic;
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
        }

        public void AddGuiInput(GuiInput guiInput)
        {
            GuiInputs.Add(guiInput);
        }

        internal void RemoveGuiInput(GuiInput guiInput)
        {
            GuiInputs.Remove(guiInput);
        }
    }
}