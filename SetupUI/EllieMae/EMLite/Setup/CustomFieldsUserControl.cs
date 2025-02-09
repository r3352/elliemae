// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CustomFieldsUserControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CustomFieldsUserControl : SettingsUserControl
  {
    private CustomFieldsEditor fieldEditor;

    public CustomFieldsUserControl(
      Sessions.Session session,
      SetUpContainer setupContainer,
      bool allowMultiselect)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.fieldEditor = new CustomFieldsEditor(session, allowMultiselect);
      this.fieldEditor.Dock = DockStyle.Fill;
      this.Controls.Add((Control) this.fieldEditor);
      this.setDirtyFlag(false);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScroll = true;
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (CustomFieldsUserControl);
      this.Size = new Size(592, 404);
      this.ResumeLayout(false);
    }

    public string[] SelectedFieldIDs
    {
      get => this.fieldEditor.SelectedFieldIDs;
      set => this.fieldEditor.SelectedFieldIDs = value;
    }

    public void EditSelectedItem() => this.fieldEditor.EditSelectedItem();

    public void SetLoanCustomFieldId(string loanCustomFieldId)
    {
      this.fieldEditor.SelectedFieldIDs = new string[1]
      {
        loanCustomFieldId
      };
    }
  }
}
