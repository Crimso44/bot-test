using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Eve
{
    public class BrowserButtonClickEventArgs : EventArgs
    {
        public HtmlElement Button { get; set; }
    }

    public class SetLikeEventArgs : EventArgs
    {
        public int AnswerId { get; set; }
        public short Like { get; set; }
    }

    public class BrowserXlnkClickEventArgs : EventArgs
    {
        public string Text { get; set; }
        public string Category { get; set; }
        public string Context { get; set; }
    }

    public class AnswerDtoReceivedEventArgs : EventArgs
    {
        public AnswerDto Answer { get; set; }
    }

    public class RosterDtoReceivedEventArgs : EventArgs
    {
        public List<RosterDto> Roster { get; set; }
        public FindRequestDto Request { get; set; }
    }

    public class StringReceivedEventArgs : EventArgs
    {
        public string StringAnswer { get; set; }
    }

    public class HistoryReceivedEventArgs : EventArgs
    {
        public List<HistoryDto> HistoryAnswer { get; set; }
    }

    [DataContract]
    public class Pair<T>
    {
        [DataMember]
        public T Id;

        [DataMember]
        public string Title;
    }
}
