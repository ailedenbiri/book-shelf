using System;
namespace System.Reflection
{
	public static class SystemExtensions
	{
		public static Type GetType(this Assembly assembly, string name, bool throwOnError, bool ignoreCase, bool ignoreNamespace)
		{
			if (!ignoreNamespace)
			{
				if (assembly == null)
				{
					return null;
				}
				return assembly.GetType(name, throwOnError, ignoreCase);
			}
			else
			{
				Type type = assembly.GetType(name);
				if (type != null)
				{
					return type;
				}
				AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
				if (referencedAssemblies == null)
				{
					Type[] types = assembly.GetTypes();
					if (types == null)
					{
						return null;
					}
					Type[] array = types;
					for (int i = 0; i < array.Length; i++)
					{
						Type type2 = array[i];
						if (type2.FullName == name)
						{
							return type2;
						}
					}
					Type[] array2 = types;
					for (int j = 0; j < array2.Length; j++)
					{
						Type type3 = array2[j];
						if (type3.Name == name)
						{
							return type3;
						}
					}
				}
				else
				{
					AssemblyName[] array3 = referencedAssemblies;
					for (int k = 0; k < array3.Length; k++)
					{
						AssemblyName assemblyRef = array3[k];
						Assembly assembly2 = Assembly.Load(assemblyRef);
						if (!(assembly2 == null))
						{
							Type[] types2 = assembly2.GetTypes();
							if (types2 == null)
							{
								return null;
							}
							Type[] array4 = types2;
							for (int l = 0; l < array4.Length; l++)
							{
								Type type4 = array4[l];
								if (type4.FullName == name)
								{
									return type4;
								}
							}
							Type[] array5 = types2;
							for (int m = 0; m < array5.Length; m++)
							{
								Type type5 = array5[m];
								if (type5.Name == name)
								{
									return type5;
								}
							}
						}
					}
				}
				return null;
			}
		}
	}
}
