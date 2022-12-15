using System.Runtime.InteropServices;


namespace SleepPreventor
{
    /// <summary>
    /// Main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Size of <see cref="Input"/>.
        /// </summary>
        private static readonly int SizeOfInput;

        /// <summary>
        /// Initialize static members.
        /// </summary>
        static Program()
        {
            SizeOfInput = Marshal.SizeOf(typeof(Input));
        }

        /// <summary>
        /// An entry point of this program.
        /// </summary>
        public static void Main()
        {
            Console.WriteLine("Start Sleep Preventor.");

            var inputs = new[]
            {
                new Input()
                {
                    Type = InputType.Mouse,
                    Ui = new InputUnion()
                    {
                        Mouse = new MouseInput()
                        {
                            X = 1,
                            Y = 0,
                            Data = 0,
                            Flags = MouseEventF.Move,
                            Time = 0,
                            ExtraInfo = IntPtr.Zero
                        }
                    }
                },
                new Input()
                {
                    Type = InputType.Mouse,
                    Ui = new InputUnion()
                    {
                        Mouse = new MouseInput()
                        {
                            X = -1,
                            Y = 0,
                            Data = 0,
                            Flags = MouseEventF.Move,
                            Time = 0,
                            ExtraInfo = IntPtr.Zero
                        }
                    }
                },
            };

            for (; ; )
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    SendInput(ref inputs[i]);
                    Thread.Sleep(10000);
                }
            }
        }

        /// <summary>
        /// Managed wrapper of <see cref="NativeMethods.SendInput(int, ref Input, int)"/>.
        /// </summary>
        /// <param name="input">An reference of <see cref="Input"/> structures.
        /// This structure represents an event to be inserted into the keyboard or mouse input stream.</param>
        /// <returns>The number of events that it successfully inserted into the keyboard or mouse input stream.</returns>
        private static int SendInput(ref Input input)
        {
            return NativeMethods.SendInput(1, ref input, SizeOfInput);
        }

        /// <summary>
        /// Managed wrapper of <see cref="NativeMethods.SendInput(int, Input[], int)"/>.
        /// </summary>
        /// <param name="input">An array of <see cref="Input"/> structures.
        /// Each structure represents an event to be inserted into the keyboard or mouse input stream.</param>
        /// <returns>The number of events that it successfully inserted into the keyboard or mouse input stream.</returns>
        private static int SendInput(Input[] inputs)
        {
            return NativeMethods.SendInput(inputs.Length, inputs, SizeOfInput);
        }
    }

    /// <summary>
    /// The type of the input event.
    /// </summary>
    internal enum InputType
    {
        /// <summary>
        /// The event is a mouse event. Use the <see cref="InputUnion.Mouse"/> structure of the union.
        /// </summary>
        Mouse = 0,
        /// <summary>
        /// The event is a keyboard event. Use the <see cref="InputUnion.Keyboard"/> structure of the union.
        /// </summary>
        Keyboard = 1,
        /// <summary>
        /// The event is a hardware event. Use the <see cref="InputUnion.Hardware"/> structure of the union.
        /// </summary>
        Hardware = 2,
    }

    /// <summary>
    /// Flag values of <see cref="MouseInput.Flags"/>.
    /// </summary>
    /// <remarks>
    /// <para><seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mouseinput"/></para>
    /// <para>If the mouse has moved, indicated by <see cref="Move"/>, dx and dy specify information about that movement.
    /// The information is specified as absolute or relative integer values.</para>
    /// <para>If <see cref="Absolute"/> value is specified, <see cref="MouseInput.X"/> and <see cref="MouseInput.Y"/> contain normalized absolute coordinates between 0 and 65,535.
    /// The event procedure maps these coordinates onto the display surface.
    /// Coordinate (0,0) maps onto the upper-left corner of the display surface; coordinate (65535, 65535) maps onto the lower-right corner.
    /// In a multimonitor system, the coordinates map to the primary monitor.</para>
    /// <para>If <see cref="VirtualDesk"/> is specified, the coordinates map to the entire virtual desktop.</para>
    /// <para>If the <see cref="Absolute"/> value is not specified, <see cref="MouseInput.X"/> and <see cref="MouseInput.Y"/> specify movement relative to the previous mouse event (the last reported position).
    /// Positive values mean the mouse moved right (or down); negative values mean the mouse moved left (or up).</para>
    /// <para>Relative mouse motion is subject to the effects of the mouse speed and the two-mouse threshold values.
    /// A user sets these three values with the Pointer Speed slider of the Control Panel's Mouse Properties sheet.
    /// You can obtain and set these values using the SystemParametersInfo function.</para>
    /// <para>The system applies two tests to the specified relative mouse movement.
    /// If the specified distance along either the x or y axis is greater than the first mouse threshold value, and the mouse speed is not zero, the system doubles the distance.
    /// If the specified distance along either the x or y axis is greater than the second mouse threshold value, and the mouse speed is equal to two, the system doubles the distance that resulted from applying the first threshold test.
    /// It is thus possible for the system to multiply specified relative mouse movement along the x or y axis by up to four times.</para>
    /// </remarks>
    [Flags]
    internal enum MouseEventF : int
    {
        /// <summary>
        /// Movement occurred.
        /// </summary>
        Move = 0x00000001,
        /// <summary>
        /// The left button was pressed.
        /// </summary>
        LeftDown = 0x00000002,
        /// <summary>
        /// The left button was released.
        /// </summary>
        LeftUp = 0x00000004,
        /// <summary>
        /// The right button was pressed.
        /// </summary>
        RightDown = 0x00000008,
        /// <summary>
        /// The right button was released.
        /// </summary>
        RightUp = 0x00000010,
        /// <summary>
        /// The middle button was pressed.
        /// </summary>
        MiddleDown = 0x00000020,
        /// <summary>
        /// The middle button was released.
        /// </summary>
        MiddleUp = 0x00000040,
        /// <summary>
        /// An X button was pressed.
        /// </summary>
        XDown = 0x00000080,
        /// <summary>
        /// An X button was released.
        /// </summary>
        XUp = 0x00000100,
        /// <summary>
        /// The wheel was moved, if the mouse has a wheel.
        /// The amount of movement is specified in <see cref="MouseInput.Data"/>.
        /// </summary>
        Wheel = 0x00000800,
        /// <summary>
        /// <para>The wheel was moved horizontally, if the mouse has a wheel.
        /// The amount of movement is specified in <see cref="MouseInput.Data"/>.</para>
        /// <para>Windows XP/2000: This value is not supported.</para>
        /// </summary>
        HWheel = 0x00001000,
        /// <summary>
        /// <para>The WM_MOUSEMOVE messages will not be coalesced.
        /// The default behavior is to coalesce WM_MOUSEMOVE messages.</para>
        /// <para>Windows XP/2000: This value is not supported.</para>
        /// </summary>
        MoveNoCoalesce = 0x00002000,
        /// <summary>
        /// Maps coordinates to the entire desktop. Must be used with <see cref="Absolute"/>.
        /// </summary>
        VirtualDesk = 0x00004000,
        /// <summary>
        /// The <see cref="MouseInput.X"/> and <see cref="MouseInput.Y"/> members contain normalized absolute coordinates.
        /// If the flag is not set, <see cref="MouseInput.X"/> and <see cref="MouseInput.Y"/> contain relative data (the change in position since the last reported position).
        /// This flag can be set, or not set, regardless of what kind of mouse or other pointing device, if any, is connected to the system.
        /// For further information about relative mouse motion, see the Remarks section.
        /// </summary>
        Absolute = 0x00008000
    }

    /// <summary>
    /// Flag values of <see cref="KeyboardInput.Flags"/>.
    /// </summary>
    /// <remarks>
    /// <para><seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-keybdinput"/></para>
    /// <para><see cref="KeyboardInput"/> supports nonkeyboard-input methods—such as handwriting recognition or voice recognition
    /// —as if it were text input by using the <see cref="Unicode"/> flag.
    /// If <see cref="Unicode"/> is specified, <see cref="NativeMethods.SendInput(int, ref Input, int)"/> or <see cref="NativeMethods.SendInput(int, Input[], int)"/>
    /// sends a WM_KEYDOWN or WM_KEYUP message to the foreground thread's message queue with wParam equal to VK_PACKET.
    /// Once GetMessage or PeekMessage obtains this message, passing the message to TranslateMessage posts a WM_CHAR message with the Unicode character originally specified by wScan.
    /// This Unicode character will automatically be converted to the appropriate ANSI value if it is posted to an ANSI window.</para>
    /// <para>Set the <see cref="ScanCode"/> flag to define keyboard input in terms of the scan code.
    /// This is useful to simulate a physical keystroke regardless of which keyboard is currently being used.
    /// The virtual key value of a key may alter depending on the current keyboard layout or what other keys were pressed, but the scan code will always be the same.</para>
    /// </remarks>
    [Flags]
    internal enum KeyEventF : int
    {
        /// <summary>
        /// If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).
        /// </summary>
        ExtendedKey = 0x00000001,
        /// <summary>
        /// If specified, the key is being released. If not specified, the key is being pressed.
        /// </summary>
        KeyUp = 0x00000002,
        /// <summary>
        /// If specified, wScan identifies the key and <see cref="KeyboardInput.VirtualKey"/> is ignored.
        /// </summary>
        Unicode = 0x00000004,
        /// <summary>
        /// If specified, the system synthesizes a VK_PACKET keystroke.
        /// The wVk parameter must be zero.
        /// This flag can only be combined with the <see cref="KeyUp"/> flag.
        /// For more information, see the Remarks section.
        /// </summary>
        ScanCode = 0x00000008
    }

    /// <summary>
    /// Used by <see cref="NativeMethods.SendInput(int, ref Input, int)"/> or <see cref="NativeMethods.SendInput(int, Input[], int)"/> to store information for synthesizing input events such as keystrokes, mouse movement, and mouse clicks.
    /// </summary>
    /// <remarks><seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-input"/></remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Input
    {
        /// <summary>
        /// The type of the input event.
        /// </summary>
        public InputType Type;
        /// <summary>
        /// The information about a simulated mouse, keyboard or hardware event.
        /// </summary>
        public InputUnion Ui;
    }

    /// <summary>
    /// Union structure of <see cref="MouseInput"/>, <see cref="KeyboardInput"/> or <see cref="HardwareInput"/>.
    /// </summary>
    /// <remarks>
    /// <para><seealso cref="Input"/></para>
    /// <para><see cref="KeyboardInput"/> supports nonkeyboard input methods, such as handwriting recognition or voice recognition, as if it were text input by using the <see cref="KeyEventF.Unicode"/> flag.
    /// For more information, see the remarks section of <see cref="KeyboardInput"/>.</para>
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    internal struct InputUnion
    {
        /// <summary>
        /// The information about a simulated mouse event.
        /// </summary>
        [FieldOffset(0)]
        public MouseInput Mouse;
        /// <summary>
        /// The information about a simulated keyboard event.
        /// </summary>
        [FieldOffset(0)]
        public KeyboardInput Keyboard;
        /// <summary>
        /// The information about a simulated hardware event.
        /// </summary>
        [FieldOffset(0)]
        public HardwareInput Hardware;
    }

    /// <summary>
    /// Contains information about a simulated mouse event.
    /// </summary>
    /// <remarks><seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mouseinput"/></remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MouseInput
    {
        /// <summary>
        /// <para>The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the <see cref="Flags"/> member.</para>
        /// <para>Absolute data is specified as the x coordinate of the mouse; relative data is specified as the number of pixels moved.</para>
        /// </summary>
        public int X;
        /// <summary>
        /// <para>The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the <see cref="Flags"/> member.</para>
        /// <para>Absolute data is specified as the y coordinate of the mouse; relative data is specified as the number of pixels moved.</para>
        /// </summary>
        public int Y;
        /// <summary>
        /// <para>If <see cref="Flags"/> contains <see cref="MouseEventF.Wheel"/>, then mouseData specifies the amount of wheel movement.
        /// A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user.
        /// One wheel click is defined as WHEEL_DELTA, which is 120.</para>
        /// <para>Windows Vista: If <see cref="Flags"/> contains <see cref="MouseEventF.HWheel"/>, then dwData specifies the amount of wheel movement.
        /// A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left.
        /// One wheel click is defined as WHEEL_DELTA, which is 120.</para>
        /// <para>If <see cref="Flags"/> does not contain <see cref="MouseEventF.Wheel"/>, <see cref="MouseEventF.XDown"/>, or <see cref="MouseEventF.XUp"/>,
        /// then <see cref="Data"/> should be zero.</para>
        /// <para>If <see cref="Flags"/> contains MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP, then mouseData specifies which X buttons were pressed or released.
        /// This value may be any combination of the following flags.</para>
        /// </summary>
        public int Data;
        /// <summary>
        /// <para>A set of bit flags that specify various aspects of mouse motion and button clicks.
        /// The bits in this member can be any reasonable combination of the <see cref="MouseEventF"/> values.</para>
        /// <para>The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions.
        /// For example, if the left mouse button is pressed and held down, <see cref="MouseEventF.LeftDown"/> is set when the left button is first pressed, but not for subsequent motions.
        /// Similarly <see cref="MouseEventF.LeftUp"/> is set only when the button is first released.</para>
        /// <para>You cannot specify both the <see cref="MouseEventF.Wheel"/> flag and either <see cref="MouseEventF.XDown"/> or <see cref="MouseEventF.XUp"/> flags simultaneously
        /// in the <see cref="Flags"/> parameter, because they both require use of the mouseData field.</para>
        /// </summary>
        public MouseEventF Flags;
        /// <summary>
        /// The time stamp for the event, in milliseconds.
        /// If this parameter is 0, the system will provide its own time stamp.
        /// </summary>
        public int Time;
        /// <summary>
        /// An additional value associated with the mouse event.
        /// An application calls GetMessageExtraInfo to obtain this extra information.
        /// </summary>
        public IntPtr ExtraInfo;
    }

    /// <summary>
    /// Contains information about a simulated keyboard event.
    /// </summary>
    /// <remarks><seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-keybdinput"/></remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct KeyboardInput
    {
        /// <summary>
        /// A virtual-key code.
        /// The code must be a value in the range 1 to 254.
        /// If the <see cref="Flags"/> member specifies <see cref="KeyEventF.Unicode"/>, <see cref="VirtualKey"/> must be 0.
        /// </summary>
        public short VirtualKey;
        /// <summary>
        /// A hardware scan code for the key.
        /// If <see cref="Flags"/> specifies <see cref="KeyEventF.Unicode"/>,
        /// <see cref="ScanCode"/> specifies a Unicode character which is to be sent to the foreground application.
        /// </summary>
        public short ScanCode;
        /// <summary>
        /// Specifies various aspects of a keystroke.
        /// This member can be certain combinations of the <see cref="KeyEventF"/> values.
        /// </summary>
        public KeyEventF Flags;
        /// <summary>
        /// The time stamp for the event, in milliseconds.
        /// If this parameter is zero, the system will provide its own time stamp.
        /// </summary>
        public int Time;
        /// <summary>
        /// An additional value associated with the keystroke.
        /// Use the GetMessageExtraInfo function to obtain this information.
        /// </summary>
        public IntPtr ExtraInfo;
    }

    /// <summary>
    /// Contains information about a simulated message generated by an input device other than a keyboard or mouse.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct HardwareInput
    {
        /// <summary>
        /// The message generated by the input hardware.
        /// </summary>
        public int Message;
        /// <summary>
        /// The low-order word of the lParam parameter for <see cref="Message"/>.
        /// </summary>
        public short ParamL;
        /// <summary>
        /// The high-order word of the lParam parameter for <see cref="Message"/>.
        /// </summary>
        public short ParamH;
    }

    /// <summary>
    /// Provides native methods.
    /// </summary>
    internal class NativeMethods
    {
        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        /// <param name="nInputs">The number of structures in the <paramref name="input"/>. Muse be 1.</param>
        /// <param name="input">An reference of <see cref="Input"/> structures.
        /// This structure represents an event to be inserted into the keyboard or mouse input stream.</param>
        /// <param name="cbSize">The size, in bytes, of an <see cref="Input"/> structure.
        /// If cbSize is not the size of an <see cref="Input"/> structure, the function fails.</param>
        /// <returns>
        /// <para>The function returns the number of events that it successfully inserted into the keyboard or mouse input stream.
        /// If the function returns zero, the input was already blocked by another thread.
        /// To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</para>
        /// <para>This function fails when it is blocked by UIPI.
        /// Note that neither <see cref="Marshal.GetLastWin32Error"/> nor the return value will indicate the failure was caused by UIPI blocking.</para>
        /// </returns>
        /// <remarks><seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput"/></remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public extern static int SendInput(int nInputs, ref Input input, int cbSize);

        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        /// <param name="nInputs">The number of structures in the <paramref name="input"/>. Muse be 1.</param>
        /// <param name="inputs">An array of <see cref="Input"/> structures.
        /// Each structure represents an event to be inserted into the keyboard or mouse input stream.</param>
        /// <param name="cbSize">The size, in bytes, of an <see cref="Input"/> structure.
        /// If cbSize is not the size of an <see cref="Input"/> structure, the function fails.</param>
        /// <returns>
        /// <para>The function returns the number of events that it successfully inserted into the keyboard or mouse input stream.
        /// If the function returns zero, the input was already blocked by another thread.
        /// To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</para>
        /// <para>This function fails when it is blocked by UIPI.
        /// Note that neither <see cref="Marshal.GetLastWin32Error"/> nor the return value will indicate the failure was caused by UIPI blocking.</para>
        /// </returns>
        /// <remarks><seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput"/></remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public extern static int SendInput(int nInputs, Input[] inputs, int cbSize);
    }
}
