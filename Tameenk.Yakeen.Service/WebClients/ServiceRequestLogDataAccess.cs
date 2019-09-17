using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using Tameenk.Yakeen.DAL;
using Tameenk.Yakeen.Service.Models;
using Tameenk.Yakeen.Service.Utilities;

namespace Tameenk.Yakeen.Service.WebClients
{
    public class ServiceRequestLogDataAccess
    {
        public static bool AddtoServiceRequestLogs(ServiceRequestLog toSaveLog)
        {
            try
            {
                using (YakeenContext context = new YakeenContext())
                {
                    //context.Database.CommandTimeout = 60;
                    toSaveLog.CreatedDate = DateTime.Now;
                    context.ServiceRequestLog.Add(toSaveLog);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {

                    }
                }
                return false;
            }
        }


        public static List<ServiceRequestLog> GetQuotationListForCompany(int commandTimeout, string company)
        {
            try
            {
                using (YakeenContext context = new YakeenContext())
                {
                    //context.Database.CommandTimeout = commandTimeout;

                    DateTime startDate = DateTime.Now.Date.AddDays(-1).AddHours(0).AddMinutes(0).AddSeconds(0);// new DateTime(DateTime.Now.Year, DateTime.Now.Month, startDay, 0, 0, 0);
                    DateTime endDate = DateTime.Now.Date.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59); //new DateTime(DateTime.Now.Year, DateTime.Now.Month, startDay, 23, 59, 59);

                    var quotations = (from d in context.ServiceRequestLog
                                      where
                                       d.CreatedDate >= startDate
                                      && d.CreatedDate <= endDate && d.CompanyName == company &&
                                   d.Method == "Quotation"
                                      orderby d.CreatedDate
                                      select d);

                    List<ServiceRequestLog> quotationList = quotations.ToList<ServiceRequestLog>();

                    if (quotationList.Count > 0)
                        return quotationList;
                    else
                        return null;
                }
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return null;

            }
        }


