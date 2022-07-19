namespace hospins.Models
{
    public class EmployeePdfModel
    {
        public EmployeePdfModel()
        {
            EmployeeHistory = new List<EmployeeHistoryPdfModel>();
            EmployeeDocument = new List<EmployeeDocumentPdfModel>();
        }
        public int EmployeeId { get; set; }
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string Mobile { get; set; }
        public string? BloodGroup { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Picture { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public EmployeeAddressPdfModel? CurrentAddress { get; set; }
        public EmployeeAddressPdfModel? PermanentAddress { get; set; }
        public EmployeeSalarySetupPdfModel? EmployeeSalarySetup { get; set; }
        public List<EmployeeHistoryPdfModel> EmployeeHistory { get; set; }
        public List<EmployeeDocumentPdfModel> EmployeeDocument { get; set; }
    }

    public class EmployeeAddressPdfModel
    {
        public string Address { get; set; } = null!;
        public int CountryId { get; set; }
        public string Country { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public string City { get; set; } = null!;
        public string? ZipCode { get; set; }
    }

    public class EmployeeSalarySetupPdfModel
    {
        public int SalaryTypeId { get; set; }
        public string SalaryType { get; set; }
        public string Basis { get; set; } = null!;
        public string Transport { get; set; } = null!;
        public string? Health { get; set; }
        public int? Pf { get; set; }
        public int? Tax { get; set; }
        public int? GrossSalary { get; set; }
        public int? Ctc { get; set; }
        public string? SalaryBenefits { get; set; }
        public string? BenefitsType { get; set; }
    }

    public class EmployeeHistoryPdfModel
    {
        public string CompanyName { get; set; } = null!;
        public DateTime? JoiningDate { get; set; }
        public DateTime? ReleasingDate { get; set; }
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public string LastSalary { get; set; }
    }

    public class EmployeeDocumentPdfModel
    {
        public string? FileName { get; set; }
    }
}
