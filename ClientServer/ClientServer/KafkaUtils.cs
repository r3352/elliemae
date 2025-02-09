// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.KafkaUtils
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.Common.Security;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public static class KafkaUtils
  {
    public static List<KeyValuePair<string, string>> ProducerConfigs { get; private set; }

    public static List<KeyValuePair<string, string>> ConsumerConfigs { get; private set; }

    public static Dictionary<string, string> ProducerSettings { get; private set; }

    static KafkaUtils()
    {
      if (ConfigurationManager.GetSection("Kafka/Settings") is NameValueCollection section1)
      {
        if (KafkaUtils.ProducerSettings == null)
          KafkaUtils.ProducerSettings = new Dictionary<string, string>();
        foreach (string allKey in section1.AllKeys)
          KafkaUtils.ProducerSettings.Add(allKey, section1[allKey].ToString());
      }
      if (ConfigurationManager.GetSection("Kafka/ProducerConfigs") is NameValueCollection section2)
      {
        if (KafkaUtils.ProducerConfigs == null)
          KafkaUtils.ProducerConfigs = new List<KeyValuePair<string, string>>();
        foreach (string allKey in section2.AllKeys)
        {
          string str = section2[allKey].ToString();
          if (allKey == EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerConfigsEnum.SSL_CA_LOCATION))
            str = KafkaUtils.GetKafkaCertFileLocation(KafkaUtils.ProducerSettings[EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_CACERT_PROFILE)].ToString(), str);
          else if (allKey == EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerConfigsEnum.SASL_PASSWORD))
            str = KafkaUtils.KafkaDecryptedSaslPassword(str);
          KafkaUtils.ProducerConfigs.Add(new KeyValuePair<string, string>(allKey, str));
        }
      }
      KafkaUtils.LoadConsumerConfigs();
    }

    private static void LoadConsumerConfigs()
    {
      KafkaUtils.ConsumerConfigs = new List<KeyValuePair<string, string>>();
      KafkaUtils.ConsumerConfigs.AddRange((IEnumerable<KeyValuePair<string, string>>) KafkaUtils.ProducerConfigs);
    }

    public static string DeploymentProfile
    {
      get
      {
        return KafkaUtils.ProducerSettings[EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_DEPLOYMENT_PROFILE)];
      }
    }

    public static string AwsRegion
    {
      get
      {
        return KafkaUtils.ProducerSettings[EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_AWS_REGION)].ToString();
      }
    }

    public static string Region
    {
      get
      {
        return KafkaUtils.ProducerSettings[EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_REGION)].ToString();
      }
    }

    public static bool IsAsyncEnabled
    {
      get
      {
        return Utils.ParseBoolean((object) KafkaUtils.ProducerSettings[EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_PRODUCER_ASYNC_ENABLED)]);
      }
    }

    public static bool IsEnableAsyncTaskProducer
    {
      get
      {
        return Utils.ParseBoolean((object) KafkaUtils.ProducerSettings[EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_ENABLE_ASYNC_PRODUCER)]);
      }
    }

    public static int SyncTimeout
    {
      get
      {
        return Utils.ParseInt((object) KafkaUtils.ProducerSettings[EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_PRODUCERf_SYNC_TIMEOUT)]);
      }
    }

    public static bool UseAggregatedLogging
    {
      get
      {
        string str;
        return KafkaUtils.ProducerSettings.TryGetValue(EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_ENABLE_AGGREGATED_LOGGING), out str) && Utils.ParseBoolean((object) str);
      }
    }

    private static string GetKafkaCertFileLocation(string kafkaCertificateProfile, string file)
    {
      return !string.IsNullOrWhiteSpace(kafkaCertificateProfile) && !string.IsNullOrWhiteSpace(EnConfigurationSettings.GlobalSettings.KafkaCertFolderLocation) ? Path.Combine(EnConfigurationSettings.GlobalSettings.KafkaCertFolderLocation, kafkaCertificateProfile.Trim() + file) : (string) null;
    }

    public static bool IsKafkaDebugLogEnabled
    {
      get
      {
        bool result = false;
        string enumDescription = EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_ENABLEDEBUGLOG);
        if (KafkaUtils.ProducerSettings[enumDescription] != null)
          bool.TryParse(KafkaUtils.ProducerSettings[enumDescription], out result);
        return result;
      }
    }

    public static bool IsSyncProduceMethodEnabled
    {
      get
      {
        bool result = false;
        string enumDescription = EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_PRODUCE_METHOD_FLAG);
        if (KafkaUtils.ProducerSettings[enumDescription] != null)
          bool.TryParse(KafkaUtils.ProducerSettings[enumDescription], out result);
        return result;
      }
    }

    public static bool IsProducerStatic
    {
      get
      {
        bool result = false;
        string enumDescription = EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_STATIC_PRODUCER);
        if (KafkaUtils.ProducerSettings[enumDescription] != null)
          bool.TryParse(KafkaUtils.ProducerSettings[enumDescription], out result);
        return result;
      }
    }

    public static bool IsKafkaWarningLogEnabled
    {
      get
      {
        bool result = false;
        try
        {
          string enumDescription = EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_ENABLEWARNINGLOG);
          if (KafkaUtils.ProducerSettings[enumDescription] != null)
            bool.TryParse(KafkaUtils.ProducerSettings[enumDescription], out result);
        }
        catch (Exception ex)
        {
          result = false;
        }
        return result;
      }
    }

    public static bool IsKafkaFlushEnabled
    {
      get
      {
        bool result = false;
        string enumDescription = EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_PRODUCER_ENABLEFLUSH);
        if (KafkaUtils.ProducerSettings[enumDescription] != null)
          bool.TryParse(KafkaUtils.ProducerSettings[enumDescription], out result);
        return result;
      }
    }

    private static string KafkaDecryptedSaslPassword(string kafkaSaslPassword)
    {
      try
      {
        if (!string.IsNullOrEmpty(kafkaSaslPassword))
          return Rijndael256Util.Decrypt(KafkaUtils.ConvertHexToString(kafkaSaslPassword, Encoding.Unicode));
      }
      catch (Exception ex)
      {
        Tracing.Log(true, TraceLevel.Error.ToString(), nameof (KafkaUtils), string.Format("Error while decypting password with exceptionMessage:{0}, Stacktrace:{1}", (object) ex.Message, (object) ex.StackTrace));
      }
      return string.Empty;
    }

    private static string ConvertHexToString(string hexInput, Encoding encoding)
    {
      try
      {
        int length = hexInput.Length;
        byte[] bytes = new byte[length / 2];
        for (int startIndex = 0; startIndex < length; startIndex += 2)
          bytes[startIndex / 2] = Convert.ToByte(hexInput.Substring(startIndex, 2), 16);
        return encoding.GetString(bytes);
      }
      catch (Exception ex)
      {
        Tracing.Log(true, TraceLevel.Error.ToString(), nameof (KafkaUtils), "Error while decypting password");
        return string.Empty;
      }
    }

    public static bool IsKafkaEnableNoKeyProducer
    {
      get
      {
        bool result = false;
        string enumDescription = EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerSettingsEnum.KAFKA_PRODUCER_ENABLENOKEY);
        if (KafkaUtils.ProducerSettings[enumDescription] != null)
          bool.TryParse(KafkaUtils.ProducerSettings[enumDescription], out result);
        return result;
      }
    }
  }
}
