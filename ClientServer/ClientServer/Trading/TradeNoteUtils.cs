// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Trading.TradeNoteUtils
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Trading
{
  public class TradeNoteUtils
  {
    private const string DATE_SPLIT_PATTERN = "(\\d{1,2}/\\d{1,2}/\\d{4} \\d{1,2}:\\d{1,2} [A-Z]{2,})�";

    public static string SerializeTradeNotes(
      string tradeNotes,
      string userId,
      string userName,
      SessionObjects session)
    {
      UserInfo[] allUsers = session?.OrganizationManager.GetAllUsers();
      List<TradeNoteModel> tradeNoteModelList = new List<TradeNoteModel>();
      try
      {
        string[] strArray1 = Regex.Split(tradeNotes, "(\\d{1,2}/\\d{1,2}/\\d{4} \\d{1,2}:\\d{1,2} [A-Z]{2,} \\([A-Z]{3,}\\))");
        int index1 = 2;
        int index2 = 0;
        if (((IEnumerable<string>) strArray1[0].Split('>')).Count<string>() == 1 && strArray1[0].Trim().Length != 0)
        {
          tradeNoteModelList.Add(TradeNoteUtils.generateNoteModel(index2, strArray1[0], Utils.CreateTimestamp(true), userId, userName));
          ++index2;
        }
        for (; strArray1.Length - 1 >= index1; index1 += 2)
        {
          string[] strArray2 = strArray1[index1].Split('>');
          if (!string.IsNullOrEmpty(strArray2[0].Trim()))
            userName = strArray2[0].Trim();
          if (strArray2.Length == 2)
          {
            UserInfo userInfo = (UserInfo) null;
            if (allUsers != null)
              userInfo = ((IEnumerable<UserInfo>) allUsers).Where<UserInfo>((Func<UserInfo, bool>) (user => user.userName.Equals(userName) || user.FullName.Equals(userName))).FirstOrDefault<UserInfo>();
            tradeNoteModelList.Add(TradeNoteUtils.generateNoteModel(index2, strArray2[1].Trim(), strArray1[index1 - 1], userInfo != (UserInfo) null ? userInfo.Userid : userId, userName));
            ++index2;
          }
        }
        return JsonConvert.SerializeObject((object) tradeNoteModelList);
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    private static TradeNoteModel generateNoteModel(
      int index,
      string noteText,
      string dateToSplit,
      string userId,
      string userName)
    {
      TradeNoteModel noteModel = new TradeNoteModel();
      noteModel.Id = index;
      noteModel.Details = noteText;
      string[] strArray = Regex.Split(dateToSplit, "(\\d{1,2}/\\d{1,2}/\\d{4} \\d{1,2}:\\d{1,2} [A-Z]{2,})");
      DateTime result;
      DateTime.TryParse(strArray[1], out result);
      noteModel.CreatedTimeStamp = result != DateTime.MinValue ? TradeNoteUtils.GetDate(strArray[2], result, "to") : new DateTime?();
      noteModel.TimezoneAbbrev = strArray[2].Trim().Substring(1, strArray[2].Length - 3);
      noteModel.CreateBy = new EntityReference()
      {
        EntityId = userId,
        EntityName = userName,
        EntityType = EntityRefTypeContract.User
      };
      return noteModel;
    }

    public static TradeNoteModel[] DeserializeTradeNotes(string tradeNotesJson)
    {
      TradeNoteModel[] result = (TradeNoteModel[]) null;
      TradeNoteUtils.tryParseJson(tradeNotesJson, out result);
      return result;
    }

    public static string getTimestampFormat(DateTime createdDate, string timezoneAbbrev)
    {
      if (string.IsNullOrEmpty(timezoneAbbrev))
        timezoneAbbrev = Utils.GetTimezoneAbbrev(TimeZone.CurrentTimeZone);
      return createdDate.ToString("MM/dd/yyyy h:mm tt") + " (" + timezoneAbbrev + ")";
    }

    public static DateTime? GetDate(
      string timeZoneAbbrevation,
      DateTime sourceDateTime,
      string utcConversion)
    {
      DateTime? date = new DateTime?(new DateTime());
      Dictionary<string, System.TimeZoneInfo> source = new Dictionary<string, System.TimeZoneInfo>()
      {
        {
          "PST",
          System.TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")
        },
        {
          "MST",
          System.TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time")
        },
        {
          "CST",
          System.TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")
        },
        {
          "EST",
          System.TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
        },
        {
          "HST",
          System.TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time")
        },
        {
          "AST",
          System.TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time")
        },
        {
          "USMST",
          System.TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time")
        },
        {
          "CAST",
          System.TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time")
        },
        {
          "USEST",
          System.TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time")
        },
        {
          "ESAST",
          System.TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")
        }
      };
      System.TimeZoneInfo timeZoneInfo = source.FirstOrDefault<KeyValuePair<string, System.TimeZoneInfo>>((Func<KeyValuePair<string, System.TimeZoneInfo>, bool>) (x => x.Key == timeZoneAbbrevation.Trim().Trim('(').Trim(')'))).Value;
      if (utcConversion.ToUpper() == "TO")
        date = new DateTime?(System.TimeZoneInfo.ConvertTimeToUtc(sourceDateTime, timeZoneInfo == null ? source.FirstOrDefault<KeyValuePair<string, System.TimeZoneInfo>>((Func<KeyValuePair<string, System.TimeZoneInfo>, bool>) (x => x.Key == "PST")).Value : timeZoneInfo));
      else if (utcConversion.ToUpper() == "FROM")
        date = new DateTime?(System.TimeZoneInfo.ConvertTimeFromUtc(sourceDateTime, timeZoneInfo == null ? source.FirstOrDefault<KeyValuePair<string, System.TimeZoneInfo>>((Func<KeyValuePair<string, System.TimeZoneInfo>, bool>) (x => x.Key == "PST")).Value : timeZoneInfo));
      return date;
    }

    public static bool tryParseJson(string notesJson, out TradeNoteModel[] result)
    {
      try
      {
        result = JsonConvert.DeserializeObject<TradeNoteModel[]>(notesJson, new JsonSerializerSettings()
        {
          MissingMemberHandling = MissingMemberHandling.Error
        });
        return true;
      }
      catch (Exception ex)
      {
        result = (TradeNoteModel[]) null;
        return false;
      }
    }

    public static TradeNoteModel PrepareTradeNoteModel(
      int index,
      string noteText,
      string userId,
      string userName)
    {
      TradeNoteModel tradeNoteModel = new TradeNoteModel();
      tradeNoteModel.Id = index;
      tradeNoteModel.Details = noteText;
      string[] strArray = Regex.Split(Utils.CreateTimestamp(true), "(\\d{1,2}/\\d{1,2}/\\d{4} \\d{1,2}:\\d{1,2} [A-Z]{2,})");
      DateTime result;
      DateTime.TryParse(strArray[1], out result);
      tradeNoteModel.CreatedTimeStamp = result != DateTime.MinValue ? TradeNoteUtils.GetDate(strArray[2], result, "to") : new DateTime?();
      tradeNoteModel.TimezoneAbbrev = strArray[2].Trim().Substring(1, strArray[2].Length - 3);
      tradeNoteModel.CreateBy = new EntityReference()
      {
        EntityId = userId,
        EntityName = userName,
        EntityType = EntityRefTypeContract.User
      };
      return tradeNoteModel;
    }
  }
}
