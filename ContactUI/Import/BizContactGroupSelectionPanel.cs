// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.BizContactGroupSelectionPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class BizContactGroupSelectionPanel : ContactImportWizardItem
  {
    private ColumnHeader columnHeader1;
    private ListView listViewGroup;
    private Panel panel2;
    private Label label1;
    private System.ComponentModel.Container components;

    public BizContactGroupSelectionPanel(ContactImportWizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
      this.Header = "Business Contact Groups";
      this.Subheader = "";
      this.InitialData();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.listViewGroup = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.panel2 = new Panel();
      this.label1 = new Label();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.listViewGroup.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader1
      });
      this.listViewGroup.FullRowSelect = true;
      this.listViewGroup.GridLines = true;
      this.listViewGroup.HideSelection = false;
      this.listViewGroup.Location = new Point(80, 56);
      this.listViewGroup.Name = "listViewGroup";
      this.listViewGroup.Size = new Size(308, 144);
      this.listViewGroup.TabIndex = 10;
      this.listViewGroup.View = View.Details;
      this.columnHeader1.Text = "Group Name";
      this.columnHeader1.Width = 302;
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.listViewGroup);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 11;
      this.label1.Location = new Point(80, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(308, 28);
      this.label1.TabIndex = 11;
      this.label1.Text = "Add the contacts to the business contact groups selected below";
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.panel2);
      this.Name = nameof (BizContactGroupSelectionPanel);
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void InitialData()
    {
      ContactGroupInfo[] contactGroupInfoArray = Session.ContactGroupManager.GetPublicBizContactGroups();
      if (!Session.UserInfo.IsSuperAdministrator())
      {
        BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(Session.UserID, true);
        ArrayList arrayList = new ArrayList();
        foreach (ContactGroupInfo contactGroupInfo in contactGroupInfoArray)
        {
          foreach (BizGroupRef bizGroupRef in contactGroupRefs)
          {
            if (bizGroupRef.BizGroupID == contactGroupInfo.GroupId && !arrayList.Contains((object) contactGroupInfo))
            {
              arrayList.Add((object) contactGroupInfo);
              break;
            }
          }
        }
        contactGroupInfoArray = (ContactGroupInfo[]) arrayList.ToArray(typeof (ContactGroupInfo));
      }
      foreach (ContactGroupInfo contactGroupInfo in contactGroupInfoArray)
        this.listViewGroup.Items.Add(new ListViewItem(contactGroupInfo.GroupName)
        {
          Tag = (object) contactGroupInfo
        });
    }

    private void SetGroupListParameter()
    {
      if (this.listViewGroup.SelectedItems == null || this.listViewGroup.SelectedItems.Count <= 0)
        return;
      ArrayList arrayList = new ArrayList();
      foreach (ListViewItem selectedItem in this.listViewGroup.SelectedItems)
      {
        if ((object) (selectedItem.Tag as ContactGroupInfo) != null)
          arrayList.Add(selectedItem.Tag);
      }
      if (arrayList.Count <= 0)
        return;
      this.ImportParameters.GroupList = (ContactGroupInfo[]) arrayList.ToArray(typeof (ContactGroupInfo));
    }

    public override WizardItem Next()
    {
      if (this.listViewGroup.SelectedItems == null || this.listViewGroup.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one business contact group.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (WizardItem) null;
      }
      this.SetGroupListParameter();
      if (this.ImportParameters.ImportMethod == ImportMethod.Outlook)
        return (WizardItem) new OutlookImportPanel((ContactImportWizardItem) this);
      return this.ImportParameters.ImportMethod == ImportMethod.CSV ? (WizardItem) new CsvFileSelectionPanel((ContactImportWizardItem) this) : (WizardItem) new PointFolderSelectionPanel((ContactImportWizardItem) this);
    }

    public override WizardItem Back()
    {
      this.ImportParameters.GroupList = (ContactGroupInfo[]) null;
      return base.Back();
    }
  }
}
