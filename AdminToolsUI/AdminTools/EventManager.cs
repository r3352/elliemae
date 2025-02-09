// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EventManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class EventManager : Form
  {
    private Panel panel1;
    private Panel panel2;
    private Button btnNone;
    private Button btnAll;
    private Label label1;
    private ListView lvwEvents;
    private RichTextBox txtEvents;
    private System.ComponentModel.Container components;

    public EventManager()
    {
      this.InitializeComponent();
      this.BackColor = EllieMae.EMLite.AdminTools.AdminTools.FormBackgroundColor;
      this.txtEvents.MaxLength = int.MaxValue;
      this.lvwEvents.Clear();
      for (int index = 0; index < ServerEventDescriptor.AllEvents.Length; ++index)
        this.lvwEvents.Items.Add(new ListViewItem(new string[2]
        {
          ServerEventDescriptor.AllEvents[index].Name,
          ServerEventDescriptor.AllEvents[index].Description
        }));
      Session.Connection.ServerEvent += new ServerEventHandler(this.onServerEvent);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        Session.Connection.Session.UnregisterForEvents(typeof (ServerMonitorEvent));
        Session.Connection.ServerEvent -= new ServerEventHandler(this.onServerEvent);
        if (this.components != null)
          this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EventManager));
      this.panel1 = new Panel();
      this.txtEvents = new RichTextBox();
      this.panel2 = new Panel();
      this.btnNone = new Button();
      this.btnAll = new Button();
      this.label1 = new Label();
      this.lvwEvents = new ListView();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.txtEvents);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(200, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(358, 344);
      this.panel1.TabIndex = 6;
      this.txtEvents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEvents.Location = new Point(0, 14);
      this.txtEvents.Name = "txtEvents";
      this.txtEvents.Size = new Size(346, 318);
      this.txtEvents.TabIndex = 0;
      this.txtEvents.Text = "";
      this.txtEvents.KeyPress += new KeyPressEventHandler(this.txtEvents_KeyPress);
      this.panel2.Controls.Add((Control) this.btnNone);
      this.panel2.Controls.Add((Control) this.btnAll);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.lvwEvents);
      this.panel2.Dock = DockStyle.Left;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(200, 344);
      this.panel2.TabIndex = 7;
      this.btnNone.Location = new Point(20, 268);
      this.btnNone.Name = "btnNone";
      this.btnNone.Size = new Size(162, 24);
      this.btnNone.TabIndex = 8;
      this.btnNone.Text = "&Deselect All";
      this.btnNone.Click += new EventHandler(this.btnNone_Click);
      this.btnAll.Location = new Point(20, 236);
      this.btnAll.Name = "btnAll";
      this.btnAll.Size = new Size(162, 24);
      this.btnAll.TabIndex = 7;
      this.btnAll.Text = "&Select All";
      this.btnAll.Click += new EventHandler(this.btnAll_Click);
      this.label1.Location = new Point(18, 184);
      this.label1.Name = "label1";
      this.label1.Size = new Size(166, 23);
      this.label1.TabIndex = 6;
      this.label1.Text = "Select the event(s) to monitor.";
      this.lvwEvents.CheckBoxes = true;
      this.lvwEvents.LabelWrap = false;
      this.lvwEvents.Location = new Point(18, 14);
      this.lvwEvents.Name = "lvwEvents";
      this.lvwEvents.Size = new Size(162, 166);
      this.lvwEvents.TabIndex = 5;
      this.lvwEvents.UseCompatibleStateImageBehavior = false;
      this.lvwEvents.View = View.List;
      this.lvwEvents.ItemCheck += new ItemCheckEventHandler(this.lvwEvents_ItemCheck);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(558, 344);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.panel2);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (EventManager);
      this.Text = "Event Manager";
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void lvwEvents_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (e.NewValue == CheckState.Checked)
        this.subscribeEvent(this.lvwEvents.Items[e.Index].Text);
      else
        this.unsubscribeEvent(this.lvwEvents.Items[e.Index].Text);
    }

    private void subscribeEvent(string name)
    {
      for (int index = 0; index < ServerEventDescriptor.AllEvents.Length; ++index)
      {
        if (ServerEventDescriptor.AllEvents[index].Name == name)
        {
          Session.Connection.Session.RegisterForEvents(ServerEventDescriptor.AllEvents[index].EventType);
          this.appendToLog("Subscribed to " + ServerEventDescriptor.AllEvents[index].Name + " events");
          break;
        }
      }
    }

    private void unsubscribeEvent(string name)
    {
      for (int index = 0; index < ServerEventDescriptor.AllEvents.Length; ++index)
      {
        if (ServerEventDescriptor.AllEvents[index].Name == name)
        {
          Session.Connection.Session.UnregisterForEvents(ServerEventDescriptor.AllEvents[index].EventType);
          this.appendToLog("Unsubscribed to " + ServerEventDescriptor.AllEvents[index].Name + " events");
          break;
        }
      }
    }

    private void onServerEvent(IConnection conn, ServerEvent e)
    {
      if (!(e is ServerMonitorEvent))
        return;
      this.appendToLog(e.ToString());
    }

    private void btnAll_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.lvwEvents.Items.Count; ++index)
      {
        if (!this.lvwEvents.Items[index].Checked)
          this.lvwEvents.Items[index].Checked = true;
      }
    }

    private void btnNone_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.lvwEvents.Items.Count; ++index)
      {
        if (this.lvwEvents.Items[index].Checked)
          this.lvwEvents.Items[index].Checked = false;
      }
    }

    private void appendToLog(string text)
    {
      this.txtEvents.AppendText("[" + (object) DateTime.Now + "] " + text + "\r\n");
    }

    private void txtEvents_KeyPress(object sender, KeyPressEventArgs e) => e.Handled = true;
  }
}
