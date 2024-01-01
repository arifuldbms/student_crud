
namespace Easy_crud.CommonLayer.Model
{
    public class UploadExcelFileRequest
    {
        public IFormFile File { get; set; }
    }

    public class UploadExcelFileResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ExcelBulkUploadParameter
    {
       
        public string? StudentName { get; set; }
        public string? StudentRoll { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StudentEmail { get; set; }
        public string? StudentAddress { get; set; }
    }


    //public class ExcelBulkUploadParameter
    //{
    //    public long StudentID { get; set; }
    //    public string StudentName { get; set; }
    //    public string StudentDeptID { get; set; }
    //    public string StudentSemesterID { get; set; }
    //    public string StudentShiftID { get; set; }
    //    public float amount { get; set; }
    //}

    //public class ExcelBulkUploadParameter
    //{
    //    public string? UserName { get; set; }
    //    public string? EmailID { get; set; }
    //    public string? MobileNumber { get; set; }
    //    public int Age { get; set; }
    //    public int Salary { get; set; }
    //    public string? Gender { get; set; }
    //}

}

