// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.MilestoneLabel
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class MilestoneLabel : Element
  {
    private const int imageSpacing = 5;
    private string msName;
    private string displayName;
    private Element iconElement;
    private HorizontalAlignment iconAlignment;
    private Font font;
    private CoreMilestoneID coreMsID = CoreMilestoneID.Started;
    private EllieMae.EMLite.Workflow.Milestone milestone;

    public MilestoneLabel(EllieMae.EMLite.Workflow.Milestone milestone, PipelineElementData data)
      : base((object) data)
    {
      this.milestone = milestone;
      this.msName = data.ToString();
      this.displayName = MilestoneLabel.getDisplayNameForMilestone(data);
      this.iconElement = (Element) this.getIconElementForMilestone(data);
    }

    public MilestoneLabel(CoreMilestoneID coreMsId, HorizontalAlignment iconAlignment)
    {
      this.msName = CoreMilestoneIDEnumUtil.ValueToName(coreMsId);
      this.displayName = CoreMilestoneIDEnumUtil.GetDisplayName(coreMsId);
      this.iconElement = (Element) new ImageElement(MilestoneLabel.GetImageForMilestone(coreMsId, false));
      this.iconAlignment = iconAlignment;
      this.coreMsID = coreMsId;
    }

    public MilestoneLabel(
      CustomMilestone ms,
      CoreMilestoneID coreMsId,
      HorizontalAlignment iconAlignment)
    {
      this.msName = ms.MilestoneName;
      this.displayName = ms.DisplayName;
      if (ms.Archived)
        this.displayName += " (Archived)";
      this.iconElement = (Element) new ImageElement(MilestoneLabel.GetImageForMilestone(coreMsId, true));
      this.iconAlignment = iconAlignment;
      this.coreMsID = coreMsId;
    }

    public MilestoneLabel(EllieMae.EMLite.Workflow.Milestone milestone)
    {
      this.displayName = this.msName = milestone.Name;
      if (milestone.Archived)
        this.displayName += " (Archived)";
      this.iconElement = (Element) new GradientIconElement(milestone.DisplayColor, new Size(12, 12));
      this.coreMsID = CoreMilestoneID.Started;
    }

    public CoreMilestoneID CoreMsID => this.coreMsID;

    public Font Font
    {
      get => this.font;
      set => this.font = value;
    }

    public override Rectangle Draw(ItemDrawArgs e)
    {
      if ((this.displayName ?? "") == "")
        return Rectangle.Empty;
      if (this.font != null)
        e = e.ChangeFont(this.font);
      return new MultiValueElement(this.iconElement, (Element) new TextElement(this.displayName), 5, LayoutDirection.Horizontal).Draw(e);
    }

    public override Size Measure(ItemDrawArgs e)
    {
      if ((this.displayName ?? "") == "")
        return Size.Empty;
      if (this.iconAlignment == HorizontalAlignment.Right)
        return e.Bounds.Size;
      if (this.font != null)
        e = e.ChangeFont(this.font);
      return new MultiValueElement(this.iconElement, (Element) new TextElement(this.displayName), 5, LayoutDirection.Horizontal).Measure(e);
    }

    public string DisplayName
    {
      get => this.displayName;
      set => this.displayName = value;
    }

    public HorizontalAlignment IconAlignment => this.iconAlignment;

    public string MilestoneName => this.msName;

    public override string ToString() => this.displayName;

    private static string getDisplayNameForMilestone(PipelineElementData data)
    {
      MilestoneLabel.getMilestoneID(data);
      return data.ToString();
    }

    private static string getMilestoneID(PipelineElementData data)
    {
      if (string.Compare(data.FieldName, "NextMilestone.MilestoneName", true) == 0 || string.Compare(data.FieldName, "Loan.NextMilestoneName", true) == 0)
        return string.Concat(data.PipelineInfo.GetField("Loan.NextMilestoneID"));
      return string.Compare(data.FieldName, "Loan.CurrentCoreMilestoneName", true) == 0 ? string.Concat(data.PipelineInfo.GetField("CurrentCoreMilestone.MilestoneID")) : string.Concat(data.PipelineInfo.GetField("Loan.CurrentMilestoneID"));
    }

    private GradientIconElement getIconElementForMilestone(PipelineElementData data)
    {
      return MilestoneLabel.getMilestoneID(data) == "" ? (GradientIconElement) null : this.GetImageForMilestone();
    }

    public static Image GetImageForMilestone(CoreMilestoneID coreMsId, bool isCustom)
    {
      return isCustom ? MilestoneLabel.GetCustomMilestoneImage(coreMsId) : MilestoneLabel.GetCoreMilestoneImage(coreMsId);
    }

    public GradientIconElement GetImageForMilestone()
    {
      return this.milestone == null ? (GradientIconElement) null : new GradientIconElement(this.milestone.DisplayColor, new Size(12, 12));
    }

    public static Image GetCoreMilestoneImage(CoreMilestoneID msid)
    {
      switch (msid)
      {
        case CoreMilestoneID.Started:
          return (Image) Resources.milestone_started;
        case CoreMilestoneID.Processing:
          return (Image) Resources.milestone_processing;
        case CoreMilestoneID.Submitted:
          return (Image) Resources.milestone_submittal;
        case CoreMilestoneID.Approved:
          return (Image) Resources.milestone_approval;
        case CoreMilestoneID.DocSigned:
          return (Image) Resources.milestone_doc_signing;
        case CoreMilestoneID.Funded:
          return (Image) Resources.milestone_funding;
        case CoreMilestoneID.Completed:
          return (Image) Resources.milestone_completion;
        default:
          throw new ArgumentException("Invalid core milestone ID specified");
      }
    }

    public static Image GetCustomMilestoneImage(CoreMilestoneID msid)
    {
      switch (msid)
      {
        case CoreMilestoneID.Started:
          return (Image) Resources.milestone_started_custom;
        case CoreMilestoneID.Processing:
          return (Image) Resources.milestone_processing_custom;
        case CoreMilestoneID.Submitted:
          return (Image) Resources.milestone_submittal_custom;
        case CoreMilestoneID.Approved:
          return (Image) Resources.milestone_approval_custom;
        case CoreMilestoneID.DocSigned:
          return (Image) Resources.milestone_doc_signing_custom;
        case CoreMilestoneID.Funded:
          return (Image) Resources.milestone_funding_custom;
        case CoreMilestoneID.Completed:
          return (Image) Resources.milestone_completion;
        default:
          throw new ArgumentException("Invalid core milestone ID specified");
      }
    }
  }
}
