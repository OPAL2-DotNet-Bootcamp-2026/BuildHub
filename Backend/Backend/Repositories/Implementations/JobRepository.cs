using Backend.Data;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class JobRepository : IJobRepository
    {
        private readonly BuildHubDbContext _context;

        public JobRepository(BuildHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Job>> GetAllAsync() =>
            await _context.Jobs.AsNoTracking().ToListAsync();

        public async Task<Job?> GetByIdAsync(int id) =>
            await _context.Jobs.AsNoTracking().FirstOrDefaultAsync(j => j.JobId == id);

        public async Task<Job> CreateAsync(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;
        }

        public async Task<Job?> UpdateAsync(int id, Job input)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job is null) return null;

            // The details of the request itself, editable by the homeowner who posted it.
            job.Title = input.Title;
            job.Description = input.Description;
            job.CategoryId = input.CategoryId;
            job.City = input.City;
            job.Budget = input.Budget;
            job.Deadline = input.Deadline;

            // Deliberately not updated here:
            //   HomeownerId - who posted the job never changes
            //   Status      - a state machine: Open -> Hired -> Completed / Cancelled,
            //                 driven by accepting an offer and confirming the work
            //   CreatedAt   - a historical fact

            await _context.SaveChangesAsync();
            return job;
        }

        public async Task<bool> SetStatusAsync(int id, JobStatus status)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job is null) return false;

            job.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job is null) return false;

            // Cascades to this job's offers. An offer that became an agreement is
            // protected: Offer -> Agreement is Restrict, so the delete is refused.
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
