using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.DashBoardService
{
    public interface IDashService
    {
        Task<ResultModel> GetTotalIncomeAsync(string? token);
        Task<ResultModel> GetIncomeByCashNumberAsync(string? token);
        Task<ResultModel> GetIncomeByMonthAsync(string? token);
    }
}
