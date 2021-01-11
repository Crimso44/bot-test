namespace Eve
{
    partial class MainDialog
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.picClear = new System.Windows.Forms.PictureBox();
            this.picPeople = new System.Windows.Forms.PictureBox();
            this.picSend = new System.Windows.Forms.PictureBox();
            this.pPeople = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lRosterCaption = new System.Windows.Forms.Label();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openDialogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbPeople = new Eve.ScrollableListBox();
            this.pChatListTop = new Eve.ScrollablePanel();
            this.pChatList = new System.Windows.Forms.TableLayoutPanel();
            this.chatHello = new Eve.ChatMessage();
            this.tPeople = new Eve.WaterMarkTextBox();
            this.tInput = new Eve.WaterMarkTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSend)).BeginInit();
            this.pPeople.SuspendLayout();
            this.panel2.SuspendLayout();
            this.trayMenu.SuspendLayout();
            this.pChatListTop.SuspendLayout();
            this.pChatList.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.picClear);
            this.panel1.Controls.Add(this.tPeople);
            this.panel1.Controls.Add(this.picPeople);
            this.panel1.Controls.Add(this.picSend);
            this.panel1.Controls.Add(this.tInput);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 418);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 32);
            this.panel1.TabIndex = 0;
            // 
            // picClear
            // 
            this.picClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.picClear.Image = ((System.Drawing.Image)(resources.GetObject("picClear.Image")));
            this.picClear.Location = new System.Drawing.Point(732, 0);
            this.picClear.Name = "picClear";
            this.picClear.Size = new System.Drawing.Size(32, 32);
            this.picClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picClear.TabIndex = 4;
            this.picClear.TabStop = false;
            this.picClear.Visible = false;
            this.picClear.Click += new System.EventHandler(this.picClear_Click);
            // 
            // picPeople
            // 
            this.picPeople.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPeople.Dock = System.Windows.Forms.DockStyle.Left;
            this.picPeople.Image = ((System.Drawing.Image)(resources.GetObject("picPeople.Image")));
            this.picPeople.Location = new System.Drawing.Point(0, 0);
            this.picPeople.Name = "picPeople";
            this.picPeople.Size = new System.Drawing.Size(32, 32);
            this.picPeople.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picPeople.TabIndex = 2;
            this.picPeople.TabStop = false;
            this.picPeople.Click += new System.EventHandler(this.picPeople_Click);
            // 
            // picSend
            // 
            this.picSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSend.Dock = System.Windows.Forms.DockStyle.Right;
            this.picSend.Image = ((System.Drawing.Image)(resources.GetObject("picSend.Image")));
            this.picSend.Location = new System.Drawing.Point(764, 0);
            this.picSend.Name = "picSend";
            this.picSend.Size = new System.Drawing.Size(36, 32);
            this.picSend.TabIndex = 1;
            this.picSend.TabStop = false;
            this.picSend.Click += new System.EventHandler(this.picSend_Click);
            // 
            // pPeople
            // 
            this.pPeople.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pPeople.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPeople.Controls.Add(this.lbPeople);
            this.pPeople.Controls.Add(this.panel2);
            this.pPeople.ForeColor = System.Drawing.Color.Silver;
            this.pPeople.Location = new System.Drawing.Point(10, 260);
            this.pPeople.Name = "pPeople";
            this.pPeople.Size = new System.Drawing.Size(778, 165);
            this.pPeople.TabIndex = 1;
            this.pPeople.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
            this.panel2.Controls.Add(this.lRosterCaption);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(776, 19);
            this.panel2.TabIndex = 1;
            // 
            // lRosterCaption
            // 
            this.lRosterCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRosterCaption.ForeColor = System.Drawing.Color.DimGray;
            this.lRosterCaption.Location = new System.Drawing.Point(0, 0);
            this.lRosterCaption.Name = "lRosterCaption";
            this.lRosterCaption.Size = new System.Drawing.Size(776, 19);
            this.lRosterCaption.TabIndex = 0;
            this.lRosterCaption.Text = "Выберите сотрудника";
            this.lRosterCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Чат-бот Инна";
            this.trayIcon.DoubleClick += new System.EventHandler(this.trayIcon_DoubleClick);
            // 
            // trayMenu
            // 
            this.trayMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDialogMenuItem,
            this.exitMenuItem});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(225, 80);
            // 
            // openDialogMenuItem
            // 
            this.openDialogMenuItem.Name = "openDialogMenuItem";
            this.openDialogMenuItem.Size = new System.Drawing.Size(224, 24);
            this.openDialogMenuItem.Text = "Открыть чат с Инной";
            this.openDialogMenuItem.Click += new System.EventHandler(this.openDialogMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(211, 24);
            this.exitMenuItem.Text = "Выход";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // lbPeople
            // 
            this.lbPeople.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbPeople.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPeople.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbPeople.FormattingEnabled = true;
            this.lbPeople.IntegralHeight = false;
            this.lbPeople.ItemHeight = 20;
            this.lbPeople.Items.AddRange(new object[] {
            "Не выбрано"});
            this.lbPeople.Location = new System.Drawing.Point(0, 19);
            this.lbPeople.Name = "lbPeople";
            this.lbPeople.Size = new System.Drawing.Size(776, 144);
            this.lbPeople.TabIndex = 0;
            this.lbPeople.Scroll += new Eve.ScrollableListBox.ScrollableListBoxScrollDelegate(this.lbPeople_Scroll);
            this.lbPeople.Click += new System.EventHandler(this.lbPeople_Click);
            // 
            // pChatListTop
            // 
            this.pChatListTop.AutoScroll = true;
            this.pChatListTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pChatListTop.Controls.Add(this.pChatList);
            this.pChatListTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pChatListTop.Location = new System.Drawing.Point(0, 0);
            this.pChatListTop.MinimumSize = new System.Drawing.Size(10, 10);
            this.pChatListTop.Name = "pChatListTop";
            this.pChatListTop.Size = new System.Drawing.Size(800, 418);
            this.pChatListTop.TabIndex = 2;
            this.pChatListTop.Scrolled += new Eve.ScrollablePanel.ScrollablePanelScrollDelegate(this.pChatListTop_Scrolled);
            // 
            // pChatList
            // 
            this.pChatList.AutoSize = true;
            this.pChatList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pChatList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pChatList.Controls.Add(this.chatHello, 0, 0);
            this.pChatList.Dock = System.Windows.Forms.DockStyle.Top;
            this.pChatList.Location = new System.Drawing.Point(0, 0);
            this.pChatList.MinimumSize = new System.Drawing.Size(10, 10);
            this.pChatList.Name = "pChatList";
            this.pChatList.RowCount = 1;
            this.pChatList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pChatList.Size = new System.Drawing.Size(800, 71);
            this.pChatList.TabIndex = 2;
            this.pChatList.ClientSizeChanged += new System.EventHandler(this.pChatList_ClientSizeChanged);
            // 
            // chatHello
            // 
            this.chatHello.Answer = null;
            this.chatHello.IsLikeable = false;
            this.chatHello.IsLoaded = true;
            this.chatHello.Location = new System.Drawing.Point(3, 3);
            this.chatHello.Message = "...";
            this.chatHello.Name = "chatHello";
            this.chatHello.Size = new System.Drawing.Size(790, 65);
            this.chatHello.TabIndex = 0;
            // 
            // tPeople
            // 
            this.tPeople.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tPeople.BackColor = System.Drawing.Color.White;
            this.tPeople.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tPeople.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.tPeople.Location = new System.Drawing.Point(10, 8);
            this.tPeople.Name = "tPeople";
            this.tPeople.Size = new System.Drawing.Size(748, 19);
            this.tPeople.TabIndex = 3;
            this.tPeople.Visible = false;
            this.tPeople.WaterMarkColor = System.Drawing.Color.Gray;
            this.tPeople.WaterMarkText = "Выбор сотрудника...";
            this.tPeople.TextChanged += new System.EventHandler(this.tPeople_TextChanged);
            this.tPeople.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tPeople_KeyDown);
            // 
            // tInput
            // 
            this.tInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tInput.BackColor = System.Drawing.Color.White;
            this.tInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.tInput.Location = new System.Drawing.Point(38, 8);
            this.tInput.Name = "tInput";
            this.tInput.Size = new System.Drawing.Size(720, 19);
            this.tInput.TabIndex = 0;
            this.tInput.WaterMarkColor = System.Drawing.Color.Gray;
            this.tInput.WaterMarkText = "Введите сообщение...";
            this.tInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tInput_KeyDown);
            // 
            // MainDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pPeople);
            this.Controls.Add(this.pChatListTop);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainDialog";
            this.Text = "Чат с Инной";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDialog_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPeople)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSend)).EndInit();
            this.pPeople.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.trayMenu.ResumeLayout(false);
            this.pChatListTop.ResumeLayout(false);
            this.pChatListTop.PerformLayout();
            this.pChatList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private WaterMarkTextBox tInput;
        private System.Windows.Forms.PictureBox picSend;
        private System.Windows.Forms.PictureBox picPeople;
        private System.Windows.Forms.Panel pPeople;
        private ScrollableListBox lbPeople;
        private WaterMarkTextBox tPeople;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lRosterCaption;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem openDialogMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.TableLayoutPanel pChatList;
        private ChatMessage chatHello;
        private ScrollablePanel pChatListTop;
        private System.Windows.Forms.PictureBox picClear;
    }
}

