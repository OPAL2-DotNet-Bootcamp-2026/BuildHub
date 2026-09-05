using Backend.Exceptions;
using Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<JobResponse>> GetAllAsync();

        /// <summary>Null when no job has this id.</summary>
        Task<JobResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Posts a job as Open.
        /// Throws <see cref="NotFoundException"/> when the homeowner or category does not exist.
        /// </summary>
        Task<JobResponse> CreateAsync(CreateJobRequest request);

        /// <summary>
        /// Null when no job has this id.
        /// Throws <see cref="NotFoundException"/> when the category does not exist.
        /// </summary>
        Task<JobResponse?> UpdateAsync(int id, UpdateJobRequest request);

        /// <summary>
        /// False when no job has this id.
        /// Throws <see cref="ConflictException"/> once one of its offers became an agreement.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
