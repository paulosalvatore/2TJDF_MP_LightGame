namespace ArdJoystick
{
    public enum ArdKeyCode
    {
        BUTTON_A,
        BUTTON_B,
        BUTTON_X,
        BUTTON_Y
    }

    public class ArdButton
    {
        private ArdKeyCode keyCode;

        public bool keyDown;
        public bool keyPress;
        public bool keyUp;

        private int oldData = 0;

        public bool processed;

        public ArdButton(ArdKeyCode keyCode)
        {
            this.keyCode = keyCode;
        }

        public void ProcessData(int data)
        {
            processed = false;

            if (data == 1 && oldData == 0)
            {
                keyDown = true;
            }
            else if (data == 0 && oldData == 1)
            {
                keyUp = true;
            }

            if (data == 1)
            {
                keyPress = true;
            }
            else
            {
                keyPress = false;
            }

            oldData = data;
        }
    }
}