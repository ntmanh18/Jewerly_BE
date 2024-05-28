﻿using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.Validate
{
    public interface IUserValidate
    {
        Task<ResultModel> IsUserNameUnique(string username);
        Task<ResultModel> IsPhoneValid(string phone);

    }
}
