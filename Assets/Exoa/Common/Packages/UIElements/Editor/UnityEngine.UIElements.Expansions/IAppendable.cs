using System;
namespace UnityEngine.UIElements.Expansions
{
	public interface IAppendable
	{
		void Append(string templateId, string name = null);
		void Remove(string name = null);
		void Clear();
		void RemoveAt(int index);
	}
}
