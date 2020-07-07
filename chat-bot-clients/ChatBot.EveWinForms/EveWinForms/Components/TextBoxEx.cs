using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Eve
{
    public class TextBoxEx : TextBox
    {
        protected override void WndProc(ref Message m)
        {
            // Send WM_MOUSEWHEEL messages to the parent
            if (m.Msg == 0x20a) SendMessage(this.Parent.Handle, m.Msg, m.WParam, m.LParam);
            else base.WndProc(ref m);
        }
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
