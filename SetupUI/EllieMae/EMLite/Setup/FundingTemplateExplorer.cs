// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FundingTemplateExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FundingTemplateExplorer : SimpleTemplateExplorer
  {
    private Sessions.Session session;

    public FundingTemplateExplorer()
      : this(Session.DefaultInstance, false)
    {
    }

    public FundingTemplateExplorer(Sessions.Session session, bool allowMultiSelect)
      : base(session, allowMultiSelect)
    {
      this.session = session;
    }

    protected override TemplateSettingsType TemplateType => TemplateSettingsType.FundingTemplate;

    protected override string HeaderText => "Funding Templates";

    protected override void ConfigureTemplateListView(GridView listView)
    {
      listView.Columns[0].Width = (int) ((double) listView.Size.Width * 0.6);
      GVColumn gvColumn = listView.Columns.Add("2015 Itemization", 110);
      gvColumn.TextAlign = HorizontalAlignment.Center;
      gvColumn.Tag = (object) "For2010GFE";
      listView.Columns.Add("Description", 250).Tag = (object) "Description";
      listView.Sort(0, SortOrder.Ascending);
    }

    protected override bool CreateNew()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      FundingTemplate fundingTemplate = (FundingTemplate) null;
      using (CreateNewTemplateDialog newTemplateDialog = new CreateNewTemplateDialog(true))
      {
        if (newTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return false;
        fundingTemplate = new FundingTemplate();
        fundingTemplate.For2010GFE = newTemplateDialog.Use2010;
        fundingTemplate.RESPAVersion = newTemplateDialog.Use2015 ? "2015" : (newTemplateDialog.Use2010 ? "2010" : "2009");
      }
      using (TemplateDialog templateDialog = new TemplateDialog((FieldDataTemplate) fundingTemplate, TemplateSettingsType.FundingTemplate, true, !aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Company_SecondarySetup), this.session))
      {
        if (fundingTemplate.RESPAVersion == "2015")
          templateDialog.LoadForm("Funding Template", "FundingTemplate2015");
        else if (fundingTemplate.RESPAVersion == "2010" || fundingTemplate.For2010GFE)
          templateDialog.LoadForm("Funding Template", "FundingTemplate2010");
        else
          templateDialog.LoadForm("Funding Template", "FundingTemplateForm");
        DialogResult dialogResult = templateDialog.ShowDialog();
        if (dialogResult == DialogResult.OK)
        {
          try
          {
            this.session.ConfigurationManager.SaveTemplateSettings(TemplateSettingsType.FundingTemplate, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, fundingTemplate.TemplateName, FileSystemEntry.Types.File, (string) null), (BinaryObject) (BinaryConvertibleObject) fundingTemplate);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Can't save Funding template. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
        return dialogResult == DialogResult.OK;
      }
    }

    protected override bool Edit(BinaryObject template)
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      FundingTemplate fundingTemplate = (FundingTemplate) template;
      string templateName = fundingTemplate.TemplateName;
      using (TemplateDialog templateDialog = new TemplateDialog((FieldDataTemplate) fundingTemplate, TemplateSettingsType.FundingTemplate, true, !aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Company_SecondarySetup), this.session))
      {
        if (fundingTemplate.RESPAVersion == "2015")
          templateDialog.LoadForm("Funding Template", "FundingTemplate2015");
        else if (fundingTemplate.RESPAVersion == "2010" || fundingTemplate.For2010GFE)
          templateDialog.LoadForm("Funding Template", "FundingTemplate2010");
        else
          templateDialog.LoadForm("Funding Template", "FundingTemplateForm");
        DialogResult dialogResult = templateDialog.ShowDialog();
        if (dialogResult == DialogResult.OK)
        {
          if (templateName != "")
          {
            if (templateName != fundingTemplate.TemplateName)
            {
              try
              {
                this.session.ConfigurationManager.DeleteTemplateSettingsObject(TemplateSettingsType.FundingTemplate, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, templateName, FileSystemEntry.Types.File, (string) null));
              }
              catch (Exception ex)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "Can't delete old Funding template '" + templateName + "'. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
            }
          }
          try
          {
            this.session.ConfigurationManager.SaveTemplateSettings(TemplateSettingsType.FundingTemplate, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, fundingTemplate.TemplateName, FileSystemEntry.Types.File, (string) null), (BinaryObject) (BinaryConvertibleObject) fundingTemplate);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Can't save Funding template. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
        return dialogResult == DialogResult.OK;
      }
    }

    protected override BinaryObject Duplicate(BinaryObject template)
    {
      string str = "";
      FundingTemplate fundingTemplate1 = (FundingTemplate) template;
      if (fundingTemplate1.RESPAVersion == "2015" || fundingTemplate1.RESPAVersion == "2010" || fundingTemplate1.For2010GFE)
      {
        using (CreateNewTemplateDialog newTemplateDialog = new CreateNewTemplateDialog(true))
        {
          if (newTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return (BinaryObject) null;
          str = newTemplateDialog.Use2015 ? "2015" : (newTemplateDialog.Use2010 ? "2010" : "2009");
        }
      }
      BinaryObject binaryObject = (BinaryObject) (BinaryConvertibleObject) fundingTemplate1.Duplicate();
      if (str != "")
      {
        FundingTemplate fundingTemplate2 = (FundingTemplate) binaryObject;
        fundingTemplate2.For2010GFE = str == "2010";
        fundingTemplate2.RESPAVersion = str;
        binaryObject = (BinaryObject) (BinaryConvertibleObject) fundingTemplate2;
      }
      return binaryObject;
    }
  }
}
