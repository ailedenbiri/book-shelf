using System;
namespace UnityEngine.UIElements.Expansions
{
	public interface IPlayback
	{
		bool IsPlaying
		{
			get;
		}
		void Play();
		void Stop();
		void Pause();
	}
}
