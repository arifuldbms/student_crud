using Microsoft.EntityFrameworkCore;

namespace Easy_crud.CommonLayer.Model
{
    public class UploadExcelFileContext : DbContext
    {
        public UploadExcelFileContext(DbContextOptions<UploadExcelFileContext> options) : base(options) 
        {
        }
    }
}
