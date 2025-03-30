using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Humanizer;

namespace BlazorApp.Helpers
{
	public static class HumanizerExtensions
	{
		public static string ToHumanizedString(this object obj, int level = 0)
		{
			if (obj == null)
				return string.Empty;

			// If the object is an enumerable (but not a string), process each item recursively.
			if (obj is IEnumerable enumerable && !(obj is string))
			{
				var items = enumerable.Cast<object>()
									  .Select(item => item.ToHumanizedString(level + 1));
				// At level 1, use line breaks; otherwise commas.
				var separator = level == 0 ? Environment.NewLine : ", ";
				//var separator = Environment.NewLine;
				return string.Join(separator, items);
			}

			// If the object is a primitive type or string, just return its string representation.
			if (obj.GetType().IsPrimitive || obj is string || obj is DateTime || obj is decimal)
				return obj.ToString()!;

			// Process the object's properties.
			var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var humanizedProperties = properties
				.Select(p =>
				{
					var value = p.GetValue(obj);
					if (value == null) return null;

					// For DateTime, use round-trip format.
					string formattedValue = value is DateTime dt
						? dt.ToString("o")
						: (value.GetType().IsPrimitive || value is string
							? value.ToString()
							: value.ToHumanizedString(level + 1));

					return $"{p.Name.Humanize(LetterCasing.Title)}: {formattedValue}";
				})
				.Where(x => !string.IsNullOrWhiteSpace(x));

			// Use line breaks at recursion level 1, commas otherwise.
			var separatorForProperties = level == 0 ? Environment.NewLine : ", ";
			//var separatorForProperties = Environment.NewLine;
			return string.Join(separatorForProperties, humanizedProperties);
		}
	}
}
