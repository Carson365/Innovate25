using static BlazorApp.Data.Classes;

namespace BlazorApp.Data
{
	public class Tools
	{

		// MSH Segment – Message Header
		public class MSH
		{
			//public required string FieldSeparator { get; set; }
			public required string EncodingCharacters { get; set; }
			public string? SendingApplication { get; set; }
			public string? SendingFacility { get; set; }
			public string? ReceivingApplication { get; set; }
			public string? ReceivingFacility { get; set; }
			public required DateTime DateTimeOfMessage { get; set; }
			public string? Security { get; set; }
			public required MSG MessageType { get; set; }
			public required string MessageControlId { get; set; }
			public required string ProcessingId { get; set; }
			public required string VersionId { get; set; }
			public long? SequenceNumber { get; set; }
			public string? ContinuationPointer { get; set; }
			public string? AcceptAcknowledgmentType { get; set; }
			public string? ApplicationAcknowledgmentType { get; set; }
			public string? CountryCode { get; set; }
			public string? CharacterSet { get; set; }
			public string? PrincipalLanguageOfMessage { get; set; }
			public string? AlternateCharacterSetHandlingScheme { get; set; }
			public EI? MessageProfileIdentifiers { get; set; }
		}


		// EVN Segment – Event Type
		public class EVN
		{
			public string? EventTypeCode { get; set; }
			public required DateTime RecordedDateTime { get; set; }
			public DateTime? DateTimePlannedEvent { get; set; }
			public string? EventReasonCode { get; set; }
			public XCN? OperatorIds { get; set; }
			public DateTime? EventOccurred { get; set; }
			public string? EventFacility { get; set; }
		}


		// PID Segment – Patient Identification
		public class PID
		{
			public int? SetIdPID { get; set; }
			public string? PatientID { get; set; }
			public required CX PatientIdentifierList { get; set; }
			public CX AlternatePatientIDs { get; set; }
			public required PatientName PatientName { get; set; }
			public XPN MothersMaidenNames { get; set; }
			public DateTime? DateTimeOfBirth { get; set; }
			public string? AdministrativeSex { get; set; }
			public XPN PatientAliases { get; set; }
			public CE? Races { get; set; }
			public XAD? PatientAddresses { get; set; }
			public string? CountyCode { get; set; }
			public XTN? HomePhoneNumbers { get; set; }
			public XTN? BusinessPhoneNumbers { get; set; }
			public string? PrimaryLanguage { get; set; }
			public string? MaritalStatus { get; set; }
			public string? Religion { get; set; }
			public string? PatientAccountNumber { get; set; }
			public string? SSNNumber { get; set; }
			public string? DriversLicenseNumber { get; set; }
			public CX? MothersIdentifiers { get; set; }
			public CE? EthnicGroups { get; set; }
			public string? BirthPlace { get; set; }
			public string? MultipleBirthIndicator { get; set; }
			public int? BirthOrder { get; set; }
			public CE? Citizenships { get; set; }
			public string? VeteransMilitaryStatus { get; set; }
			public string? Nationality { get; set; }
			public DateTime? PatientDeathDateTime { get; set; }
			public string? PatientDeathIndicator { get; set; }
			public string? IdentityUnknownIndicator { get; set; }
			public string? IdentityReliabilityCodes { get; set; }
			public DateTime? LastUpdateDateTime { get; set; }
			public string? LastUpdateFacility { get; set; }
			public string? SpeciesCode { get; set; }
			public string? BreedCode { get; set; }
			public string? Strain { get; set; }
			public string? ProductionClassCode { get; set; }
			public CWE? TribalCitizenships { get; set; }
		}


		// PV1 Segment – Patient Visit
		public class PV1
		{
			public int? SetIdPV1 { get; set; }
			public required string PatientClass { get; set; }
			public List<string?> AssignedPatientLocation { get; set; }
			public string? AdmissionType { get; set; }
			public string? PreadmitNumber { get; set; }
			public List<string>? PriorPatientLocation { get; set; }
			public List<string>? AttendingDoctors { get; set; }
			public List<string>? ReferringDoctors { get; set; }
			public List<string>? ConsultingDoctors { get; set; }
			public string? HospitalService { get; set; }
			public string? TemporaryLocation { get; set; }
			public string? PreadmitTestIndicator { get; set; }
			public string? ReadmissionIndicator { get; set; }
			public string? AdmitSource { get; set; }
			public List<string>? AmbulatoryStatuses { get; set; }
			public string? VIPIndicator { get; set; }
			public List<string>? AdmittingDoctors { get; set; }
			public string? PatientType { get; set; }
			public string? VisitNumber { get; set; }
			public List<string>? FinancialClasses { get; set; }
			public string? ChargePriceIndicator { get; set; }
			public string? CourtesyCode { get; set; }
			public string? CreditRating { get; set; }
			public List<string>? ContractCodes { get; set; }
			public DateTime? ContractEffectiveDate { get; set; }
			public decimal? ContractAmount { get; set; }
			public int? ContractPeriod { get; set; }
			public string? InterestCode { get; set; }
			public string? TransferToBadDebtCode { get; set; }
			public DateTime? TransferToBadDebtDate { get; set; }
			public string? BadDebtAgencyCode { get; set; }
			public decimal? BadDebtTransferAmount { get; set; }
			public decimal? BadDebtRecoveryAmount { get; set; }
			public string? DeleteAccountIndicator { get; set; }
			public DateTime? DeleteAccountDate { get; set; }
			public string? DischargeDisposition { get; set; }
			public string? DischargedToLocation { get; set; }
			public string? DietType { get; set; }
			public List<string>? ServicingFacility { get; set; }
			public string? BedStatus { get; set; }
			public string? AccountStatus { get; set; }
			public string? PendingLocation { get; set; }
			public string? PriorTemporaryLocation { get; set; }
			public DateTime? AdmitDateTime { get; set; }
			public List<DateTime>? DischargeDateTimes { get; set; }
			public decimal? CurrentPatientBalance { get; set; }
			public decimal? TotalCharges { get; set; }
			public decimal? TotalAdjustments { get; set; }
			public decimal? TotalPayments { get; set; }
			public string? AlternateVisitID { get; set; }
			public string? VisitIndicator { get; set; }
			public List<string>? OtherHealthcareProviders { get; set; }
		}


