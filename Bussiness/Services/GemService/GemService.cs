using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.GemModel;
using Data.Model.ResultModel;
using Data.Repository.GemRepo;
using System;
using System.Net;

namespace Bussiness.Services.GemService
{
    public class GemService : IGemService
    {
        private readonly IGemRepo  _gemRepo;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;
        public GemService(IGemRepo gemRepo, IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService)
        {
            _gemRepo = gemRepo;
            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
        }

        public async Task<IEnumerable<GemRequestModel>> GetGem()
        {
            var gem = await _gemRepo.GetGem();
            List<GemRequestModel> getGem = new List<GemRequestModel>();
            foreach (var gem1 in gem) 
            {
                GemRequestModel gem2 = new GemRequestModel
                {
                    GemId = gem1.GemId,
                    Name = gem1.Name,
                    Type = gem1.Type,
                    Price = gem1.Price,
                    Desc = gem1.Desc,
                }; 
                getGem.Add(gem2);
            }
            return getGem;
        }

        public async Task<ResultModel> GetGemById(string? token, string gemId)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 1, 2, 3 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            IEnumerable<Gem> gems = await _gemRepo.GetGem();
            List<GemRequestModel> gemList = new List<GemRequestModel>();
            foreach (var gem1 in gems)
            {
                GemRequestModel gem2 = new GemRequestModel
                {
                    GemId = gem1.GemId,
                    Name = gem1.Name,
                    Type = gem1.Type,
                    Price = gem1.Price,
                    Desc = gem1.Desc,
                };
                gemList.Add(gem2);

            }

            IEnumerable<GemRequestModel> gemsList2 = gemList;



            if (String.IsNullOrEmpty(gemId))
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Please enter any gemId";
                resultModel.Data = gemsList2;
            }
            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                for (int i = 0; i < gemList.Count; i++)
                {
                    if (gemList[i].GemId != gemId)
                    {
                        gemList.RemoveAll(p => p.GemId == gemList[i].GemId);
                        i = i - 1;
                    }

                }
                if (gemList.Count() > 0)
                {
                    resultModel.Message = "Gem found";
                }
                else
                {
                    resultModel.Message = "Not found";
                }
                resultModel.Data = gemsList2;
            }
            return resultModel;
        }

        public async Task<ResultModel> GetGemByName(string? token, string gemName)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 1, 2, 3 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            IEnumerable<Gem> gems = await _gemRepo.GetGem();
            List<GemRequestModel> gemList = new List<GemRequestModel>();
            foreach (var gem1 in gems)
            {
                GemRequestModel gem2 = new GemRequestModel
                {
                    GemId = gem1.GemId,
                    Name = gem1.Name,
                    Type = gem1.Type,
                    Price = gem1.Price,
                    Desc = gem1.Desc,
                };
                gemList.Add(gem2);

            }

            IEnumerable<GemRequestModel> gemsList2 = gemList;



            if (String.IsNullOrEmpty(gemName))
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Please enter any gem name";
                resultModel.Data = gemsList2;
            }
            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                gemsList2 = gemsList2.Where(p => p.Name.ToLower().Contains(gemName.ToLower()));
                if (gemsList2.Count() > 0)
                {
                    resultModel.Message = $"Success - There are {gems.Count()} found";
                }
                else
                {
                    resultModel.Message = "Not found";
                }

                resultModel.Data = gemsList2;
            }
            return resultModel;
        }

        public async Task<ResultModel> UpdateGem(string? token, GemRequestModel gemRequestModel)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2, 3 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var updatedGem = await _gemRepo.UpdateGemAsync(gemRequestModel);
            if (updatedGem == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.NotFound;
                resultModel.Message = "Gem not found.";
                return resultModel;
            }
            resultModel.Data = updatedGem;
            resultModel.Message = "Gem updated successfully.";
            return resultModel;
        }
        public async Task<ResultModel> CreateGem(string token, CreateGemModel CreateGemModel)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2, 3 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var existingGem = await _gemRepo.GetGemByName(CreateGemModel.Name);
            if (existingGem != null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Conflict;
                resultModel.Message = "A gem with the same name already exists.";
                return resultModel;
            }
            var gem = new Gem
            {
                Name = CreateGemModel.Name,
                Type = CreateGemModel.Type,
                Price = CreateGemModel.Price,
                Desc = CreateGemModel.Desc,
            };
            await _gemRepo.CreateGem(gem);
            resultModel.Data = gem;
            resultModel.IsSuccess = true;
            resultModel.Code = (int)HttpStatusCode.OK;
            resultModel.Message = "Gem created successfully.";
            return resultModel;
        }

    }
}
