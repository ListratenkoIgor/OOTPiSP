using PluginInterface;
using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ZBase32_Plugin
{
	[Plugin(PluginType.Coding)]
    public class MyPlugin : ICoding
    {
        public string Name
        {
            get { return "ZBase32 Encoding Plugin"; }
        }

        public string Version
        {
            get { return "1.0.0"; }
        }

        public string Author
        {
            get { return "KvanTTT"; }
        }

        public string CodingName
        {
            get { return "ZBase32"; }
        }
        static byte[] GetBytes(string randomKeyText)
        {
            byte[] bytes = new byte[randomKeyText.Length * sizeof(char)];
            System.Buffer.BlockCopy(randomKeyText.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
		public byte[] Encode(byte[] data) {
			ZBase32 zBase32 = new ZBase32();
			return GetBytes(zBase32.Encode(data));
		}
		public byte[] Decode(byte[] data) {
			ZBase32 zBase32 = new ZBase32();
			return zBase32.Decode(data.ToString());
		}
		public abstract class Base
		{
			public uint CharsCount { get; }

			public double BitsPerChars => (double)BlockBitsCount / BlockCharsCount;

			public int BlockBitsCount { get; protected set; }

			public int BlockCharsCount { get; protected set; }

			public string Alphabet { get; }

			public char Special { get; }

			public abstract bool HasSpecial { get; }

			public Encoding Encoding { get; set; }

			public bool Parallel { get; set; }

			protected readonly int[] InvAlphabet;

			public Base(uint charsCount, string alphabet, char special, Encoding encoding = null, bool parallel = false)
			{
				if (alphabet.Length != charsCount)
					throw new ArgumentException($"Base string should contain {charsCount} chars");

				for (int i = 0; i < charsCount; i++)
					for (int j = i + 1; j < charsCount; j++)
						if (alphabet[i] == alphabet[j])
							throw new ArgumentException("Base string should contain distinct chars");

				if (alphabet.Contains(special))
					throw new ArgumentException("Base string should not contain special char");

				CharsCount = charsCount;
				Alphabet = alphabet;
				Special = special;
				int bitsPerChar = LogBase2(charsCount);
				BlockBitsCount = LCM(bitsPerChar, 8);
				BlockCharsCount = BlockBitsCount / bitsPerChar;

				InvAlphabet = new int[Alphabet.Max() + 1];

				for (int i = 0; i < InvAlphabet.Length; i++)
					InvAlphabet[i] = -1;

				for (int i = 0; i < charsCount; i++)
					InvAlphabet[Alphabet[i]] = i;

				Encoding = encoding ?? Encoding.UTF8;
				Parallel = parallel;
			}

			public virtual string EncodeString(string data)
			{
				return Encode(Encoding.GetBytes(data));
			}

			public abstract string Encode(byte[] data);

			public virtual string DecodeToString(string data)
			{
				return Encoding.GetString(Decode(Regex.Replace(data, @"\r\n?|\n", "")));
			}

			public abstract byte[] Decode(string data);

			/// <summary>
			/// From: http://stackoverflow.com/a/600306/1046374
			/// </summary>
			/// <param name="x"></param>
			/// <returns></returns>
			public static bool IsPowerOf2(uint x)
			{
				return (x != 0) && ((x & (x - 1)) == 0);
			}

			/// <summary>
			/// From: http://stackoverflow.com/a/13569863/1046374
			/// </summary>
			public static int LCM(int a, int b)
			{
				int num1, num2;
				if (a > b)
				{
					num1 = a;
					num2 = b;
				}
				else
				{
					num1 = b;
					num2 = a;
				}

				for (int i = 1; i <= num2; i++)
					if ((num1 * i) % num2 == 0)
						return i * num1;
				return num2;
			}

			public static uint NextPowOf2(uint x)
			{
				x--;
				x |= x >> 1;
				x |= x >> 2;
				x |= x >> 4;
				x |= x >> 8;
				x |= x >> 16;
				x++;
				return x;
			}

			public static ulong IntPow(ulong x, int exp)
			{
				ulong result = 1;
				for (int i = 0; i < exp; i++)
					result *= x;
				return result;
			}

			public static BigInteger BigIntPow(BigInteger x, int exp)
			{
				BigInteger result = 1;
				for (int i = 0; i < exp; i++)
					result *= x;
				return result;
			}

			public static int LogBase2(uint x)
			{
				int r = 0;
				while ((x >>= 1) != 0)
					r++;
				return r;
			}

			public static int LogBase2(ulong x)
			{
				int r = 0;
				while ((x >>= 1) != 0)
					r++;
				return r;
			}

			public static int LogBaseN(uint x, uint n)
			{
				int r = 0;
				while ((x /= n) != 0)
					r++;
				return r;
			}

			public static int LogBaseN(ulong x, uint n)
			{
				int r = 0;
				while ((x /= n) != 0)
					r++;
				return r;
			}

			public static int GetOptimalBitsCount2(uint charsCount, out uint charsCountInBits,
				uint maxBitsCount = 64, bool base2BitsCount = false)
			{
				int result = 0;
				charsCountInBits = 0;
				int n1 = LogBase2(charsCount);
				double charsCountLog = Math.Log(2, charsCount);
				double maxRatio = 0;

				for (int n = n1; n <= maxBitsCount; n++)
				{
					if (base2BitsCount && n % 8 != 0)
						continue;

					uint l1 = (uint)Math.Ceiling(n * charsCountLog);
					double ratio = (double)n / l1;
					if (ratio > maxRatio)
					{
						maxRatio = ratio;
						result = n;
						charsCountInBits = l1;
					}
				}

				return result;
			}

			public static int GetOptimalBitsCount(uint charsCount, out uint charsCountInBits,
				uint maxBitsCount = 64, uint radix = 2)
			{
				int result = 0;
				charsCountInBits = 0;
				int n0 = LogBaseN(charsCount, radix);
				double charsCountLog = Math.Log(radix, charsCount);
				double maxRatio = 0;

				for (int n = n0; n <= maxBitsCount; n++)
				{
					uint k = (uint)Math.Ceiling(n * charsCountLog);
					double ratio = (double)n / k;
					if (ratio > maxRatio)
					{
						maxRatio = ratio;
						result = n;
						charsCountInBits = k;
					}
				}

				return result;
			}
		}
		public class ZBase32 : Base
		{
			public const string DefaultAlphabet = "ybndrfg8ejkmcpqxot1uwisza345h769";
			public const char DefaultSpecial = (char)0;
			public override bool HasSpecial => false;

			public ZBase32(string alphabet = DefaultAlphabet, char special = DefaultSpecial, Encoding textEncoding = null)
				: base(32, alphabet, special, textEncoding)
			{
			}

			public override string Encode(byte[] data)
			{
				unchecked
				{
					var encodedResult = new StringBuilder((int)Math.Ceiling(data.Length * 8.0 / 5.0));

					for (var i = 0; i < data.Length; i += 5)
					{
						var byteCount = Math.Min(5, data.Length - i);

						ulong buffer = 0;
						for (var j = 0; j < byteCount; ++j)
							buffer = (buffer << 8) | data[i + j];

						var bitCount = byteCount * 8;
						while (bitCount > 0)
						{
							var index = bitCount >= 5
										? (int)(buffer >> (bitCount - 5)) & 0x1f
										: (int)(buffer & (ulong)(0x1f >> (5 - bitCount))) << (5 - bitCount);

							encodedResult.Append(DefaultAlphabet[index]);
							bitCount -= 5;
						}
					}

					return encodedResult.ToString();
				}
			}

			public override byte[] Decode(string data)
			{
				if (string.IsNullOrEmpty(data))
					return new byte[0];

				var result = new List<byte>((int)Math.Ceiling(data.Length * 5.0 / 8.0));

				var index = new int[8];
				for (var i = 0; i < data.Length;)
				{
					i = CreateIndexByOctetAndMovePosition(ref data, i, ref index);

					var shortByteCount = 0;
					ulong buffer = 0;
					for (var j = 0; j < 8 && index[j] != -1; ++j)
					{
						buffer = (buffer << 5) | (ulong)(InvAlphabet[index[j]] & 0x1f);
						shortByteCount++;
					}

					var bitCount = shortByteCount * 5;
					while (bitCount >= 8)
					{
						result.Add((byte)((buffer >> (bitCount - 8)) & 0xff));
						bitCount -= 8;
					}
				}

				return result.ToArray();
			}

			private int CreateIndexByOctetAndMovePosition(ref string data, int currentPosition, ref int[] index)
			{
				var j = 0;
				while (j < 8)
				{
					if (currentPosition >= data.Length)
					{
						index[j++] = -1;
						continue;
					}
					if (InvAlphabet[data[currentPosition]] == -1)
					{
						currentPosition++;
						continue;
					}
					index[j] = data[currentPosition];
					j++;
					currentPosition++;
				}
				return currentPosition;
			}
		}
	}
}