        public static List<ServiceRequestLog> GetQuotationList(int commandTimeout)
        {
            try
            {
                using (YakeenContext context = new YakeenContext())
                {
                    //context.Database.CommandTimeout = commandTimeout;


                    DateTime startDate = DateTime.Now.Date.AddDays(-1).AddHours(0).AddMinutes(0).AddSeconds(0);// new DateTime(DateTime.Now.Year, DateTime.Now.Month, startDay, 0, 0, 0);
                    DateTime endDate = DateTime.Now.Date.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59); //new DateTime(DateTime.Now.Year, DateTime.Now.Month, startDay, 23, 59, 59);

                    var quotations = (from d in context.ServiceRequestLog
                                      where
                                       d.CreatedDate >= startDate
                                      && d.CreatedDate <= endDate &&
                                   d.Method == "Quotation"
                                      orderby d.CreatedDate
                                      select d);

                    List<ServiceRequestLog> quotationList = quotations.ToList<ServiceRequestLog>();

                    return quotationList;
                }
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return null;

            }
        }


        public static List<ServiceRequestLog> GetPolicyListForCompany(int commandTimeout, string company)
        {
            try
            {
                using (YakeenContext context = new YakeenContext())
                {
                    //context.Database.CommandTimeout = commandTimeout;


                    DateTime startDate = DateTime.Now.Date.AddDays(-1).AddHours(0).AddMinutes(0).AddSeconds(0);// new DateTime(DateTime.Now.Year, DateTime.Now.Month, startDay, 0, 0, 0);
                    DateTime endDate = DateTime.Now.Date.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59); //new DateTime(DateTime.Now.Year, DateTime.Now.Month, startDay, 23, 59, 59);

                    var policies = (from d in context.ServiceRequestLog
                                    where
                                     d.CreatedDate >= startDate
                                    && d.CreatedDate <= endDate &&
                                    d.Method == "policy" && d.CompanyName == company
                                    orderby d.CreatedDate
                                    select d);

                    List<ServiceRequestLog> policiesList = policies.ToList<ServiceRequestLog>();

                    if (policiesList.Count > 0)
                        return policiesList;
                    else
                        return null;
                }
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return null;

            }
        }


        public static ServiceRequestLog GetPolicyByRefernceId(string refernceId)
        {
            try
            {
                using (YakeenContext context = new YakeenContext())
                {

                    var policy = (from d in context.ServiceRequestLog
                                  where d.ReferenceId == refernceId
                                  && d.Method == "Policy" && d.ErrorCode == 1
                                  orderby d.ID descending
                                  select d).FirstOrDefault();
                    return policy;
                }
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return null;

            }
        }

        public static List<string> GetAllMethodInServiceRequestLog()
        {
            try
            {
                using (YakeenContext context = new YakeenContext())
                {
                    var methods = (from d in context.ServiceRequestLog
                                   select d.Method).Distinct().ToList();
                    return methods;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.Message, ex, false);
                return null;
            }
        }


        public static List<ServiceRequestLog> GetAllServiceRequestLog()
        {
            try
            {
                using (YakeenContext context = new YakeenContext())
                {
                    var query = (from d in context.ServiceRequestLog
                                 select d).Distinct();

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.Message, ex, false);
                return null;
            }
        }

        public static List<ServiceRequestLog> GetAllServiceRequestLogBasedOnFilter(ServiceRequestFilter serviceRequestFilter, out int total, int pageIndex = 0, int pageSize = int.MaxValue, string sortField = "ID", bool? sortOrder = false)
        {
            try
            {
                using (YakeenContext context = new YakeenContext())
                {
                    var query = (from d in context.ServiceRequestLog select d);

                    if (serviceRequestFilter.EndDate.HasValue && serviceRequestFilter.StartDate.HasValue)
                    {
                        DateTime dtEnd = new DateTime(serviceRequestFilter.EndDate.Value.Year, serviceRequestFilter.EndDate.Value.Month, serviceRequestFilter.EndDate.Value.Day, 23, 59, 59);
                        DateTime dtStart = new DateTime(serviceRequestFilter.StartDate.Value.Year, serviceRequestFilter.StartDate.Value.Month, serviceRequestFilter.StartDate.Value.Day, 0, 0, 0);

                        if (serviceRequestFilter.EndDate.Value == serviceRequestFilter.StartDate.Value)
                        {
                            // DateTime dtEnd = serviceRequestFilter.EndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                            query = query.Where(e => e.CreatedDate >= dtStart && e.CreatedDate <= dtEnd);
                        }
                        else
                        {
                            query = query.Where(e => e.CreatedDate >= dtStart && e.CreatedDate <= dtEnd);
                        }
                    }
                    else if (serviceRequestFilter.EndDate.HasValue)
                    {
                        DateTime dtEnd = new DateTime(serviceRequestFilter.EndDate.Value.Year, serviceRequestFilter.EndDate.Value.Month, serviceRequestFilter.EndDate.Value.Day, 23, 59, 59);
                        query = query.Where(e => e.CreatedDate <= serviceRequestFilter.EndDate.Value.Date);

                    }
                    else if (serviceRequestFilter.StartDate.HasValue)
                    {
                        DateTime dtStart = new DateTime(serviceRequestFilter.StartDate.Value.Year, serviceRequestFilter.StartDate.Value.Month, serviceRequestFilter.StartDate.Value.Day, 0, 0, 0);
                        query = query.Where(e => e.CreatedDate >= dtStart);
                    }
                    if (!string.IsNullOrEmpty(serviceRequestFilter.Method))
                        query = query.Where(q => q.Method.Equals(serviceRequestFilter.Method));

                    if (!string.IsNullOrEmpty(serviceRequestFilter.NationalId))
                        query = query.Where(q => q.DriverNin.Equals(serviceRequestFilter.NationalId));

                    if (!string.IsNullOrEmpty(serviceRequestFilter.VehicleId))
                        query = query.Where(q => q.VehicleId.Equals(serviceRequestFilter.VehicleId));

                    if (!string.IsNullOrEmpty(serviceRequestFilter.ReferenceNo))
                        query = query.Where(q => q.ReferenceId.Equals(serviceRequestFilter.ReferenceNo));

                    if (serviceRequestFilter.StatusCode.HasValue)
                    {
                        if (serviceRequestFilter.StatusCode.Value == 1)
                            query = query.Where(q => q.ErrorCode == 1);
                        else
                            query = query.Where(q => q.ErrorCode != 1);
                    }
                    if (serviceRequestFilter.InsuranceCompanyId.HasValue)
                        query = query.Where(q => q.CompanyID == serviceRequestFilter.InsuranceCompanyId);

                    if (!string.IsNullOrEmpty(serviceRequestFilter.PolicyNo))
                        query = query.Where(q => q.PolicyNo.Equals(serviceRequestFilter.PolicyNo));

                    total = query.Count();
                    if (total == 0)
                        return null;
                    int TotalCount = total;
                    int TotalPages = total / pageSize;

                    if (total % pageSize > 0)
                        TotalPages++;

                    if (!string.IsNullOrEmpty(sortField))
                    {
                        if (sortOrder.HasValue)
                        {
                            if (sortOrder.GetValueOrDefault())
                            {
                                query = query.OrderBy("ID DESC");
                            }
                            else
                            {
                                query = query.OrderBy("ID");
                            }
                        }
                    }
                    query = query.Skip(pageIndex * pageSize).Take(pageSize);
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                total = 0;
                ErrorLogger.LogError(ex.Message, ex, false);
                return null;
            }
        }


    }
}
