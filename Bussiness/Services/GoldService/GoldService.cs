using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.CustomerModel;
using Data.Model.GoldModel;
using Data.Model.ResultModel;
using Data.Repository.CustomerRepo;
using Data.Repository.GoldRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.GoldService
{
    public class GoldService : IGoldService
    {

        private static Random _random = new Random();
        private readonly IGoldRepo _goldRepo;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;

        public GoldService(
            IGoldRepo goldRepo,
            IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService
            )
        {


            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
            _goldRepo = goldRepo;

        }

        public async Task<ResultModel> GetGolds(string token)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() {1, 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }

            
            var golds = await _goldRepo.GetGolds();
            List<GoldRequestModel> updatedGolds = new List<GoldRequestModel>();

            foreach (var gold in golds)
            {
                GoldRequestModel gold1 = new GoldRequestModel
                {
                    GoldId = gold.GoldId,
                    GoldName = gold.GoldName,
                    PurchasePrice = gold.PurchasePrice,
                    SalePrice = gold.SalePrice,
                    ModifiedBy = gold.ModifiedBy,
                    ModifiedDate = gold.ModifiedDate,
                    Kara = gold.Kara,
                    GoldPercent = gold.GoldPercent,
                    WorldPrice = gold.WorldPrice,
                };
                updatedGolds.Add(gold1);

            }

            resultModel.Data = updatedGolds;
            return resultModel;
        }

        public async Task<ResultModel> GetGoldById(string? token, string id)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() {1, 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }

            IEnumerable<Gold> golds = await _goldRepo.GetGolds();
            List<GoldRequestModel> updatedGolds = new List<GoldRequestModel>();
            foreach (var gold in golds)
            {
                GoldRequestModel gold1 = new GoldRequestModel
                {
                    GoldId = gold.GoldId,
                    GoldName = gold.GoldName,
                    PurchasePrice = gold.PurchasePrice,
                    SalePrice = gold.SalePrice,
                    ModifiedBy = gold.ModifiedBy,
                    ModifiedDate = gold.ModifiedDate,
                    Kara = gold.Kara,
                    GoldPercent = gold.GoldPercent,
                    WorldPrice = gold.WorldPrice,
                };
                updatedGolds.Add(gold1);

            }

            if (String.IsNullOrEmpty(id))
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Please enter any id";

            }

            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;

                for (int i = 0; i < updatedGolds.Count; i++)
                {
                    if (updatedGolds[i].GoldId != id)
                    {
                        updatedGolds.RemoveAll(c => c.GoldId == updatedGolds[i].GoldId);
                        i = i - 1;
                    }

                }
                if (updatedGolds.Count() > 0)
                {
                    resultModel.Message = "Gold found";
                }
                else
                {
                    resultModel.Message = "Not found";
                }
                IEnumerable<GoldRequestModel> updatedCustomers2 = updatedGolds;
                resultModel.Data = updatedCustomers2;
            }
            return resultModel;

        }

        public async Task<ResultModel> CreateGold(string? token, GoldCreateModel goldModel,string userId)
        {
            //var existingUser = await _goldRepo.GetByIdAsync(goldModel.ModifiedBy);


            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }

            try
            {
                //if (existingUser == null)
                //{
                //    resultModel.IsSuccess = false;
                //    resultModel.Code = 400;
                //    resultModel.Message = "User not exist";

                //}
                //else
                ////nghiên cứu auto-mapper
                //{
                    Gold gold = new Gold
                    {
                        GoldId = await GenerateGoldId(),
                        GoldName = goldModel.GoldName,
                        PurchasePrice = goldModel.PurchasePrice,
                        SalePrice = goldModel.SalePrice,
                        ModifiedBy = userId,
                        ModifiedDate = goldModel.ModifiedDate,
                        Kara = goldModel.Kara,
                        GoldPercent = goldModel.GoldPercent,
                        WorldPrice = goldModel.WorldPrice,
                    };
                    await _goldRepo.CreateGold(gold);
                    resultModel.IsSuccess = true;
                    resultModel.Code = 200;
                    resultModel.Data = gold;
                    resultModel.Message = "Correct";
                //}
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return resultModel;
        }
        private async Task<string> GenerateGoldId()
        {
            var existingIds = await _goldRepo.GetGolds();
            HashSet<string> existingIdSet = new HashSet<string>(existingIds.Select(op => op.GoldId));

            string newItem;
            do
            {
                int randomNumber = _random.Next(1, 99);
                string numberPart = randomNumber.ToString("D2");
                newItem = numberPart;
            } while (existingIdSet.Contains(newItem));

            return newItem;
        }

        public async Task<ResultModel> UpdateGold(string? token, GoldUpdateModel goldModel, string userId)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var existingGold = await GetGoldById(token, goldModel.GoldId);

            if (existingGold.Message == "Not found")
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Request gold not found";
            }
            else if (existingGold.Message == "Gold found")
            {
                    resultModel.Code = 200;
                    resultModel.IsSuccess = true;
                    resultModel.Message = "Update success";
                    Gold gold = new Gold
                    {
                        GoldId = goldModel.GoldId,
                        GoldName = goldModel.GoldName,
                        PurchasePrice = goldModel.PurchasePrice,
                        SalePrice = goldModel.SalePrice,
                        ModifiedBy = userId,
                        ModifiedDate = goldModel.ModifiedDate,
                        Kara = goldModel.Kara,
                        GoldPercent = goldModel.GoldPercent,
                        WorldPrice = goldModel.WorldPrice,
                    };
                    var goldUpdate = await _goldRepo.UpdateGold(gold);

                    resultModel.Data = goldUpdate;
                
            }
            return resultModel;
        }
        public async Task<ResultModel> DeleteListGold(string token)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }

            List<Gold> listGold = await _goldRepo.GetAll();

            bool res =await _goldRepo.RemoveRange(listGold);

            if (res)
            {
                resultModel.IsSuccess = true;
                resultModel.Code = (int)HttpStatusCode.OK;
                resultModel.Message = "Delete Success";

                return resultModel;
            }
            else
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.NotFound;
                resultModel.Message = "Delete Fail";

                return resultModel;
            }
        }

    }
}
