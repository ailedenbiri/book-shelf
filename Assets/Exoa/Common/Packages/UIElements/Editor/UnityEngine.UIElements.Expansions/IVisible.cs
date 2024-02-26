using System;
using System.Threading.Tasks;
namespace UnityEngine.UIElements.Expansions
{
	public interface IVisible
	{
		bool IsShow
		{
			get;
		}
		Task Show(int duration = -1);
		Task Hide(int duration = -1);
		Task Toggle(int duration = -1);
	}
}
