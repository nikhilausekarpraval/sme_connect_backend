using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Modals;
using DemoDotNetCoreApplication.Constatns;
using Microsoft.EntityFrameworkCore;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Dtos;

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

        public async Task<ApiResponse<Practice>> getPractice(string practiceId)
        {
            try
            {
                var practice = await _context.Practices.FirstAsync(p => p.Name == practiceId);
                return new ApiResponse<Practice>(Constants.ApiResponseType.Success, practice);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<Practice>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }


        public async Task<ApiResponse<bool>> DeletePractice(List<int> practicesIds)
        {
            try
            {
                var practicesToRemove = _context.Practices.Where(practice => practicesIds.Contains(practice.Id)).ToList();

                if (practicesToRemove.Any())
                {
                    _context.Practices.RemoveRange(practicesToRemove);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                else
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "No matching practices found.");
                }
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

        public async Task<ApiResponse<bool>> UpdatePractice(Practice practice)
        {
            try
            {
                var existingpractice = await _context.Practices.FindAsync(practice.Id);
                if (existingpractice == null)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "Selected practice not found.");
                }

                existingpractice.Name = practice.Name;
                existingpractice.Description = practice.Description;

                _context.Practices.Update(existingpractice);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }


    }
}
