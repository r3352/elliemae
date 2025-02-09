// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ImportCompanyDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web.Security;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ImportCompanyDialog : Form
  {
    private Sessions.Session session;
    private int parent;
    private int depth;
    private string hierarchyPath;
    private int oid;
    private bool forLender;
    private Hashtable parentContacts;
    private List<FetchContacts> tpoContactList = new List<FetchContacts>();
    private Dictionary<string, string> alreadyExists = new Dictionary<string, string>();
    private Dictionary<int, string> hierarchyNodes = new Dictionary<int, string>();
    private Dictionary<int, string> nodesToRemove = new Dictionary<int, string>();
    private IContainer components;
    private GradientPanel gradientPanel2;
    private Label lblSubTitle;
    private GroupContainer grpPlans;
    private GridView gridViewContactsTPO;
    private Button btnImport;
    private Button btnCancel;

    public Dictionary<int, string> HierarchyNodes => this.hierarchyNodes;

    public Dictionary<int, string> NodesToRemove => this.nodesToRemove;

    public ImportCompanyDialog(
      Sessions.Session session,
      int parent,
      int depth,
      string hierarchyPath)
    {
      this.session = session;
      this.parent = parent;
      this.depth = depth;
      this.hierarchyPath = hierarchyPath;
      if (hierarchyPath.StartsWith("Lenders"))
        this.forLender = true;
      this.InitializeComponent();
      Cursor.Current = Cursors.WaitCursor;
      this.fetchTPOCompanies();
      Cursor.Current = Cursors.Default;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gridViewContactsTPO.SelectedItems)
      {
        FetchContacts tag = (FetchContacts) selectedItem.Tag;
        int parentOID = this.addCompany(tag);
        if (parentOID != 0 && this.parentContacts.ContainsKey((object) tag.ExternalID))
        {
          List<FetchContacts> parentContact = (List<FetchContacts>) this.parentContacts[(object) tag.ExternalID];
          this.addBranches(parentOID, tag, parentContact);
        }
      }
      if (this.alreadyExists.Count > 0)
      {
        using (AlreadyExists alreadyExists = new AlreadyExists(this.alreadyExists))
        {
          if (alreadyExists.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            List<string> stringList = alreadyExists.Value;
            for (int index1 = 0; index1 < stringList.Count; ++index1)
            {
              FetchContacts parentContact1 = (FetchContacts) null;
              for (int index2 = 0; index2 < this.tpoContactList.Count; ++index2)
              {
                if (this.tpoContactList[index2].ExternalID == stringList[index1])
                {
                  parentContact1 = this.tpoContactList[index2];
                  break;
                }
              }
              int oidByTpoId = this.session.ConfigurationManager.GetOidByTPOId(this.forLender, parentContact1.ExternalID);
              if (this.parent == oidByTpoId)
              {
                int num1 = (int) Utils.Dialog((IWin32Window) this, "You can't assign a company to itself in the hierarchy.", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
              }
              else if (this.hierarchyPath.ToLower().IndexOf("\\" + parentContact1.OrganizationName.ToLower() + "\\") > -1)
              {
                int num2 = (int) Utils.Dialog((IWin32Window) this, "You can't assign a company to itself in the hierarchy.", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
              }
              else
              {
                string hierarchyPath = this.hierarchyPath + "\\" + parentContact1.OrganizationName;
                int depth = this.depth + 1;
                this.session.ConfigurationManager.MoveExternalCompany(this.forLender, new HierarchySummary(oidByTpoId, this.parent, parentContact1.ExternalID, parentContact1.OrganizationName, parentContact1.CompanyLegalName, parentContact1.CompanyDBAName, depth, hierarchyPath));
                this.session.ConfigurationManager.OverwriteTPOContact(this.forLender, oidByTpoId, parentContact1.OrganizationName, parentContact1.CompanyDBAName, parentContact1.CompanyLegalName, parentContact1.Address, parentContact1.City, parentContact1.State, parentContact1.Zip, parentContact1.EntityType, this.parent, depth, hierarchyPath);
                this.hierarchyNodes.Add(oidByTpoId, parentContact1.OrganizationName);
                if (this.parentContacts.ContainsKey((object) parentContact1.ExternalID))
                {
                  List<FetchContacts> parentContact2 = (List<FetchContacts>) this.parentContacts[(object) parentContact1.ExternalID];
                  this.addBranches(oidByTpoId, parentContact1, parentContact2);
                }
              }
            }
          }
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private int addCompany(FetchContacts contact)
    {
      this.oid = this.session.ConfigurationManager.GetOidByTPOId(this.forLender, contact.ExternalID);
      contact.Parent = this.parent;
      contact.HierarchyPath = this.hierarchyPath + "\\" + contact.OrganizationName;
      contact.Depth = this.depth + 1;
      if (this.oid == 0)
      {
        this.oid = this.session.ConfigurationManager.AddTPOContact(this.forLender, contact.ExternalID, contact.OrganizationName, contact.CompanyDBAName, contact.CompanyLegalName, contact.EntityType, contact.Address, contact.City, contact.State, contact.Zip, contact.Parent, contact.Depth, contact.HierarchyPath);
        this.hierarchyNodes.Add(this.oid, contact.OrganizationName);
        return this.oid;
      }
      this.alreadyExists.Add(contact.ExternalID, contact.OrganizationName);
      return 0;
    }

    private void addBranches(
      int parentOID,
      FetchContacts parentContact,
      List<FetchContacts> contacts)
    {
      int num = 0;
      for (int index = 0; index < contacts.Count; ++index)
      {
        int oidByTpoId = this.session.ConfigurationManager.GetOidByTPOId(this.forLender, contacts[index].ExternalID);
        contacts[index].HierarchyPath = parentContact.HierarchyPath + "\\" + contacts[index].OrganizationName;
        contacts[index].Depth = parentContact.Depth + 1;
        contacts[index].Parent = parentOID;
        if (oidByTpoId == 0)
        {
          num = this.session.ConfigurationManager.AddTPOContact(this.forLender, contacts[index].ExternalID, contacts[index].OrganizationName, contacts[index].CompanyDBAName, contacts[index].CompanyLegalName, contacts[index].EntityType, contacts[index].Address, contacts[index].City, contacts[index].State, contacts[index].Zip, contacts[index].Parent, contacts[index].Depth, contacts[index].HierarchyPath);
        }
        else
        {
          this.session.ConfigurationManager.MoveExternalCompany(this.forLender, new HierarchySummary(oidByTpoId, contacts[index].Parent, contacts[index].ExternalID, contacts[index].OrganizationName, contacts[index].CompanyLegalName, contacts[index].CompanyDBAName, contacts[index].Depth, contacts[index].HierarchyPath));
          this.nodesToRemove.Add(oidByTpoId, contacts[index].OrganizationName);
          this.session.ConfigurationManager.OverwriteTPOContact(this.forLender, oidByTpoId, contacts[index].OrganizationName, contacts[index].CompanyDBAName, contacts[index].CompanyDBAName, contacts[index].Address, contacts[index].City, contacts[index].State, contacts[index].Zip, contacts[index].EntityType, contacts[index].Parent, contacts[index].Depth, contacts[index].HierarchyPath);
        }
      }
    }

    private void fetchTPOCompanies()
    {
      if (this.parentContacts != null)
        this.parentContacts.Clear();
      this.parentContacts = new Hashtable();
      this.tpoContactList.Clear();
      XmlDocument xmlDocument = new XmlDocument();
      string str = FormsAuthentication.HashPasswordForStoringInConfigFile(this.session.CompanyInfo.ClientID + this.session.CompanyInfo.ClientID, "sha1");
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        HttpWebResponse response = (WebRequest.Create(string.Format("https://www.encompasswebcenter.com/ImportTPOBrokers/ImportTPOBrokers.svc/Import?clientId={0}&key={1}", (object) this.session.CompanyInfo.ClientID, (object) str)) as HttpWebRequest).GetResponse() as HttpWebResponse;
        if (response.StatusCode == HttpStatusCode.OK)
        {
          string xml = string.Empty;
          using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            xml = streamReader.ReadToEnd();
          xmlDocument.LoadXml(xml);
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Cannot find any company information from TPO WebCenter.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot read company information from TPO WebCenter due to error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
      XmlNodeList xmlNodeList1 = xmlDocument.SelectNodes("BrokerCompaniesResponse/BrokerCompaniesResult/TPOSiteList/TPOSite");
      if (xmlNodeList1 == null || xmlNodeList1.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Cannot find company information in TPO WebCenter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        string empty4 = string.Empty;
        string empty5 = string.Empty;
        string empty6 = string.Empty;
        string empty7 = string.Empty;
        string empty8 = string.Empty;
        foreach (XmlNode xmlNode1 in xmlNodeList1)
        {
          XmlNodeList xmlNodeList2 = xmlNode1.SelectNodes("TPOCompanyList/TPOCompany");
          if (xmlNodeList2 != null && xmlNodeList2.Count != 0)
          {
            foreach (XmlNode xmlNode2 in xmlNodeList2)
            {
              FetchContacts fetchContacts1 = this.buildContact(xmlNode2.ChildNodes);
              this.tpoContactList.Add(fetchContacts1);
              if (!this.parentContacts.ContainsKey((object) fetchContacts1.ExternalID))
                this.parentContacts.Add((object) fetchContacts1.ExternalID, (object) new List<FetchContacts>());
              XmlNodeList xmlNodeList3 = xmlNode2.SelectNodes("TPOBranchList/TPOBranch");
              if (xmlNodeList3 != null && xmlNodeList3.Count != 0)
              {
                List<FetchContacts> parentContact = (List<FetchContacts>) this.parentContacts[(object) fetchContacts1.ExternalID];
                foreach (XmlNode xmlNode3 in xmlNodeList3)
                {
                  FetchContacts fetchContacts2 = this.buildContact(xmlNode3.ChildNodes);
                  parentContact.Add(fetchContacts2);
                }
              }
            }
          }
        }
        this.refreshListView(this.tpoContactList);
        this.gridViewContactsTPO_SelectedIndexChanged((object) null, (EventArgs) null);
        this.grpPlans.Text = "Companies (" + (object) this.gridViewContactsTPO.Items.Count + ")";
      }
    }

    private FetchContacts buildContact(XmlNodeList node)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      string str3 = string.Empty;
      string str4 = string.Empty;
      string str5 = string.Empty;
      string str6 = string.Empty;
      string str7 = string.Empty;
      string str8 = string.Empty;
      foreach (XmlNode xmlNode in node)
      {
        switch (xmlNode.Name)
        {
          case "BranchID":
            str1 = xmlNode.InnerText;
            continue;
          case "City":
            str5 = xmlNode.InnerText;
            continue;
          case "CompanyID":
            str1 = xmlNode.InnerText;
            continue;
          case "DBAName":
            str2 = xmlNode.InnerText;
            continue;
          case "LegalName":
            str3 = xmlNode.InnerText;
            continue;
          case "Originator":
            str8 = xmlNode.InnerText;
            continue;
          case "State":
            str6 = xmlNode.InnerText;
            continue;
          case "StreetAddress":
            str4 = xmlNode.InnerText;
            continue;
          case "Zip":
            str7 = xmlNode.InnerText;
            continue;
          default:
            continue;
        }
      }
      string id = str1;
      string organizationName = str2.Trim();
      string companyDBAName = str2.Trim();
      string companyLegalName = str3.Trim();
      int entityType;
      switch (str8)
      {
        case "Correspondent":
          entityType = 2;
          break;
        case "Both":
          entityType = 3;
          break;
        default:
          entityType = 1;
          break;
      }
      string address = str4.Trim();
      string city = str5.Trim();
      string state = str6.Trim();
      string zip = str7.Trim();
      return new FetchContacts(id, organizationName, companyDBAName, companyLegalName, (ExternalOriginatorEntityType) entityType, address, city, state, zip)
      {
        Parent = this.parent,
        Depth = this.depth,
        HierarchyPath = this.hierarchyPath
      };
    }

    private void refreshListView(List<FetchContacts> data)
    {
      this.Cursor = Cursors.WaitCursor;
      this.gridViewContactsTPO.BeginUpdate();
      try
      {
        this.gridViewContactsTPO.Items.Clear();
        if (data == null)
          return;
        GVItem gvItem1 = (GVItem) null;
        foreach (FetchContacts contact in data)
        {
          GVItem gvItem2 = this.createGVItem(contact);
          if (gvItem1 == null)
            gvItem1 = gvItem2;
          this.gridViewContactsTPO.Items.Add(gvItem2);
        }
      }
      finally
      {
        this.gridViewContactsTPO.EndUpdate();
        this.Cursor = Cursors.Default;
      }
    }

    private GVItem createGVItem(FetchContacts contact)
    {
      return new GVItem(contact.CompanyDBAName)
      {
        SubItems = {
          (object) contact.CompanyLegalName,
          (object) contact.Address,
          (object) contact.City,
          (object) contact.State,
          (object) contact.Zip
        },
        Tag = (object) contact
      };
    }

    private void gridViewContactsTPO_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnImport.Enabled = this.gridViewContactsTPO.SelectedItems.Count > 0;
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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.gradientPanel2 = new GradientPanel();
      this.lblSubTitle = new Label();
      this.grpPlans = new GroupContainer();
      this.gridViewContactsTPO = new GridView();
      this.btnImport = new Button();
      this.btnCancel = new Button();
      this.gradientPanel2.SuspendLayout();
      this.grpPlans.SuspendLayout();
      this.SuspendLayout();
      this.gradientPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel2.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel2.Controls.Add((Control) this.lblSubTitle);
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.GradientPaddingColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      this.gradientPanel2.Location = new Point(0, 0);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(793, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 5;
      this.lblSubTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSubTitle.AutoEllipsis = true;
      this.lblSubTitle.BackColor = Color.Transparent;
      this.lblSubTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblSubTitle.Location = new Point(10, 9);
      this.lblSubTitle.Name = "lblSubTitle";
      this.lblSubTitle.Size = new Size(755, 14);
      this.lblSubTitle.TabIndex = 0;
      this.lblSubTitle.Text = "Select Company(s) and/or Branches to import from TPO WebCenter.";
      this.lblSubTitle.TextAlign = ContentAlignment.MiddleLeft;
      this.grpPlans.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpPlans.Controls.Add((Control) this.gridViewContactsTPO);
      this.grpPlans.HeaderForeColor = SystemColors.ControlText;
      this.grpPlans.Location = new Point(0, 31);
      this.grpPlans.Name = "grpPlans";
      this.grpPlans.Size = new Size(792, 464);
      this.grpPlans.TabIndex = 6;
      this.grpPlans.Text = "Companies/Branches";
      this.gridViewContactsTPO.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "DBA";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Company Name";
      gvColumn2.Width = 180;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Address";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "City";
      gvColumn4.Width = 140;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "State";
      gvColumn5.Width = 40;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "Zip";
      gvColumn6.Width = 80;
      this.gridViewContactsTPO.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gridViewContactsTPO.Dock = DockStyle.Fill;
      this.gridViewContactsTPO.Location = new Point(1, 26);
      this.gridViewContactsTPO.Name = "gridViewContactsTPO";
      this.gridViewContactsTPO.Size = new Size(790, 437);
      this.gridViewContactsTPO.TabIndex = 1;
      this.gridViewContactsTPO.SelectedIndexChanged += new EventHandler(this.gridViewContactsTPO_SelectedIndexChanged);
      this.btnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnImport.Location = new Point(625, 501);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(75, 23);
      this.btnImport.TabIndex = 2;
      this.btnImport.Text = "Select";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(706, 501);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnImport;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(792, 532);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnImport);
      this.Controls.Add((Control) this.grpPlans);
      this.Controls.Add((Control) this.gradientPanel2);
      this.MinimizeBox = false;
      this.Name = nameof (ImportCompanyDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import from TPO WebCenter";
      this.gradientPanel2.ResumeLayout(false);
      this.grpPlans.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
