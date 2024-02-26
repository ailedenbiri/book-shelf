using System;
namespace UnityEngine.Binary
{
	public static class UnityEngineExtensions
	{
		public static byte[] ToBinary(this AudioClip audioClip)
		{
			if (audioClip == null)
			{
				return new byte[0];
			}
			try
			{
				float[] array = new float[audioClip.samples * audioClip.channels];
				audioClip.GetData(array, 0);
				byte[] array2 = new byte[array.Length * 4];
				Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
				return array2;
			}
			catch
			{
			}
			return new byte[0];
		}
		public static AudioClip ToAudioClip(this byte[] bytes, int sampleRate = 44100)
		{
			if (bytes == null)
			{
				return null;
			}
			try
			{
				float[] array = new float[bytes.Length / 4];
				Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
				AudioClip audioClip = AudioClip.Create(Guid.NewGuid().ToString("N"), array.Length, 1, sampleRate, false);
				audioClip.SetData(array, 0);
				return audioClip;
			}
			catch
			{
			}
			return null;
		}
		public static byte[] ToBinary(this Texture2D texture)
		{
			if (texture == null)
			{
				return new byte[0];
			}
			try
			{
				return texture.GetRawTextureData();
			}
			catch
			{
			}
			return new byte[0];
		}
		public static Texture2D ToTexture(this byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			try
			{
				Texture2D texture2D = new Texture2D(1, 1);
				ImageConversion.LoadImage(texture2D, bytes, true);
				return texture2D;
			}
			catch
			{
			}
			return null;
		}
		public static byte[] ToBinary(this Vector2 vector)
		{
			byte[] array = new byte[8];
			try
			{
				Buffer.BlockCopy(BitConverter.GetBytes(vector.x), 0, array, 0, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(vector.y), 0, array, 4, 4);
				return array;
			}
			catch
			{
			}
			return new byte[0];
		}
		public static byte[] ToBinary(this Vector3 vector)
		{
			byte[] array = new byte[12];
			try
			{
				Buffer.BlockCopy(BitConverter.GetBytes(vector.x), 0, array, 0, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(vector.y), 0, array, 4, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(vector.z), 0, array, 8, 4);
				return array;
			}
			catch
			{
			}
			return new byte[0];
		}
		public static byte[] ToBinary(this Vector4 vector)
		{
			byte[] array = new byte[16];
			try
			{
				Buffer.BlockCopy(BitConverter.GetBytes(vector.x), 0, array, 0, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(vector.y), 0, array, 4, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(vector.z), 0, array, 8, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(vector.w), 0, array, 12, 4);
				return array;
			}
			catch
			{
			}
			return new byte[0];
		}
		public static byte[] ToBinary(this Rect rect)
		{
			byte[] array = new byte[16];
			try
			{
				Buffer.BlockCopy(BitConverter.GetBytes(rect.x), 0, array, 0, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(rect.y), 0, array, 4, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(rect.width), 0, array, 8, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(rect.height), 0, array, 12, 4);
				return array;
			}
			catch
			{
			}
			return new byte[0];
		}
		public static byte[] ToBinary(this Color color)
		{
			byte[] array = new byte[16];
			try
			{
				Buffer.BlockCopy(BitConverter.GetBytes(color.r), 0, array, 0, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(color.g), 0, array, 4, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(color.b), 0, array, 8, 4);
				Buffer.BlockCopy(BitConverter.GetBytes(color.a), 0, array, 12, 4);
				return array;
			}
			catch
			{
			}
			return new byte[0];
		}
		public static Vector2 ToVector2(this byte[] bytes)
		{
			if (bytes == null)
			{
				return Vector2.zero;
			}
			try
			{
				return new Vector2(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, 4));
			}
			catch
			{
			}
			return Vector2.zero;
		}
		public static Vector3 ToVector3(this byte[] bytes)
		{
			if (bytes == null)
			{
				return Vector3.zero;
			}
			try
			{
				return new Vector3(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, 4), BitConverter.ToSingle(bytes, 8));
			}
			catch
			{
			}
			return Vector3.zero;
		}
		public static Vector4 ToVector4(this byte[] bytes)
		{
			if (bytes == null)
			{
				return Vector4.zero;
			}
			try
			{
				return new Vector4(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, 4), BitConverter.ToSingle(bytes, 8), BitConverter.ToSingle(bytes, 12));
			}
			catch
			{
			}
			return Vector4.zero;
		}
		public static Rect ToRect(this byte[] bytes)
		{
			if (bytes == null)
			{
				return Rect.zero;
			}
			try
			{
				return new Rect(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, 4), BitConverter.ToSingle(bytes, 8), BitConverter.ToSingle(bytes, 12));
			}
			catch
			{
			}
			return Rect.zero;
		}
		public static Color ToColor(this byte[] bytes)
		{
			if (bytes == null)
			{
				return Color.black;
			}
			try
			{
				return new Color(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, 4), BitConverter.ToSingle(bytes, 8), BitConverter.ToSingle(bytes, 12));
			}
			catch
			{
			}
			return Color.black;
		}
	}
}
