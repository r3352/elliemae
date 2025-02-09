// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Workflow.MilestoneTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Workflow
{
  [Serializable]
  public class MilestoneTemplate : IEnumerable<TemplateMilestone>, IEnumerable
  {
    private string templateId;
    private string name;
    private int sortIndex;
    private bool active;
    private MilestoneTemplate.TemplateMilestones sequentialMilestones;
    private MilestoneTemplate.TemplateFreeRoles freeRoles;
    private string comment;
    private string updateBorrowerContactMilestoneID;
    private string autoLoanNumberingMilestoneID;
    public const string AutoLoanNumberDefaultMilestoneKey = "POLICIES.LOANNUMBER";
    public const string eDisclosureDefaultMilestoneKey = "EDISCLOSURE";
    private Dictionary<string, string> eDisclosureMilestoneSettings = new Dictionary<string, string>();
    private Guid templateGuid;

    public MilestoneTemplate()
    {
      this.templateId = Guid.NewGuid().ToString("B");
      this.templateGuid = Guid.NewGuid();
      this.name = "";
      this.sequentialMilestones = new MilestoneTemplate.TemplateMilestones(this);
      this.freeRoles = new MilestoneTemplate.TemplateFreeRoles(this);
      this.sortIndex = -1;
      this.active = false;
      this.comment = "";
      this.Add(new TemplateMilestone("1", 1, 0));
      this.Add(new TemplateMilestone("7", 2, 0));
    }

    public MilestoneTemplate(
      string templateId,
      string name,
      int sortIndex,
      bool active,
      string comment,
      string updateBorrowerContactMilestoneID,
      string autoLoanNumberingMilestoneID,
      Dictionary<string, string> eDisclosureMilestoneSettings,
      Guid templateGuid)
    {
      this.templateId = templateId;
      this.name = name;
      this.sequentialMilestones = new MilestoneTemplate.TemplateMilestones(this);
      this.freeRoles = new MilestoneTemplate.TemplateFreeRoles(this);
      this.sortIndex = sortIndex;
      this.active = active;
      this.comment = comment;
      this.updateBorrowerContactMilestoneID = updateBorrowerContactMilestoneID;
      this.autoLoanNumberingMilestoneID = autoLoanNumberingMilestoneID;
      this.eDisclosureMilestoneSettings = eDisclosureMilestoneSettings;
      this.templateGuid = templateGuid;
    }

    public MilestoneTemplate(XmlElement e, XmlNodeList milestones, XmlNodeList freeRolesXml)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.templateId = attributeReader.GetString("MilestoneTemplateID");
      this.name = attributeReader.GetString("MilestoneTemplateName");
      this.comment = attributeReader.GetString(nameof (Comment));
      this.updateBorrowerContactMilestoneID = attributeReader.GetString("BorrowerContactUpdate");
      this.autoLoanNumberingMilestoneID = attributeReader.GetString("AutoLoanNumbering");
      this.sequentialMilestones = new MilestoneTemplate.TemplateMilestones(this);
      foreach (XmlElement milestone in milestones)
        this.sequentialMilestones.Insert(new TemplateMilestone(milestone));
      this.freeRoles = new MilestoneTemplate.TemplateFreeRoles(this);
      if (freeRolesXml == null)
        return;
      foreach (XmlElement e1 in freeRolesXml)
        this.freeRoles.Insert(new TemplateFreeRole(e1));
    }

    public string TemplateID => this.templateId;

    public Guid TemplateGuid => this.templateGuid;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public int SortIndex
    {
      get => this.sortIndex;
      set => this.sortIndex = value;
    }

    public string Comment
    {
      get => this.comment;
      set => this.comment = value;
    }

    public bool Active
    {
      get => this.active;
      set => this.active = value;
    }

    public string UpdateBorrowerContactMilestoneID
    {
      get => this.updateBorrowerContactMilestoneID;
      set => this.updateBorrowerContactMilestoneID = value;
    }

    public string AutoLoanNumberingMilestoneID
    {
      get => this.autoLoanNumberingMilestoneID;
      set => this.autoLoanNumberingMilestoneID = value;
    }

    public Dictionary<string, string> EDisclosureMilestoneSettings
    {
      get => this.eDisclosureMilestoneSettings;
      set => this.eDisclosureMilestoneSettings = value;
    }

    public MilestoneTemplate.TemplateMilestones SequentialMilestones => this.sequentialMilestones;

    public MilestoneTemplate.TemplateFreeRoles FreeRoles => this.freeRoles;

    public bool IsDefaultTemplate => this.Name.Equals("Default Template");

    public bool Contains(string milestoneId)
    {
      foreach (TemplateMilestone templateMilestone in this)
      {
        if (string.Compare(templateMilestone.MilestoneID, milestoneId, true) == 0)
          return true;
      }
      return false;
    }

    public bool ContainsRole(int roleId)
    {
      foreach (TemplateMilestone templateMilestone in this)
      {
        if (templateMilestone.RoleID == roleId)
          return true;
      }
      foreach (TemplateFreeRole freeRole in this.freeRoles)
      {
        if (freeRole.RoleID == roleId)
          return true;
      }
      return false;
    }

    public bool Equals(MilestoneTemplate templateTwo)
    {
      return this.TemplateID.Equals(templateTwo.TemplateID);
    }

    public void Add(TemplateMilestone msTemplate)
    {
      if (this.Contains(msTemplate.MilestoneID))
        throw new ArgumentException("The specified milestone is already in the template.");
      this.sequentialMilestones.Insert(msTemplate);
    }

    public void Add(TemplateFreeRole freeRole)
    {
      if (this.ContainsRole(freeRole.RoleID))
        return;
      this.freeRoles.Insert(freeRole);
    }

    public void Clear() => this.sequentialMilestones.Clear();

    public IEnumerator<TemplateMilestone> GetEnumerator()
    {
      List<TemplateMilestone> templateMilestoneList = new List<TemplateMilestone>();
      templateMilestoneList.AddRange((IEnumerable<TemplateMilestone>) this.sequentialMilestones);
      return (IEnumerator<TemplateMilestone>) templateMilestoneList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void ToXml(XmlElement e, bool MSLock, bool MSDateLock)
    {
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("MilestoneTemplateID", (object) this.templateId);
      attributeWriter.Write("MilestoneTemplateName", (object) this.name);
      attributeWriter.Write("IsTemplateLocked", MSLock ? (object) "Y" : (object) "N");
      attributeWriter.Write("IsTemplateDatesLocked", MSDateLock ? (object) "Y" : (object) "N");
      attributeWriter.Write("Comment", (object) this.comment);
    }

    [Serializable]
    public class TemplateMilestones : IEnumerable<TemplateMilestone>, IEnumerable
    {
      private MilestoneTemplate parentTemplate;
      private List<TemplateMilestone> milestones = new List<TemplateMilestone>();

      internal TemplateMilestones(MilestoneTemplate parentTemplate)
      {
        this.parentTemplate = parentTemplate;
      }

      public TemplateMilestone this[int index] => this.milestones[index];

      public int Count() => this.milestones.Count;

      public TemplateMilestone Append(Milestone milestone, int daysToComplete)
      {
        this.ensureNotExists(milestone.MilestoneID);
        TemplateMilestone templateMilestone = new TemplateMilestone(milestone.MilestoneID, this.milestones.Count, daysToComplete);
        this.milestones.Add(templateMilestone);
        return templateMilestone;
      }

      public TemplateMilestone Insert(int index, Milestone milestone)
      {
        return this.Insert(new TemplateMilestone(milestone.MilestoneID, index, -1));
      }

      public TemplateMilestone Insert(TemplateMilestone ms)
      {
        this.ensureNotExists(ms.MilestoneID);
        this.milestones.Add(ms);
        return ms;
      }

      public TemplateMilestone InsertInBetween(int index, Milestone milestone)
      {
        this.ensureNotExists(milestone.MilestoneID);
        TemplateMilestone templateMilestone = new TemplateMilestone(milestone.MilestoneID, this.milestones.Count, milestone.DefaultDays);
        if (index > this.milestones.Count - 1)
          this.milestones.Insert(this.milestones.Count - 1, templateMilestone);
        else
          this.milestones.Insert(index, templateMilestone);
        this.reindexMilestones();
        return templateMilestone;
      }

      public void Remove(TemplateMilestone milestone)
      {
        this.milestones.Remove(milestone);
        this.reindexMilestones();
      }

      public void Clear() => this.milestones.Clear();

      public TemplateMilestone GetMilestone(string milestoneId)
      {
        foreach (TemplateMilestone milestone in this.milestones)
        {
          if (string.Compare(milestone.MilestoneID, milestoneId, true) == 0)
            return milestone;
        }
        return (TemplateMilestone) null;
      }

      public IEnumerator<TemplateMilestone> GetEnumerator()
      {
        return (IEnumerator<TemplateMilestone>) this.milestones.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.milestones.GetEnumerator();

      private void reindexMilestones()
      {
        for (int index = 0; index < this.milestones.Count; ++index)
          this.milestones[index].SetSortIndex(index + 1);
      }

      private void ensureNotExists(string milestoneId)
      {
        if (this.parentTemplate.Contains(milestoneId))
          throw new ArgumentException("The specified milestone is already in the template.");
      }
    }

    [Serializable]
    public class TemplateFreeRoles : IEnumerable<TemplateFreeRole>, IEnumerable
    {
      private MilestoneTemplate parentTemplate;
      private List<TemplateFreeRole> roles = new List<TemplateFreeRole>();

      internal TemplateFreeRoles(MilestoneTemplate parentTemplate)
      {
        this.parentTemplate = parentTemplate;
      }

      public TemplateFreeRole this[int index] => this.roles[index];

      public int Count() => this.roles.Count;

      public TemplateFreeRole Append(RoleInfo role, int daysToComplete)
      {
        this.ensureNotExists(role.RoleID);
        TemplateFreeRole templateFreeRole = new TemplateFreeRole(role.RoleID);
        this.roles.Add(templateFreeRole);
        return templateFreeRole;
      }

      public TemplateFreeRole Insert(int index, RoleInfo role)
      {
        return this.Insert(new TemplateFreeRole(role.RoleID));
      }

      public TemplateFreeRole Insert(TemplateFreeRole role)
      {
        this.ensureNotExists(role.RoleID);
        this.roles.Add(role);
        return role;
      }

      public void Remove(TemplateFreeRole role) => this.roles.Remove(role);

      public void Clear() => this.roles.Clear();

      public TemplateFreeRole GetFreeRole(int roleID)
      {
        foreach (TemplateFreeRole role in this.roles)
        {
          if (role.RoleID == roleID)
            return role;
        }
        return (TemplateFreeRole) null;
      }

      public IEnumerator<TemplateFreeRole> GetEnumerator()
      {
        return (IEnumerator<TemplateFreeRole>) this.roles.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.roles.GetEnumerator();

      private void ensureNotExists(int roleID)
      {
        if (this.parentTemplate.ContainsRole(roleID))
          throw new ArgumentException("The specified role is already in the template.");
      }
    }
  }
}
