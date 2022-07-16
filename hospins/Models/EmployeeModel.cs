namespace hospins.Models
{
    public class EmployeeModel
    {
        public EmployeeModel()
        {
            EmployeeHistory = new List<EmployeeHistoryModel>();
            EmployeeDocument = new List<EmployeeDocumentModel>();
        }
        public int EmployeeId { get; set; }
        public int DesignationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string Mobile { get; set; }
        public string? BloodGroup { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public IFormFile? File { get; set; }
        public string? Picture { get; set; }
        public int? CountryId { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public EmployeeAddressModel? CurrentAddress { get; set; }
        public EmployeeAddressModel? PermanentAddress { get; set; }
        public EmployeeSalarySetupModel? EmployeeSalarySetup { get; set; }
        public List<EmployeeHistoryModel> EmployeeHistory { get; set; }
        public List<EmployeeDocumentModel> EmployeeDocument { get; set; }

    }

    public class EmployeeAddressModel
    {
        public string Address { get; set; } = null!;
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string City { get; set; } = null!;
        public string? ZipCode { get; set; }
    }

    public class EmployeeSalarySetupModel
    {
        public int SalaryTypeId { get; set; }
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

    public class EmployeeHistoryModel
    {
        public string CompanyName { get; set; } = null!;
        public DateTime? JoiningDate { get; set; }
        public DateTime? ReleasingDate { get; set; }
        public int DesignationId { get; set; }
        public string LastSalary { get; set; }
    }

    public class EmployeeDocumentModel
    {
        public int DocumentTypeId { get; set; }
        public IFormFile? File { get; set; }
        public string? FileName { get; set; }
    }
}
