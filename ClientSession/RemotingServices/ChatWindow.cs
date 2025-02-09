// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.ChatWindow
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class ChatWindow : Form
  {
    public static int SND_ASYNC = 1;
    public static int SND_FILENAME = 131072;
    public static int SND_PURGE = 64;
    private GradientMenuStrip gradientMenuStrip1;
    private ToolStripMenuItem conversationToolStripMenuItem;
    private ToolStripMenuItem sendToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem changeFontToolStripMenuItem;
    private ToolStripMenuItem changeColorToolStripMenuItem;
    private ToolStripMenuItem playSoundToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem closeToolStripMenuItem;
    private static bool playSound = true;
    private static string imSoundFilePath = (string) null;
    private static int _maxMsgLength = 512;
    private static Hashtable chatWindows = new Hashtable();
    public static readonly Color DefaultTextColor = Color.Black;
    public static readonly Font DefaultTextFont = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);
    private Color textColor = ChatWindow.DefaultTextColor;
    private Font textFont = ChatWindow.DefaultTextFont;
    private string sessionID;
    private string userID;
    private RichTextBox rtBoxChat;
    private RichTextBox rtBoxSend;
    private Button btnSend;
    private Button btnProfile;

    [DllImport("user32.dll")]
    private static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr GetTopWindow(IntPtr hwnd);

    [DllImport("kernel32.dll")]
    private static extern bool Beep(int freq, int duration);

    [DllImport("WinMM.dll")]
    public static extern bool PlaySound(string fname, int Mod, int flag);

    static ChatWindow()
    {
      ChatWindow.playSound = (Session.GetPrivateProfileString("IM", "PlaySound") ?? "").Trim().ToLower() != "false";
      if (AssemblyResolver.IsSmartClient)
        ChatWindow.imSoundFilePath = AssemblyResolver.GetResourceFileFullPath("documents\\im-notifier.wav");
      else
        ChatWindow.imSoundFilePath = Path.Combine(SystemSettings.DocDirAbsPath, "im-notifier.wav");
    }

    public ChatWindow(string sessionID, string userID)
    {
      this.InitializeComponent();
      this.playSoundToolStripMenuItem.Checked = ChatWindow.playSound;
      this.rtBoxSend.Font = this.textFont;
      this.rtBoxSend.ForeColor = this.textColor;
      this.sessionID = sessionID;
      this.userID = userID;
      this.Text = "To: " + this.userID;
      this.rtBoxSend.Focus();
      this.rtBoxSend.Select();
    }

    protected override void Dispose(bool disposing)
    {
      lock (ChatWindow.chatWindows)
      {
        if (ChatWindow.chatWindows != null)
        {
          if (ChatWindow.chatWindows.Contains((object) this.sessionID))
            ChatWindow.chatWindows.Remove((object) this.sessionID);
        }
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ChatWindow));
      this.rtBoxChat = new RichTextBox();
      this.rtBoxSend = new RichTextBox();
      this.btnSend = new Button();
      this.btnProfile = new Button();
      this.gradientMenuStrip1 = new GradientMenuStrip();
      this.conversationToolStripMenuItem = new ToolStripMenuItem();
      this.sendToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.changeFontToolStripMenuItem = new ToolStripMenuItem();
      this.changeColorToolStripMenuItem = new ToolStripMenuItem();
      this.playSoundToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.closeToolStripMenuItem = new ToolStripMenuItem();
      this.gradientMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      this.rtBoxChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.rtBoxChat.BackColor = SystemColors.Window;
      this.rtBoxChat.BorderStyle = BorderStyle.FixedSingle;
      this.rtBoxChat.ForeColor = SystemColors.WindowText;
      this.rtBoxChat.HideSelection = false;
      this.rtBoxChat.Location = new Point(4, 27);
      this.rtBoxChat.Name = "rtBoxChat";
      this.rtBoxChat.ReadOnly = true;
      this.rtBoxChat.Size = new Size(472, 183);
      this.rtBoxChat.TabIndex = 0;
      this.rtBoxChat.Text = "";
      this.rtBoxChat.Layout += new LayoutEventHandler(this.rtBoxChat_Layout);
      this.rtBoxSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.rtBoxSend.BackColor = SystemColors.Window;
      this.rtBoxSend.BorderStyle = BorderStyle.FixedSingle;
      this.rtBoxSend.Location = new Point(4, 216);
      this.rtBoxSend.Name = "rtBoxSend";
      this.rtBoxSend.Size = new Size(472, 68);
      this.rtBoxSend.TabIndex = 1;
      this.rtBoxSend.Text = "";
      this.rtBoxSend.KeyPress += new KeyPressEventHandler(this.rtBoxSend_KeyPress);
      this.rtBoxSend.TextChanged += new EventHandler(this.rtBoxSend_TextChanged);
      this.btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSend.Location = new Point(482, 218);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(66, 66);
      this.btnSend.TabIndex = 2;
      this.btnSend.Text = "Send";
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.btnProfile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnProfile.Image = (Image) componentResourceManager.GetObject("btnProfile.Image");
      this.btnProfile.ImageAlign = ContentAlignment.TopCenter;
      this.btnProfile.Location = new Point(482, 27);
      this.btnProfile.Name = "btnProfile";
      this.btnProfile.Size = new Size(66, 66);
      this.btnProfile.TabIndex = 4;
      this.btnProfile.Text = "Contact Profile";
      this.btnProfile.TextAlign = ContentAlignment.BottomCenter;
      this.btnProfile.Click += new EventHandler(this.btnProfile_Click);
      this.gradientMenuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.conversationToolStripMenuItem
      });
      this.gradientMenuStrip1.Location = new Point(0, 0);
      this.gradientMenuStrip1.Name = "gradientMenuStrip1";
      this.gradientMenuStrip1.Size = new Size(554, 24);
      this.gradientMenuStrip1.TabIndex = 5;
      this.gradientMenuStrip1.Text = "gradientMenuStrip1";
      this.conversationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.sendToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.changeFontToolStripMenuItem,
        (ToolStripItem) this.changeColorToolStripMenuItem,
        (ToolStripItem) this.playSoundToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.closeToolStripMenuItem
      });
      this.conversationToolStripMenuItem.Name = "conversationToolStripMenuItem";
      this.conversationToolStripMenuItem.Size = new Size(83, 20);
      this.conversationToolStripMenuItem.Text = "Conversation";
      this.sendToolStripMenuItem.Name = "sendToolStripMenuItem";
      this.sendToolStripMenuItem.Size = new Size(152, 22);
      this.sendToolStripMenuItem.Text = "Send";
      this.sendToolStripMenuItem.Click += new EventHandler(this.sendToolStripMenuItem_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(149, 6);
      this.changeFontToolStripMenuItem.Name = "changeFontToolStripMenuItem";
      this.changeFontToolStripMenuItem.Size = new Size(152, 22);
      this.changeFontToolStripMenuItem.Text = "Change Font";
      this.changeFontToolStripMenuItem.Click += new EventHandler(this.changeFontToolStripMenuItem_Click);
      this.changeColorToolStripMenuItem.Name = "changeColorToolStripMenuItem";
      this.changeColorToolStripMenuItem.Size = new Size(152, 22);
      this.changeColorToolStripMenuItem.Text = "Change Color";
      this.changeColorToolStripMenuItem.Click += new EventHandler(this.menuItemChgColor_Click);
      this.playSoundToolStripMenuItem.Name = "playSoundToolStripMenuItem";
      this.playSoundToolStripMenuItem.Size = new Size(152, 22);
      this.playSoundToolStripMenuItem.Text = "Play Sound";
      this.playSoundToolStripMenuItem.Click += new EventHandler(this.menuItemPlaySound_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(149, 6);
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new Size(152, 22);
      this.closeToolStripMenuItem.Text = "Close";
      this.closeToolStripMenuItem.Click += new EventHandler(this.closeToolStripMenuItem_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(554, 289);
      this.Controls.Add((Control) this.btnProfile);
      this.Controls.Add((Control) this.btnSend);
      this.Controls.Add((Control) this.rtBoxSend);
      this.Controls.Add((Control) this.rtBoxChat);
      this.Controls.Add((Control) this.gradientMenuStrip1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MainMenuStrip = (MenuStrip) this.gradientMenuStrip1;
      this.Name = nameof (ChatWindow);
      this.Text = "Chat Window";
      this.gradientMenuStrip1.ResumeLayout(false);
      this.gradientMenuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public static void AddChatWindow(string sessionID, ChatWindow chatWindow)
    {
      lock (ChatWindow.chatWindows)
        ChatWindow.chatWindows[(object) sessionID] = (object) chatWindow;
    }

    public static ChatWindow GetChatWindow(string sessionID)
    {
      return (ChatWindow) ChatWindow.chatWindows[(object) sessionID];
    }

    public void AppendChatText(string text, Font font, Color color)
    {
      this.AppendChatText(text, font, color, true);
    }

    public void AppendChatText(string text, Font font, Color color, bool notify)
    {
      IntPtr foregroundWindow = ChatWindow.GetForegroundWindow();
      if (text != null)
      {
        this.rtBoxChat.Select(this.rtBoxChat.Text.Length, 0);
        this.rtBoxChat.SelectionFont = font;
        this.rtBoxChat.SelectionColor = color;
        this.rtBoxChat.AppendText(text);
        if (notify && ChatWindow.playSound)
        {
          if (this.Handle != foregroundWindow)
          {
            try
            {
              ChatWindow.PlaySound(ChatWindow.imSoundFilePath, 0, ChatWindow.SND_FILENAME | ChatWindow.SND_ASYNC);
            }
            catch
            {
              ChatWindow.Beep(500, 250);
            }
            ChatWindow.FlashWindow(this.Handle, true);
          }
        }
      }
      this.rtBoxChat.Focus();
      this.rtBoxChat.SelectionStart = this.rtBoxChat.Text.Length;
      this.rtBoxChat.SelectionLength = 0;
      this.rtBoxChat.Select();
      this.rtBoxChat.ScrollToCaret();
      this.rtBoxSend.Focus();
      this.rtBoxSend.Select();
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      IMChatMessage imChatMessage = new IMChatMessage(this.rtBoxSend.Text.TrimEnd((char[]) null), this.textFont, this.textColor, this.sessionID);
      this.Cursor = Cursors.WaitCursor;
      try
      {
        Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) imChatMessage, this.sessionID, true);
        Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) imChatMessage, Session.ISession.SessionID, false);
      }
      catch (IMChatMessageException ex)
      {
        string text = "";
        if (ex.Cause == IMChatMessageExceptionCause.ConnectionTimeout || ex.Cause == IMChatMessageExceptionCause.NetworkDisconnected)
          text = Environment.NewLine + ex.ReceiverUserID + " is probably offline and may not receive your messages." + Environment.NewLine;
        else if (ex.Cause == IMChatMessageExceptionCause.DeliveryFailed || ex.Cause == IMChatMessageExceptionCause.Unknown)
          text = Environment.NewLine + "For some unknown reason, messages cannot be successfully delivered to " + ex.ReceiverUserID + Environment.NewLine;
        else if (ex.Cause == IMChatMessageExceptionCause.NullSession)
          text = Environment.NewLine + this.userID + " appears to be offline and will not receive your messages." + Environment.NewLine;
        this.AppendChatText(text, new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Bold | FontStyle.Underline), Color.Red);
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
      this.rtBoxSend.Text = "";
      this.rtBoxSend.Focus();
      this.rtBoxSend.Select();
    }

    private void rtBoxSend_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\r')
      {
        try
        {
          this.rtBoxSend.Text = this.rtBoxSend.Text.TrimEnd((char[]) null);
          this.btnSend_Click((object) this, (EventArgs) e);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message + "\\" + ex.StackTrace);
        }
      }
      else
      {
        if (this.rtBoxSend.Text.Length < ChatWindow._maxMsgLength)
          return;
        e.Handled = true;
      }
    }

    private void rtBoxSend_TextChanged(object sender, EventArgs e)
    {
      if (this.rtBoxSend.Text.Length <= ChatWindow._maxMsgLength)
        return;
      this.rtBoxSend.Text = this.rtBoxSend.Text.Substring(0, ChatWindow._maxMsgLength);
    }

    private void btnProfile_Click(object sender, EventArgs e)
    {
      UserInfo user = Session.OrganizationManager.GetUser(this.userID);
      if (user == (UserInfo) null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "There is no contact profile for contact " + this.userID + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        int num2 = (int) new ContactMessengerProfile(user).ShowDialog((IWin32Window) this);
      }
    }

    private void closeChatWindow() => this.Close();

    private void setFontType()
    {
      FontDialog fontDialog = new FontDialog();
      fontDialog.Font = this.textFont;
      if (fontDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      this.textFont = fontDialog.Font;
      this.rtBoxSend.Font = fontDialog.Font;
    }

    private void setColor()
    {
      ColorDialog colorDialog = new ColorDialog();
      colorDialog.Color = this.textColor;
      if (colorDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      this.textColor = colorDialog.Color;
      this.rtBoxSend.ForeColor = colorDialog.Color;
    }

    private void menuItemChgColor_Click(object sender, EventArgs e) => this.setColor();

    private void rtBoxChat_Layout(object sender, LayoutEventArgs e)
    {
      this.rtBoxChat.Focus();
      this.rtBoxChat.SelectionStart = 0;
      this.rtBoxChat.SelectionLength = 0;
      this.rtBoxChat.Select();
      this.rtBoxChat.ScrollToCaret();
      this.rtBoxChat.SelectionStart = this.rtBoxChat.Text.Length;
      this.rtBoxChat.SelectionLength = 0;
      this.rtBoxChat.Select();
      this.rtBoxChat.ScrollToCaret();
      this.rtBoxSend.Focus();
      this.rtBoxSend.Select();
    }

    private void menuItemPlaySound_Click(object sender, EventArgs e)
    {
      ChatWindow.playSound = !ChatWindow.playSound;
      Session.WritePrivateProfileString("IM", "PlaySound", ChatWindow.playSound ? "True" : "False");
      this.playSoundToolStripMenuItem.Checked = ChatWindow.playSound;
    }

    private void sendToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.btnSend_Click(sender, e);
    }

    private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.setFontType();
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e) => this.closeChatWindow();
  }
}
