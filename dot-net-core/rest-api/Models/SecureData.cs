using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RestApiShowcase.Models
{	
	public static class SecureData
	{
		private static readonly string publicKey =
			"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAteddVMz/NzUwHyWuAgS60TePsPENo3E2nqz/VQ6y3LO1A1bpws7W6+CqyV+ilG9wqS5jLQIB9fK6PefajqgC+e5/xEriG9mOgOIoG9tOrliQZ2QJlRKXoBZmblrz0ZVur5ZJ1bKJyYFBKOKRSqb6FWe00p5xSD6W4FdqQztQSG7YGNt49R+87s/ejsUl/G3mJa3l76j0sMTVd6oZnFQ69STuX93E8QWRb3Q95uJgJjMeCFyR4n80sYKhWMR42t8m7CFfyJzON5WlaTMZeKIZg8NmLtd1v7AXgh9x6m6FdiDOPi9Uy35wRWp9cVnB/rR8CoIxQnO15tV8ovjzuw8NFwIDAQAB";
		
		private static readonly string privateKey =
			"MIIEogIBAAKCAQEAnzwbcBsR1Y3m+/cq10ABLu19zB24R7BLR3f2Wlt0LHdrAcK04WbVP7fV2+BaVF0naQ1FfsDJypGG1xE/BI5ArESUa2qZN2ZkuTsvHvxDUZoApXshszYySL9yDRioRRsbSv47MeH7PyoSu2bSi847aM4ZEv0uJzl5rrDDqAY9fgG1+QM0shy+AoBtZGPdW8G/HnJEX4fz2aCUvmEmMtH8espVV2h1i5xcDvYVghbRm8QiBfSCYDdssFXmBngJ6xyuXUu6yMP6az0EjhdeA/wMEjmq/PWNmErX3K1p/pODOWaxQfdSIWw64Y2YwVaeSGxjZ1BGr5wmA+YhidsRq7EVbQIDAQABAoIBAHE9nsmwBpPX0a/YzBe+BdlovfC+JgKdhjuyPXlu1oSU/H2JCXfO2NLymt0+hR7ADLnx3T1SP2+W5MzoD+fefnHEZ8SoT4QNho4QX+dqQTm0CsekdIftf/8qXyKfKXPT3ioL+O71peb6tc2eRxdzt95Fy6Esenkcr8OjI9HHdk24LJIT7SlH+c+fmkicJeOEO6oNrbyh1R3QmIfcqh+7lZWyoUy3MsaCjkvGO5muQfghBuxDzklvajHFsADxHh0GyQXr0fZ28ccdgJvGtQ602uaGf3Jmv37L5bXI7yaAnoHp3nqF9YFcHJy94N1NE4ls54sQNwQX6wCjMi4HIku/weECgYEAy6Zveo4noc6P9CahaXKfZuj7Vg0g5QKVU+LLwA1cgG681gpmRvIvzzJP+bNfWeMtZpiVhld223x2zI76S/xF1FGj754w5JknNOOYLyjj+B4hDj5rSpSuN7TANGEN6ZWjP0f0toQwTSBkpEglexcF2X5EF/GshkZ6PhTjuRaIssUCgYEAyCrXAZ8uRy+NyqXfPjL0tFTaivqBMZxpGxkg7lvIoFo/A6mzgyle0GB3Vf4WOYvwODLaV8XGgcLyay3C8dJypygssk9jK7bgzWP2fHdHz95yc57oBfs3fZS3u4U2WGvn3vNKSE3Zg/iAifDXLw31f5QkZk4FmZLzVFTXy0FSYokCgYBXZiBlumhr2tiQ8ZtIQ/cBFW/4KD47ynYHwEetLo7KV7wXJ/No8zttcqw7/60mSIcNhuJ0+0h5PdtGQv5MagIV8sumZH+bO9dFrX98jdH4hAWvtT4ajeCO67WpRRXRb3TWDa2KPwTztLk8f/IF94V9J+FVXVSrc8e2qQRqRK95+QKBgClhEcl4Cq0Um49E8v18szGESRlp58NFvkSn0TL1LmhXv6cWMdHvf3Y8Ou8/84A5+ZLkkFaMwBibIFofv6kWBDWiDHtgBfRHffl3rMDWN7Y1Ghvkwbpa0IHZeJH/W9izld91E+oWjdzGRmw9qX5EuHeDPHMZtfaldj/8ug9+lXVhAoGAQCiNbixspipqD2PD2w+gOAPQGZM9P7Gqk2DOuaasemvT9wa44gmbzs6yJvZo70GP7pQUtWm3SpTojFTqdwnOC+lYuWvF1wxyN2tV9I8k9H7ODp9DQ02k/dQXMBlH7GwK1Sm+Iy39EkfLbdOtdE/+BAfPAaFblimXBBpCSNCke6U=";


		private static readonly RSA _privateKeyRsaProvider;
		private static readonly RSA _publicKeyRsaProvider;
		private static readonly HashAlgorithmName _hashAlgorithmName;
		private static readonly Encoding _encoding;

		static SecureData()
		{
			_encoding = Encoding.UTF8;
			if (!string.IsNullOrEmpty(privateKey))
			{
				_privateKeyRsaProvider = CreateRsaProviderFromPrivateKey(privateKey);
			}

			if (!string.IsNullOrEmpty(publicKey))
			{
				_publicKeyRsaProvider = CreateRsaProviderFromPublicKey(publicKey);
			}

			_hashAlgorithmName = HashAlgorithmName.SHA256;
		}

		#region Sign with private key

		public static string Sign(string data)
		{
			byte[] dataBytes = _encoding.GetBytes(data);

			var signatureBytes = _privateKeyRsaProvider.SignData(dataBytes, _hashAlgorithmName, RSASignaturePadding.Pkcs1);

			return Convert.ToBase64String(signatureBytes);
		}

		#endregion

		#region Use public key to verify signature

		public static bool Verify(string data,string sign)
		{
			byte[] dataBytes = _encoding.GetBytes(data);
			byte[] signBytes = Convert.FromBase64String(sign);

			var verify = _publicKeyRsaProvider.VerifyData(dataBytes, signBytes, _hashAlgorithmName, RSASignaturePadding.Pkcs1);

			return verify;
		}

		#endregion

		#region Decrypt
		
		public static string Copy(string text)
		{
			return text;
		}

		public static string Decrypt(string cipherText)
		{
			if (String.IsNullOrEmpty(cipherText)) return cipherText;
			if (_privateKeyRsaProvider == null)
			{
				throw new Exception("_privateKeyRsaProvider is null");
			}
			return Encoding.UTF8.GetString(_privateKeyRsaProvider.Decrypt(Convert.FromBase64String(cipherText), RSAEncryptionPadding.Pkcs1));
		}

		#endregion

		#region Encrypt

		public static string Encrypt(string text)
		{
			if (String.IsNullOrEmpty(text)) return text;
			if (_publicKeyRsaProvider == null)
			{
				throw new Exception("_publicKeyRsaProvider is null");
			}
			return Convert.ToBase64String(_publicKeyRsaProvider.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.Pkcs1));
		}

		#endregion

		#region Create an RSA instance with a private key

		public static RSA CreateRsaProviderFromPrivateKey(string privateKey)
		{
			var privateKeyBits = Convert.FromBase64String(privateKey);

			var rsa = RSA.Create();
			var rsaParameters = new RSAParameters();

			using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKeyBits)))
			{
				byte bt = 0;
				ushort twobytes = 0;
				twobytes = binr.ReadUInt16();
				if (twobytes == 0x8130)
					binr.ReadByte();
				else if (twobytes == 0x8230)
					binr.ReadInt16();
				else
					throw new Exception("Unexpected value read binr.ReadUInt16()");

				twobytes = binr.ReadUInt16();
				if (twobytes != 0x0102)
					throw new Exception("Unexpected version");

				bt = binr.ReadByte();
				if (bt != 0x00)
					throw new Exception("Unexpected value read binr.ReadByte()");

				rsaParameters.Modulus = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.Exponent = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.D = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.P = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.Q = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.DP = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.DQ = binr.ReadBytes(GetIntegerSize(binr));
				rsaParameters.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
			}

			rsa.ImportParameters(rsaParameters);
			return rsa;
		}

		#endregion

		#region Create an RSA instance using a public key

		public static RSA CreateRsaProviderFromPublicKey(string publicKeyString)
		{
			// encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
			byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
			byte[] seq = new byte[15];

			var x509Key = Convert.FromBase64String(publicKeyString);

			// ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
			using (MemoryStream mem = new MemoryStream(x509Key))
			{
				using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
				{
					byte bt = 0;
					ushort twobytes = 0;

					twobytes = binr.ReadUInt16();
					if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
						binr.ReadByte();    //advance 1 byte
					else if (twobytes == 0x8230)
						binr.ReadInt16();   //advance 2 bytes
					else
						return null;

					seq = binr.ReadBytes(15);       //read the Sequence OID
					if (!CompareBytearrays(seq, seqOid))    //make sure Sequence for OID is correct
						return null;

					twobytes = binr.ReadUInt16();
					if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
						binr.ReadByte();    //advance 1 byte
					else if (twobytes == 0x8203)
						binr.ReadInt16();   //advance 2 bytes
					else
						return null;

					bt = binr.ReadByte();
					if (bt != 0x00)     //expect null byte next
						return null;

					twobytes = binr.ReadUInt16();
					if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
						binr.ReadByte();    //advance 1 byte
					else if (twobytes == 0x8230)
						binr.ReadInt16();   //advance 2 bytes
					else
						return null;

					twobytes = binr.ReadUInt16();
					byte lowbyte = 0x00;
					byte highbyte = 0x00;

					if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
						lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
					else if (twobytes == 0x8202)
					{
						highbyte = binr.ReadByte(); //advance 2 bytes
						lowbyte = binr.ReadByte();
					}
					else
						return null;
					byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
					int modsize = BitConverter.ToInt32(modint, 0);

					int firstbyte = binr.PeekChar();
					if (firstbyte == 0x00)
					{   //if first byte (highest order) of modulus is zero, don't include it
						binr.ReadByte();    //skip this null byte
						modsize -= 1;   //reduce modulus buffer size by 1
					}

					byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

					if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
						return null;
					int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
					byte[] exponent = binr.ReadBytes(expbytes);

					// ------- create RSACryptoServiceProvider instance and initialize with public key -----
					var rsa = RSA.Create();
					RSAParameters rsaKeyInfo = new RSAParameters
					{
						Modulus = modulus,
						Exponent = exponent
					};
					rsa.ImportParameters(rsaKeyInfo);

					return rsa;
				}

			}
		}

		#endregion

		#region Import key algorithm

		private static int GetIntegerSize(BinaryReader binr)
		{
			byte bt = 0;
			int count = 0;
			bt = binr.ReadByte();
			if (bt != 0x02)
				return 0;
			bt = binr.ReadByte();

			if (bt == 0x81)
				count = binr.ReadByte();
			else
			if (bt == 0x82)
			{
				var highbyte = binr.ReadByte();
				var lowbyte = binr.ReadByte();
				byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
				count = BitConverter.ToInt32(modint, 0);
			}
			else
			{
				count = bt;
			}

			while (binr.ReadByte() == 0x00)
			{
				count -= 1;
			}
			binr.BaseStream.Seek(-1, SeekOrigin.Current);
			return count;
		}

		private static bool CompareBytearrays(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;
			int i = 0;
			foreach (byte c in a)
			{
				if (c != b[i])
					return false;
				i++;
			}
			return true;
		}

		#endregion

	}
	
}