using Tameenk.Yakeen.DAL;
using Tameenk.Yakeen.Component.Extensions;
using System;
using System.Linq;
using Tameenk.Yakeen.Service.Models;
using Tameenk.Yakeen.Component;
using Tameenk.Yakeen.Service.Utilities;

namespace YakeenComponent
{
    public static class CompanyServices
    {       
        private static readonly CompanyDataAccess Service = new CompanyDataAccess();
        private static CompanyOutput companyOutput = new CompanyOutput();
        private static ChannelDataAccess channelData = new ChannelDataAccess();
        private static CompanyRequestLogDataAccess companyRequestLogDataAccess = new CompanyRequestLogDataAccess();
        private static CompanyRequestLog companyRequestLog = new CompanyRequestLog();

        public static CompanyOutput GetCompanyBySponsorId(CompanyYakeenInfoModel companyYakeenInfoModel)
        {
            ServicesUtilities.AcceptCertificate();
            companyRequestLog.Method = "GetCompanyBySponsorId";
            companyRequestLog.ServerIP = Utilities.GetServerIP();
            
            try
            {
                if (companyYakeenInfoModel == null)
                {
                    companyRequestLog.ErrorCode = CompanyRequestLog.ErrorCodes.NullRequest;
                    companyRequestLog.ErrorDescription = "CompanyYakeenInfoModel is null";
                    companyRequestLogDataAccess.AddToCompanyLog(companyRequestLog);

                    companyOutput.ErrorCode = CompanyOutput.ErrorCodes.NullRequest;
                    companyOutput.ErrorDescription = "CompanyYakeenInfoModel model is null";
                    companyOutput.Company = null;

                    return companyOutput;
                }

                companyRequestLog.sponsorNumber = companyYakeenInfoModel.sponsorName;

                if (companyYakeenInfoModel.Channel ==0 
                    || string.IsNullOrEmpty(companyYakeenInfoModel.password)
                    || string.IsNullOrEmpty(companyYakeenInfoModel.userName)
                    || string.IsNullOrEmpty(companyYakeenInfoModel.referenceNumber)
                    || string.IsNullOrEmpty(companyYakeenInfoModel.sponsorName)
                    || string.IsNullOrEmpty(companyYakeenInfoModel.iqamaNumber)
                    || string.IsNullOrEmpty(companyYakeenInfoModel.chargeCode))
                {
                    companyRequestLog.ErrorCode = CompanyRequestLog.ErrorCodes.NullRequest;
                    companyRequestLog.ErrorDescription = "Request mandatory fields are not exist";
                    companyRequestLogDataAccess.AddToCompanyLog(companyRequestLog);

                    companyOutput.ErrorCode = CompanyOutput.ErrorCodes.NullRequest;
                    companyOutput.ErrorDescription = "Request mandatory fields are not exist";
                    companyOutput.Company = null;

                    return companyOutput;
                }

                CompanyYakeenInfoDto Company = null;
                Company companyData = Service.Find(d => d.SponsorId == companyYakeenInfoModel.SponsorId).FirstOrDefault();

                int channelExpireByDays = channelData.GetChannelExpireDateByID(companyYakeenInfoModel.Channel);

                if (companyData == null || companyData.CreatedDate >= DateTime.Now.AddDays(channelExpireByDays))
                {  
                    string channelName = channelData.GetChannelNameByID(companyYakeenInfoModel.Channel);

                    var yakeenOutput = YakeenClient.GetCompanyInfo(companyYakeenInfoModel.chargeCode, companyYakeenInfoModel.iqamaNumber, companyYakeenInfoModel.password,
                        companyYakeenInfoModel.referenceNumber, companyYakeenInfoModel.sponsorName, companyYakeenInfoModel.userName,
                      channelName);

                    if (yakeenOutput.ErrorCode == Tameenk.Yakeen.Service.WebClients.YakeenOutput.ErrorCodes.Success)
                    {
                        var companyYakeen = yakeenOutput.CompanyYakeenInfoDto.ToModel();

                        var company = InsertCompanyInfoIntoDb(companyYakeen, companyYakeenInfoModel.sponsorName);

                        Company = company.ToModel();

                        companyOutput.ErrorCode = CompanyOutput.ErrorCodes.Success;
                        companyOutput.ErrorDescription = "Success";
                        companyOutput.Company = Company;
                    }
                    else
                    {
                        companyOutput.ErrorCode = CompanyOutput.ErrorCodes.NullResponse;
                        companyOutput.ErrorDescription = yakeenOutput.ErrorDescription;
                        companyOutput.Company = null;
                    }
                }

                if (companyData != null)
                {
                    Company = companyData.ToModel();

                    companyOutput.ErrorCode = CompanyOutput.ErrorCodes.Success;
                    companyOutput.ErrorDescription = "Success";
                    companyOutput.Company = Company;
                }
            
            }
            catch(Exception exp)
            {
                companyRequestLog.ErrorCode = CompanyRequestLog.ErrorCodes.NullRequest;
                companyRequestLog.ErrorDescription = exp.ToString();
                companyRequestLogDataAccess.AddToCompanyLog(companyRequestLog);;
                             
                companyOutput.ErrorCode = CompanyOutput.ErrorCodes.NullRequest;
                companyOutput.ErrorDescription = "CompanyYakeenInfoModel through exception";
                companyOutput.Company = null;

            }

            return companyOutput;
        }

       private static Company InsertCompanyInfoIntoDb(CompanyYakeenInfoDto CompanyInfo, string SponsorId)
        {
            var CompanyData = new Company();

            try
            {
                CompanyData = new Company()
                {
                    logId = CompanyInfo.LogId,
                    SponsorId = SponsorId,
                    SponsorName = CompanyInfo.SponsorName,
                    TotalNumberOfSponsoredDependents = CompanyInfo.TotalNumberOfSponsoredDependents,
                    TotalNumberOfSponsoreds = CompanyInfo.TotalNumberOfSponsoreds,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };
                Service.Add(CompanyData);
            }
            catch(Exception exp)
            {
                companyRequestLog.Method = "InsertCompanyInfoIntoDb";
                companyRequestLog.ErrorCode = CompanyRequestLog.ErrorCodes.MethodException;
                companyRequestLog.ErrorDescription = exp.ToString();
                companyRequestLogDataAccess.AddToCompanyLog(companyRequestLog);;
            }

            return CompanyData;
        }

    }
}