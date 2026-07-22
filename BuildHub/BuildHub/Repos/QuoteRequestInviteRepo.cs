using BuildHub.Models;

namespace BuildHub.Repos
{
    public class QuoteRequestInviteRepo
        private ProjectContext context;

        public QuoteRequestInviteRepo(ProjectContext _context)
        {
            context = _context;

        }
        public List<QuoteRequestInvite> GetAllquoteRequestInvites()
}
