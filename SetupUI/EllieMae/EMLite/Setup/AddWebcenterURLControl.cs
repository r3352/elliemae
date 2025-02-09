// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddWebcenterURLControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddWebcenterURLControl : UserControl
  {
    private List<DynamicField> dynamicFields;
    private const int FIELDSPACE = 22;
    private bool useDropdown;
    private ExternalOriginatorManagementData externalOriginator;
    private ExternalUserInfo externalUserInfo;
    private Sessions.Session session;
    private List<string> urls;
    private List<ExternalOrgURL> availableURLs = new List<ExternalOrgURL>();
    private List<string> listEntity = new List<string>();
    private int orgID = -1;
    public List<ExternalOrgURL> selectedUrls = new List<ExternalOrgURL>();
    public ExternalOriginatorManagementData root;
    private int parentOid;
    private bool isOrg;
    private StandardIconButton BtnSave;
    private object caller;
    private ExternalOrgURL selectedUrl;
    private bool urlDeleted;
    private IContainer components;
    private Button btnAddMore;
    private Panel pnlAddMore;
    private FlowLayoutPanel pnlUrls;

    public event EventHandler ChangedEvent;

    public bool isOnlyURL { get; set; }

    public AddWebcenterURLControl(
      bool IsOrg,
      bool useDropdown,
      Sessions.Session session,
      int parent,
      ExternalOriginatorManagementData externalOriginator = null,
      ExternalUserInfo externalUserInfo = null,
      int orgID = -1,
      StandardIconButton btnSave = null,
      object caller = null)
    {
      this.useDropdown = useDropdown;
      this.InitializeComponent();
      this.externalUserInfo = externalUserInfo;
      this.externalOriginator = externalOriginator;
      this.session = session;
      this.orgID = orgID;
      this.BtnSave = btnSave;
      this.isOrg = IsOrg;
      if (this.orgID == -1 && this.externalOriginator != null)
        this.orgID = this.externalOriginator.oid;
      if (!this.isOrg)
      {
        this.pnlUrls.AutoScroll = false;
        this.pnlUrls.AutoSize = true;
      }
      this.Dock = DockStyle.Fill;
      this.parentOid = parent;
      this.caller = caller;
      this.initPageValue();
    }

    public void SetWebCenterUrlControlState(bool enabled)
    {
      foreach (Control control in (ArrangedElementCollection) this.pnlUrls.Controls)
      {
        if (control is WebcenterURLCtrl)
          control.Enabled = enabled;
      }
    }

    public void initPageValue()
    {
      this.SuspendLayout();
      this.dynamicFields = new List<DynamicField>();
      this.root = this.externalOriginator == null ? this.session.ConfigurationManager.GetRootOrganisation(false, this.parentOid) : this.session.ConfigurationManager.GetRootOrganisation(false, this.externalOriginator.oid);
      if (!this.isOrg)
      {
        if (this.orgID > -1)
          this.availableURLs.AddRange((IEnumerable<ExternalOrgURL>) this.session.ConfigurationManager.GetSelectedOrgUrls(this.orgID));
        else
          this.availableURLs.AddRange((IEnumerable<ExternalOrgURL>) this.session.ConfigurationManager.GetSelectedOrgUrls(this.externalOriginator.oid));
      }
      else
      {
        List<ExternalOrgURL> externalOrgUrlList = new List<ExternalOrgURL>();
        foreach (ExternalOrgURL externalOrgUrl in this.root.oid != this.orgID ? this.session.ConfigurationManager.GetSelectedOrgUrls(this.parentOid) : ((IEnumerable<ExternalOrgURL>) this.session.ConfigurationManager.GetExternalOrganizationURLs()).ToList<ExternalOrgURL>())
        {
          if (!externalOrgUrl.isDeleted)
            this.availableURLs.Add(externalOrgUrl);
        }
        if (this.externalOriginator != null && this.externalOriginator.Parent != 0)
        {
          this.selectedUrls.AddRange((IEnumerable<ExternalOrgURL>) this.session.ConfigurationManager.GetSelectedOrgUrls(this.externalOriginator.oid));
          this.BtnSave.Enabled = false;
          ((EditCompanyWebcenterControl) this.caller).IsDirty = false;
        }
        else
        {
          this.selectedUrls.AddRange((IEnumerable<ExternalOrgURL>) this.session.ConfigurationManager.GetSelectedOrgUrls(this.orgID));
          this.BtnSave.Enabled = false;
          ((EditCompanyWebcenterControl) this.caller).IsDirty = false;
        }
      }
      if (this.externalOriginator != null)
      {
        this.PopulateEntityTypes(this.externalOriginator.entityType);
      }
      else
      {
        this.listEntity.Add("Broker");
        this.listEntity.Add("Correspondent");
        this.listEntity.Add("Both");
      }
      if (this.isOrg)
      {
        if (!this.selectedUrls.Any<ExternalOrgURL>())
        {
          if (!this.availableURLs.Any<ExternalOrgURL>())
          {
            this.Enabled = false;
          }
          else
          {
            URLControlWithEntity controlWithEntity = new URLControlWithEntity(this.useDropdown, this.availableURLs, this.pnlUrls.Controls.Count, this.listEntity);
            controlWithEntity.Size = new Size(this.pnlUrls.Width - 100, controlWithEntity.Height);
            controlWithEntity.Dock = DockStyle.Top;
            controlWithEntity.DeleteEvent += new URLControlWithEntity.EventHandle(this.urlCtrl_DeleteEvent);
            controlWithEntity.ChangeEvent += new URLControlWithEntity.EventHandle(this.urlCtrl_ChangeEvent);
            controlWithEntity.UrlEnter += new URLControlWithEntity.EventHandle(this.urlCtrl_UrlEnter);
            controlWithEntity.Leave += new EventHandler(this.urlCtrl_Leave);
            controlWithEntity.UrlTextChanged += new URLControlWithEntity.EventHandle(this.urlCtrl_UrlTextChanged);
            controlWithEntity.IsWarningVisible = false;
            this.pnlUrls.Controls.Add((Control) controlWithEntity);
          }
        }
        else
        {
          foreach (ExternalOrgURL selectedUrl in this.selectedUrls)
          {
            int selectIndexEntity = this.listEntity.Count > 1 ? selectedUrl.EntityType - 1 : 0;
            URLControlWithEntity controlWithEntity = new URLControlWithEntity(this.useDropdown, this.availableURLs, this.pnlUrls.Controls.Count, this.listEntity, selectedUrl.URL, selectIndexEntity);
            controlWithEntity.Size = new Size(this.pnlUrls.Width - 100, controlWithEntity.Height);
            controlWithEntity.Dock = DockStyle.Top;
            controlWithEntity.DeleteEvent += new URLControlWithEntity.EventHandle(this.urlCtrl_DeleteEvent);
            controlWithEntity.ChangeEvent += new URLControlWithEntity.EventHandle(this.urlCtrl_ChangeEvent);
            controlWithEntity.UrlEnter += new URLControlWithEntity.EventHandle(this.urlCtrl_UrlEnter);
            controlWithEntity.Leave += new EventHandler(this.urlCtrl_Leave);
            controlWithEntity.UrlTextChanged += new URLControlWithEntity.EventHandle(this.urlCtrl_UrlTextChanged);
            controlWithEntity.IsWarningVisible = selectedUrl.isDeleted;
            this.pnlUrls.Controls.Add((Control) controlWithEntity);
          }
        }
      }
      else
      {
        this.urls = new List<string>();
        this.availableURLs.ForEach((Action<ExternalOrgURL>) (x => this.urls.Add(x.URL)));
        this.Height = 63;
        if ((UserInfo) this.externalUserInfo != (UserInfo) null)
          ((IEnumerable<ExternalUserURL>) this.session.ConfigurationManager.GetExternalUserInfoURLs(this.externalUserInfo.ExternalUserID)).ToList<ExternalUserURL>().ForEach((Action<ExternalUserURL>) (x =>
          {
            this.availableURLs.FindIndex((Predicate<ExternalOrgURL>) (y => y.URLID == x.URLID));
            this.availableURLs.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (y => y.URLID == x.URLID));
            WebcenterURLCtrl webcenterUrlCtrl = new WebcenterURLCtrl(this.useDropdown, this.availableURLs, this.pnlUrls.Controls.Count, x.URL);
            webcenterUrlCtrl.Size = new Size(this.pnlUrls.Width - 10, webcenterUrlCtrl.Height);
            webcenterUrlCtrl.IsWarningVisible = this.availableURLs.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (y => y.URLID == x.URLID)) == null || this.availableURLs.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (y => y.URLID == x.URLID)).isDeleted;
            webcenterUrlCtrl.DeleteEvent += new WebcenterURLCtrl.EventHandle(this.urlCtrl_DeleteEvent);
            webcenterUrlCtrl.ChangeEvent += new WebcenterURLCtrl.EventHandle(this.urlCtrl_ChangeEvent);
            webcenterUrlCtrl.UrlEnter += new WebcenterURLCtrl.EventHandle(this.urlCtrl_UrlEnter);
            webcenterUrlCtrl.Leave += new EventHandler(this.urlCtrl_Leave);
            webcenterUrlCtrl.TextChange += new WebcenterURLCtrl.EventHandle(this.urlCtrl_UrlTextChanged);
            this.pnlUrls.Controls.Add((Control) webcenterUrlCtrl);
            this.Height += 34;
          }));
        if (this.pnlUrls.Controls.Count == 1)
        {
          string selectedURL = "";
          if (this.availableURLs.Count == 1 && (UserInfo) this.externalUserInfo == (UserInfo) null)
          {
            selectedURL = this.availableURLs[0].URL;
            this.isOnlyURL = true;
          }
          WebcenterURLCtrl webcenterUrlCtrl = new WebcenterURLCtrl(this.useDropdown, this.availableURLs, this.pnlUrls.Controls.Count, selectedURL);
          webcenterUrlCtrl.Size = new Size(this.pnlUrls.Width - 10, webcenterUrlCtrl.Height);
          webcenterUrlCtrl.IsWarningVisible = false;
          webcenterUrlCtrl.DeleteEvent += new WebcenterURLCtrl.EventHandle(this.urlCtrl_DeleteEvent);
          webcenterUrlCtrl.ChangeEvent += new WebcenterURLCtrl.EventHandle(this.urlCtrl_ChangeEvent);
          webcenterUrlCtrl.UrlEnter += new WebcenterURLCtrl.EventHandle(this.urlCtrl_UrlEnter);
          webcenterUrlCtrl.Leave += new EventHandler(this.urlCtrl_Leave);
          webcenterUrlCtrl.TextChange += new WebcenterURLCtrl.EventHandle(this.urlCtrl_UrlTextChanged);
          this.pnlUrls.Controls.Add((Control) webcenterUrlCtrl);
        }
      }
      this.pnlUrls.Controls.SetChildIndex((Control) this.pnlAddMore, this.pnlUrls.Controls.Count);
      this.ResumeLayout();
    }

    private void urlCtrl_UrlEnter(object sender, EventArgs e)
    {
      this.selectedUrl = this.availableURLs.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == ((Control) sender).Text));
    }

    public void PopulateEntityTypes(
      ExternalOriginatorEntityType externalOriginatorEntityType)
    {
      this.listEntity = new List<string>();
      switch (externalOriginatorEntityType)
      {
        case ExternalOriginatorEntityType.Broker:
          this.listEntity.Add("Broker");
          break;
        case ExternalOriginatorEntityType.Correspondent:
          this.listEntity.Add("Correspondent");
          break;
        case ExternalOriginatorEntityType.Both:
          this.listEntity.Add("Broker");
          this.listEntity.Add("Correspondent");
          this.listEntity.Add("Both");
          break;
        default:
          this.listEntity.Add("None");
          break;
      }
    }

    private void urlCtrl_ChangeEvent(object sender, EventArgs e)
    {
      if (this.isOrg)
      {
        List<ExternalOrgURL> externalOrgUrlList = new List<ExternalOrgURL>();
        foreach (Control control in (ArrangedElementCollection) this.pnlUrls.Controls)
        {
          if (control is URLControlWithEntity)
          {
            ExternalOrgURL selectUrl = (ExternalOrgURL) ((URLControlWithEntity) control).SelectedUrl;
            if (selectUrl != null)
            {
              if (externalOrgUrlList.Contains(selectUrl))
              {
                int num = (int) MessageBox.Show("The URL you selected is already in use on this screen.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                ((URLControlWithEntity) control).SetUrlFocus = true;
                ((URLControlWithEntity) control).SelectedUrl = (object) this.selectedUrl;
                return;
              }
              externalOrgUrlList.Add(selectUrl);
              this.selectedUrls.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URLID == selectUrl.URLID));
              if (selectUrl.isDeleted)
                ((URLControlWithEntity) control).IsWarningVisible = true;
              else
                ((URLControlWithEntity) control).IsWarningVisible = false;
              if (((URLControlWithEntity) control).selectedEntity == null || (string) ((URLControlWithEntity) control).selectedEntity == string.Empty)
                ((URLControlWithEntity) control).selectedEntity = (object) Convert.ToString((object) this.externalOriginator.entityType);
            }
          }
        }
        this.BtnSave.Enabled = true;
        ((EditCompanyWebcenterControl) this.caller).IsDirty = true;
      }
      else
      {
        if (this.ChangedEvent != null)
          this.ChangedEvent(sender, e);
        List<ExternalOrgURL> externalOrgUrlList = new List<ExternalOrgURL>();
        foreach (Control control in (ArrangedElementCollection) this.pnlUrls.Controls)
        {
          if (control is WebcenterURLCtrl)
          {
            ExternalOrgURL selectedUrl = (ExternalOrgURL) ((WebcenterURLCtrl) control).SelectedURL;
            if (selectedUrl != null)
            {
              if (externalOrgUrlList.Contains(selectedUrl))
              {
                int num = (int) MessageBox.Show("The URL you selected is already in use on this screen.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                ((WebcenterURLCtrl) control).SetFocus = true;
                ((WebcenterURLCtrl) control).SelectedURL = (object) this.selectedUrl;
                break;
              }
              externalOrgUrlList.Add(selectedUrl);
              if (selectedUrl.isDeleted)
                ((WebcenterURLCtrl) control).IsWarningVisible = true;
              else
                ((WebcenterURLCtrl) control).IsWarningVisible = false;
            }
          }
        }
      }
    }

    public bool AddOrUpdateExternalOrgURLS()
    {
      List<ExternalOrgURL> externalOrgUrlList1 = new List<ExternalOrgURL>();
      List<int> intList = new List<int>();
      foreach (Control control in (ArrangedElementCollection) this.pnlUrls.Controls)
      {
        Control ctrl = control;
        if (ctrl is URLControlWithEntity)
        {
          if (((URLControlWithEntity) ctrl).IsWarningVisible)
          {
            int num = (int) MessageBox.Show("One or more selection is invalid.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            ((URLControlWithEntity) ctrl).SetUrlFocus = true;
            return false;
          }
          if ((ExternalOrgURL) ((URLControlWithEntity) ctrl).SelectedUrl == null || (string) ((URLControlWithEntity) ctrl).selectedEntity == string.Empty || ((URLControlWithEntity) ctrl).selectedEntity == null)
          {
            if (this.availableURLs.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == ((URLControlWithEntity) ctrl).Text)) == null)
            {
              int num = (int) MessageBox.Show("Your entered URL is not valid or not one of the assigned URL.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              if ((ExternalOrgURL) ((URLControlWithEntity) ctrl).SelectedUrl == null)
                ((URLControlWithEntity) ctrl).SetUrlFocus = true;
              else
                ((URLControlWithEntity) ctrl).SetEntityFocus = true;
              return false;
            }
            if ((string) ((URLControlWithEntity) ctrl).selectedEntity == string.Empty || ((URLControlWithEntity) ctrl).selectedEntity == null)
            {
              int num = (int) MessageBox.Show("Channel Type cannot be blank.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
          }
          if (externalOrgUrlList1.Contains((ExternalOrgURL) ((URLControlWithEntity) ctrl).SelectedUrl))
          {
            int num = (int) MessageBox.Show("The URL you selected is already in use on this screen.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            ((URLControlWithEntity) ctrl).SetUrlFocus = true;
            return false;
          }
          string selectUrltext = ((URLControlWithEntity) ctrl).Text;
          ExternalOrgURL externalOrgUrl = this.availableURLs.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == selectUrltext));
          string selectedEntity = (string) ((URLControlWithEntity) ctrl).selectedEntity;
          if (selectedEntity != null && !(selectedEntity == ""))
          {
            object obj = Enum.Parse(typeof (ExternalOriginatorEntityType), selectedEntity);
            externalOrgUrl.EntityType = Convert.ToInt32(obj);
            externalOrgUrlList1.Add(externalOrgUrl);
          }
        }
      }
      if (this.externalOriginator != null)
      {
        this.session.ConfigurationManager.UpdateExternalOrganizationSelectedURLs(this.orgID, externalOrgUrlList1, this.root.oid);
        foreach (int organizationDesendent in this.session.ConfigurationManager.GetExternalOrganizationDesendents(this.orgID))
        {
          List<ExternalOrgURL> selectedOrgUrls = this.session.ConfigurationManager.GetSelectedOrgUrls(organizationDesendent);
          List<ExternalOrgURL> externalOrgUrlList2 = new List<ExternalOrgURL>();
          foreach (ExternalOrgURL externalOrgUrl in selectedOrgUrls)
          {
            ExternalOrgURL u = externalOrgUrl;
            if (externalOrgUrlList1.Find((Predicate<ExternalOrgURL>) (a => a.URLID == u.URLID)) == null)
              externalOrgUrlList2.Add(u);
          }
          foreach (ExternalOrgURL externalOrgUrl in externalOrgUrlList2)
            selectedOrgUrls.Remove(externalOrgUrl);
          this.session.ConfigurationManager.UpdateExternalOrganizationSelectedURLs(organizationDesendent, selectedOrgUrls, this.root.oid);
        }
      }
      if (externalOrgUrlList1.Count == 1)
      {
        this.session.ConfigurationManager.GetAllExternalUserInfos(this.orgID);
        externalOrgUrlList1.Select<ExternalOrgURL, int>((Func<ExternalOrgURL, int>) (a => a.URLID)).ToArray<int>();
      }
      this.BtnSave.Enabled = false;
      ((EditCompanyWebcenterControl) this.caller).IsDirty = false;
      return true;
    }

    private void urlCtrl_DeleteEvent(object sender, EventArgs e)
    {
      if (!this.isOrg)
      {
        if (sender is WebcenterURLCtrl)
        {
          this.urlDeleted = true;
          ((WebcenterURLCtrl) sender).deleteControl = true;
          this.pnlUrls.Controls.Remove((Control) sender);
        }
        this.Height -= 34;
      }
      else if (sender is URLControlWithEntity)
      {
        this.urlDeleted = true;
        this.pnlUrls.Controls.Remove((Control) sender);
        this.BtnSave.Enabled = true;
        ((EditCompanyWebcenterControl) this.caller).IsDirty = true;
      }
      if (this.ChangedEvent == null)
        return;
      this.ChangedEvent(sender, e);
    }

    private void btnAddMore_Click(object sender, EventArgs e)
    {
      if (this.isOrg)
      {
        this.SuspendLayout();
        URLControlWithEntity controlWithEntity = new URLControlWithEntity(this.useDropdown, this.availableURLs, this.pnlUrls.Controls.Count, this.listEntity, selectIndexEntity: this.listEntity.Count > 1 ? 2 : 0);
        controlWithEntity.Size = new Size(this.pnlUrls.Width, controlWithEntity.Height);
        controlWithEntity.DeleteEvent += new URLControlWithEntity.EventHandle(this.urlCtrl_DeleteEvent);
        controlWithEntity.ChangeEvent += new URLControlWithEntity.EventHandle(this.urlCtrl_ChangeEvent);
        controlWithEntity.UrlEnter += new URLControlWithEntity.EventHandle(this.urlCtrl_UrlEnter);
        controlWithEntity.Leave += new EventHandler(this.urlCtrl_Leave);
        controlWithEntity.UrlTextChanged += new URLControlWithEntity.EventHandle(this.urlCtrl_UrlTextChanged);
        controlWithEntity.IsWarningVisible = false;
        this.pnlUrls.Controls.Add((Control) controlWithEntity);
        this.pnlUrls.Controls.SetChildIndex((Control) this.pnlAddMore, this.pnlUrls.Controls.Count);
        this.ResumeLayout();
        this.pnlUrls.ScrollControlIntoView((Control) this.btnAddMore);
      }
      else
      {
        this.Height += 34;
        this.SuspendLayout();
        WebcenterURLCtrl webcenterUrlCtrl = new WebcenterURLCtrl(this.useDropdown, this.availableURLs, this.pnlUrls.Controls.Count);
        webcenterUrlCtrl.Size = new Size(this.pnlUrls.Width, webcenterUrlCtrl.Height);
        webcenterUrlCtrl.DeleteEvent += new WebcenterURLCtrl.EventHandle(this.urlCtrl_DeleteEvent);
        webcenterUrlCtrl.ChangeEvent += new WebcenterURLCtrl.EventHandle(this.urlCtrl_ChangeEvent);
        webcenterUrlCtrl.UrlEnter += new WebcenterURLCtrl.EventHandle(this.urlCtrl_UrlEnter);
        webcenterUrlCtrl.Leave += new EventHandler(this.urlCtrl_Leave);
        webcenterUrlCtrl.TextChange += new WebcenterURLCtrl.EventHandle(this.urlCtrl_UrlTextChanged);
        this.pnlUrls.Controls.Add((Control) webcenterUrlCtrl);
        webcenterUrlCtrl.IsWarningVisible = false;
        this.pnlUrls.Controls.SetChildIndex((Control) this.pnlAddMore, this.pnlUrls.Controls.Count);
        this.ResumeLayout();
      }
    }

    private void urlCtrl_UrlTextChanged(object sender, EventArgs e)
    {
      if (!this.isOrg)
      {
        if (this.ChangedEvent == null)
          return;
        this.ChangedEvent(sender, e);
      }
      else
      {
        this.BtnSave.Enabled = true;
        ((EditCompanyWebcenterControl) this.caller).IsDirty = true;
      }
    }

    private void urlCtrl_Leave(object sender, EventArgs e)
    {
      if (this.isOrg)
      {
        if (this.availableURLs.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == ((URLControlWithEntity) sender).Text)) == null && !this.urlDeleted)
        {
          int num = (int) MessageBox.Show("Your entered URL is not valid or not one of the assigned URL.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          if (this.selectedUrl != null)
            ((URLControlWithEntity) sender).Text = this.selectedUrl.URL;
          else
            ((URLControlWithEntity) sender).Text = "";
        }
      }
      else if (this.availableURLs.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == ((WebcenterURLCtrl) sender).URL)) == null && !this.urlDeleted)
      {
        int num = (int) MessageBox.Show("Your entered URL is not valid or not one of the assigned URL.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        if (this.selectedUrl != null)
          ((WebcenterURLCtrl) sender).URL = this.selectedUrl.URL;
        else
          ((WebcenterURLCtrl) sender).URL = "";
      }
      this.urlDeleted = false;
    }

    private string[] GetSelectedURLs()
    {
      List<string> stringList = new List<string>();
      foreach (Control control in (ArrangedElementCollection) this.pnlUrls.Controls)
      {
        if (control is WebcenterURLCtrl)
          stringList.Add(((WebcenterURLCtrl) control).URL);
      }
      return stringList.ToArray();
    }

    public int[] GetSelectedURLIDs()
    {
      List<int> intList = new List<int>();
      if (!this.isOrg)
      {
        ExternalUserURL[] source = (UserInfo) this.externalUserInfo == (UserInfo) null ? new ExternalUserURL[0] : this.session.ConfigurationManager.GetExternalUserInfoURLs(this.externalUserInfo.ExternalUserID);
        foreach (Control control in (ArrangedElementCollection) this.pnlUrls.Controls)
        {
          Control ctrl = control;
          if (ctrl is WebcenterURLCtrl)
          {
            ExternalOrgURL externalOrgUrl = this.availableURLs.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (y => y.URL == ((WebcenterURLCtrl) ctrl).URL));
            if (externalOrgUrl != null)
            {
              intList.Add(externalOrgUrl.URLID);
            }
            else
            {
              ExternalUserURL externalUserUrl = ((IEnumerable<ExternalUserURL>) source).FirstOrDefault<ExternalUserURL>((Func<ExternalUserURL, bool>) (y => y.URL == ((WebcenterURLCtrl) ctrl).URL));
              if (externalUserUrl != null)
                intList.Add(externalUserUrl.URLID);
            }
          }
        }
      }
      return intList.ToArray();
    }

    private void pnlUrls_Resize(object sender, EventArgs e)
    {
      foreach (Control control in (ArrangedElementCollection) this.pnlUrls.Controls)
        control.Size = new Size(this.pnlUrls.Width, control.Height);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnAddMore = new Button();
      this.pnlAddMore = new Panel();
      this.pnlUrls = new FlowLayoutPanel();
      this.pnlAddMore.SuspendLayout();
      this.pnlUrls.SuspendLayout();
      this.SuspendLayout();
      this.btnAddMore.Location = new Point(3, 3);
      this.btnAddMore.Name = "btnAddMore";
      this.btnAddMore.Size = new Size(75, 23);
      this.btnAddMore.TabIndex = 0;
      this.btnAddMore.Text = "&Add More";
      this.btnAddMore.UseVisualStyleBackColor = true;
      this.btnAddMore.Click += new EventHandler(this.btnAddMore_Click);
      this.pnlAddMore.Controls.Add((Control) this.btnAddMore);
      this.pnlAddMore.Location = new Point(3, 3);
      this.pnlAddMore.Name = "pnlAddMore";
      this.pnlAddMore.Size = new Size(854, 32);
      this.pnlAddMore.TabIndex = 1;
      this.pnlUrls.AutoScroll = true;
      this.pnlUrls.Controls.Add((Control) this.pnlAddMore);
      this.pnlUrls.Dock = DockStyle.Fill;
      this.pnlUrls.FlowDirection = FlowDirection.TopDown;
      this.pnlUrls.Location = new Point(0, 0);
      this.pnlUrls.Name = "pnlUrls";
      this.pnlUrls.Size = new Size(854, 645);
      this.pnlUrls.TabIndex = 7;
      this.pnlUrls.WrapContents = false;
      this.pnlUrls.Resize += new EventHandler(this.pnlUrls_Resize);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.pnlUrls);
      this.Name = nameof (AddWebcenterURLControl);
      this.Size = new Size(854, 645);
      this.pnlAddMore.ResumeLayout(false);
      this.pnlUrls.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
