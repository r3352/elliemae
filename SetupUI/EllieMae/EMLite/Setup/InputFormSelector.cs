// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.InputFormSelector
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class InputFormSelector : Form
  {
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private ListBox lstBoxInputForms;

    public InputFormSelector(Sessions.Session session)
    {
      this.InitializeComponent();
      InputFormInfo[] formInfos = session.FormManager.GetFormInfos(InputFormType.Standard);
      this.lstBoxInputForms.Items.Add((object) new InputFormInfo("", ""));
      this.lstBoxInputForms.Items.AddRange((object[]) formInfos);
      this.lstBoxInputForms.Items.AddRange((object[]) session.FormManager.GetFormInfos(InputFormType.Custom));
      for (int index = this.lstBoxInputForms.Items.Count - 1; index >= 0; --index)
      {
        InputFormInfo inputFormInfo = (InputFormInfo) this.lstBoxInputForms.Items[index];
        if (string.Compare(inputFormInfo.FormID, "UNDERWRITERSUMMARYP2", true) == 0 || string.Compare(inputFormInfo.FormID, "RE88395PG4", true) == 0 || string.Compare(inputFormInfo.FormID, "LOANCOMP", true) == 0 || string.Compare(inputFormInfo.FormID, "LODETAILEDLOCKREQUEST", true) == 0 || string.Compare(inputFormInfo.FormID, "FILECONTACTS", true) == 0 || string.Compare(inputFormInfo.FormID, "PIGGYBACK", true) == 0 || inputFormInfo.FormID.StartsWith("PREQUAL_") || string.Compare(inputFormInfo.FormID, "SETTLEMENTSERVICELIST", true) == 0 || string.Compare(inputFormInfo.FormID, "STATEDISCLOSUREINFO", true) == 0)
          this.lstBoxInputForms.Items.RemoveAt(index);
      }
      this.lstBoxInputForms.Sorted = true;
      this.lstBoxInputForms.SelectedIndex = 0;
    }

    public string SelectedFormID
    {
      get
      {
        InputFormInfo selectedItem = (InputFormInfo) this.lstBoxInputForms.SelectedItem;
        return selectedItem == (InputFormInfo) null || selectedItem.FormID == "" ? (string) null : selectedItem.FormID;
      }
    }

    public string SelectedFormName
    {
      get
      {
        InputFormInfo selectedItem = (InputFormInfo) this.lstBoxInputForms.SelectedItem;
        return selectedItem == (InputFormInfo) null || selectedItem.FormID == "" ? (string) null : selectedItem.Name;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lstBoxInputForms = new ListBox();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(193, 443);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(274, 443);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lstBoxInputForms.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lstBoxInputForms.FormattingEnabled = true;
      this.lstBoxInputForms.Location = new Point(0, 0);
      this.lstBoxInputForms.Name = "lstBoxInputForms";
      this.lstBoxInputForms.Size = new Size(359, 433);
      this.lstBoxInputForms.TabIndex = 2;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(357, 473);
      this.Controls.Add((Control) this.lstBoxInputForms);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Name = nameof (InputFormSelector);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select an Input Form";
      this.ResumeLayout(false);
    }
  }
}
