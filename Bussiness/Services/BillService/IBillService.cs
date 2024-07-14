using Data.Model.BillModel;
using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.BillService
{
    public interface IBillService
    {
        Task<ResultModel> CraeteBill(string token, CreateBillReqModel model);
        Task<ResultModel> ViewBill(string token, BillSearchModel billSearch);
    }
}
