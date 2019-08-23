using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using System.Security.Cryptography;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace YukApiCSharp {
    public static class Tools {
        public static readonly RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public static string HeadersToString(IHeaderDictionary headers) {
            StringBuilder sb = new StringBuilder();
            foreach (var header in headers) {
                foreach (string value in header.Value) {
                    sb.Append(header.Key).Append(": ").Append(value).Append("\r\n");
                }
            }

            return sb.ToString();
        }

        public static byte[] AesGcm256Encrypt(byte[] plain, byte[] key, out byte[] nonce) {
            Debug.Assert(key.Length * 8 == 256);
            nonce = new byte[12];
            rngCsp.GetBytes(nonce);
            GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
            Console.WriteLine("AES/GCM加密，" + gcmBlockCipher.AlgorithmName);
            gcmBlockCipher.Init(true, new AeadParameters(new KeyParameter(key), 128, nonce));
            byte[] cipher = new byte[gcmBlockCipher.GetOutputSize(plain.Length)];
            int p = gcmBlockCipher.ProcessBytes(plain, 0, plain.Length, cipher, 0);
            p += gcmBlockCipher.DoFinal(cipher, p);
            Debug.Assert(p == cipher.Length);
            return cipher;
        }

        public static byte[] AesGcm256Decrypt(byte[] cipher, byte[] key, byte[] nonce) {
            GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
            Console.WriteLine("AES/GCM解密，" + gcmBlockCipher.AlgorithmName);
            gcmBlockCipher.Init(false, new AeadParameters(new KeyParameter(key), 128, nonce));
            byte[] plain = new byte[gcmBlockCipher.GetOutputSize(cipher.Length)];
            int p = gcmBlockCipher.ProcessBytes(cipher, 0, cipher.Length, plain, 0);
            p += gcmBlockCipher.DoFinal(plain, p);
            Debug.Assert(p == plain.Length);
            return plain;
        }

        public static byte[] HexToByteArray(string hex) {
            Debug.Assert(hex.Length % 2 == 0);
            byte[] rst = new byte[hex.Length / 2];
            for (int i = 0; i < rst.Length; i++) {
                rst[i] = byte.Parse(hex.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return rst;
        }
    }
}