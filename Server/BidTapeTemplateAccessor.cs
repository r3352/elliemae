// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.BidTapeTemplateAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.BidTapeManagement;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class BidTapeTemplateAccessor
  {
    public static IList<BidTapeField> GetBidTapeFields()
    {
      IList<BidTapeField> bidTapeFields = (IList<BidTapeField>) new List<BidTapeField>();
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("SELECT * FROM BidTapeTemplate");
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet != null)
        {
          if (dataSet.Tables.Count > 0)
          {
            if (dataSet.Tables[0].Rows.Count > 0)
            {
              foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
              {
                string str1 = Convert.ToString(row["BidTapeTemplateGuid"]);
                int int32 = Convert.ToInt32(row["SequenceNumber"]);
                string str2 = Convert.ToString(row["BidTapeFieldName"]);
                string str3 = Convert.ToString(row["TargetType"]);
                string str4 = Convert.ToString(row["TargetField"]);
                string str5 = Convert.ToString(row["LookupEnumerationIndicator"]);
                string str6 = Convert.ToString(row["ErrorIndicator"]);
                bool boolean1 = Convert.ToBoolean(row["UserRequiredFieldIndicator"]);
                bool boolean2 = Convert.ToBoolean(row["AppRequiredFieldIndicator"]);
                BidTapeField bidTapeField = new BidTapeField()
                {
                  BidTapeTemplateGuid = str1,
                  SequenceNumber = int32,
                  BidTapeFieldName = str2,
                  TargetTypeStr = str3,
                  TargetField = str4,
                  LookupEnumerationIndicatorStr = str5,
                  ErrorIndicatorStr = str6,
                  UserRequiredFieldIndicator = boolean1,
                  AppRequiredFieldIndicator = boolean2
                };
                bidTapeFields.Add(bidTapeField);
              }
              return bidTapeFields;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BidTapeTemplateAccessor), ex);
      }
      return bidTapeFields;
    }

    public static void UpdateBidTapeFields(IList<BidTapeField> fields)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("IF EXISTS (SELECT 1 FROM BidTapeTemplate)");
        stringBuilder.AppendLine("BEGIN");
        stringBuilder.AppendLine("DELETE FROM BidTapeTemplate");
        stringBuilder.AppendLine("END");
        foreach (BidTapeField field in (IEnumerable<BidTapeField>) fields)
          stringBuilder.AppendLine("INSERT INTO BidTapeTemplate (BidTapeTemplateGuid, SequenceNumber, BidTapeFieldName, TargetType, TargetField, LookupEnumerationIndicator, ErrorIndicator, UserRequiredFieldIndicator, AppRequiredFieldIndicator) VALUES (" + SQL.Encode((object) field.BidTapeTemplateGuid) + ", " + SQL.Encode((object) field.SequenceNumber) + ", " + SQL.Encode((object) field.BidTapeFieldName) + ", " + SQL.Encode((object) field.TargetTypeStr) + ", " + SQL.Encode((object) field.TargetField) + ", " + SQL.Encode((object) field.LookupEnumerationIndicatorStr) + ", " + SQL.Encode((object) field.ErrorIndicatorStr) + ", " + SQL.EncodeFlag(field.UserRequiredFieldIndicator) + ", " + SQL.EncodeFlag(field.AppRequiredFieldIndicator) + ")");
        dbQueryBuilder.Append(stringBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BidTapeTemplateAccessor), ex);
      }
    }
  }
}
