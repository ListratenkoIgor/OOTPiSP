using PluginInterface;
using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Base64_Plugin
{
	[Plugin(PluginType.Coding)]
	public class MyPlugin : ICoding
	{
		public string Name
		{
			get { return "Base64 Encoding Plugin"; }
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
			get { return "Base64"; }
		}
		static byte[] GetBytes(string randomKeyText)
		{
			byte[] bytes = new byte[randomKeyText.Length * sizeof(char)];
			System.Buffer.BlockCopy(randomKeyText.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}
		public byte[] Encode(byte[] data)
		{
			Base64 zBase32 = new Base64();
			return GetBytes(zBase32.Encode(data));
		}
		public byte[] Decode(byte[] data)
		{
			Base64 zBase32 = new Base64();
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
		public class Base64 : Base
		{
			public const string DefaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
			public const char DefaultSpecial = '=';

			public override bool HasSpecial
			{
				get { return true; }
			}

			public Base64(string alphabet = DefaultAlphabet, char special = DefaultSpecial,
				Encoding textEncoding = null, bool parallel = false)
				: base(64, alphabet, special, textEncoding, parallel)
			{
			}

			public override string Encode(byte[] data)
			{
				int resultLength = (data.Length + 2) / 3 * 4;
				char[] result = new char[resultLength];

				int length3 = data.Length / 3;
				if (!Parallel)
				{
					EncodeBlock(data, result, 0, length3);
				}
				else
				{
					int processorCount = Math.Min(length3, Environment.ProcessorCount);
					System.Threading.Tasks.Parallel.For(0, processorCount, i =>
					{
						int beginInd = i * length3 / processorCount;
						int endInd = (i + 1) * length3 / processorCount;
						EncodeBlock(data, result, beginInd, endInd);
					});
				}

				int ind;
				int x1, x2;
				int srcInd, dstInd;
				switch (data.Length - length3 * 3)
				{
					case 1:
						ind = length3;
						srcInd = ind * 3;
						dstInd = ind * 4;
						x1 = data[srcInd];
						result[dstInd] = Alphabet[x1 >> 2];
						result[dstInd + 1] = Alphabet[(x1 << 4) & 0x30];
						result[dstInd + 2] = Special;
						result[dstInd + 3] = Special;
						break;
					case 2:
						ind = length3;
						srcInd = ind * 3;
						dstInd = ind * 4;
						x1 = data[srcInd];
						x2 = data[srcInd + 1];
						result[dstInd] = Alphabet[x1 >> 2];
						result[dstInd + 1] = Alphabet[((x1 << 4) & 0x30) | (x2 >> 4)];
						result[dstInd + 2] = Alphabet[(x2 << 2) & 0x3C];
						result[dstInd + 3] = Special;
						break;
				}

				return new string(result);
			}
			public override byte[] Decode(string data)
			{
				unchecked
				{
					if (string.IsNullOrEmpty(data))
						return new byte[0];

					int lastSpecialInd = data.Length;
					while (data[lastSpecialInd - 1] == Special)
						lastSpecialInd--;
					int tailLength = data.Length - lastSpecialInd;

					int resultLength = (data.Length + 3) / 4 * 3 - tailLength;
					byte[] result = new byte[resultLength];

					int length4 = (data.Length - tailLength) / 4;
					if (!Parallel)
					{
						DecodeBlock(data, result, 0, length4);
					}
					else
					{
						int processorCount = Math.Min(length4, Environment.ProcessorCount);
						System.Threading.Tasks.Parallel.For(0, processorCount, i =>
						{
							int beginInd = i * length4 / processorCount;
							int endInd = (i + 1) * length4 / processorCount;
							DecodeBlock(data, result, beginInd, endInd);
						});
					}

					int ind;
					int x1, x2, x3;
					int srcInd, dstInd;
					switch (tailLength)
					{
						case 2:
							ind = length4;
							srcInd = ind * 4;
							dstInd = ind * 3;
							x1 = InvAlphabet[data[srcInd]];
							x2 = InvAlphabet[data[srcInd + 1]];
							result[dstInd] = (byte)((x1 << 2) | ((x2 >> 4) & 0x3));
							break;
						case 1:
							ind = length4;
							srcInd = ind * 4;
							dstInd = ind * 3;
							x1 = InvAlphabet[data[srcInd]];
							x2 = InvAlphabet[data[srcInd + 1]];
							x3 = InvAlphabet[data[srcInd + 2]];
							result[dstInd] = (byte)((x1 << 2) | ((x2 >> 4) & 0x3));
							result[dstInd + 1] = (byte)((x2 << 4) | ((x3 >> 2) & 0xF));
							break;
					}

					return result;
				}
			}
			private void EncodeBlock(byte[] src, char[] dst, int beginInd, int endInd)
			{
				for (int ind = beginInd; ind < endInd; ind++)
				{
					int srcInd = ind * 3;
					int dstInd = ind * 4;

					byte x1 = src[srcInd];
					byte x2 = src[srcInd + 1];
					byte x3 = src[srcInd + 2];

					dst[dstInd] = Alphabet[x1 >> 2];
					dst[dstInd + 1] = Alphabet[((x1 << 4) & 0x30) | (x2 >> 4)];
					dst[dstInd + 2] = Alphabet[((x2 << 2) & 0x3C) | (x3 >> 6)];
					dst[dstInd + 3] = Alphabet[x3 & 0x3F];
				}
			}
			private void DecodeBlock(string src, byte[] dst, int beginInd, int endInd)
			{
				unchecked
				{
					for (int ind = beginInd; ind < endInd; ind++)
					{
						int srcInd = ind * 4;
						int dstInd = ind * 3;

						int x1 = InvAlphabet[src[srcInd]];
						int x2 = InvAlphabet[src[srcInd + 1]];
						int x3 = InvAlphabet[src[srcInd + 2]];
						int x4 = InvAlphabet[src[srcInd + 3]];

						dst[dstInd] = (byte)((x1 << 2) | ((x2 >> 4) & 0x3));
						dst[dstInd + 1] = (byte)((x2 << 4) | ((x3 >> 2) & 0xF));
						dst[dstInd + 2] = (byte)((x3 << 6) | (x4 & 0x3F));
					}
				}
			}
		}

	}
}
