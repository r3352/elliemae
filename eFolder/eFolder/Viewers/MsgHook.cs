// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.MsgHook
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class MsgHook : NativeWindow
  {
    private List<int> msgList = new List<int>();

    public MsgHook(Control control)
    {
      if (control.IsHandleCreated)
        this.AssignHandle(control.Handle);
      else
        control.HandleCreated += new EventHandler(this.control_HandleCreated);
      control.HandleDestroyed += new EventHandler(this.control_HandleDestroyed);
    }

    public void CaptureMsg(int msgID)
    {
      if (this.msgList.Contains(msgID))
        return;
      this.msgList.Add(msgID);
    }

    private void control_HandleCreated(object sender, EventArgs e)
    {
      if (!(sender is Control control))
        return;
      this.AssignHandle(control.Handle);
    }

    private void control_HandleDestroyed(object sender, EventArgs e) => this.ReleaseHandle();

    protected override void WndProc(ref Message m)
    {
      if (this.msgList.Contains(m.Msg))
      {
        if (this.OnMessage == null)
          return;
        this.OnMessage(ref m);
      }
      else
        base.WndProc(ref m);
    }

    public event CallBackHandler OnMessage;
  }
}
