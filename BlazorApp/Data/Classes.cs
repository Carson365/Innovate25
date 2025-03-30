using BlazorApp.Helpers;
using Humanizer;
using System.Text;

namespace BlazorApp.Data
{
	public class Classes
	{
		public class XPN
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

			public static XPN FromDelimitedFields(List<string> fields)
			{
				return new XPN
				{
					FamilyName = fields.ElementAtOrDefault(0),
					GivenName = fields.ElementAtOrDefault(1),
					SecondAndFurtherGivenNames = fields.ElementAtOrDefault(2),
					Suffix = fields.ElementAtOrDefault(3),
					Prefix = fields.ElementAtOrDefault(4),
					Degree = fields.ElementAtOrDefault(5),
					NameTypeCode = fields.ElementAtOrDefault(6),
					NameRepresentationCode = fields.ElementAtOrDefault(7),
					NameContext = fields.ElementAtOrDefault(8),
					NameValidityRange = fields.ElementAtOrDefault(9),
					NameAssemblyOrder = fields.ElementAtOrDefault(10),
					EffectiveDate = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(11)),
					ExpirationDate = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(12)),
					ProfessionalSuffix = fields.ElementAtOrDefault(13)
				};
			}
			public override string ToString()
			{
				var sb = new StringBuilder();
				if (!string.IsNullOrEmpty(Prefix)) sb.Append(Prefix).Append(" ");
				if (!string.IsNullOrEmpty(GivenName)) sb.Append(GivenName).Append(" ");
				if (!string.IsNullOrEmpty(SecondAndFurtherGivenNames)) sb.Append(SecondAndFurtherGivenNames).Append(" ");
				if (!string.IsNullOrEmpty(FamilyName)) sb.Append(FamilyName).Append(" ");
				if (!string.IsNullOrEmpty(Suffix)) sb.Append(", ").Append(Suffix);
				if (!string.IsNullOrEmpty(ProfessionalSuffix)) sb.Append(", ").Append(ProfessionalSuffix);
				return sb.ToString().Trim().Humanize(LetterCasing.Title);
			}
		}

		public class MSG
		{
			public required string MessageCode { get; set; }
			public required string TriggerEvent { get; set; }
			public required string MessageStructure { get; set; }

			public static MSG FromDelimitedFields(List<string> fields)
			{
				return new MSG
				{
					MessageCode = fields.ElementAtOrDefault(0) ?? string.Empty,
					TriggerEvent = fields.ElementAtOrDefault(1) ?? string.Empty,
					MessageStructure = fields.ElementAtOrDefault(2) ?? string.Empty
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}

		public class EI
		{
			public string? EntityIdentifier { get; set; } // Length: 199, Data Type: ST, Optional
			public string? NamespaceId { get; set; } // Length: 20, Data Type: IS, Optional, Table: 0363
			public string? UniversalId { get; set; } // Length: 199, Data Type: ST, Conditional
			public string? UniversalIdType { get; set; } // Length: 6, Data Type: ID, Conditional, Table: 0301

			public static EI FromDelimitedFields(List<string> fields)
			{
				return new EI
				{
					EntityIdentifier = fields.ElementAtOrDefault(0),
					NamespaceId = fields.ElementAtOrDefault(1),
					UniversalId = fields.ElementAtOrDefault(2),
					UniversalIdType = fields.ElementAtOrDefault(3)
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}

		public class XCN
		{
			public string? IdNumber { get; set; } // Length: 15, Data Type: ST, Optional
			public string? FamilyName { get; set; } // Length: 194, Data Type: FN, Optional
			public string? GivenName { get; set; } // Length: 30, Data Type: ST, Optional, Table: FirstName
			public string? SecondAndFurtherGivenNamesOrInitialsThereof { get; set; } // Length: 30, Data Type: ST, Optional
			public string? Suffix { get; set; } // Length: 20, Data Type: ST, Optional
			public string? Prefix { get; set; } // Length: 20, Data Type: ST, Optional
			public string? Degree { get; set; } // Length: 5, Data Type: IS, Both, Table: 0360
			public string? SourceTable { get; set; } // Length: 4, Data Type: IS, Conditional, Table: 0297
			public string? AssigningAuthority { get; set; } // Length: 227, Data Type: HD, Optional, Table: 0363
			public string? NameTypeCode { get; set; } // Length: 1, Data Type: ID, Optional, Table: 0200
			public string? IdentifierCheckDigit { get; set; } // Length: 1, Data Type: ST, Optional
			public string? CheckDigitScheme { get; set; } // Length: 3, Data Type: ID, Conditional, Table: 0061
			public string? IdentifierTypeCode { get; set; } // Length: 5, Data Type: ID, Optional, Table: 0203
			public string? AssigningFacility { get; set; } // Length: 227, Data Type: HD, Optional
			public string? NameRepresentationCode { get; set; } // Length: 1, Data Type: ID, Optional, Table: 0465
			public string? NameContext { get; set; } // Length: 483, Data Type: CE, Optional, Table: 0448
			public string? NameValidityRange { get; set; } // Length: 53, Data Type: DR, Both
			public string? NameAssemblyOrder { get; set; } // Length: 1, Data Type: ID, Optional, Table: 0444
			public DateTime? EffectiveDate { get; set; } // Length: 26, Data Type: TS, Optional
			public DateTime? ExpirationDate { get; set; } // Length: 26, Data Type: TS, Optional
			public string? ProfessionalSuffix { get; set; } // Length: 199, Data Type: ST, Optional
			public string? AssigningJurisdiction { get; set; } // Length: 705, Data Type: CWE, Optional
			public string? AssigningAgencyOrDepartment { get; set; } // Length: 705, Data Type: CWE, Optional

			public static XCN FromDelimitedFields(List<string> fields)
			{
				return new XCN
				{
					IdNumber = fields.ElementAtOrDefault(0),
					FamilyName = fields.ElementAtOrDefault(1),
					GivenName = fields.ElementAtOrDefault(2),
					SecondAndFurtherGivenNamesOrInitialsThereof = fields.ElementAtOrDefault(3),
					Suffix = fields.ElementAtOrDefault(4),
					Prefix = fields.ElementAtOrDefault(5),
					Degree = fields.ElementAtOrDefault(6),
					SourceTable = fields.ElementAtOrDefault(7),
					AssigningAuthority = fields.ElementAtOrDefault(8),
					NameTypeCode = fields.ElementAtOrDefault(9),
					IdentifierCheckDigit = fields.ElementAtOrDefault(10),
					CheckDigitScheme = fields.ElementAtOrDefault(11),
					IdentifierTypeCode = fields.ElementAtOrDefault(12),
					AssigningFacility = fields.ElementAtOrDefault(13),
					NameRepresentationCode = fields.ElementAtOrDefault(14),
					NameContext = fields.ElementAtOrDefault(15),
					NameValidityRange = fields.ElementAtOrDefault(16),
					NameAssemblyOrder = fields.ElementAtOrDefault(17),
					EffectiveDate = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(18)),
					ExpirationDate = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(19)),
					ProfessionalSuffix = fields.ElementAtOrDefault(20),
					AssigningJurisdiction = fields.ElementAtOrDefault(21),
					AssigningAgencyOrDepartment = fields.ElementAtOrDefault(22)
				};
			}
			public override string ToString()
			{
				//return this.ToHumanizedString(", ");

				var sb = new StringBuilder();
				if (!string.IsNullOrEmpty(Prefix)) sb.Append(Prefix).Append(" ");
				if (!string.IsNullOrEmpty(GivenName)) sb.Append(GivenName).Append(" ");
				if (!string.IsNullOrEmpty(SecondAndFurtherGivenNamesOrInitialsThereof)) sb.Append(SecondAndFurtherGivenNamesOrInitialsThereof).Append(" ");
				if (!string.IsNullOrEmpty(FamilyName)) sb.Append(FamilyName).Append(" ");
				if (!string.IsNullOrEmpty(Suffix)) sb.Append(", ").Append(Suffix);
				if (!string.IsNullOrEmpty(ProfessionalSuffix)) sb.Append(", ").Append(ProfessionalSuffix);
				return sb.ToString().Trim().Humanize(LetterCasing.Title);
			}
		}

		public class CX
		{
			public string IdNumber { get; set; } // Length: 15, Data Type: ST, Required
			public string? CheckDigit { get; set; } // Length: 1, Data Type: ST, Optional
			public string? CheckDigitScheme { get; set; } // Length: 3, Data Type: ID, Optional, Table: 0061
			public string? AssigningAuthority { get; set; } // Length: 227, Data Type: HD, Optional, Table: 0363
			public string? IdentifierTypeCode { get; set; } // Length: 5, Data Type: ID, Optional, Table: 0203
			public string? AssigningFacility { get; set; } // Length: 227, Data Type: HD, Optional
			public DateTime? EffectiveDate { get; set; } // Length: 8, Data Type: DT, Optional
			public DateTime? ExpirationDate { get; set; } // Length: 8, Data Type: DT, Optional
			public string? AssigningJurisdiction { get; set; } // Length: 705, Data Type: CWE, Optional
			public string? AssigningAgencyOrDepartment { get; set; } // Length: 705, Data Type: CWE, Optional

			public static CX FromDelimitedFields(List<string> fields)
			{
				return new CX
				{
					IdNumber = fields.ElementAtOrDefault(0) ?? string.Empty,
					CheckDigit = fields.ElementAtOrDefault(1),
					CheckDigitScheme = fields.ElementAtOrDefault(2),
					AssigningAuthority = fields.ElementAtOrDefault(3),
					IdentifierTypeCode = fields.ElementAtOrDefault(4),
					AssigningFacility = fields.ElementAtOrDefault(5),
					EffectiveDate = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(6)),
					ExpirationDate = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(7)),
					AssigningJurisdiction = fields.ElementAtOrDefault(8),
					AssigningAgencyOrDepartment = fields.ElementAtOrDefault(9)
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}
		public class CE
		{
			public string? Identifier { get; set; } // Length: 20, Data Type: ST, Optional
			public string? Text { get; set; } // Length: 199, Data Type: ST, Optional
			public string? NameOfCodingSystem { get; set; } // Length: 20, Data Type: ID, Optional, Table: 0396
			public string? AlternateIdentifier { get; set; } // Length: 20, Data Type: ST, Optional
			public string? AlternateText { get; set; } // Length: 199, Data Type: ST, Optional
			public string? NameOfAlternateCodingSystem { get; set; } // Length: 20, Data Type: ID, Optional, Table: 0396

			public static CE FromDelimitedFields(List<string> fields)
			{
				return new CE
				{
					Identifier = fields.ElementAtOrDefault(0),
					Text = fields.ElementAtOrDefault(1),
					NameOfCodingSystem = fields.ElementAtOrDefault(2),
					AlternateIdentifier = fields.ElementAtOrDefault(3),
					AlternateText = fields.ElementAtOrDefault(4),
					NameOfAlternateCodingSystem = fields.ElementAtOrDefault(5)
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}


		public class XAD
		{
			public string? StreetAddress { get; set; } // Length: 184, Data Type: SAD, Optional, Table: Street
			public string? OtherDesignation { get; set; } // Length: 120, Data Type: ST, Optional
			public string? City { get; set; } // Length: 50, Data Type: ST, Optional, Table: City
			public string? StateOrProvince { get; set; } // Length: 50, Data Type: ST, Optional, Table: State
			public string? ZipOrPostalCode { get; set; } // Length: 12, Data Type: ST, Optional, Table: ZipCode
			public string? Country { get; set; } // Length: 3, Data Type: ID, Optional, Table: 0399
			public string? AddressType { get; set; } // Length: 3, Data Type: ID, Optional, Table: 0190
			public string? OtherGeographicDesignation { get; set; } // Length: 50, Data Type: ST, Optional
			public string? CountyParishCode { get; set; } // Length: 20, Data Type: IS, Optional, Table: 0289
			public string? CensusTract { get; set; } // Length: 20, Data Type: IS, Optional, Table: 0288
			public string? AddressRepresentationCode { get; set; } // Length: 1, Data Type: ID, Optional, Table: 0465
			public string? AddressValidityRange { get; set; } // Length: 53, Data Type: DR, Both
			public DateTime? EffectiveDate { get; set; } // Length: 26, Data Type: TS, Optional
			public DateTime? ExpirationDate { get; set; } // Length: 26, Data Type: TS, Optional

			public static XAD FromDelimitedFields(List<string> fields)
			{
				return new XAD
				{
					StreetAddress = fields.ElementAtOrDefault(0),
					OtherDesignation = fields.ElementAtOrDefault(1),
					City = fields.ElementAtOrDefault(2),
					StateOrProvince = fields.ElementAtOrDefault(3),
					ZipOrPostalCode = fields.ElementAtOrDefault(4),
					Country = fields.ElementAtOrDefault(5),
					AddressType = fields.ElementAtOrDefault(6),
					OtherGeographicDesignation = fields.ElementAtOrDefault(7),
					CountyParishCode = fields.ElementAtOrDefault(8),
					CensusTract = fields.ElementAtOrDefault(9),
					AddressRepresentationCode = fields.ElementAtOrDefault(10),
					AddressValidityRange = fields.ElementAtOrDefault(11),
					EffectiveDate = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(12)),
					ExpirationDate = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(13)),
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}
		public class XTN
		{
			public string? TelephoneNumber { get; set; } // Length: 199, Data Type: ST, Both, Table: PhoneNumber
			public string? TelecommunicationUseCode { get; set; } // Length: 3, Data Type: ID, Optional, Table: 0201
			public string? TelecommunicationEquipmentType { get; set; } // Length: 8, Data Type: ID, Optional, Table: 0202
			public string? EmailAddress { get; set; } // Length: 199, Data Type: ST, Optional
			public int? CountryCode { get; set; } // Length: 3, Data Type: NM, Optional
			public int? AreaCityCode { get; set; } // Length: 5, Data Type: NM, Optional
			public int? LocalNumber { get; set; } // Length: 9, Data Type: NM, Optional
			public int? Extension { get; set; } // Length: 5, Data Type: NM, Optional
			public string? AnyText { get; set; } // Length: 199, Data Type: ST, Optional
			public string? ExtensionPrefix { get; set; } // Length: 4, Data Type: ST, Optional
			public string? SpeedDialCode { get; set; } // Length: 6, Data Type: ST, Optional
			public string? UnformattedTelephoneNumber { get; set; } // Length: 199, Data Type: ST, Conditional

			public static XTN FromDelimitedFields(List<string> fields)
			{
				return new XTN
				{
					TelephoneNumber = fields.ElementAtOrDefault(0),
					TelecommunicationUseCode = fields.ElementAtOrDefault(1),
					TelecommunicationEquipmentType = fields.ElementAtOrDefault(2),
					EmailAddress = fields.ElementAtOrDefault(3),
					CountryCode = int.TryParse(fields.ElementAtOrDefault(4), out int countryCode) ? countryCode : null,
					AreaCityCode = int.TryParse(fields.ElementAtOrDefault(5), out int areaCityCode) ? areaCityCode : null,
					LocalNumber = int.TryParse(fields.ElementAtOrDefault(6), out int localNumber) ? localNumber : null,
					Extension = int.TryParse(fields.ElementAtOrDefault(7), out int extension) ? extension : null,
					AnyText = fields.ElementAtOrDefault(8),
					ExtensionPrefix = fields.ElementAtOrDefault(9),
					SpeedDialCode = fields.ElementAtOrDefault(10),
					UnformattedTelephoneNumber = fields.ElementAtOrDefault(11)
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}
		public class CWE
		{
			public string? Identifier { get; set; } // Length: 20, Data Type: ST, Optional
			public string? Text { get; set; } // Length: 199, Data Type: ST, Optional
			public string? NameOfCodingSystem { get; set; } // Length: 20, Data Type: ID, Optional, Table: 0396
			public string? AlternateIdentifier { get; set; } // Length: 20, Data Type: ST, Optional
			public string? AlternateText { get; set; } // Length: 199, Data Type: ST, Optional
			public string? NameOfAlternateCodingSystem { get; set; } // Length: 20, Data Type: ID, Optional, Table: 0396
			public string? CodingSystemVersionId { get; set; } // Length: 10, Data Type: ST, Conditional
			public string? AlternateCodingSystemVersionId { get; set; } // Length: 10, Data Type: ST, Optional
			public string? OriginalText { get; set; } // Length: 199, Data Type: ST, Optional

			public static CWE FromDelimitedFields(List<string> fields)
			{
				return new CWE
				{
					Identifier = fields.ElementAtOrDefault(0),
					Text = fields.ElementAtOrDefault(1),
					NameOfCodingSystem = fields.ElementAtOrDefault(2),
					AlternateIdentifier = fields.ElementAtOrDefault(3),
					AlternateText = fields.ElementAtOrDefault(4),
					NameOfAlternateCodingSystem = fields.ElementAtOrDefault(5),
					CodingSystemVersionId = fields.ElementAtOrDefault(6),
					AlternateCodingSystemVersionId = fields.ElementAtOrDefault(7),
					OriginalText = fields.ElementAtOrDefault(8)
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}


		public class NDL
		{
			public string? Name { get; set; } // NDL.1 - Optional
			public DateTime? StartDateTime { get; set; } // NDL.2 - Optional
			public DateTime? EndDateTime { get; set; } // NDL.3 - Optional
			public string? PointOfCare { get; set; } // NDL.4 - Optional (Table 0302)
			public string? Room { get; set; } // NDL.5 - Optional (Table 0303)
			public string? Bed { get; set; } // NDL.6 - Optional (Table 0304)
			public string? Facility { get; set; } // NDL.7 - Optional
			public string? LocationStatus { get; set; } // NDL.8 - Optional (Table 0306)
			public string? PatientLocationType { get; set; } // NDL.9 - Optional (Table 0305)
			public string? Building { get; set; } // NDL.10 - Optional (Table 0307)
			public string? Floor { get; set; } // NDL.11 - Optional (Table 0308)

			public static NDL FromDelimitedFields(List<string> fields)
			{
				return new NDL
				{
					Name = fields.ElementAtOrDefault(0),
					StartDateTime = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(1)),
					EndDateTime = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(2)),
					PointOfCare = fields.ElementAtOrDefault(3),
					Room = fields.ElementAtOrDefault(4),
					Bed = fields.ElementAtOrDefault(5),
					Facility = fields.ElementAtOrDefault(6),
					LocationStatus = fields.ElementAtOrDefault(7),
					PatientLocationType = fields.ElementAtOrDefault(8),
					Building = fields.ElementAtOrDefault(9),
					Floor = fields.ElementAtOrDefault(10)
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}

		public class TQ
		{
			public string? Quantity { get; set; } // TQ.1 - Optional (CQ: Composite Quantity)
			public string? Interval { get; set; } // TQ.2 - Optional (RI: Repeating Interval)
			public string? Duration { get; set; } // TQ.3 - Optional (ST: String)
			public DateTime? StartDateTime { get; set; } // TQ.4 - Optional (TS: Timestamp)
			public DateTime? EndDateTime { get; set; } // TQ.5 - Optional (TS: Timestamp)
			public string? Priority { get; set; } // TQ.6 - Optional (ST: String)
			public string? Condition { get; set; } // TQ.7 - Optional (ST: String)
			public string? Text { get; set; } // TQ.8 - Optional (TX: Text)
			public string? Conjunction { get; set; } // TQ.9 - Optional (ID: Identifier, Table 0472)
			public string? OrderSequencing { get; set; } // TQ.10 - Optional (OSD: Order Sequencing Definition)
			public string? OccurrenceDuration { get; set; } // TQ.11 - Optional (CE: Coded Element)
			public int? TotalOccurrences { get; set; } // TQ.12 - Optional (NM: Numeric)

			public static TQ FromDelimitedFields(List<string> fields)
			{
				return new TQ
				{
					Quantity = fields.ElementAtOrDefault(0),
					Interval = fields.ElementAtOrDefault(1),
					Duration = fields.ElementAtOrDefault(2),
					StartDateTime = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(3)),
					EndDateTime = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(4)),
					Priority = fields.ElementAtOrDefault(5),
					Condition = fields.ElementAtOrDefault(6),
					Text = fields.ElementAtOrDefault(7),
					Conjunction = fields.ElementAtOrDefault(8),
					OrderSequencing = fields.ElementAtOrDefault(9),
					OccurrenceDuration = fields.ElementAtOrDefault(10),
					TotalOccurrences = int.TryParse(fields.ElementAtOrDefault(11), out int totalOccurrences) ? totalOccurrences : null
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}



		public class TS
		{
			public required DateTime Time { get; set; } // TS.1 - Required (DTM: Date/Time)
			public string? DegreeOfPrecision { get; set; } // TS.2 - Business Rule Dependent (Table 0529)

			public static TS FromDelimitedFields(List<string> fields)
			{
				return new TS
				{
					Time = (DateTime)EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(0)),
					DegreeOfPrecision = fields.ElementAtOrDefault(1)
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}

		public class FC
		{
			public required string FinancialClassCode { get; set; } // FC.1 - Required (Table 0064)
			public DateTime? EffectiveDate { get; set; } // FC.2 - Optional (TS: Timestamp)

			public static FC FromDelimitedFields(List<string> fields)
			{
				return new FC
				{
					FinancialClassCode = fields.ElementAtOrDefault(0) ?? string.Empty,
					EffectiveDate = EmployeeService.ParseHL7Date(fields.ElementAtOrDefault(1)),
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}


		public class PL
		{
			public string? PointOfCare { get; set; } // PL.1 - Optional (Table 0302)
			public string? Room { get; set; } // PL.2 - Optional (Table 0303)
			public string? Bed { get; set; } // PL.3 - Optional (Table 0304)
			public string? Facility { get; set; } // PL.4 - Optional
			public string? LocationStatus { get; set; } // PL.5 - Optional (Table 0306)
			public string? PersonLocationType { get; set; } // PL.6 - Conditional (Table 0305)
			public string? Building { get; set; } // PL.7 - Optional (Table 0307)
			public string? Floor { get; set; } // PL.8 - Optional (Table 0308)
			public string? LocationDescription { get; set; } // PL.9 - Optional
			public string? ComprehensiveLocationIdentifier { get; set; } // PL.10 - Optional (EI: Entity Identifier)
			public string? AssigningAuthorityForLocation { get; set; } // PL.11 - Optional (HD: Hierarchic Designator)

			public static PL FromDelimitedFields(List<string> fields)
			{
				return new PL
				{
					PointOfCare = fields.ElementAtOrDefault(0),
					Room = fields.ElementAtOrDefault(1),
					Bed = fields.ElementAtOrDefault(2),
					Facility = fields.ElementAtOrDefault(3),
					LocationStatus = fields.ElementAtOrDefault(4),
					PersonLocationType = fields.ElementAtOrDefault(5),
					Building = fields.ElementAtOrDefault(6),
					Floor = fields.ElementAtOrDefault(7),
					LocationDescription = fields.ElementAtOrDefault(8),
					ComprehensiveLocationIdentifier = fields.ElementAtOrDefault(9),
					AssigningAuthorityForLocation = fields.ElementAtOrDefault(10)
				};
			}
			public override string ToString()
			{
				return this.ToHumanizedString(", ");
			}
		}
	}
}
