using System;
using System.Linq;
using System.Windows.Forms;

namespace Eve
{
    public class ScrollablePanel : Panel
    {
        // Event declaration
        public delegate void ScrollablePanelScrollDelegate(object Sender, ScrollablePanelScrollArgs e);
        public event ScrollablePanelScrollDelegate Scrolled;
        // WM_VSCROLL message constants
        private const int WM_VSCROLL = 0x0115;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int SB_THUMBTRACK = 5;
        private const int SB_ENDSCROLL = 8;

        protected override void WndProc(ref Message m)
        {
            // Trap the WM_VSCROLL message to generate the Scroll event
            base.WndProc(ref m);
            if (m.Msg == WM_VSCROLL || m.Msg == WM_MOUSEWHEEL)
            {
                int nfy = m.WParam.ToInt32() & 0xFFFF;
                if (Scrolled != null && (m.Msg == WM_MOUSEWHEEL || nfy == SB_THUMBTRACK || nfy == SB_ENDSCROLL))
                {
                    Scrolled(this, new ScrollablePanelScrollArgs(nfy == SB_THUMBTRACK, m.Msg == WM_MOUSEWHEEL, (int)m.WParam > 0));
                }
            }
        }

        public class ScrollablePanelScrollArgs
        {
            // Scroll event argument
            private bool mTracking;
            private bool mMouse;
            private bool mUp;
            public ScrollablePanelScrollArgs(bool tracking, bool mouse, bool up)
            {
                mTracking = tracking;
                mMouse = mouse;
                mUp = up;
            }
            public bool Tracking
            {
                get { return mTracking; }
            }
            public bool Mouse
            {
                get { return mMouse; }
            }
            public bool Up
            {
                get { return mUp; }
            }
        }
    }
}
