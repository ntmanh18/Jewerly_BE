using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.GemModel;
using Data.Model.ResultModel;
using Data.Repository.GemRepo;
using Microsoft.EntityFrameworkCore;
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
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
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
            await _gemRepo.UpdateGem(updatedGem);
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
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
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
            if (CreateGemModel.Type!=1 && CreateGemModel.Type !=2)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Conflict;
                resultModel.Message = "The Type of the gem is incorrect. It must be 1 or 2.";
                return resultModel;
            }
            if (CreateGemModel.Rate<0 || CreateGemModel.Rate>1)
            {
                CreateGemModel.Rate = 0;
            }
            var gem = new Gem
            {
                Name = CreateGemModel.Name,
                Type = CreateGemModel.Type,
                Price = CreateGemModel.Price,
                Desc = CreateGemModel.Desc,
                Rate = CreateGemModel.Rate
            };
            await _gemRepo.CreateGem(gem);
            resultModel.Data = gem;
            resultModel.IsSuccess = true;
            resultModel.Code = (int)HttpStatusCode.OK;
            resultModel.Message = "Gem created successfully.";
            return resultModel;
        }

        public async Task<ResultModel> ViewListGem(string? token, GemSearchModel gemSearch)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 1, 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";
                return resultModel;
            }
            var query = _gemRepo.GetVoucherQuery();
            if (!string.IsNullOrEmpty(gemSearch.GemId))
            {
                query = query.Where(v => v.GemId == gemSearch.GemId);
            }

            if (!string.IsNullOrEmpty(gemSearch.Name))
            {
                query = query.Where(v => v.Name.Contains(gemSearch.Name));
            }
            var gems = await query.ToListAsync();
            resultModel.Data = gems;

            return resultModel;
        }
    }
}
