using Microsoft.AspNetCore.Http;
using Easy_crud.CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Easy_crud.DataAccessLayer
{
    public interface IUploadFileDL
    {
        public Task<UploadExcelFileResponse> UploadExcelFile(UploadExcelFileRequest request, string Path);

        //public Task<UploadCSVFileResponse> UploadCSVFile(UploadCSVFileRequest request, string Path);
        //public Task<ReadRecordResponse> ReadRecord(ReadRecordRequest request);
        //public Task<DeleteRecordResponse> DeleteRecord(DeleteRecordRequest request);
    }
}
