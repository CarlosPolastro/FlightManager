using System;
using System.Collections.Generic;

namespace FlightManager.Web.Extensions
{
	public static class ListExtension
	{
		public static List<T> DefaultIfNull<T>(this List<T> list, List<T> newValue = null)
		{
			if(list == null)
			{
				if (newValue != null)
					list = newValue;
				else
					list = new List<T>();
            }

			return list;
		}
	}
}

