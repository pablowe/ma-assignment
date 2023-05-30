using System.Collections.Generic;
using System.Linq;

public static class ServiceLocator
{
	private static readonly HashSet<object> registeredObjects;

	static ServiceLocator()
	{
		registeredObjects = new HashSet<object>();
	}

	public static void Register<T>(T obj) where T : class
	{
		registeredObjects.Add(obj);
	}

	public static void Unregister<T>(T obj) where T : class
	{
		registeredObjects.Remove(obj);
	}

	public static T ResolveAndGet<T>() where T : class
	{
		var obj = registeredObjects.SingleOrDefault(x => x is T);
		
		return obj as T;
	}
}
