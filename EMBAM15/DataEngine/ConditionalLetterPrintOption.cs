// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.ConditionalLetterPrintOption
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class ConditionalLetterPrintOption : PurchaseAdviceTemplate
  {
    private const string className = "ConditionalLetterPrintOption";
    private static string sw = Tracing.SwOutsideLoan;

    public ConditionalLetterPrintOption()
    {
      this.ConditionOption = 1;
      this.IncludeDescription = true;
      this.SortBy = 1;
      this.ShowPageNumber = false;
      this.NeedGroup = false;
      this.LetterType = 0;
    }

    public ConditionalLetterPrintOption(XmlSerializationInfo info)
      : base(info)
    {
    }

    public static explicit operator ConditionalLetterPrintOption(BinaryObject obj)
    {
      return (ConditionalLetterPrintOption) BinaryConvertibleObject.Parse(obj, typeof (ConditionalLetterPrintOption));
    }

    public string StartingPages
    {
      get => this.GetField(nameof (StartingPages));
      set => this.SetCurrentField(nameof (StartingPages), value);
    }

    public string EndingPages
    {
      get => this.GetField(nameof (EndingPages));
      set => this.SetCurrentField(nameof (EndingPages), value);
    }

    public int ConditionOption
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (ConditionOption)) == "" ? (object) "0" : (object) this.GetField(nameof (ConditionOption)));
      }
      set => this.SetCurrentField(nameof (ConditionOption), value.ToString());
    }

    public bool IncludeDescription
    {
      get => this.GetField(nameof (IncludeDescription)) == "1";
      set => this.SetCurrentField(nameof (IncludeDescription), value ? "1" : "0");
    }

    public int SortBy
    {
      get
      {
        return Utils.ParseInt(this.GetField("SortedBy") == "" ? (object) "0" : (object) this.GetField("SortedBy"));
      }
      set => this.SetCurrentField("SortedBy", value.ToString());
    }

    public bool ShowPageNumber
    {
      get => this.GetField(nameof (ShowPageNumber)) == "1";
      set => this.SetCurrentField(nameof (ShowPageNumber), value ? "1" : "0");
    }

    public int StartingPageNumber
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (StartingPageNumber)) == "" ? (object) "0" : (object) this.GetField(nameof (StartingPageNumber)));
      }
      set => this.SetCurrentField(nameof (StartingPageNumber), value.ToString());
    }

    public bool NeedGroup
    {
      get => this.GetField(nameof (NeedGroup)) == "1";
      set => this.SetCurrentField(nameof (NeedGroup), value ? "1" : "0");
    }

    public int GroupBy
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (GroupBy)) == "" ? (object) "0" : (object) this.GetField(nameof (GroupBy)));
      }
      set => this.SetCurrentField(nameof (GroupBy), value.ToString());
    }

    public int GroupingPage
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (GroupingPage)) == "" ? (object) "0" : (object) this.GetField(nameof (GroupingPage)));
      }
      set => this.SetCurrentField(nameof (GroupingPage), value.ToString());
    }

    public bool UseDue
    {
      get => this.GetField(nameof (UseDue)) == "1";
      set => this.SetCurrentField(nameof (UseDue), value ? "1" : "0");
    }

    public bool UseCategory
    {
      get => this.GetField(nameof (UseCategory)) == "1";
      set => this.SetCurrentField(nameof (UseCategory), value ? "1" : "0");
    }

    public bool UseOwnerName
    {
      get => this.GetField(nameof (UseOwnerName)) == "1";
      set => this.SetCurrentField(nameof (UseOwnerName), value ? "1" : "0");
    }

    public bool UseCurrentStatus
    {
      get => this.GetField(nameof (UseCurrentStatus)) == "1";
      set => this.SetCurrentField(nameof (UseCurrentStatus), value ? "1" : "0");
    }

    public bool UseStatusAdded
    {
      get => this.GetField(nameof (UseStatusAdded)) == "1";
      set => this.SetCurrentField(nameof (UseStatusAdded), value ? "1" : "0");
    }

    public bool UseStatusFulfilled
    {
      get => this.GetField(nameof (UseStatusFulfilled)) == "1";
      set => this.SetCurrentField(nameof (UseStatusFulfilled), value ? "1" : "0");
    }

    public bool UseStatusReceived
    {
      get => this.GetField(nameof (UseStatusReceived)) == "1";
      set => this.SetCurrentField(nameof (UseStatusReceived), value ? "1" : "0");
    }

    public bool UseStatusReviewed
    {
      get => this.GetField(nameof (UseStatusReviewed)) == "1";
      set => this.SetCurrentField(nameof (UseStatusReviewed), value ? "1" : "0");
    }

    public bool UseStatusRejected
    {
      get => this.GetField(nameof (UseStatusRejected)) == "1";
      set => this.SetCurrentField(nameof (UseStatusRejected), value ? "1" : "0");
    }

    public bool UseStatusCleared
    {
      get => this.GetField(nameof (UseStatusCleared)) == "1";
      set => this.SetCurrentField(nameof (UseStatusCleared), value ? "1" : "0");
    }

    public bool UseStatusWaived
    {
      get => this.GetField(nameof (UseStatusWaived)) == "1";
      set => this.SetCurrentField(nameof (UseStatusWaived), value ? "1" : "0");
    }

    public int StatusCurrentType
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (StatusCurrentType)) == "" ? (object) "0" : (object) this.GetField(nameof (StatusCurrentType)));
      }
      set => this.SetCurrentField(nameof (StatusCurrentType), value.ToString());
    }

    public int StatusAddedType
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (StatusAddedType)) == "" ? (object) "0" : (object) this.GetField(nameof (StatusAddedType)));
      }
      set => this.SetCurrentField(nameof (StatusAddedType), value.ToString());
    }

    public int StatusFulfilledType
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (StatusFulfilledType)) == "" ? (object) "0" : (object) this.GetField(nameof (StatusFulfilledType)));
      }
      set => this.SetCurrentField(nameof (StatusFulfilledType), value.ToString());
    }

    public int StatusReceivedType
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (StatusReceivedType)) == "" ? (object) "0" : (object) this.GetField(nameof (StatusReceivedType)));
      }
      set => this.SetCurrentField(nameof (StatusReceivedType), value.ToString());
    }

    public int StatusReviewedType
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (StatusReviewedType)) == "" ? (object) "0" : (object) this.GetField(nameof (StatusReviewedType)));
      }
      set => this.SetCurrentField(nameof (StatusReviewedType), value.ToString());
    }

    public int StatusRejectedType
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (StatusRejectedType)) == "" ? (object) "0" : (object) this.GetField(nameof (StatusRejectedType)));
      }
      set => this.SetCurrentField(nameof (StatusRejectedType), value.ToString());
    }

    public int StatusClearedType
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (StatusClearedType)) == "" ? (object) "0" : (object) this.GetField(nameof (StatusClearedType)));
      }
      set => this.SetCurrentField(nameof (StatusClearedType), value.ToString());
    }

    public int StatusWaivedType
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (StatusWaivedType)) == "" ? (object) "0" : (object) this.GetField(nameof (StatusWaivedType)));
      }
      set => this.SetCurrentField(nameof (StatusWaivedType), value.ToString());
    }

    public int LetterType
    {
      get
      {
        return Utils.ParseInt(this.GetField(nameof (LetterType)) == "" ? (object) "0" : (object) this.GetField(nameof (LetterType)));
      }
      set => this.SetCurrentField(nameof (LetterType), value.ToString());
    }

    public bool SpaceCondensed
    {
      get => this.GetField(nameof (SpaceCondensed)) != "";
      set => this.SetCurrentField(nameof (SpaceCondensed), value ? "1" : "");
    }

    public bool UseLegalSize
    {
      get => this.GetField(nameof (UseLegalSize)) != "";
      set => this.SetCurrentField(nameof (UseLegalSize), value ? "1" : "");
    }
  }
}
