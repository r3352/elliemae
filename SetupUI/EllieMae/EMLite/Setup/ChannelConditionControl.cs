// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ChannelConditionControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ChannelConditionControl : UserControl
  {
    private IContainer components;
    private CheckedListBox chkListChannels;

    public event EventHandler ChangesMadeToChannel;

    public ChannelConditionControl()
      : this((string) null)
    {
    }

    public ChannelConditionControl(string options)
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      for (int index = 0; index < this.chkListChannels.Items.Count; ++index)
        this.chkListChannels.SetItemChecked(index, true);
      if (options == null)
        return;
      this.ChannelValue = options;
    }

    public string ChannelValue
    {
      set
      {
        for (int index = 0; index < this.chkListChannels.Items.Count; ++index)
          this.chkListChannels.SetItemChecked(index, false);
        string[] strArray = value.Split(',');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (Utils.ParseInt((object) strArray[index]) >= 0)
            this.chkListChannels.SetItemChecked(Utils.ParseInt((object) strArray[index]), true);
        }
      }
      get
      {
        string channelValue = string.Empty;
        for (int index = 0; index < this.chkListChannels.Items.Count; ++index)
        {
          if (this.chkListChannels.GetItemChecked(index))
          {
            if (channelValue != string.Empty)
              channelValue += ",";
            channelValue += (string) (object) index;
          }
        }
        if (channelValue == string.Empty)
          channelValue = "0";
        return channelValue;
      }
    }

    public void DisableControls() => this.chkListChannels.Enabled = false;

    private void chkListChannels_SelectedValueChanged(object sender, EventArgs e)
    {
      if (this.ChangesMadeToChannel == null)
        return;
      this.ChangesMadeToChannel((object) this, EventArgs.Empty);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkListChannels = new CheckedListBox();
      this.SuspendLayout();
      this.chkListChannels.CheckOnClick = true;
      this.chkListChannels.Dock = DockStyle.Fill;
      this.chkListChannels.FormattingEnabled = true;
      this.chkListChannels.Items.AddRange(new object[5]
      {
        (object) "No channel selected",
        (object) "Banked – Retail",
        (object) "Banked – Wholesale",
        (object) "Brokered",
        (object) "Correspondent"
      });
      this.chkListChannels.Location = new Point(0, 0);
      this.chkListChannels.Name = "chkListChannels";
      this.chkListChannels.Size = new Size(677, 79);
      this.chkListChannels.TabIndex = 3;
      this.chkListChannels.SelectedValueChanged += new EventHandler(this.chkListChannels_SelectedValueChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.chkListChannels);
      this.Name = nameof (ChannelConditionControl);
      this.Size = new Size(677, 85);
      this.ResumeLayout(false);
    }
  }
}
