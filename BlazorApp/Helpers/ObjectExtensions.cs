using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Humanizer;

namespace BlazorApp.Helpers
{
	public static class HumanizerExtensions
	{
		public static string ToHumanizedString(this object obj, string separator)
		{
			if (obj == null)
				return string.Empty;

			// If the object is an enumerable (but not a string), process each item.
			if (obj is IEnumerable enumerable && !(obj is string))
			{
				var items = enumerable.Cast<object>().Select(item => item?.ToString() ?? "null");
				return string.Join(separator, items);
			}

			// If the object is a primitive type, string, DateTime, or decimal, return its string representation.
			if (obj.GetType().IsPrimitive || obj is string || obj is DateTime || obj is decimal)
				return obj.ToString()!;

			// Process the object's public instance properties.
			var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var humanizedProperties = properties
				.Select(p =>
				{
					var value = p.GetValue(obj);
					return value != null ? $"{p.Name.Humanize(LetterCasing.Title)}: {value}" : null;
				})
				.Where(x => !string.IsNullOrWhiteSpace(x));

			return string.Join(separator, humanizedProperties);
		}
	}
}