		// OBR Segment – Observation Request
		public class OBR
		{
			public int? SetIdOBR { get; set; }
			public string? PlacerOrderNumber { get; set; }
			public string? FillerOrderNumber { get; set; }
			public required string UniversalServiceIdentifier { get; set; }
			public string? PriorityOBR { get; set; }
			public DateTime? RequestedDateTime { get; set; }
			public DateTime? ObservationDateTime { get; set; }
			public DateTime? ObservationEndDateTime { get; set; }
			public string? CollectionVolume { get; set; }
			public List<string>? CollectorIdentifiers { get; set; }
			public string? SpecimenActionCode { get; set; }
			public string? DangerCode { get; set; }
			public string? RelevantClinicalInformation { get; set; }
			public DateTime? SpecimenReceivedDateTime { get; set; }
			public string? SpecimenSource { get; set; }
			public List<string>? OrderingProviders { get; set; }
			public List<string>? OrderCallbackPhoneNumbers { get; set; }
			public string? PlacerField1 { get; set; }
			public string? PlacerField2 { get; set; }
			public string? FillerField1 { get; set; }
			public string? FillerField2 { get; set; }
			public DateTime? ResultsRptStatusChangeDateTime { get; set; }
			public string? ChargeToPractice { get; set; }
			public string? DiagnosticServSectID { get; set; }
			public string? ResultStatus { get; set; }
			public string? ParentResult { get; set; }
			public List<string>? QuantityTiming { get; set; }
			public List<string>? ResultCopiesTo { get; set; }
			public string? Parent { get; set; }
			public string? TransportationMode { get; set; }
			public List<string>? ReasonForStudy { get; set; }
			public string? PrincipalResultInterpreter { get; set; }
			public List<string>? AssistantResultInterpreters { get; set; }
			public List<string>? Technicians { get; set; }
			public List<string>? Transcriptionists { get; set; }
			public DateTime? ScheduledDateTime { get; set; }
			public int? NumberOfSampleContainers { get; set; }
			public List<string>? TransportLogisticsOfCollectedSample { get; set; }
			public List<string>? CollectorsComment { get; set; }
			public string? TransportArrangementResponsibility { get; set; }
			public string? TransportArranged { get; set; }
			public string? EscortRequired { get; set; }
			public List<string>? PlannedPatientTransportComments { get; set; }
			public string? ProcedureCode { get; set; }
			public List<string>? ProcedureCodeModifiers { get; set; }
			public List<string>? PlacerSupplementalServiceInformation { get; set; }
			public List<string>? FillerSupplementalServiceInformation { get; set; }
			public string? MedicallyNecessaryDuplicateProcedureReason { get; set; }
			public string? ResultHandling { get; set; }
		}


		// ORC Segment – Common Order
		public class ORC
		{
			public string? OrderControl { get; set; }
			public string? PlacerOrderNumber { get; set; }
			public string? FillerOrderNumber { get; set; }
			public DateTime? OrderDateTime { get; set; }
			public string? OrderingProvider { get; set; }
			public string? EnteringOrganization { get; set; }
			// Include other fields as needed.
		}

		public class OBX
		{
			public int? SetID { get; set; }
			public string? ValueType { get; set; }
			public List<string>? ObservationIdentifier { get; set; }
			public string? ObservationSubID { get; set; }
			public List<string>? ObservationValue { get; set; }
			public string? Units { get; set; }
			public string? ReferencesRange { get; set; }
			public List<string>? AbnormalFlags { get; set; }
			public decimal? Probability { get; set; }
			public List<string>? NatureOfAbnormalTest { get; set; }
			public string? ObservationResultStatus { get; set; }
			public DateTime? EffectiveDateOfReferenceRange { get; set; }
			public string? UserDefinedAccessChecks { get; set; }
			public DateTime? DateTimeOfObservation { get; set; }
			public string? ProducersID { get; set; }
			public List<string>? ResponsibleObserver { get; set; }
			public List<string>? ObservationMethod { get; set; }
			public List<string>? EquipmentInstanceIdentifier { get; set; }
			public DateTime? DateTimeOfAnalysis { get; set; }
		}

		public class Message
		{
			public MSH? MessageHeader { get; set; }
			public EVN? EventType { get; set; }
			public PID? PatientIdentification { get; set; }
			public PV1? PatientVisit { get; set; }
			public OBR? ObservationRequest { get; set; }
			public ORC? CommonOrder { get; set; }
			public List<OBX>? ObservationResult { get; set; }
		}


	}
}
