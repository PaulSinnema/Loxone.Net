using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Net
{
	internal static class Tools
	{

		public static byte[] GetBytes(string input)
		{
			var outputLength = input.Length / 2;
			var output = new byte[outputLength];
			for (var i = 0; i < outputLength; i++)
				output[i] = Convert.ToByte(input.Substring(i * 2, 2), 16);
			return output;
		}

		public static string GetUuid(byte[] data, int start = 0)
		{
			StringBuilder bld = new StringBuilder();
			bld.Append(data[start + 3].ToString("x2"));
			bld.Append(data[start + 2].ToString("x2"));
			bld.Append(data[start + 1].ToString("x2"));
			bld.Append(data[start + 0].ToString("x2"));
			bld.Append("-");
			bld.Append(data[start + 5].ToString("x2"));
			bld.Append(data[start + 4].ToString("x2"));
			bld.Append("-");
			bld.Append(data[start + 7].ToString("x2"));
			bld.Append(data[start + 6].ToString("x2"));
			bld.Append("-");
			bld.Append(data[start + 8].ToString("x2"));
			bld.Append(data[start + 9].ToString("x2"));
			bld.Append(data[start + 10].ToString("x2"));
			bld.Append(data[start + 11].ToString("x2"));
			bld.Append(data[start + 12].ToString("x2"));
			bld.Append(data[start + 13].ToString("x2"));
			bld.Append(data[start + 14].ToString("x2"));
			bld.Append(data[start + 15].ToString("x2"));


			return bld.ToString();
		}

		public static UInt32 GetUInt32(byte[] data, int start = 0)
		{
			return BitConverter.ToUInt32(new byte[] { data[start + 3], data[start + 2], data[start + 1], data[start + 0] }, 0);

		}

		public static UInt64 GetUInt64(byte[] data, int start = 0) {
			return BitConverter.ToUInt64(new byte[] { data[start + 7], data[start + 6], data[start + 5], data[start + 4], data[start + 3], data[start + 2], data[start + 1], data[start + 0] }, 0);
		}


		public static string HashSHA1(string stringToHash) {
			using (var sha1 = new SHA1Managed()) {
				return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash)));
			}
		}


		public static string GetSalt() {
			var random = new RNGCryptoServiceProvider();

			// Maximum length of salt
			int max_length = 32;

			// Empty salt array
			byte[] salt = new byte[max_length];

			// Build the random bytes
			random.GetNonZeroBytes(salt);

			// Return the string encoded salt
			return Convert.ToBase64String(salt);
		}


	
		public static string EncryptAes(string plainText, byte[] Key = null, byte[] IV = null) {
			byte[] encrypted;
			// Create a new AesManaged.    
			using (AesManaged aes = new AesManaged()) {

				// Create encryptor    
				ICryptoTransform encryptor = aes.CreateEncryptor(Key ?? aes.Key, IV ?? aes.IV);
				// Create MemoryStream    
				using (MemoryStream ms = new MemoryStream()) {
					// Create crypto stream using the CryptoStream class. This class is the key to encryption    
					// and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
					// to encrypt    
					using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
						// Create StreamWriter and write data to a stream    
						using (StreamWriter sw = new StreamWriter(cs))
							sw.Write(plainText);
						encrypted = ms.ToArray();
					}
				}
			}
			// Return encrypted data    


			return System.Convert.ToBase64String(encrypted);
		}
		public static string DecryptAes(byte[] cipherText, byte[] Key, byte[] IV) {
			string plaintext = null;
			// Create AesManaged    
			using (AesManaged aes = new AesManaged()) {
				// Create a decryptor    
				ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
				// Create the streams used for decryption.    
				using (MemoryStream ms = new MemoryStream(cipherText)) {
					// Create crypto stream    
					using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
						// Read crypto stream    
						using (StreamReader reader = new StreamReader(cs))
							plaintext = reader.ReadToEnd();
					}
				}
			}
			return plaintext;
		}


		public static string EncryptRsa(string plainText, string publicKey) {
			byte[] data = Encoding.UTF8.GetBytes(plainText);

			//Encode with public key
			RSACryptoServiceProvider rsaPublic = new RSACryptoServiceProvider();

			RSAParameters param = rsaPublic.ExportParameters(false);
			param.Modulus = Convert.FromBase64String(publicKey);

			rsaPublic.ImportParameters(param);
			//rsaPublic.FromXmlString($"<RSAKeyValue><Modulus>{publicKey}</RSAKeyValue>");

			byte[] encryptedRSA = rsaPublic.Encrypt(data, false);
			return Convert.ToBase64String(encryptedRSA);
		}
	}
}
