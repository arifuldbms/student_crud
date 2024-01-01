using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
//using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Easy_crud.CommonLayer.Model;
using Easy_crud.DataAccessLayer;
using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Easy_crud.DataAccessLayer
{
    public class UploadFileDL : IUploadFileDL
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _sqlConnection;

        public UploadFileDL(IConfiguration configuration)
        {
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:CRUDCS"]);
            //_sqlConnection = new SqlConnection(_configuration["ConnectionStrings:SqlServerDBConnection"]);
        }

              public async Task<UploadExcelFileResponse> UploadExcelFile(UploadExcelFileRequest request, string path)
        {
            UploadExcelFileResponse response = new UploadExcelFileResponse();
            List<ExcelBulkUploadParameter> Parameters = new List<ExcelBulkUploadParameter>();
            DataSet dataSet;
            response.IsSuccess = true;
            response.Message = "Successful";

            try
            {
                if (request.File.FileName.ToLower().Contains(value:".xlsx"))
                {
                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                    dataSet = reader.AsDataSet(
                        new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = false,
                            ConfigureDataTable = ( IExcelDataReader tableReader) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }

                        });
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        ExcelBulkUploadParameter rows = new ExcelBulkUploadParameter();
                        rows.StudentName = dataSet.Tables[0].Rows[i].ItemArray[0] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[0]) : "-1";
                        rows.StudentRoll = dataSet.Tables[0].Rows[i].ItemArray[1] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1]) : "-1";
                        rows.PhoneNumber = dataSet.Tables[0].Rows[i].ItemArray[2] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[2]) : "-1";
                        rows.StudentEmail = dataSet.Tables[0].Rows[i].ItemArray[3] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[3]) : "-1";
                        rows.StudentAddress = dataSet.Tables[0].Rows[i].ItemArray[4] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[4]) : "-1";

                        //rows.UserName = dataSet.Tables[0].Rows[i].ItemArray[0] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[0]) : "-1";
                        //rows.EmailID = dataSet.Tables[0].Rows[i].ItemArray[1] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1]) : "-1";
                        //rows.MobileNumber = dataSet.Tables[0].Rows[i].ItemArray[2] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[2]) : "-1";
                        //rows.Age = dataSet.Tables[0].Rows[i].ItemArray[3] != null ? Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[3]) : -1;
                        //rows.Salary = dataSet.Tables[0].Rows[i].ItemArray[4] != null ? Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[4]) : -1;
                        //rows.Gender = dataSet.Tables[0].Rows[i].ItemArray[5] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[5]) : "-1";
                        Parameters.Add(rows);
                    }



                    //for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    //{
                    //    ExcelBulkUploadParameter rows = new ExcelBulkUploadParameter();
                    //    rows.StudentID = dataSet.Tables[0].Rows[i].ItemArray[0] != null ? Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]) : -1;
                    //    rows.StudentName = dataSet.Tables[0].Rows[i].ItemArray[1] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1]) : "-1";
                    //    rows.StudentDeptID = dataSet.Tables[0].Rows[i].ItemArray[2] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[2]) : "-1";
                    //    rows.StudentSemesterID = dataSet.Tables[0].Rows[i].ItemArray[3] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[3]) : "-1";
                    //    rows.StudentShiftID = dataSet.Tables[0].Rows[i].ItemArray[4] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[4]) : "-1";
                    //    rows.amount = dataSet.Tables[0].Rows[i].ItemArray[5] != null ? Convert.ToSingle(dataSet.Tables[0].Rows[i].ItemArray[5]) : -1;
                    //    Parameters.Add(rows);
                    //}

                    stream.Close();

                    if (Parameters.Count > 0)
                    {
                        if (ConnectionState.Open != _sqlConnection.State)
                        {
                            await _sqlConnection.OpenAsync();
                        }
                        //string SqlQuery = "INSERT INTO PaymentRequest (StudentID, StudentName, StudentDeptID, StudentSemesterID, StudentShiftID, amount) VALUES (@StudentID, @StudentName, @StudentDeptID, @StudentSemesterID, @StudentShiftID, @amount)";
                        //string SqlQuery = "INSERT INTO BulkUploadTable (UserName,EmailID,MobileNumber,Age,Salary,Gender) VALUES (@UserName, @EmailID, @MobileNumber, @Age, @Salary, @Gender)";

                        string SqlQuery = "INSERT INTO Employees (StudentName, StudentRoll, PhoneNumber, StudentEmail, StudentAddress) VALUES (@StudentName, @StudentRoll, @PhoneNumber, @StudentEmail, @StudentAddress)";
                        foreach (ExcelBulkUploadParameter rows in Parameters)
                        {
                            using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _sqlConnection))
                            {
                                //UserName, StudentSemesterID, StudentSemesterID, amount, StudentID, StudentShiftID
                                //sqlCommand.CommandText = SqlQueries.InsertBulkUploadData;
                                //sqlCommand.CommandText = SqlQuery;

                                sqlCommand.CommandType = CommandType.Text;
                                sqlCommand.CommandTimeout = 180;

                                //sqlCommand.Parameters.AddWithValue("@UserName", rows.UserName);
                                //sqlCommand.Parameters.AddWithValue("@EmailID", rows.EmailID);
                                //sqlCommand.Parameters.AddWithValue("@MobileNumber", rows.MobileNumber);
                                //sqlCommand.Parameters.AddWithValue("@Age", rows.Age);
                                //sqlCommand.Parameters.AddWithValue("@Salary", rows.Salary);
                                //sqlCommand.Parameters.AddWithValue("@Gender", rows.Gender);

                                sqlCommand.Parameters.AddWithValue("@StudentName", rows.StudentName);
                                sqlCommand.Parameters.AddWithValue("@StudentRoll", rows.StudentRoll);
                                sqlCommand.Parameters.AddWithValue("@PhoneNumber", rows.PhoneNumber);
                                sqlCommand.Parameters.AddWithValue("@StudentEmail", rows.StudentEmail);
                                sqlCommand.Parameters.AddWithValue("@StudentAddress", rows.StudentAddress);

                                //sqlCommand.Parameters.AddWithValue("@StudentID", rows.StudentID);
                                //sqlCommand.Parameters.AddWithValue("@StudentName", rows.StudentName);
                                //sqlCommand.Parameters.AddWithValue("@StudentDeptID", rows.StudentDeptID);
                                //sqlCommand.Parameters.AddWithValue("@StudentSemesterID", rows.StudentSemesterID);
                                //sqlCommand.Parameters.AddWithValue("@StudentShiftID", rows.StudentShiftID);
                                //sqlCommand.Parameters.AddWithValue("@amount", rows.amount);
                                int Status = await sqlCommand.ExecuteNonQueryAsync();
                                if (Status <= 0)
                                {
                                    response.IsSuccess = false;
                                    response.Message = "Query Not Executed";
                                    return response;
                                }
                            }
                        }
                    }

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid File";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _sqlConnection.CloseAsync();
                await _sqlConnection.DisposeAsync();
            }

            return response;
        }


        //public async Task<DeleteRecordResponse> DeleteRecord(DeleteRecordRequest request)
        //{
        //    DeleteRecordResponse response = new DeleteRecordResponse();
        //    response.IsSuccess = true;
        //    response.Message = "Successful";
        //    try
        //    {
        //        if (_sqlConnection.State != ConnectionState.Open)
        //        {
        //            await _sqlConnection.OpenAsync();
        //        }

        //        using (SqlCommand sqlCommand = new SqlCommand(SqlQueries.DeleteRecord, _sqlConnection))
        //        {
        //            sqlCommand.CommandType = CommandType.Text;
        //            sqlCommand.CommandTimeout = 180;
        //            sqlCommand.Parameters.AddWithValue("@ID", request.ID);
        //            int Status = await sqlCommand.ExecuteNonQueryAsync();
        //            if (Status <= 0)
        //            {
        //                response.IsSuccess = false;
        //                response.Message = "Delete Query Not Executed";
        //                return response;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = ex.Message;
        //    }
        //    finally
        //    {
        //        await _sqlConnection.CloseAsync();
        //        await _sqlConnection.DisposeAsync();
        //    }

        //    return response;
        //}

        //public async Task<ReadRecordResponse> ReadRecord(ReadRecordRequest request)
        //{
        //    ReadRecordResponse response = new ReadRecordResponse();
        //    response.IsSuccess = true;
        //    response.Message = "Successful";
        //    int Count = 0;
        //    try
        //    {
        //        if (_sqlConnection.State != ConnectionState.Open)
        //        {
        //            await _sqlConnection.OpenAsync();
        //        }

        //        using (SqlCommand sqlCommand = new SqlCommand(SqlQueries.ReadRecord, _sqlConnection))
        //        {
        //            int Offset = (request.PageNumber - 1) * request.RecordPerPage;
        //            sqlCommand.CommandType = CommandType.Text;
        //            sqlCommand.CommandTimeout = 180;
        //            sqlCommand.Parameters.AddWithValue("@Offset", Offset);
        //            sqlCommand.Parameters.AddWithValue("@RecordPerPage", request.RecordPerPage);
        //            using (SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
        //            {
        //                if (dataReader.HasRows)
        //                {
        //                    response.readRecord = new List<ReadRecord>();
        //                    while (await dataReader.ReadAsync())
        //                    {
        //                        ReadRecord getdata = new ReadRecord();
        //                        getdata.ID = dataReader["ID"] != DBNull.Value ? Convert.ToInt32(dataReader["ID"]) : 0;
        //                        getdata.StudentName = dataReader["StudentName"] != DBNull.Value ? Convert.ToString(dataReader["StudentName"]) : string.Empty;
        //                        getdata.StudentDeptID = dataReader["StudentDeptID"] != DBNull.Value ? Convert.ToString(dataReader["StudentDeptID"]) : string.Empty;
        //                        getdata.StudentSemesterID = dataReader["StudentSemesterID"] != DBNull.Value ? Convert.ToString(dataReader["StudentSemesterID"]) : string.Empty;
        //                        getdata.amount = dataReader["amount"] != DBNull.Value ? Convert.ToInt32(dataReader["amount"]) : 0;
        //                        getdata.StudentID = dataReader["StudentID"] != DBNull.Value ? Convert.ToInt32(dataReader["StudentID"]) : 0;
        //                        getdata.StudentShiftID = dataReader["StudentShiftID"] != DBNull.Value ? Convert.ToString(dataReader["StudentShiftID"]) : string.Empty;
        //                        if (Count == 0)
        //                        {
        //                            Count++;
        //                            response.TotalRecords = dataReader["TotalRecord"] != DBNull.Value ? Convert.ToInt32(dataReader["TotalRecord"]) : 0;
        //                            response.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(response.TotalRecords / request.RecordPerPage)));
        //                            response.CurrentPage = request.PageNumber;
        //                        }
        //                        response.readRecord.Add(getdata);
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //public async Task<UploadCSVFileResponse> UploadCSVFile(UploadCSVFileRequest request, string Path)
        //{
        //    UploadCSVFileResponse response = new UploadCSVFileResponse();
        //    List<ExcelBulkUploadParameter> Parameters = new List<ExcelBulkUploadParameter>();
        //    response.IsSuccess = true;
        //    response.Message = "Successful";
        //    try
        //    {

        //        if (request.File.FileName.ToLower().Contains(".csv"))
        //        {

        //            DataTable value = new DataTable();
        //            //Install Library : LumenWorksCsvReader 
        //            using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(Path)), true))
        //            {
        //                value.Load(csvReader);
        //            };

        //            for (int i = 0; i < value.Rows.Count; i++)
        //            {
        //                ExcelBulkUploadParameter readData = new ExcelBulkUploadParameter();
        //                readData.StudentName = value.Rows[i][0] != null ? Convert.ToString(value.Rows[i][0]) : "-1";
        //                readData.StudentSemesterID = value.Rows[i][1] != null ? Convert.ToString(value.Rows[i][1]) : "-1";
        //                readData.StudentDeptID = value.Rows[i][2] != null ? Convert.ToString(value.Rows[i][2]) : "-1";
        //                readData.amount = value.Rows[i][3] != null ? Convert.ToInt32(value.Rows[i][3]) : -1;
        //                readData.StudentID = value.Rows[i][4] != null ? Convert.ToInt32(value.Rows[i][4]) : -1;
        //                readData.StudentShiftID = value.Rows[i][5] != null ? Convert.ToString(value.Rows[i][5]) : "-1";
        //                Parameters.Add(readData);
        //            }

        //            if (Parameters.Count > 0)
        //            {
        //                if (ConnectionState.Open != _sqlConnection.State)
        //                {
        //                    await _sqlConnection.OpenAsync();
        //                }

        //                foreach (ExcelBulkUploadParameter rows in Parameters)
        //                {
        //                    using (SqlCommand sqlCommand = new SqlCommand(SqlQueries.InsertBulkUploadData, _sqlConnection))
        //                    {
        //                        //StudentName, StudentSemesterID, StudentDeptID, amount, StudentID, StudentShiftID

        //                        sqlCommand.CommandType = CommandType.Text;
        //                        sqlCommand.CommandTimeout = 180;
        //                        sqlCommand.Parameters.AddWithValue("@StudentName", rows.StudentName);
        //                        sqlCommand.Parameters.AddWithValue("@StudentSemesterID", rows.StudentSemesterID);
        //                        sqlCommand.Parameters.AddWithValue("@StudentDeptID", rows.StudentDeptID);
        //                        sqlCommand.Parameters.AddWithValue("@amount", rows.amount);
        //                        sqlCommand.Parameters.AddWithValue("@StudentID", rows.StudentID);
        //                        sqlCommand.Parameters.AddWithValue("@StudentShiftID", rows.StudentShiftID);
        //                        int Status = await sqlCommand.ExecuteNonQueryAsync();
        //                        if (Status <= 0)
        //                        {
        //                            response.IsSuccess = false;
        //                            response.Message = "Query Not Executed";
        //                            return response;
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //        else
        //        {
        //            response.IsSuccess = false;
        //            response.Message = "InValid File";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = ex.Message;
        //    }
        //    finally
        //    {
        //        await _sqlConnection.CloseAsync();
        //        await _sqlConnection.DisposeAsync();
        //    }
        //    return response;
        //}

    }
}
