using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Eve
{
    public partial class MainDialog : Form
    {
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_RESTORE = 0xF120;

        private string _server = "https://new.sbt-dup-004.sigma.sbrf.ru/_api/";

        private EveWebRequest _webRequest = new EveWebRequest();
        private Timer _timerPeople = new Timer();
        private List<RosterDto> _roster = new List<RosterDto>();
        private ChatMessage _focusedMessage = null;
        private List<ChatMessage> _messagesAnswerToLoad = new List<ChatMessage>();
        private List<ChatMessage> _messagesHistoryToLoad = new List<ChatMessage>();

        private string _context = "";
        private bool _isRequiredRoster = false;
        private bool _noMoreRoster = false;
        private bool _waitingRoster = false;
        private bool _waitingHistory = false;
        private bool _exiting = false;
        private int _chatWidth = -1;

        private string _peopleMode = "";
        private string PeopleMode { get { return _peopleMode; } set
            {
                if (_peopleMode != value)
                {
                    _peopleMode = value;

                    pPeople.Visible = value != "";
                    picSend.Visible = value == "";
                    tPeople.Visible = value != "";
                    tInput.Visible = value == "";
                    picPeople.Visible = value == "";
                    picClear.Visible = value != "";
                    if (value == "")
                    {
                        tInput.Focus();
                    }
                    else
                    {
                        tPeople.Text = tInput.Text;
                        tPeople.Focus();
                        FindRoster(0);
                        _timerPeople.Stop();
                        if (value == "E")
                        {
                            tPeople.WaterMarkText = "Выберите сотрудника";
                            lRosterCaption.Text = "Выберите сотрудника";
                        }
                    }
                }
            } 
        }

        public MainDialog()
        {
            InitializeComponent();

#if DEV
                _server = "http://localhost/_api/";
#endif
#if TEST
                _server = "https://new.sbt-dup-004.sigma.sbrf.ru/_api/";
#endif
#if PROD
                _server = "https://new.sbt-life.sberbank.ru/_api/";
#endif
#if PRE
                _server = "https://new.sbt-osop-226.sigma.sbrf.ru/_api/";
#endif
#if PRE
                _server = "https://new.sbt-osop-234.sigma.sbrf.ru/_api/";
#endif

            _focusedMessage = chatHello;

            _webRequest.AnswerReceived += OnAnswerReceived;
            _webRequest.HiddenAnswerReceived += OnHiddenAnswerReceived;
            _webRequest.RosterReceived += OnRosterReceived;
            _webRequest.StringReceived += OnStringReceived;
            _webRequest.HistoryReceived += OnHistoryReceived;

            _webRequest.PostQuestion($"{_server}sbot/askByButton",
                new RequestDto() { variables = new RequestDataDto() { Title = "", Category = "hello", Id = "hello" } }, false);

            _timerPeople.Interval = 1000;
            _timerPeople.Tick += OnTimerPeople;

            var screen = Screen.FromControl(this).Bounds;
            Left = 11 * screen.Width / 16;
            Width = 4 * screen.Width / 16;
            Top = 3 * screen.Height / 8;
            Height = 4 * screen.Height / 8;
        }

        private void FindRoster(int skip)
        {
            _waitingRoster = true;
            if (skip == 0) _noMoreRoster = false;
            _webRequest.PostRosterData($"{_server}roster/find", 
                new FindRequestDto() { Query = tPeople.Text, Skip = skip, Take = 30, Source = _peopleMode });
        }

        private void SendQuestion()
        {
            if (!string.IsNullOrEmpty(tInput.Text))
            {
                var msg = new ChatMessage()
                {
                    SendTime = DateTime.Now,
                    IsEve = false,
                    Message = tInput.Text,
                    Width = pChatList.ClientSize.Width
                };
                AddMessageToBottom(msg);

                var question = tInput.Text;
                tInput.Text = "";
                tInput.Focus();

                _webRequest.PostQuestion($"{_server}sbot/ask", 
                    new RequestDto() { variables = new RequestDataDto() { Title = question, Id = _context } }, true);

            }
        }

        private void AddMessageToBottom(ChatMessage msg)
        {
            pChatList.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pChatList.RowCount++;
            pChatList.Controls.Add(msg, 0, pChatList.RowCount - 1);
            _focusedMessage = msg;
            pChatListTop.ScrollControlIntoView(msg);
        }

        private void AddMessagesToBottom(List<ChatMessage> messages)
        {
            var cnt = messages.Count;
            pChatListTop.SuspendLayout();
            pChatList.SuspendLayout();
            for (var i = 0; i < cnt; i++)
                pChatList.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var oldRowCnt = pChatList.RowCount;
            pChatList.RowCount += cnt;
            for (var i = 0; i < cnt; i++)
            {
                var msg = messages[i];
                pChatList.Controls.Add(msg, 0, oldRowCnt + i);
                _focusedMessage = msg;
            }
            pChatList.ResumeLayout();
            pChatListTop.ResumeLayout();
            pChatListTop.ScrollControlIntoView(_focusedMessage);
        }


        private void AddMessagesToTop(List<ChatMessage> messages)
        {
            var cnt = messages.Count;
            pChatListTop.SuspendLayout();
            pChatList.SuspendLayout();
            for (var i = 0; i < cnt; i++)
                pChatList.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pChatList.RowCount += cnt;
            foreach (Control c in pChatList.Controls)
            {
                pChatList.SetRow(c, pChatList.GetRow(c) + cnt);
            }
            for (var i = 0; i < cnt; i++)
            {
                var msg = messages[i];
                pChatList.Controls.Add(msg, 0, cnt - i - 1);
            }
            pChatList.ResumeLayout();
            pChatListTop.ResumeLayout();
        }

        private void CheckLoadedMessagesAnswer()
        {
            if (_messagesAnswerToLoad.All(x => x.IsLoaded))
            {
                AddMessagesToBottom(_messagesAnswerToLoad);
                _messagesAnswerToLoad.Clear();
            }
        }

        private void CheckLoadedMessagesHistory()
        {
            if (_messagesHistoryToLoad.All(x => x.IsLoaded))
            {
                AddMessagesToTop(_messagesHistoryToLoad);
                _messagesHistoryToLoad.Clear();
                pChatListTop.ScrollControlIntoView(_focusedMessage);
            }
        }

        void OnAnswerReceived(object sender, AnswerDtoReceivedEventArgs e)
        {
            ShowAnswer(e.Answer);
        }

        void OnHiddenAnswerReceived(object sender, AnswerDtoReceivedEventArgs e)
        {
            chatHello.SendTime = DateTime.Now;
            chatHello.IsEve = true;
            chatHello.Message = e.Answer.Title;
        }

        void OnRosterReceived(object sender, RosterDtoReceivedEventArgs e)
        {
            _waitingRoster = false;
            if (e.Request.Skip == 0)
            {
                lbPeople.Items.Clear();
                lbPeople.Items.Add("Не выбрано");
                _roster.Clear();
            }
            foreach (var r in e.Roster)
            {
                lbPeople.Items.Add(r.Name);
            }
            _roster.AddRange(e.Roster);

            if (e.Roster.Count < 30) _noMoreRoster = true;
        }

        void OnStringReceived(object sender, StringReceivedEventArgs e)
        {
            chatHello.SendTime = DateTime.Now;
            chatHello.IsEve = true;
            chatHello.Message = $"Привет, {e.StringAnswer}!\r\nМеня зовут Ева!\r\nНапиши свой вопрос о Компании или рабочих процессах, и я постараюсь ответить на него.";
        }

        void OnHistoryReceived(object sender, HistoryReceivedEventArgs e)
        {
            _waitingHistory = false;
            foreach (var hist in e.HistoryAnswer)
            {
                var msg = CreateAnswer(hist);
                msg.SendTime = hist.QuestionDate;

                if (Util.HtmlToString(hist.AnswerText, false).Contains("<"))
                {
                    msg.DocumentLoaded += OnHtmlHistoryLoaded;
                }
                _messagesHistoryToLoad.Add(msg);

                var question = hist.OriginalQuestion;
                if (!string.IsNullOrEmpty(hist.Question) && (
                    hist.Question[0] == '[' && hist.Question[hist.Question.Length - 1] == ']' ||
                    (!string.IsNullOrEmpty(question) && question[0] == '(' && question[question.Length - 1] == ')')))
                    question = hist.Question;
                msg = new ChatMessage()
                {
                    SendTime = hist.QuestionDate,
                    IsEve = false,
                    Message = question,
                    Width = pChatList.ClientSize.Width
                };
                _messagesHistoryToLoad.Add(msg);
            }
            CheckLoadedMessagesHistory();
        }

        void OnHtmlLoaded(object sender, EventArgs e)
        {
            var msg = (ChatMessage)sender;
            msg.ButtonPressed += OnButtonPressed;
            msg.XlnkPressed += OnXlnkPressed;
            CheckLoadedMessagesAnswer();
        }

        void OnHtmlHistoryLoaded(object sender, EventArgs e)
        {
            var msg = (ChatMessage)sender;
            msg.ButtonPressed += OnButtonPressed;
            msg.XlnkPressed += OnXlnkPressed;
            CheckLoadedMessagesHistory();
        }

        private void OnButtonPressed(object sender, BrowserButtonClickEventArgs e)
        {
            AskByButton(e.Button.InnerHtml, e.Button.GetAttribute("category"));
        }

        private void OnXlnkPressed(object sender, BrowserXlnkClickEventArgs e)
        {
            AskByButton(e.Text, e.Category);
        }

        void OnSetLikePressed(object sender, SetLikeEventArgs e)
        {
            _webRequest.PostData($"{_server}sbot/setLike",
                new Pair<int>() { Title = e.Like.ToString(), Id = e.AnswerId } 
            );
            if (e.Like < 0) {
                _webRequest.PostQuestion($"{_server}sbot/askByButton",
                    new RequestDto() { variables = new RequestDataDto() { Title = "", Category = "dislike", Id = "dislike" } }, true);
            }
        }


        private void AskByButton(string question, string category)
        {
            var msg = new ChatMessage()
            {
                SendTime = DateTime.Now,
                IsEve = false,
                Message = question,
                Width = pChatList.ClientSize.Width
            };
            AddMessageToBottom(msg);

            tInput.Text = "";
            tInput.Focus();

            _webRequest.PostQuestion($"{_server}sbot/askByButton",
                new RequestDto() { variables = new RequestDataDto() { Title = question, Category = category, Id = _context } }, true);

        }

        private void OnTimerPeople(Object source, EventArgs e)
        {
            _timerPeople.Stop();
            if (PeopleMode != "")
            {
                FindRoster(0);
            }
        }

        private void ShowAnswer(AnswerDto res)
        {
            _context = res.Context;
            _isRequiredRoster = false;
            var answer = res.Title;

            var rosterRegex = new Regex("<xrst type=['\"](.*?)['\"]>(.*?)</xrst>");
            var match = rosterRegex.Match(answer);
            if (match.Success)
            {
                answer = answer.Replace(match.Value, "");
                var source = match.Groups[1].Value;
                var question = match.Groups[2].Value;

                PeopleMode = source;
                lRosterCaption.Text = answer;
                tPeople.WaterMarkText = answer;
                _isRequiredRoster = true;
                return;
            }

            var preRegex = new Regex("<xpre(.*?)>(.*?)</xpre>");
            match = preRegex.Match(res.Title);
            if (match.Success)
            {
                tInput.Text = match.Groups[2].Value;
                tInput.SelectionStart = tInput.Text.Length;
                tInput.SelectionLength = 0;
                tInput.Focus();
                res.Title = res.Title.Replace(match.Value, "");
            }

            var msg = CreateAnswer(res);
            _messagesAnswerToLoad.Add(msg);
            if (Util.HtmlToString(msg.Message, false).Contains("<"))
            {
                msg.DocumentLoaded += OnHtmlLoaded;
            }
            CheckLoadedMessagesAnswer();
        }

        private ChatMessage CreateAnswer(HistoryDto hist)
        {
            var text = hist.AnswerText;
            var rosterRegex = new Regex("<xrst(.*?)>(.*?)</xrst>");
            text = rosterRegex.Replace(text, "");
            var preRegex = new Regex("<xpre(.*?)>(.*?)</xpre>");
            text = preRegex.Replace(text, "");

            var res = CreateAnswer(new AnswerDto()
            {
                Id = hist.Id,
                Title = text,
                Rate = hist.Rate ?? 1,
                IsLikeable = string.IsNullOrEmpty(hist.AnswerType) && hist.Answer != "dislike"
            });
            res.Like = hist.Like;
            return res;
        }


        private ChatMessage CreateAnswer(AnswerDto res)
        {
            var answer = Util.MakeResponse(res.Title);

            var msg = new ChatMessage()
            {
                Dock = DockStyle.Top,
                SendTime = DateTime.Now,
                IsEve = true,
                Width = pChatList.ClientSize.Width,
                IsLikeable = res.IsLikeable
            };
            if (msg.IsLikeable)
            {
                msg.SetLikePressed += OnSetLikePressed;
            }

            if (Util.HtmlToString(answer, false).Contains("<"))
            {
                var xlnkRegex = new Regex("<xlnk (.*?)>(.*?)</xlnk>");
                var matches = xlnkRegex.Matches(answer);
                foreach (Match m in matches)
                {
                    var cat = "";
                    var catRegex = new Regex("category=['\"](.*?)['\"]");
                    var mx = catRegex.Match(m.Groups[1].Value);
                    if (mx.Success) cat = mx.Groups[1].Value;

                    var con = _context;
                    var conRegex = new Regex("context=['\"](.*?)['\"]");
                    mx = conRegex.Match(m.Groups[1].Value);
                    if (mx.Success) con = mx.Groups[1].Value;

                    answer = answer.Replace(m.Value, $"<a href='#' type='xlnk' category='{cat}' context='{con}'>{m.Groups[2]}</a>");
                }

                msg.MessageHtml = answer;
            }
            else
            {
                msg.Message = answer;
            }
            msg.Rate = res.Rate;
            msg.Answer = res;

            return msg;
        }

        private void tInput_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) {
                case Keys.Enter:
                    SendQuestion();
                    break;
                case Keys.D2:
                    if (e.Modifiers == Keys.Shift && tInput.Text == "")
                    {
                        PeopleMode = "E";
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void tPeople_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    PeopleMode = "";
                    tInput.Text = "";
                    break;
            }
        }

        private void pChatList_ClientSizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && _chatWidth != pChatList.ClientSize.Width)
            {
                pChatListTop.SuspendLayout();
                pChatList.SuspendLayout();
                foreach (Control ctrl in pChatList.Controls)
                {
                    if (ctrl is ChatMessage) ctrl.Width = pChatList.ClientSize.Width;
                }
                pChatList.ResumeLayout();
                pChatListTop.ResumeLayout();

                _chatWidth = pChatList.ClientSize.Width;
            }
        }

        private void picSend_Click(object sender, EventArgs e)
        {
            SendQuestion();
        }

        private void picPeople_Click(object sender, EventArgs e)
        {
            PeopleMode = "E";
        }

        private void lbPeople_Click(object sender, EventArgs e)
        {
            if (lbPeople.SelectedIndex > 0)
            {
                var r = _roster[lbPeople.SelectedIndex - 1];
                tInput.Text = $"[{r.Source}:{r.Code}|{r.Name}]";
                tInput.SelectionStart = tInput.Text.Length;
                tInput.SelectionLength = 0;
                if (_isRequiredRoster)
                {
                    AskByButton(tInput.Text, _context + tInput.Text);
                }
            }
            PeopleMode = "";
        }

        private void lbPeople_Scroll(object Sender, ScrollableListBox.ScrollableListBoxScrollArgs e)
        {
            if (_noMoreRoster || _waitingRoster) return;

            var lastVisible = lbPeople.TopIndex + (lbPeople.ClientRectangle.Height / lbPeople.ItemHeight);
            if (lastVisible >= lbPeople.Items.Count)
            {
                FindRoster(lbPeople.Items.Count - 1);
            }
        }

        private void tPeople_TextChanged(object sender, EventArgs e)
        {
            _timerPeople.Stop();
            _timerPeople.Start();
        }

        private void MainDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_exiting)
            {
                e.Cancel = true;
                trayIcon.Visible = true;
                this.Hide();

                pChatListTop.SuspendLayout();
                pChatList.SuspendLayout();
                foreach (var control in pChatList.Controls.Cast<ChatMessage>().ToList())
                {
                    pChatList.Controls.Remove(control);
                    if (control != chatHello)
                        control.Dispose();
                }
                pChatList.Controls.Clear();
                pChatList.RowStyles.Clear();
                pChatList.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                pChatList.RowCount = 1;
                pChatList.Controls.Add(chatHello, 0, 0);
                _focusedMessage = chatHello;
                pChatList.ResumeLayout();
                pChatListTop.ResumeLayout();
                GC.Collect();
            }
        }

        private void openDialogMenuItem_Click(object sender, EventArgs e)
        {
            ShowNewDialog();
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowNewDialog();
        }

        private void ShowNewDialog()
        {
            trayIcon.Visible = false;
            pChatListTop.ScrollControlIntoView(chatHello);
            this.Show();
            this.Activate();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            _exiting = true;
            Application.Exit();
        }

        private void pChatListTop_Scrolled(object Sender, ScrollablePanel.ScrollablePanelScrollArgs e)
        {
            if (e.Mouse && e.Up && !_waitingHistory)
            {
                var controls = pChatList.Controls.Cast<Control>().OrderBy(x => x.Top).ToList();
                var top = controls.Where(cntrl => cntrl.Top + pChatList.Top >= 0).FirstOrDefault();
                if (top != null)
                {
                    _focusedMessage = (ChatMessage)top;
                    var topIndex = controls.IndexOf(top);
                    var topId = 100000000;
                    for (var i = topIndex; i >= 0 && i < controls.Count; i++)
                    {
                        var cntrl = (ChatMessage)controls[i];
                        if (cntrl.Answer != null)
                        {
                            topId = cntrl.Answer.Id;
                            break;
                        }
                    }
                    if (topIndex == 0)
                    {
                        _waitingHistory = true;
                        _webRequest.GetHistory($"{_server}sbot/history", topId);
                    }
                }
            }
        }

        private void picClear_Click(object sender, EventArgs e)
        {
            PeopleMode = "";
            tInput.Text = "";
        }


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_RESTORE)
            {
                // костыль, чтобы не изменялся pChatList.ClientSize
                this.Height = this.Height + 1;
                this.Height = this.Height - 1;
            }
            if (m.Msg == Util.WM_USER_RESTORE)
            {
                ShowNewDialog();
            }
        }

    }
}
