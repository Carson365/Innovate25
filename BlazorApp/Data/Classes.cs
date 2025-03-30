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

		public class MSG
		{
			public required string MessageCode { get; set; }
			public required string TriggerEvent { get; set; }
			public required string MessageStructure { get; set; }
		}

        public class EI
        {
            public string? EntityIdentifier { get; set; } // Length: 199, Data Type: ST, Optional
            public string? NamespaceId { get; set; } // Length: 20, Data Type: IS, Optional, Table: 0363
            public string? UniversalId { get; set; } // Length: 199, Data Type: ST, Conditional
            public string? UniversalIdType { get; set; } // Length: 6, Data Type: ID, Conditional, Table: 0301
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
        }

        public class XPN
        {
            public string? FamilyName { get; set; } // Length: 194, Data Type: FN, Optional
            public string? GivenName { get; set; } // Length: 30, Data Type: ST, Optional, Table: FirstName
            public string? SecondAndFurtherGivenNamesOrInitials { get; set; } // Length: 30, Data Type: ST, Optional
            public string? Suffix { get; set; } // Length: 20, Data Type: ST, Optional
            public string? Prefix { get; set; } // Length: 20, Data Type: ST, Optional
            public string? Degree { get; set; } // Length: 6, Data Type: IS, Both, Table: 0360
            public string? NameTypeCode { get; set; } // Length: 1, Data Type: ID, Optional, Table: 0200
            public string? NameRepresentationCode { get; set; } // Length: 1, Data Type: ID, Optional, Table: 0465
            public string? NameContext { get; set; } // Length: 483, Data Type: CE, Optional, Table: 0448
            public string? NameValidityRange { get; set; } // Length: 53, Data Type: DR, Both
            public string? NameAssemblyOrder { get; set; } // Length: 1, Data Type: ID, Optional, Table: 0444
            public DateTime? EffectiveDate { get; set; } // Length: 26, Data Type: TS, Optional
            public DateTime? ExpirationDate { get; set; } // Length: 26, Data Type: TS, Optional
            public string? ProfessionalSuffix { get; set; } // Length: 199, Data Type: ST, Optional
        }

        public class CE
        {
            public string? Identifier { get; set; } // Length: 20, Data Type: ST, Optional
            public string? Text { get; set; } // Length: 199, Data Type: ST, Optional
            public string? NameOfCodingSystem { get; set; } // Length: 20, Data Type: ID, Optional, Table: 0396
            public string? AlternateIdentifier { get; set; } // Length: 20, Data Type: ST, Optional
            public string? AlternateText { get; set; } // Length: 199, Data Type: ST, Optional
            public string? NameOfAlternateCodingSystem { get; set; } // Length: 20, Data Type: ID, Optional, Table: 0396
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
        }

    }
}
