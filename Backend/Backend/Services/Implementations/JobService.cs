using Backend.Exceptions;
using Backend.Models.Dtos;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Implementations
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public JobService(
            IJobRepository jobRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository)
        {
            _jobRepository = jobRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<JobResponse>> GetAllAsync()
        {
            var jobs = await _jobRepository.GetAllAsync();
            return jobs.Select(ToResponse);
        }

        public async Task<JobResponse?> GetByIdAsync(int id)
        {
            var job = await _jobRepository.GetByIdAsync(id);
            return job is null ? null : ToResponse(job);
        }

        public async Task<JobResponse> CreateAsync(CreateJobRequest request)
        {
            if (await _userRepository.GetByIdAsync(request.HomeownerId) is null)
            {
                throw new NotFoundException($"No user with id {request.HomeownerId}.");
            }

            if (await _categoryRepository.GetByIdAsync(request.CategoryId) is null)
            {
                throw new NotFoundException($"No category with id {request.CategoryId}.");
            }

            var created = await _jobRepository.CreateAsync(new Job
            {
                HomeownerId = request.HomeownerId,
                CategoryId = request.CategoryId,
                Title = request.Title.Trim(),
                Description = request.Description.Trim(),
                City = request.City.Trim(),
                Budget = request.Budget,
                Deadline = request.Deadline,
                // Step 1 of the flow: a new job is always Open. Set here, never by the
                // caller and never by a database default.
                Status = JobStatus.Open,
                CreatedAt = DateTime.UtcNow
            });

            return ToResponse(created);
        }

        public async Task<JobResponse?> UpdateAsync(int id, UpdateJobRequest request)
        {
            if (await _categoryRepository.GetByIdAsync(request.CategoryId) is null)
            {
                throw new NotFoundException($"No category with id {request.CategoryId}.");
            }

            var updated = await _jobRepository.UpdateAsync(id, new Job
            {
                Title = request.Title.Trim(),
                Description = request.Description.Trim(),
                CategoryId = request.CategoryId,
                City = request.City.Trim(),
                Budget = request.Budget,
                Deadline = request.Deadline
            });

            return updated is null ? null : ToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _jobRepository.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                // Deleting a job cascades to its offers, but an offer that became an
                // agreement is Restrict-protected, so the whole delete is refused.
                throw new ConflictException(
                    "This job cannot be deleted because one of its offers has been accepted into an agreement.");
            }
        }

        private static JobResponse ToResponse(Job job) => new()
        {
            JobId = job.JobId,
            HomeownerId = job.HomeownerId,
            CategoryId = job.CategoryId,
            Title = job.Title,
            Description = job.Description,
            City = job.City,
            Budget = job.Budget,
            Deadline = job.Deadline,
            Status = job.Status,
            CreatedAt = job.CreatedAt
        };
    }
}
