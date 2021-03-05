using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Eve
{
    public partial class ChatMessage : UserControl
    {
        private bool _isHtml = false;
        private string _text = "";
        private WebBrowser _webBrowser = null;

        public ChatMessage()
        {
            InitializeComponent();
            IsLoaded = false;
        }

        public AnswerDto Answer { get; set; }
        public bool IsLoaded { get; set; }

        public string Message { 
            get { return _text; } 
            set
            {
                _text = value;
                lText.Text = Util.HtmlToString(_text, true);
                IsLoaded = true;
                ChatMessage_Resize(this, new EventArgs());
            }
        }

        public string MessageHtml
        {
            set
            {
                _text = value;
                _isHtml = true;
                lText.Visible = false;
                if (_webBrowser == null)
                {
                    _webBrowser = new WebBrowser()
                    {
                        AllowNavigation = false,
                        ScrollBarsEnabled = false,
                        IsWebBrowserContextMenuEnabled = false,
                        Location = new Point(10, 10),
                        Width = this.Width - panel1.Width - panel3.Width - 10
                    };
                }
                _webBrowser.DocumentCompleted += lText_DocumentCompleted;
                _webBrowser.NewWindow += Browser_NewWindow;
                pMessageInner.Controls.Add(_webBrowser);

                var html = @"<!DOCTYPE html><html><head>
                        <meta charset='utf-8'>
                        <meta http-equiv='X-UA-Compatible' content='IE=Edge'/>
                    <style>" +
                        Css.Message + 
                    "</style></head><body>" + value + "</body></html>";
                _webBrowser.DocumentText = html;
                IsLoaded = false;
            }
        }

        public DateTime SendTime { set
            {
                lSendTime.Text = value.ToString("HH:mm");
                if (value.Date != DateTime.Today)
                {
                    lSendTime.Text = value.ToString("d MMM yyyy", CultureInfo.CreateSpecificCulture("ru-RU")) + " г., " + lSendTime.Text;
                }
                lEveSendTime.Text = "Инна " + lSendTime.Text;
            } 
        }

        private bool _isEve = true;
        public bool IsEve { set
            {
                _isEve = value;
                lSendTime.Visible = !value;
                lEveSendTime.Visible = value;
                picEveYes.Visible = value;
            } 
        }

        public decimal Rate
        {
            set
            {
                if (picEveYes.Visible && value == 0)
                {
                    picEveYes.Visible = false;
                    picEveNo.Visible = true;
                }
            }
        }

        public bool IsLikeable { 
            get { return pLikes.Visible; }
            set
            {
                pLikes.Visible = value;
            } 
        }

        public short? Like { set
            {
                if ((value ?? 0) > 0)
                {
                    picLikeOn.Visible = true;
                    picLikeOff.Visible = false;
                } else if ((value ?? 0) < 0)
                {
                    picDislikeOn.Visible = true;
                    picDislikeOff.Visible = false;
                }
            }
        }


        public event EventHandler<EventArgs> DocumentLoaded;
        public event EventHandler<BrowserButtonClickEventArgs> ButtonPressed;
        public event EventHandler<BrowserXlnkClickEventArgs> XlnkPressed;
        public event EventHandler<SetLikeEventArgs> SetLikePressed;

        protected void OnDocumentLoaded(EventArgs e)
        {
            ChatMessage_Resize(this, e);
            _webBrowser.Document.Body.MouseDown += new HtmlElementEventHandler(Browser_MouseDown);
            IsLoaded = true;
            DocumentLoaded?.Invoke(this, e);
        }

        protected void OnButtonPressed(BrowserButtonClickEventArgs e)
        {
            ButtonPressed?.Invoke(this, e);
        }

        protected void OnXlnkPressed(BrowserXlnkClickEventArgs e)
        {
            XlnkPressed?.Invoke(this, e);
        }


        private void Browser_NewWindow(Object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void Browser_MouseDown(Object sender, HtmlElementEventArgs e)
        {
            switch (e.MouseButtonsPressed)
            {
                case MouseButtons.Left:
                    HtmlElement element = _webBrowser.Document.GetElementFromPoint(e.ClientMousePosition);
                    if (element != null)
                    {
                        if ("button".Equals(element.GetAttribute("type"), StringComparison.OrdinalIgnoreCase) ||
                            "button".Equals(element.TagName, StringComparison.OrdinalIgnoreCase))
                        {
                            OnButtonPressed(new BrowserButtonClickEventArgs() { Button = element });
                        }
                        else if (element.TagName.Equals("a", StringComparison.OrdinalIgnoreCase))
                        {
                            if ("xlnk".Equals(element.GetAttribute("type"), StringComparison.OrdinalIgnoreCase))
                            {
                                OnXlnkPressed(new BrowserXlnkClickEventArgs()
                                {
                                    Text = element.InnerHtml,
                                    Category = element.GetAttribute("category"),
                                    Context = element.GetAttribute("context")
                                });
                            } else
                            {
                                var url = element.GetAttribute("href");
                                if (!string.IsNullOrEmpty(url) && (url.ToLower().StartsWith("http") || url.ToLower().StartsWith("mailto")))
                                {
                                    var sInfo = new ProcessStartInfo(url);
                                    Process.Start(sInfo);
                                }
                            }
                        }
                        else if (element.Parent != null && element.Parent.TagName.Equals("a", StringComparison.OrdinalIgnoreCase))
                        {
                            var url = element.Parent.GetAttribute("href");
                            if (!string.IsNullOrEmpty(url) && (url.ToLower().StartsWith("http") || url.ToLower().StartsWith("mailto")))
                            {
                                var sInfo = new ProcessStartInfo(url);
                                Process.Start(sInfo);
                            }
                        }
                    }
                    break;
            }
        }


        private void pMessage_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            Util.DropShadow(sender, e);
        }

        private void pMessageInner_Paint(object sender, PaintEventArgs e)
        {
            var p = (Panel)sender;
            var g = e.Graphics;
            var gp = Util.GetRoundRect(p.ClientRectangle.Left, p.ClientRectangle.Top, p.ClientRectangle.Width - 1, p.ClientRectangle.Height - 1, Util.cornerRadius);
            //g.DrawPath(Pens.Blue, gp);
            g.FillRegion(Brushes.White, new Region(gp));

            gp = Util.GetCornerRect(p.ClientRectangle.Left, p.ClientRectangle.Top, p.ClientRectangle.Width - 1, p.ClientRectangle.Height - 1, Util.cornerRadius);
            g.FillRegion(new SolidBrush(Color.FromArgb(Util.shadowColor, Util.shadowColor, Util.shadowColor)), new Region(gp));

            base.OnPaint(e);
        }


        private void ChatMessage_Resize(object sender, EventArgs e)
        {
            if (_isHtml)
            {
                Size size;
                if (_webBrowser.Document == null)
                {
                    size = new Size(pMessage.Width - 6, _webBrowser.Height);
                }
                else
                {
                    var height = _webBrowser.Document.Body?.ScrollRectangle.Height ?? _webBrowser.Height;
                    var width = pMessage.Width - 6;
                    size = new Size(width, height);
                }
                _webBrowser.Size = size;
                pMessageInner.Width = pMessage.Width - 6;
                this.Height = size.Height + 30 + pTimeSend.Height + (pLikes.Visible ? pLikes.Height : 0);
            }
            else
            {
                lText_TextChanged(sender, e);
            }
        }

        private void lText_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ChatMessage_Resize(sender, new EventArgs());
            OnDocumentLoaded(new EventArgs());
        }

        private void lText_TextChanged(object sender, EventArgs e)
        {
            int borders = lText.Height - lText.ClientSize.Height;
            int bordersw = lText.Width - lText.ClientSize.Width;

            var w = pMessage.Width - 20;
            lText.Width = w;
            Size sz = new Size(w, int.MaxValue);
            TextFormatFlags flags = TextFormatFlags.WordBreak;
            int padding = 3;
            sz = TextRenderer.MeasureText(lText.Text, lText.Font, sz, flags);
            int h = sz.Height + borders + padding;
            if (lText.Top + h > this.ClientSize.Height - 10)
            {
                h = this.ClientSize.Height - 10 - lText.Top;
            }
            lText.Height = h;
            this.Height = h + 25 + pTimeSend.Height + (pLikes.Visible ? pLikes.Height : 0);

            int ww = sz.Width + bordersw + padding + 20;
            if (!string.IsNullOrEmpty(lText.Text))
            {
                if (ww > this.Width - panel1.Width - panel3.Width - 10)
                {
                    ww = this.Width - panel1.Width - panel3.Width - 10;
                }
                pMessageInner.Left = 3;
                pMessageInner.Width = ww;
                if (!_isEve)
                {
                    pMessageInner.Left = pMessage.ClientSize.Width - ww - 5;
                }
            }
        }

        private void picLikeOn_Click(object sender, EventArgs e)
        {
            short like = 0;
            if (sender == picLikeOn)
            {
                picLikeOn.Visible = false;
                picLikeOff.Visible = true;
            }
            else if (sender == picLikeOff)
            {
                like = 1;
                picLikeOn.Visible = true;
                picLikeOff.Visible = false;
                picDislikeOn.Visible = false;
                picDislikeOff.Visible = true;
            }
            else if (sender == picDislikeOn)
            {
                picDislikeOn.Visible = false;
                picDislikeOff.Visible = true;
            }
            else if (sender == picDislikeOff)
            {
                like = -1;
                picLikeOn.Visible = false;
                picLikeOff.Visible = true;
                picDislikeOn.Visible = true;
                picDislikeOff.Visible = false;
            }
            SetLikePressed?.Invoke(this, new SetLikeEventArgs() { AnswerId = Answer.Id, Like = like });
        }
    }
}
