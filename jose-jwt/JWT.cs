// Decompiled with JetBrains decompiler
// Type: Jose.JWT
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Jose.jwe;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Jose
{
  public static class JWT
  {
    private static Dictionary<JwsAlgorithm, IJwsAlgorithm> HashAlgorithms;
    private static Dictionary<JweEncryption, IJweAlgorithm> EncAlgorithms;
    private static Dictionary<JweAlgorithm, IKeyManagement> KeyAlgorithms;
    private static Dictionary<JweCompression, ICompression> CompressionAlgorithms;
    private static Dictionary<JweAlgorithm, string> JweAlgorithms = new Dictionary<JweAlgorithm, string>();
    private static Dictionary<JweEncryption, string> JweEncryptionMethods = new Dictionary<JweEncryption, string>();
    private static Dictionary<JweCompression, string> JweCompressionMethods = new Dictionary<JweCompression, string>();
    private static Dictionary<JwsAlgorithm, string> JwsAlgorithms = new Dictionary<JwsAlgorithm, string>();
    private static IJsonMapper jsMapper;

    public static IJsonMapper JsonMapper
    {
      set => JWT.jsMapper = value;
    }

    static JWT()
    {
      JWT.JsonMapper = (IJsonMapper) new JSSerializerMapper();
      JWT.HashAlgorithms = new Dictionary<JwsAlgorithm, IJwsAlgorithm>()
      {
        {
          JwsAlgorithm.none,
          (IJwsAlgorithm) new Plaintext()
        },
        {
          JwsAlgorithm.HS256,
          (IJwsAlgorithm) new HmacUsingSha("SHA256")
        },
        {
          JwsAlgorithm.HS384,
          (IJwsAlgorithm) new HmacUsingSha("SHA384")
        },
        {
          JwsAlgorithm.HS512,
          (IJwsAlgorithm) new HmacUsingSha("SHA512")
        },
        {
          JwsAlgorithm.RS256,
          (IJwsAlgorithm) new RsaUsingSha("SHA256")
        },
        {
          JwsAlgorithm.RS384,
          (IJwsAlgorithm) new RsaUsingSha("SHA384")
        },
        {
          JwsAlgorithm.RS512,
          (IJwsAlgorithm) new RsaUsingSha("SHA512")
        },
        {
          JwsAlgorithm.ES256,
          (IJwsAlgorithm) new EcdsaUsingSha(256)
        },
        {
          JwsAlgorithm.ES384,
          (IJwsAlgorithm) new EcdsaUsingSha(384)
        },
        {
          JwsAlgorithm.ES512,
          (IJwsAlgorithm) new EcdsaUsingSha(521)
        }
      };
      JWT.JwsAlgorithms[JwsAlgorithm.none] = "none";
      JWT.JwsAlgorithms[JwsAlgorithm.HS256] = "HS256";
      JWT.JwsAlgorithms[JwsAlgorithm.HS384] = "HS384";
      JWT.JwsAlgorithms[JwsAlgorithm.HS512] = "HS512";
      JWT.JwsAlgorithms[JwsAlgorithm.RS256] = "RS256";
      JWT.JwsAlgorithms[JwsAlgorithm.RS384] = "RS384";
      JWT.JwsAlgorithms[JwsAlgorithm.RS512] = "RS512";
      JWT.JwsAlgorithms[JwsAlgorithm.ES256] = "ES256";
      JWT.JwsAlgorithms[JwsAlgorithm.ES384] = "ES384";
      JWT.JwsAlgorithms[JwsAlgorithm.ES512] = "ES512";
      JWT.HashAlgorithms[JwsAlgorithm.PS256] = (IJwsAlgorithm) new RsaPssUsingSha(32);
      JWT.HashAlgorithms[JwsAlgorithm.PS384] = (IJwsAlgorithm) new RsaPssUsingSha(48);
      JWT.HashAlgorithms[JwsAlgorithm.PS512] = (IJwsAlgorithm) new RsaPssUsingSha(64);
      JWT.JwsAlgorithms[JwsAlgorithm.PS256] = "PS256";
      JWT.JwsAlgorithms[JwsAlgorithm.PS384] = "PS384";
      JWT.JwsAlgorithms[JwsAlgorithm.PS512] = "PS512";
      JWT.EncAlgorithms = new Dictionary<JweEncryption, IJweAlgorithm>()
      {
        {
          JweEncryption.A128CBC_HS256,
          (IJweAlgorithm) new AesCbcHmacEncryption(JWT.HashAlgorithms[JwsAlgorithm.HS256], 256)
        },
        {
          JweEncryption.A192CBC_HS384,
          (IJweAlgorithm) new AesCbcHmacEncryption(JWT.HashAlgorithms[JwsAlgorithm.HS384], 384)
        },
        {
          JweEncryption.A256CBC_HS512,
          (IJweAlgorithm) new AesCbcHmacEncryption(JWT.HashAlgorithms[JwsAlgorithm.HS512], 512)
        }
      };
      JWT.JweEncryptionMethods[JweEncryption.A128CBC_HS256] = "A128CBC-HS256";
      JWT.JweEncryptionMethods[JweEncryption.A192CBC_HS384] = "A192CBC-HS384";
      JWT.JweEncryptionMethods[JweEncryption.A256CBC_HS512] = "A256CBC-HS512";
      JWT.EncAlgorithms[JweEncryption.A128GCM] = (IJweAlgorithm) new AesGcmEncryption(128);
      JWT.EncAlgorithms[JweEncryption.A192GCM] = (IJweAlgorithm) new AesGcmEncryption(192);
      JWT.EncAlgorithms[JweEncryption.A256GCM] = (IJweAlgorithm) new AesGcmEncryption(256);
      JWT.JweEncryptionMethods[JweEncryption.A128GCM] = "A128GCM";
      JWT.JweEncryptionMethods[JweEncryption.A192GCM] = "A192GCM";
      JWT.JweEncryptionMethods[JweEncryption.A256GCM] = "A256GCM";
      JWT.KeyAlgorithms = new Dictionary<JweAlgorithm, IKeyManagement>()
      {
        {
          JweAlgorithm.RSA_OAEP,
          (IKeyManagement) new RsaKeyManagement(true)
        },
        {
          JweAlgorithm.RSA_OAEP_256,
          (IKeyManagement) new RsaKeyManagement(true, true)
        },
        {
          JweAlgorithm.RSA1_5,
          (IKeyManagement) new RsaKeyManagement(false)
        },
        {
          JweAlgorithm.DIR,
          (IKeyManagement) new DirectKeyManagement()
        },
        {
          JweAlgorithm.A128KW,
          (IKeyManagement) new AesKeyWrapManagement(128)
        },
        {
          JweAlgorithm.A192KW,
          (IKeyManagement) new AesKeyWrapManagement(192)
        },
        {
          JweAlgorithm.A256KW,
          (IKeyManagement) new AesKeyWrapManagement(256)
        },
        {
          JweAlgorithm.ECDH_ES,
          (IKeyManagement) new EcdhKeyManagement(true)
        },
        {
          JweAlgorithm.ECDH_ES_A128KW,
          (IKeyManagement) new EcdhKeyManagementWithAesKeyWrap(128, new AesKeyWrapManagement(128))
        },
        {
          JweAlgorithm.ECDH_ES_A192KW,
          (IKeyManagement) new EcdhKeyManagementWithAesKeyWrap(192, new AesKeyWrapManagement(192))
        },
        {
          JweAlgorithm.ECDH_ES_A256KW,
          (IKeyManagement) new EcdhKeyManagementWithAesKeyWrap(256, new AesKeyWrapManagement(256))
        },
        {
          JweAlgorithm.PBES2_HS256_A128KW,
          (IKeyManagement) new Pbse2HmacShaKeyManagementWithAesKeyWrap(128, new AesKeyWrapManagement(128))
        },
        {
          JweAlgorithm.PBES2_HS384_A192KW,
          (IKeyManagement) new Pbse2HmacShaKeyManagementWithAesKeyWrap(192, new AesKeyWrapManagement(192))
        },
        {
          JweAlgorithm.PBES2_HS512_A256KW,
          (IKeyManagement) new Pbse2HmacShaKeyManagementWithAesKeyWrap(256, new AesKeyWrapManagement(256))
        },
        {
          JweAlgorithm.A128GCMKW,
          (IKeyManagement) new AesGcmKeyWrapManagement(128)
        },
        {
          JweAlgorithm.A192GCMKW,
          (IKeyManagement) new AesGcmKeyWrapManagement(192)
        },
        {
          JweAlgorithm.A256GCMKW,
          (IKeyManagement) new AesGcmKeyWrapManagement(256)
        }
      };
      JWT.JweAlgorithms[JweAlgorithm.RSA1_5] = "RSA1_5";
      JWT.JweAlgorithms[JweAlgorithm.RSA_OAEP] = "RSA-OAEP";
      JWT.JweAlgorithms[JweAlgorithm.RSA_OAEP_256] = "RSA-OAEP-256";
      JWT.JweAlgorithms[JweAlgorithm.DIR] = "dir";
      JWT.JweAlgorithms[JweAlgorithm.A128KW] = "A128KW";
      JWT.JweAlgorithms[JweAlgorithm.A192KW] = "A192KW";
      JWT.JweAlgorithms[JweAlgorithm.A256KW] = "A256KW";
      JWT.JweAlgorithms[JweAlgorithm.ECDH_ES] = "ECDH-ES";
      JWT.JweAlgorithms[JweAlgorithm.ECDH_ES_A128KW] = "ECDH-ES+A128KW";
      JWT.JweAlgorithms[JweAlgorithm.ECDH_ES_A192KW] = "ECDH-ES+A192KW";
      JWT.JweAlgorithms[JweAlgorithm.ECDH_ES_A256KW] = "ECDH-ES+A256KW";
      JWT.JweAlgorithms[JweAlgorithm.PBES2_HS256_A128KW] = "PBES2-HS256+A128KW";
      JWT.JweAlgorithms[JweAlgorithm.PBES2_HS384_A192KW] = "PBES2-HS384+A192KW";
      JWT.JweAlgorithms[JweAlgorithm.PBES2_HS512_A256KW] = "PBES2-HS512+A256KW";
      JWT.JweAlgorithms[JweAlgorithm.A128GCMKW] = "A128GCMKW";
      JWT.JweAlgorithms[JweAlgorithm.A192GCMKW] = "A192GCMKW";
      JWT.JweAlgorithms[JweAlgorithm.A256GCMKW] = "A256GCMKW";
      JWT.CompressionAlgorithms = new Dictionary<JweCompression, ICompression>()
      {
        {
          JweCompression.DEF,
          (ICompression) new DeflateCompression()
        }
      };
      JWT.JweCompressionMethods[JweCompression.DEF] = "DEF";
    }

    public static string Encode(
      object payload,
      object key,
      JweAlgorithm alg,
      JweEncryption enc,
      JweCompression? compression = null)
    {
      return JWT.Encode(JWT.jsMapper.Serialize(payload), key, alg, enc);
    }

    public static string Encode(
      string payload,
      object key,
      JweAlgorithm alg,
      JweEncryption enc,
      JweCompression? compression = null)
    {
      Ensure.IsNotEmpty(payload, "Payload expected to be not empty, whitespace or null.");
      IKeyManagement keyAlgorithm = JWT.KeyAlgorithms[alg];
      IJweAlgorithm encAlgorithm = JWT.EncAlgorithms[enc];
      Dictionary<string, object> header = new Dictionary<string, object>()
      {
        {
          nameof (alg),
          (object) JWT.JweAlgorithms[alg]
        },
        {
          nameof (enc),
          (object) JWT.JweEncryptionMethods[enc]
        }
      };
      byte[][] numArray1 = keyAlgorithm.WrapNewKey(encAlgorithm.KeySize, key, (IDictionary<string, object>) header);
      byte[] cek = numArray1[0];
      byte[] numArray2 = numArray1[1];
      byte[] plainText = Encoding.UTF8.GetBytes(payload);
      if (compression.HasValue)
      {
        header["zip"] = (object) JWT.JweCompressionMethods[compression.Value];
        plainText = JWT.CompressionAlgorithms[compression.Value].Compress(plainText);
      }
      byte[] bytes1 = Encoding.UTF8.GetBytes(JWT.jsMapper.Serialize((object) header));
      byte[] bytes2 = Encoding.UTF8.GetBytes(Compact.Serialize(bytes1));
      byte[][] numArray3 = encAlgorithm.Encrypt(bytes2, plainText, cek);
      return Compact.Serialize(bytes1, numArray2, numArray3[0], numArray3[1], numArray3[2]);
    }

    public static string Encode(object payload, object key, JwsAlgorithm algorithm)
    {
      return JWT.Encode(JWT.jsMapper.Serialize(payload), key, algorithm);
    }

    public static string Encode(string payload, object key, JwsAlgorithm algorithm)
    {
      Ensure.IsNotEmpty(payload, "Payload expected to be not empty, whitespace or null.");
      Dictionary<string, object> dictionary = new Dictionary<string, object>()
      {
        {
          "typ",
          (object) nameof (JWT)
        },
        {
          "alg",
          (object) JWT.JwsAlgorithms[algorithm]
        }
      };
      byte[] bytes1 = Encoding.UTF8.GetBytes(JWT.jsMapper.Serialize((object) dictionary));
      byte[] bytes2 = Encoding.UTF8.GetBytes(payload);
      byte[] bytes3 = Encoding.UTF8.GetBytes(Compact.Serialize(bytes1, bytes2));
      byte[] numArray = JWT.HashAlgorithms[algorithm].Sign(bytes3, key);
      return Compact.Serialize(bytes1, bytes2, numArray);
    }

    public static string Decode(string token, object key = null)
    {
      Ensure.IsNotEmpty(token, "Incoming token expected to be in compact serialization form, not empty, whitespace or null.");
      byte[][] parts = Compact.Parse(token);
      string str;
      if (parts.Length == 5)
      {
        str = JWT.Decrypt(parts, key);
      }
      else
      {
        byte[] bytes1 = parts[0];
        byte[] bytes2 = parts[1];
        byte[] signature = parts[2];
        byte[] bytes3 = Encoding.UTF8.GetBytes(Compact.Serialize(bytes1, bytes2));
        string algorithm = (string) JWT.jsMapper.Parse<Dictionary<string, object>>(Encoding.UTF8.GetString(bytes1))["alg"];
        if (!JWT.HashAlgorithms[JWT.GetHashAlgorithm(algorithm)].Verify(signature, bytes3, key))
          throw new IntegrityException("Invalid signature.");
        str = Encoding.UTF8.GetString(bytes2);
      }
      return str;
    }

    public static T Decode<T>(string token, object key = null)
    {
      return JWT.jsMapper.Parse<T>(JWT.Decode(token, key));
    }

    private static string Decrypt(byte[][] parts, object key)
    {
      byte[] part1 = parts[0];
      byte[] part2 = parts[1];
      byte[] part3 = parts[2];
      byte[] part4 = parts[3];
      byte[] part5 = parts[4];
      IDictionary<string, object> header = (IDictionary<string, object>) JWT.jsMapper.Parse<Dictionary<string, object>>(Encoding.UTF8.GetString(part1));
      IKeyManagement keyAlgorithm = JWT.KeyAlgorithms[JWT.GetJweAlgorithm((string) header["alg"])];
      IJweAlgorithm encAlgorithm = JWT.EncAlgorithms[JWT.GetJweEncryption((string) header["enc"])];
      byte[] cek = keyAlgorithm.Unwrap(part2, key, encAlgorithm.KeySize, header);
      byte[] bytes = Encoding.UTF8.GetBytes(Compact.Serialize(part1));
      byte[] numArray = encAlgorithm.Decrypt(bytes, cek, part3, part4, part5);
      if (header.ContainsKey("zip"))
        numArray = JWT.CompressionAlgorithms[JWT.GetJweCompression((string) header["zip"])].Decompress(numArray);
      return Encoding.UTF8.GetString(numArray);
    }

    private static JwsAlgorithm GetHashAlgorithm(string algorithm)
    {
      foreach (KeyValuePair<JwsAlgorithm, string> jwsAlgorithm in JWT.JwsAlgorithms)
      {
        if (jwsAlgorithm.Value.Equals(algorithm))
          return jwsAlgorithm.Key;
      }
      throw new InvalidAlgorithmException(string.Format("Signing algorithm is not supported: {0}", (object) algorithm));
    }

    private static JweAlgorithm GetJweAlgorithm(string algorithm)
    {
      foreach (KeyValuePair<JweAlgorithm, string> jweAlgorithm in JWT.JweAlgorithms)
      {
        if (jweAlgorithm.Value.Equals(algorithm))
          return jweAlgorithm.Key;
      }
      throw new InvalidAlgorithmException(string.Format("Algorithm is not supported: {0}.", (object) algorithm));
    }

    private static JweEncryption GetJweEncryption(string algorithm)
    {
      foreach (KeyValuePair<JweEncryption, string> encryptionMethod in JWT.JweEncryptionMethods)
      {
        if (encryptionMethod.Value.Equals(algorithm))
          return encryptionMethod.Key;
      }
      throw new InvalidAlgorithmException(string.Format("Encryption algorithm is not supported: {0}.", (object) algorithm));
    }

    private static JweCompression GetJweCompression(string algorithm)
    {
      foreach (KeyValuePair<JweCompression, string> compressionMethod in JWT.JweCompressionMethods)
      {
        if (compressionMethod.Value.Equals(algorithm))
          return compressionMethod.Key;
      }
      throw new InvalidAlgorithmException(string.Format("Compression algorithm is not supported: {0}.", (object) algorithm));
    }
  }
}
