// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.EPassMessageDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class EPassMessageDialog : Form
  {
    private IContainer components;
    private GridView gvMessages;
    private Button btnClose;

    public EPassMessageDialog(string loanGuid)
    {
      this.InitializeComponent();
      this.loadMessageList(Session.ConfigurationManager.GetEPassMessagesForLoan(loanGuid, Session.UserID));
    }

    private void loadMessageList(EPassMessageInfo[] messages)
    {
      foreach (EPassMessageInfo message in messages)
        this.gvMessages.Items.Add(this.createGVItemForMessage(message));
      this.Text = messages.Length.ToString() + " Messages";
    }

    private GVItem createGVItemForMessage(EPassMessageInfo msg)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Text = msg.Description
          },
          [1] = {
            Text = msg.Source
          },
          [2] = {
            Text = EPassUtils.EPassTimeToLocalTime(msg.Timestamp).ToString("M/d/yyyy h:mm tt")
          }
        },
        Tag = (object) msg
      };
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.gvMessages = new GridView();
      this.btnClose = new Button();
      this.SuspendLayout();
      this.gvMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Message";
      gvColumn1.Width = 270;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column3";
      gvColumn2.Text = "Source";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.Text = "Date";
      gvColumn3.Width = 150;
      this.gvMessages.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvMessages.Location = new Point(10, 11);
      this.gvMessages.Name = "gvMessages";
      this.gvMessages.Size = new Size(572, 258);
      this.gvMessages.SortOption = GVSortOption.None;
      this.gvMessages.TabIndex = 2;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(508, 279);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 3;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(592, 310);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.gvMessages);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EPassMessageDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Messages";
      this.ResumeLayout(false);
    }
  }
}
