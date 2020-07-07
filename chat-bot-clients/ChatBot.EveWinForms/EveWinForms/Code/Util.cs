using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eve
{
    public static class Util
    {
        public const int cornerRadius = 10;
        public const int shadowWidth = 5;
        public const int shadowColor = 0xe2;
        public const int WM_USER = 0x0400;
        public const int WM_USER_RESTORE = WM_USER + 1;

        public const string InputGreetings = "Введите сообщение...";

        public static GraphicsPath GetRoundRect(float X, float Y, float width, float height, float radius)
        {
            var gp = new GraphicsPath();
            gp.AddLine(X + radius, Y, X + width - (radius * 2), Y);
            gp.AddArc(X + width - (radius * 2), Y, radius * 2, radius * 2, 270, 90);
            gp.AddLine(X + width, Y + radius, X + width, Y + height - (radius * 2));
            gp.AddArc(X + width - (radius * 2), Y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
            gp.AddLine(X + width - (radius * 2), Y + height, X + radius, Y + height);
            gp.AddArc(X, Y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
            gp.AddLine(X, Y + height - (radius * 2), X, Y + radius);
            gp.AddArc(X, Y, radius * 2, radius * 2, 180, 90);
            gp.CloseFigure();
            return gp;
        }

        public static GraphicsPath GetCornerRect(float X, float Y, float width, float height, float radius)
        {

            var gp = new GraphicsPath();
            gp.AddArc(X + width - (radius * 2), Y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
            gp.AddLine(X + width - (radius * 2), Y + height + 1, X + width + 1, Y + height + 1);
            gp.AddLine(X + width + 1, Y + height + 1, X + width + 1, Y + height - (radius * 2));
            gp.CloseFigure();
            return gp;
        }

        public static void DropShadow(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            var colTo = 0xf1;
            var colStep = (colTo - shadowColor) / (shadowWidth - 1);

            Pen pen = new Pen(Color.White);
            using (pen)
            {
                foreach (Panel p in panel.Controls.OfType<Panel>())
                {
                    Point pt = p.Location;
                    pt.Y += p.Height;
                    for (var sp = 0; sp < shadowWidth; sp++)
                    {
                        var col = shadowColor + colStep * sp;
                        pen.Color = Color.FromArgb(col, col, col);
                        //e.Graphics.DrawLine(pen, pt.X, pt.Y, pt.X + p.Width - 1, pt.Y);
                        e.Graphics.DrawLine(pen, pt.X + sp + cornerRadius/2, pt.Y, pt.X + p.Width - 1 - sp, pt.Y); 
                        e.Graphics.DrawLine(pen, p.Right + sp, p.Top + sp + cornerRadius / 2, p.Right + sp, p.Bottom - sp);
                        pt.Y++;
                    }
                }
            }
        }

        public static string MakeResponse(string text) {

            var answer = text; //!!! WebUtility.HtmlDecode(text);

            answer = answer
                .Replace("<xbtn", "<button").Replace("</xbtn", "</button")
                .Replace("<org>", "<b>").Replace("</org>", "</b>")
                .Replace("<xorg ", "<span ").Replace("</xorg>", "</span>");

            answer = answer.Trim();
            return answer;
        }

        public static string HtmlToString(string text, bool removeTags)
        {

            var answer = WebUtility.HtmlDecode(text);

            answer = answer
                .Replace("\n", " ")
                .Replace("<br/>", "\n").Replace("<br>", "\n");
            var answLst = answer.Split('\n');
            for (var i = 0; i < answLst.Length; i++)
                answLst[i] = answLst[i].Trim();
            answer = string.Join("\r\n", answLst);

            if (removeTags)
            {
                var tagsReg = new Regex("<(.*?)>");
                answer = tagsReg.Replace(answer, "");
            }

            return answer;
        }
    }
}
