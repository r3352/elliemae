// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.ComplianceCalendarException
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  [Serializable]
  public class ComplianceCalendarException : Exception
  {
    public ComplianceCalendarException(DateTime startDate, int daysToAdd)
      : base("Date year range for compliance calendar must be between 2000-2029 (Start Date:" + startDate.ToString("MM/dd/yyyy") + ", Days to Add:" + (object) daysToAdd + ").")
    {
    }

    private ComplianceCalendarException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public ComplianceCalendarException(ComplianceCalendarException exception, string fieldID)
      : base("Field " + fieldID + ": " + exception.Message)
    {
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
    }
  }
}
