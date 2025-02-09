// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ConventionalCountyLimitUtil
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class ConventionalCountyLimitUtil
  {
    public ConventionalCountyLimit[] GetConventionalCountyLimits(
      string url,
      int year,
      bool isCustomUrl = false)
    {
      List<ConventionalCountyLimit> conventionalCountyLimitList = new List<ConventionalCountyLimit>();
      try
      {
        string str1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "conventionalCountyLimits.xlsx");
        if (System.IO.File.Exists(str1))
          System.IO.File.Delete(str1);
        new WebClient().DownloadFile(url, str1);
        // ISSUE: variable of a compiler-generated type
        Application instance = (Application) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        Workbook workbook = instance.Workbooks.Open(str1, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        // ISSUE: reference to a compiler-generated field
        if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, _Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (_Worksheet), typeof (ConventionalCountyLimitUtil)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        _Worksheet worksheet = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__0.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__0, workbook.Sheets[(object) 1]);
        // ISSUE: variable of a compiler-generated type
        Range usedRange = worksheet.UsedRange;
        if (isCustomUrl)
        {
          year = this.populateYearFromExcel(usedRange);
          if (year == -1)
            throw new Exception();
        }
        int count1 = usedRange.Rows.Count;
        int count2 = usedRange.Columns.Count;
        int num = 1;
        for (int RowIndex = 3; RowIndex <= count1; ++RowIndex)
        {
          ConventionalCountyLimit conventionalCountyLimit1 = new ConventionalCountyLimit();
          conventionalCountyLimit1.LimitYear = year;
          conventionalCountyLimit1.ID = num;
          for (int ColumnIndex = 1; ColumnIndex <= count2; ++ColumnIndex)
          {
            switch (ColumnIndex)
            {
              case 1:
                ConventionalCountyLimit conventionalCountyLimit2 = conventionalCountyLimit1;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__3 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConventionalCountyLimitUtil)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, string> target1 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__3.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, string>> p3 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__3;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__2 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target2 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__2.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p2 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__2;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__1 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value2", typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj1 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__1.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__1, usedRange.Cells[(object) RowIndex, (object) ColumnIndex]);
                object obj2 = target2((CallSite) p2, obj1);
                string str2 = target1((CallSite) p3, obj2);
                conventionalCountyLimit2.FIPSStateCode = str2;
                break;
              case 2:
                ConventionalCountyLimit conventionalCountyLimit3 = conventionalCountyLimit1;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__6 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConventionalCountyLimitUtil)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, string> target3 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__6.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, string>> p6 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__6;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__5 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target4 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__5.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p5 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__5;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__4 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value2", typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj3 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__4.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__4, usedRange.Cells[(object) RowIndex, (object) ColumnIndex]);
                object obj4 = target4((CallSite) p5, obj3);
                string str3 = target3((CallSite) p6, obj4);
                conventionalCountyLimit3.FIPSCountyCode = str3;
                break;
              case 3:
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__9 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConventionalCountyLimitUtil)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, string> target5 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__9.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, string>> p9 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__9;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__8 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target6 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__8.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p8 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__8;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__7 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value2", typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj5 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__7.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__7, usedRange.Cells[(object) RowIndex, (object) ColumnIndex]);
                object obj6 = target6((CallSite) p8, obj5);
                string str4 = this.cleanCountyName(target5((CallSite) p9, obj6));
                conventionalCountyLimit1.CountyName = str4;
                break;
              case 4:
                ConventionalCountyLimit conventionalCountyLimit4 = conventionalCountyLimit1;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__12 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConventionalCountyLimitUtil)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, string> target7 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__12.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, string>> p12 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__12;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__11 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target8 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__11.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p11 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__11;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__10 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value2", typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj7 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__10.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__10, usedRange.Cells[(object) RowIndex, (object) ColumnIndex]);
                object obj8 = target8((CallSite) p11, obj7);
                string str5 = target7((CallSite) p12, obj8);
                conventionalCountyLimit4.StateCode = str5;
                break;
              case 5:
                ConventionalCountyLimit conventionalCountyLimit5 = conventionalCountyLimit1;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__15 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConventionalCountyLimitUtil)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, string> target9 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__15.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, string>> p15 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__15;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__14 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target10 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__14.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p14 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__14;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__13 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value2", typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj9 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__13.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__13, usedRange.Cells[(object) RowIndex, (object) ColumnIndex]);
                object obj10 = target10((CallSite) p14, obj9);
                string str6 = target9((CallSite) p15, obj10);
                conventionalCountyLimit5.CBSANumber = str6;
                break;
              case 6:
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__18 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConventionalCountyLimitUtil)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, string> target11 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__18.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, string>> p18 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__18;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__17 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target12 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__17.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p17 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__17;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__16 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value2", typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj11 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__16.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__16, usedRange.Cells[(object) RowIndex, (object) ColumnIndex]);
                object obj12 = target12((CallSite) p17, obj11);
                string s1 = target11((CallSite) p18, obj12).Trim('$', ',');
                conventionalCountyLimit1.LimitFor1Unit = int.Parse(s1);
                break;
              case 7:
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__21 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConventionalCountyLimitUtil)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, string> target13 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__21.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, string>> p21 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__21;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__20 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target14 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__20.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p20 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__20;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__19 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value2", typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj13 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__19.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__19, usedRange.Cells[(object) RowIndex, (object) ColumnIndex]);
                object obj14 = target14((CallSite) p20, obj13);
                string s2 = target13((CallSite) p21, obj14).Trim('$', ',');
                conventionalCountyLimit1.LimitFor2Units = int.Parse(s2);
                break;
              case 8:
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__24 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConventionalCountyLimitUtil)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, string> target15 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__24.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, string>> p24 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__24;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__23 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target16 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__23.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p23 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__23;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__22 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value2", typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj15 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__22.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__22, usedRange.Cells[(object) RowIndex, (object) ColumnIndex]);
                object obj16 = target16((CallSite) p23, obj15);
                string s3 = target15((CallSite) p24, obj16).Trim('$', ',');
                conventionalCountyLimit1.LimitFor3Units = int.Parse(s3);
                break;
              case 9:
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__27 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConventionalCountyLimitUtil)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, string> target17 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__27.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, string>> p27 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__27;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__26 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target18 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__26.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p26 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__26;
                // ISSUE: reference to a compiler-generated field
                if (ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__25 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value2", typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj17 = ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__25.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__0.\u003C\u003Ep__25, usedRange.Cells[(object) RowIndex, (object) ColumnIndex]);
                object obj18 = target18((CallSite) p26, obj17);
                string s4 = target17((CallSite) p27, obj18).Trim('$', ',');
                conventionalCountyLimit1.LimitFor4Units = int.Parse(s4);
                break;
            }
          }
          conventionalCountyLimit1.LastModifiedDateTime = DateTime.Now;
          ++num;
          conventionalCountyLimitList.Add(conventionalCountyLimit1);
        }
        // ISSUE: reference to a compiler-generated method
        workbook.Close(Type.Missing, Type.Missing, Type.Missing);
      }
      catch (Exception ex)
      {
        Console.Write(ex.Message);
      }
      return conventionalCountyLimitList.ToArray();
    }

    private string cleanCountyName(string name)
    {
      if (name.StartsWith("COLONIAL HEIGHTS"))
        return name;
      if (name.EndsWith(" COUNTY"))
        name = name.Substring(0, name.Length - 7);
      else if (name.EndsWith(" MUNICIPIO"))
        name = name.Substring(0, name.Length - 10);
      else if (name.EndsWith(" CITY AND BOROUGH"))
        name = name.Substring(0, name.Length - 17);
      else if (name.EndsWith(" BOROUGH"))
        name = name.Substring(0, name.Length - 8);
      else if (name.EndsWith(" MUNICIPALITY"))
        name = name.Substring(0, name.Length - 13);
      else if (name.EndsWith(" CENSUS AREA"))
        name = name.Substring(0, name.Length - 12);
      else if (name.EndsWith(" PARISH"))
        name = name.Substring(0, name.Length - 7);
      else if (name.EndsWith(" DISTRICT"))
        name = name.Substring(0, name.Length - 9);
      else if (name == "ST. CROIX ISLAND" || name == "ST. JOHN ISLAND" || name == "ST. THOMAS ISLAND")
        name = name.Substring(0, name.Length - 7);
      return name;
    }

    private int populateYearFromExcel(Range xlRange)
    {
      int num1 = -1;
      string str1 = "calendar year ";
      // ISSUE: reference to a compiler-generated field
      if (ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConventionalCountyLimitUtil)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target1 = ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p2 = ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target2 = ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p1 = ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value2", typeof (ConventionalCountyLimitUtil), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) ConventionalCountyLimitUtil.\u003C\u003Eo__2.\u003C\u003Ep__0, xlRange.Cells[(object) 1, (object) 1]);
      object obj2 = target2((CallSite) p1, obj1);
      string str2 = target1((CallSite) p2, obj2);
      int num2 = str2.ToLowerInvariant().IndexOf("calendar year");
      if (num2 >= 0)
      {
        int startIndex = num2 + str1.Length;
        if (str2.Length > startIndex + 4)
          num1 = int.Parse(str2.Substring(startIndex, 4));
      }
      return num1;
    }
  }
}
