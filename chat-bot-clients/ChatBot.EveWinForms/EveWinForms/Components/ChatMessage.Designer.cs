namespace Eve
{
    partial class ChatMessage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._webBrowser != null)
                {
                    if (this._webBrowser.Container != null)
                    {
                        this._webBrowser.Container.Remove(this);
                    }
                    this._webBrowser.Dispose();
                }
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatMessage));
            this.panel1 = new System.Windows.Forms.Panel();
            this.picEveNo = new System.Windows.Forms.PictureBox();
            this.picEveYes = new System.Windows.Forms.PictureBox();
            this.pTimeSend = new System.Windows.Forms.Panel();
            this.lEveSendTime = new System.Windows.Forms.Label();
            this.lSendTime = new System.Windows.Forms.Label();
            this.pMessage = new System.Windows.Forms.Panel();
            this.pMessageInner = new System.Windows.Forms.Panel();
            this.pLikes = new System.Windows.Forms.Panel();
            this.picDislikeOn = new System.Windows.Forms.PictureBox();
            this.picDislikeOff = new System.Windows.Forms.PictureBox();
            this.picLikeOn = new System.Windows.Forms.PictureBox();
            this.picLikeOff = new System.Windows.Forms.PictureBox();
            this.lText = new Eve.TextBoxEx();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEveNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEveYes)).BeginInit();
            this.pTimeSend.SuspendLayout();
            this.pMessage.SuspendLayout();
            this.pMessageInner.SuspendLayout();
            this.pLikes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDislikeOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDislikeOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLikeOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLikeOff)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.picEveNo);
            this.panel1.Controls.Add(this.picEveYes);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(60, 150);
            this.panel1.TabIndex = 0;
            // 
            // picEveNo
            // 
            this.picEveNo.Image = ((System.Drawing.Image)(resources.GetObject("picEveNo.Image")));
            this.picEveNo.Location = new System.Drawing.Point(10, 5);
            this.picEveNo.Name = "picEveNo";
            this.picEveNo.Size = new System.Drawing.Size(40, 40);
            this.picEveNo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picEveNo.TabIndex = 1;
            this.picEveNo.TabStop = false;
            this.picEveNo.Visible = false;
            // 
            // picEveYes
            // 
            this.picEveYes.Image = ((System.Drawing.Image)(resources.GetObject("picEveYes.Image")));
            this.picEveYes.Location = new System.Drawing.Point(10, 5);
            this.picEveYes.Name = "picEveYes";
            this.picEveYes.Size = new System.Drawing.Size(40, 40);
            this.picEveYes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picEveYes.TabIndex = 0;
            this.picEveYes.TabStop = false;
            this.picEveYes.Visible = false;
            // 
            // pTimeSend
            // 
            this.pTimeSend.Controls.Add(this.lEveSendTime);
            this.pTimeSend.Controls.Add(this.lSendTime);
            this.pTimeSend.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTimeSend.Location = new System.Drawing.Point(60, 0);
            this.pTimeSend.Name = "pTimeSend";
            this.pTimeSend.Size = new System.Drawing.Size(488, 20);
            this.pTimeSend.TabIndex = 1;
            // 
            // lEveSendTime
            // 
            this.lEveSendTime.AutoSize = true;
            this.lEveSendTime.ForeColor = System.Drawing.Color.Gray;
            this.lEveSendTime.Location = new System.Drawing.Point(3, 3);
            this.lEveSendTime.Name = "lEveSendTime";
            this.lEveSendTime.Size = new System.Drawing.Size(24, 17);
            this.lEveSendTime.TabIndex = 1;
            this.lEveSendTime.Text = "    ";
            // 
            // lSendTime
            // 
            this.lSendTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lSendTime.ForeColor = System.Drawing.Color.Gray;
            this.lSendTime.Location = new System.Drawing.Point(226, 3);
            this.lSendTime.Name = "lSendTime";
            this.lSendTime.Size = new System.Drawing.Size(259, 16);
            this.lSendTime.TabIndex = 0;
            this.lSendTime.Text = "    ";
            this.lSendTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pMessage
            // 
            this.pMessage.Controls.Add(this.pMessageInner);
            this.pMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMessage.Location = new System.Drawing.Point(60, 20);
            this.pMessage.Name = "pMessage";
            this.pMessage.Size = new System.Drawing.Size(488, 130);
            this.pMessage.TabIndex = 2;
            this.pMessage.Paint += new System.Windows.Forms.PaintEventHandler(this.pMessage_Paint);
            // 
            // pMessageInner
            // 
            this.pMessageInner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pMessageInner.Controls.Add(this.pLikes);
            this.pMessageInner.Controls.Add(this.lText);
            this.pMessageInner.Location = new System.Drawing.Point(3, 3);
            this.pMessageInner.Name = "pMessageInner";
            this.pMessageInner.Padding = new System.Windows.Forms.Padding(10);
            this.pMessageInner.Size = new System.Drawing.Size(482, 124);
            this.pMessageInner.TabIndex = 1;
            this.pMessageInner.Paint += new System.Windows.Forms.PaintEventHandler(this.pMessageInner_Paint);
            // 
            // pLikes
            // 
            this.pLikes.BackColor = System.Drawing.Color.White;
            this.pLikes.Controls.Add(this.picDislikeOn);
            this.pLikes.Controls.Add(this.picDislikeOff);
            this.pLikes.Controls.Add(this.picLikeOn);
            this.pLikes.Controls.Add(this.picLikeOff);
            this.pLikes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pLikes.Location = new System.Drawing.Point(10, 98);
            this.pLikes.Margin = new System.Windows.Forms.Padding(0);
            this.pLikes.Name = "pLikes";
            this.pLikes.Size = new System.Drawing.Size(462, 16);
            this.pLikes.TabIndex = 1;
            this.pLikes.Visible = false;
            // 
            // picDislikeOn
            // 
            this.picDislikeOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picDislikeOn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picDislikeOn.Image = ((System.Drawing.Image)(resources.GetObject("picDislikeOn.Image")));
            this.picDislikeOn.Location = new System.Drawing.Point(443, 0);
            this.picDislikeOn.Name = "picDislikeOn";
            this.picDislikeOn.Size = new System.Drawing.Size(16, 16);
            this.picDislikeOn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDislikeOn.TabIndex = 3;
            this.picDislikeOn.TabStop = false;
            this.picDislikeOn.Visible = false;
            this.picDislikeOn.Click += new System.EventHandler(this.picLikeOn_Click);
            // 
            // picDislikeOff
            // 
            this.picDislikeOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picDislikeOff.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picDislikeOff.Image = ((System.Drawing.Image)(resources.GetObject("picDislikeOff.Image")));
            this.picDislikeOff.Location = new System.Drawing.Point(443, 0);
            this.picDislikeOff.Name = "picDislikeOff";
            this.picDislikeOff.Size = new System.Drawing.Size(16, 16);
            this.picDislikeOff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDislikeOff.TabIndex = 2;
            this.picDislikeOff.TabStop = false;
            this.picDislikeOff.Click += new System.EventHandler(this.picLikeOn_Click);
            // 
            // picLikeOn
            // 
            this.picLikeOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picLikeOn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLikeOn.Image = ((System.Drawing.Image)(resources.GetObject("picLikeOn.Image")));
            this.picLikeOn.Location = new System.Drawing.Point(421, 0);
            this.picLikeOn.Name = "picLikeOn";
            this.picLikeOn.Size = new System.Drawing.Size(16, 16);
            this.picLikeOn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLikeOn.TabIndex = 1;
            this.picLikeOn.TabStop = false;
            this.picLikeOn.Visible = false;
            this.picLikeOn.Click += new System.EventHandler(this.picLikeOn_Click);
            // 
            // picLikeOff
            // 
            this.picLikeOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picLikeOff.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLikeOff.Image = ((System.Drawing.Image)(resources.GetObject("picLikeOff.Image")));
            this.picLikeOff.Location = new System.Drawing.Point(421, 0);
            this.picLikeOff.Name = "picLikeOff";
            this.picLikeOff.Size = new System.Drawing.Size(16, 16);
            this.picLikeOff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLikeOff.TabIndex = 0;
            this.picLikeOff.TabStop = false;
            this.picLikeOff.Click += new System.EventHandler(this.picLikeOn_Click);
            // 
            // lText
            // 
            this.lText.BackColor = System.Drawing.Color.White;
            this.lText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lText.Location = new System.Drawing.Point(10, 10);
            this.lText.Multiline = true;
            this.lText.Name = "lText";
            this.lText.ReadOnly = true;
            this.lText.Size = new System.Drawing.Size(95, 63);
            this.lText.TabIndex = 0;
            this.lText.Text = "...";
            this.lText.TextChanged += new System.EventHandler(this.lText_TextChanged);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(548, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 150);
            this.panel3.TabIndex = 1;
            // 
            // ChatMessage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pMessage);
            this.Controls.Add(this.pTimeSend);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "ChatMessage";
            this.Size = new System.Drawing.Size(558, 150);
            this.Resize += new System.EventHandler(this.ChatMessage_Resize);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picEveNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEveYes)).EndInit();
            this.pTimeSend.ResumeLayout(false);
            this.pTimeSend.PerformLayout();
            this.pMessage.ResumeLayout(false);
            this.pMessageInner.ResumeLayout(false);
            this.pMessageInner.PerformLayout();
            this.pLikes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDislikeOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDislikeOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLikeOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLikeOff)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pTimeSend;
        private System.Windows.Forms.Panel pMessage;
        private System.Windows.Forms.Panel pMessageInner;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lSendTime;
        private System.Windows.Forms.Label lEveSendTime;
        private System.Windows.Forms.PictureBox picEveYes;
        private System.Windows.Forms.PictureBox picEveNo;
        private System.Windows.Forms.Panel pLikes;
        private System.Windows.Forms.PictureBox picLikeOff;
        private System.Windows.Forms.PictureBox picDislikeOn;
        private System.Windows.Forms.PictureBox picDislikeOff;
        private System.Windows.Forms.PictureBox picLikeOn;
        private TextBoxEx lText;
    }
}
