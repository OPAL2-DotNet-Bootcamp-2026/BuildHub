using Backend.Models;
using Backend.Models.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllAsync();

        /// <summary>Returns null when no job has this id.</summary>
        Task<Job?> GetByIdAsync(int id);

        Task<Job> CreateAsync(Job job);

        /// <summary>
        /// Updates the details of the request itself. Status is not editable here:
        /// it moves only through the hire/complete/cancel flow.
        /// Returns null when the id does not exist.
        /// </summary>
        Task<Job?> UpdateAsync(int id, Job input);

        /// <summary>
        /// Moves the job along its state machine. Separate from <see cref="UpdateAsync"/>
        /// so a status change is always a deliberate call, never a side effect of a
        /// profile-style edit. False when the id does not exist.
        /// </summary>
        Task<bool> SetStatusAsync(int id, JobStatus status);

        /// <summary>False when the id does not exist.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
