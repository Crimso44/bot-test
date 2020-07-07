using System;
using System.Drawing;
using System.Windows.Forms;

namespace Eve
{
    public class ScrollableListBox : ListBox
    {

        private int _index = -1;

        public ScrollableListBox()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DrawItem += OnDrawItem;
            this.MouseMove += OnMouseMove;
        }

        // Event declaration
        public delegate void ScrollableListBoxScrollDelegate(object Sender, ScrollableListBoxScrollArgs e);
        public event ScrollableListBoxScrollDelegate Scroll;
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
                if (Scroll != null && (m.Msg == WM_MOUSEWHEEL || nfy == SB_THUMBTRACK || nfy == SB_ENDSCROLL))
                {
                    Scroll(this, new ScrollableListBoxScrollArgs(this.TopIndex, nfy == SB_THUMBTRACK));
                }
            }
        }
        public class ScrollableListBoxScrollArgs
        {
            // Scroll event argument
            private int mTop;
            private bool mTracking;
            public ScrollableListBoxScrollArgs(int top, bool tracking)
            {
                mTop = top;
                mTracking = tracking;
            }
            public int Top
            {
                get { return mTop; }
            }
            public bool Tracking
            {
                get { return mTracking; }
            }
        }

        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            var index = e.Index;
            var point = this.PointToClient(Cursor.Position);
            int mouseIndex = this.IndexFromPoint(point);
            e.DrawBackground();
            if (index == mouseIndex)
            {
                Graphics g = e.Graphics;
                g.FillRectangle(new SolidBrush(Color.LightGray), e.Bounds);
            }
            if (e.State == DrawItemState.Focus)
                e.DrawFocusRectangle();
            if (index < 0 || index >= this.Items.Count) return;
            var item = this.Items[index];
            string text = (item == null) ? "(null)" : item.ToString();
            using (var brush = new SolidBrush(index == 0 ? Color.DimGray : e.ForeColor))
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                var bnds = new Rectangle(10, e.Bounds.Y + 2, e.Bounds.Width - 20, e.Bounds.Height - 4);
                e.Graphics.DrawString(text, e.Font, brush, bnds);
            }
        }


        private void OnMouseMove(object sender, EventArgs e)
        {
            var point = this.PointToClient(Cursor.Position);
            var index = this.IndexFromPoint(point);
            if (index < 0 || index >= Items.Count) return;
            if (_index != index)
            {
                this.Invalidate(this.GetItemRectangle(index));
                if (_index >= 0 && _index < Items.Count) this.Invalidate(this.GetItemRectangle(_index));
            }
            _index = index;
        }

    }
}
