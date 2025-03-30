using BlazorApp.Helpers;
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

			public override string ToString()
			{
				return this.ToHumanizedString(Environment.NewLine);
			}
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

			public override string ToString()
			{
				return this.ToHumanizedString(Environment.NewLine);
			}
		}


		// PID Segment – Patient Identification
		public class PID
		{
			public int? SetIdPID { get; set; }
			public CX? PatientID { get; set; }
			public required CX PatientIdentifierList { get; set; }
			public CX? AlternatePatientIDs { get; set; }
			public required XPN PatientName { get; set; }
			public XPN? MothersMaidenName { get; set; }
			public DateTime? DateTimeOfBirth { get; set; }
			public string? AdministrativeSex { get; set; }
			public XPN? PatientAliases { get; set; }
			public CE? Race { get; set; }
			public XAD? PatientAddress { get; set; }
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
			public CE? VeteransMilitaryStatus { get; set; }
			public CE? Nationality { get; set; }
			public DateTime? PatientDeathDateTime { get; set; }
			public string? PatientDeathIndicator { get; set; }
			public string? IdentityUnknownIndicator { get; set; }
			public string? IdentityReliabilityCode { get; set; }
			public DateTime? LastUpdateDateTime { get; set; }
			public string? LastUpdateFacility { get; set; }
			public string? SpeciesCode { get; set; }
			public string? BreedCode { get; set; }
			public string? Strain { get; set; }
			public string? ProductionClassCode { get; set; }
			public CWE? TribalCitizenships { get; set; }

			public override string ToString()
			{
				return this.ToHumanizedString(Environment.NewLine);
			}
		}


		// PV1 Segment – Patient Visit
		public class PV1
		{
			public int? SetIdPV1 { get; set; }
			public required string PatientClass { get; set; }
			public PL? AssignedPatientLocation { get; set; }
			public string? AdmissionType { get; set; }
			public string? PreadmitNumber { get; set; }
			public PL? PriorPatientLocation { get; set; }
			public XCN? AttendingDoctors { get; set; }
			public XCN? ReferringDoctors { get; set; }
			public XCN? ConsultingDoctors { get; set; }
			public string? HospitalService { get; set; }
			public string? TemporaryLocation { get; set; }
			public string? PreadmitTestIndicator { get; set; }
			public string? ReadmissionIndicator { get; set; }
			public string? AdmitSource { get; set; }
			public string? AmbulatoryStatus { get; set; }
			public string? VIPIndicator { get; set; }
			public XCN? AdmittingDoctor { get; set; }
			public string? PatientType { get; set; }
			public string? VisitNumber { get; set; }
			public FC? FinancialClass { get; set; }
			public string? ChargePriceIndicator { get; set; }
			public string? CourtesyCode { get; set; }
			public string? CreditRating { get; set; }
			public string? ContractCode { get; set; }
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
			public string? ServicingFacility { get; set; }
			public string? BedStatus { get; set; }
			public string? AccountStatus { get; set; }
			public string? PendingLocation { get; set; }
			public string? PriorTemporaryLocation { get; set; }
			public DateTime? AdmitDateTime { get; set; }
			public TS? DischargeDateTime { get; set; }
			public decimal? CurrentPatientBalance { get; set; }
			public decimal? TotalCharges { get; set; }
			public decimal? TotalAdjustments { get; set; }
			public decimal? TotalPayments { get; set; }
			public string? AlternateVisitID { get; set; }
			public string? VisitIndicator { get; set; }
			public XCN? OtherHealthcareProviders { get; set; }

			public override string ToString()
			{
				return this.ToHumanizedString(Environment.NewLine);
			}
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
			public XCN? CollectorIdentifier { get; set; }
			public string? SpecimenActionCode { get; set; }
			public string? DangerCode { get; set; }
			public string? RelevantClinicalInformation { get; set; }
			public DateTime? SpecimenReceivedDateTime { get; set; }
			public string? SpecimenSource { get; set; }
			public XCN? OrderingProviders { get; set; }
			public XTN? OrderCallbackPhoneNumbers { get; set; }
			public string? PlacerField1 { get; set; }
			public string? PlacerField2 { get; set; }
			public string? FillerField1 { get; set; }
			public string? FillerField2 { get; set; }
			public DateTime? ResultsRptStatusChangeDateTime { get; set; }
			public string? ChargeToPractice { get; set; }
			public string? DiagnosticServSectID { get; set; }
			public string? ResultStatus { get; set; }
			public string? ParentResult { get; set; }
			public TQ? QuantityTiming { get; set; }
			public XCN? ResultCopiesTo { get; set; }
			public string? Parent { get; set; }
			public string? TransportationMode { get; set; }
			public CE? ReasonForStudy { get; set; }
			public NDL? PrincipalResultInterpreter { get; set; }
			public NDL? AssistantResultInterpreters { get; set; }
			public NDL? Technicians { get; set; }
			public NDL? Transcriptionists { get; set; }
			public DateTime? ScheduledDateTime { get; set; }
			public int? NumberOfSampleContainers { get; set; }
			public CE? TransportLogisticsOfCollectedSample { get; set; }
			public CE? CollectorsComment { get; set; }
			public string? TransportArrangementResponsibility { get; set; }
			public string? TransportArranged { get; set; }
			public string? EscortRequired { get; set; }
			public CE? PlannedPatientTransportComments { get; set; }
			public string? ProcedureCode { get; set; }
			public CE? ProcedureCodeModifiers { get; set; }
			public CE? PlacerSupplementalServiceInformation { get; set; }
			public CE? FillerSupplementalServiceInformation { get; set; }
			public CWE? MedicallyNecessaryDuplicateProcedureReason { get; set; }
			public string? ResultHandling { get; set; }

			public override string ToString()
			{
				return this.ToHumanizedString(Environment.NewLine);
			}
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

			public override string ToString()
			{
				return this.ToHumanizedString(Environment.NewLine);
			}
		}

		public class OBX
		{
			public int? SetID { get; set; }
			public string? ValueType { get; set; } ///
			public required CE ObservationIdentifier { get; set; } ///
			public string? ObservationSubID { get; set; }
			public HumanizedStringList ObservationValue { get; set; }
			public string? Units { get; set; }
			public string? ReferencesRange { get; set; }
			public string? AbnormalFlags { get; set; }
			public decimal? Probability { get; set; }
			public string? NatureOfAbnormalTest { get; set; }
			public required string ObservationResultStatus { get; set; }
			public DateTime? EffectiveDateOfReferenceRange { get; set; }
			public string? UserDefinedAccessChecks { get; set; }
			public DateTime? DateTimeOfObservation { get; set; }
			public string? ProducersID { get; set; }
			public XCN? ResponsibleObserver { get; set; }
			public CE? ObservationMethod { get; set; }
			public EI? EquipmentInstanceIdentifier { get; set; }
			public DateTime? DateTimeOfAnalysis { get; set; }

			public override string ToString()
			{
				return this.ToHumanizedString(Environment.NewLine);
			}
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

			public override string ToString()
			{
				return this.ToHumanizedString(Environment.NewLine);
			}
		}
	}
}
