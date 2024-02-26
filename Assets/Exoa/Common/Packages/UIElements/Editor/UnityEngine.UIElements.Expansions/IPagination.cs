using System;
namespace UnityEngine.UIElements.Expansions
{
	public interface IPagination
	{
		int position
		{
			get;
		}
		void Next();
		void Prev();
	}
}
