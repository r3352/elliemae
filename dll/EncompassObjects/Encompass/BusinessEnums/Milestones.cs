// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.Milestones
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Workflow;
using EllieMae.Encompass.Client;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class Milestones : EnumBase, IMilestones
  {
    private Session session;
    private Hashtable coreMilestones = new Hashtable();
    private Milestone started;
    private Milestone processing;
    private Milestone submittal;
    private Milestone approval;
    private Milestone docSigning;
    private Milestone funding;
    private Milestone completion;

    internal Milestones(Session session)
    {
      this.session = session;
      IConfigurationManager iconfigurationManager = (IConfigurationManager) session.GetObject("ConfigurationManager");
      List<Milestone> milestones = this.session.SessionObjects.StartupInfo.Milestones;
      for (int index = 0; index <= milestones.Count - 1; ++index)
      {
        Milestone milestone = new Milestone(index + 1, milestones[index].Name, milestones[index].Archived, milestones[index].MilestoneID);
        this.AddItem((EnumItem) milestone);
        this.coreMilestones.Add((object) milestone.InternalName, (object) milestone);
      }
      Milestone prev = (Milestone) null;
      for (int index = 0; index < this.Count; ++index)
      {
        Milestone next = this[index];
        if (!next.IsArchived)
        {
          if ((EnumItem) prev != (EnumItem) null)
          {
            next.SetPreviousMilestone(prev);
            prev.SetNextMilestone(next);
          }
          prev = next;
        }
      }
      this.InitCoreMilestones();
    }

    private void InitCoreMilestones()
    {
      foreach (Milestone milestone in (EnumBase) this)
      {
        switch (milestone.Name.ToUpper())
        {
          case "APPROVAL":
            this.approval = milestone;
            continue;
          case "COMPLETION":
            this.completion = milestone;
            continue;
          case "DOCS SIGNING":
            this.docSigning = milestone;
            continue;
          case "FUNDING":
            this.funding = milestone;
            continue;
          case "PROCESSING":
            this.processing = milestone;
            continue;
          case "STARTED":
            this.started = milestone;
            continue;
          case "SUBMITTAL":
            this.submittal = milestone;
            continue;
          default:
            continue;
        }
      }
    }

    public Milestone First => this[0];

    public Milestone this[int index] => (Milestone) this.GetItem(index);

    public Milestone GetItemByID(int itemId) => (Milestone) base.GetItemByID(itemId);

    public Milestone GetItemByName(string name) => (Milestone) base.GetItemByName(name);

    public Milestone GetItemByMilestoneID(string milestoneID)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        if (this[index].MilestoneID == milestoneID)
          return this[index];
      }
      return (Milestone) null;
    }

    public Milestone Started
    {
      get
      {
        return !((EnumItem) this.started == (EnumItem) null) ? this.started : throw new Exception("Started Milestone not found.");
      }
    }

    public Milestone Processing
    {
      get
      {
        return !((EnumItem) this.processing == (EnumItem) null) ? this.processing : throw new Exception("Processing Milestone not found.");
      }
    }

    public Milestone Submittal
    {
      get
      {
        return !((EnumItem) this.submittal == (EnumItem) null) ? this.submittal : throw new Exception("Submittal Milestone not found.");
      }
    }

    public Milestone Approval
    {
      get
      {
        return !((EnumItem) this.approval == (EnumItem) null) ? this.approval : throw new Exception("Approval Milestone not found.");
      }
    }

    public Milestone DocsSigning
    {
      get
      {
        return !((EnumItem) this.docSigning == (EnumItem) null) ? this.docSigning : throw new Exception("Docs Signing Milestone not found.");
      }
    }

    public Milestone Funding
    {
      get
      {
        return !((EnumItem) this.funding == (EnumItem) null) ? this.funding : throw new Exception("Funding Milestone not found.");
      }
    }

    public Milestone Completion
    {
      get
      {
        return !((EnumItem) this.completion == (EnumItem) null) ? this.completion : throw new Exception("Completion Milestone not found.");
      }
    }
  }
}
