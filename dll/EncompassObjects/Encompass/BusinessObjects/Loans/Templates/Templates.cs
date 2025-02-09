// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.Templates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class Templates : SessionBoundObject, ITemplates
  {
    private DocumentTemplates documentTemplates;
    private UnderwritingConditionTemplates uwConditionTemplates;
    private PostClosingConditionTemplates pcConditionTemplates;
    private TaskTemplates taskTemplates;

    internal Templates(Session session)
      : base(session)
    {
    }

    public Template GetTemplate(TemplateType templateType, string path)
    {
      return this.GetTemplate(templateType, TemplateEntry.Parse(path, this.Session.UserID));
    }

    public Template GetTemplate(TemplateType templateType, TemplateEntry entry)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      FileSystemEntry fileSystemEntry = entry.Unwrap();
      BinaryObject templateSettings = iconfigurationManager.GetTemplateSettings((TemplateSettingsType) templateType, fileSystemEntry);
      if (templateSettings == null)
        return (Template) null;
      switch (templateType)
      {
        case TemplateType.LoanProgram:
          return (Template) new LoanProgram(this.Session, entry, LoanProgram.op_Explicit(templateSettings));
        case TemplateType.ClosingCost:
          return (Template) new ClosingCost(this.Session, entry, ClosingCost.op_Explicit(templateSettings));
        case TemplateType.DataTemplate:
          return (Template) new DataTemplate(this.Session, entry, DataTemplate.op_Explicit(templateSettings));
        case TemplateType.InputFormSet:
          return (Template) new InputFormSet(this.Session, entry, FormTemplate.op_Explicit(templateSettings));
        case TemplateType.DocumentSet:
          return (Template) new DocumentSet(this.Session, entry, DocumentSetTemplate.op_Explicit(templateSettings));
        case TemplateType.LoanTemplate:
          return (Template) new LoanTemplate(this.Session, entry, LoanTemplate.op_Explicit(templateSettings));
        case TemplateType.Investor:
          return (Template) new Investor(this.Session, entry, InvestorTemplate.op_Explicit(templateSettings));
        case TemplateType.TaskSet:
          return (Template) new TaskSet(this.Session, entry, TaskSetTemplate.op_Explicit(templateSettings));
        default:
          throw new ArgumentException("Unsupported template type specified");
      }
    }

    public TemplateEntryList GetTemplateFolderContents(
      TemplateType templateType,
      TemplateEntry folderEntry)
    {
      if (folderEntry == null)
        throw new ArgumentNullException(nameof (folderEntry));
      if (folderEntry.EntryType != TemplateEntryType.Folder)
        throw new ArgumentException("The specified folderEntry represents a template instead of a folder.");
      FileSystemEntry[] templateDirEntries = this.Session.SessionObjects.ConfigurationManager.GetFilteredTemplateDirEntries((TemplateSettingsType) templateType, folderEntry.Unwrap());
      TemplateEntryList templateFolderContents = new TemplateEntryList();
      foreach (FileSystemEntry fsEntry in templateDirEntries)
        templateFolderContents.Add(new TemplateEntry(fsEntry));
      return templateFolderContents;
    }

    public DocumentTemplates Documents
    {
      get
      {
        lock (this)
        {
          if (this.documentTemplates == null)
            this.documentTemplates = new DocumentTemplates(this.Session);
        }
        return this.documentTemplates;
      }
    }

    public TaskTemplates Tasks
    {
      get
      {
        lock (this)
        {
          if (this.taskTemplates == null)
            this.taskTemplates = new TaskTemplates(this.Session);
        }
        return this.taskTemplates;
      }
    }

    public UnderwritingConditionTemplates UnderwritingConditions
    {
      get
      {
        lock (this)
        {
          if (this.uwConditionTemplates == null)
            this.uwConditionTemplates = new UnderwritingConditionTemplates(this.Session);
        }
        return this.uwConditionTemplates;
      }
    }

    public PostClosingConditionTemplates PostClosingConditions
    {
      get
      {
        lock (this)
        {
          if (this.pcConditionTemplates == null)
            this.pcConditionTemplates = new PostClosingConditionTemplates(this.Session);
        }
        return this.pcConditionTemplates;
      }
    }
  }
}
