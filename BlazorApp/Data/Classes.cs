namespace BlazorApp.Data
{
	public class Classes
	{
		public class PatientName
		{
			public string? FamilyName { get; set; }
			public string? GivenName { get; set; }
			public string? SecondAndFurtherGivenNames { get; set; }
			public string? Suffix { get; set; }
			public string? Prefix { get; set; }
			public string? Degree { get; set; }
			public string? NameTypeCode { get; set; }
			public string? NameRepresentationCode { get; set; }
			public string? NameContext { get; set; }
			public string? NameValidityRange { get; set; }
			public string? NameAssemblyOrder { get; set; }
			public DateTime? EffectiveDate { get; set; }
			public DateTime? ExpirationDate { get; set; }
			public string? ProfessionalSuffix { get; set; }
		}
	}
}
