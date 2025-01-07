﻿using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Modals;
using DemoDotNetCoreApplication.Constatns;
using Microsoft.EntityFrameworkCore;
using DemoDotNetCoreApplication.Contracts;

namespace DemoDotNetCoreApplication.Providers
{
    public class PracticeProvider : IPracticeProvider
    {
        private readonly DcimDevContext _context;
        private readonly ILogger _logger;

        public PracticeProvider(DcimDevContext context,ILogger<PracticeProvider> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<List<Practice>>> getPractices()
        {
            try
            {
                var practices = await _context.Practices.ToListAsync();
                return new ApiResponse<List<Practice>>(Constants.ApiResponseType.Success, practices);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<List<Practice>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }


        public async Task<ApiResponse<bool>> DeletePractice(List<Practice> practices)
        {
            try
            {
                    _context.Practices.RemoveRange(practices);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreatePractice(Practice practice)
        {
            try
            {
                bool exists = await _context.Practices.AnyAsync(p => p.Name == practice.Name);
                if (exists)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "A practice with the same name already exists.");
                }

                await _context.Practices.AddAsync(practice);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }


    }
}
