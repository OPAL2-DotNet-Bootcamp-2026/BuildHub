using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Accepts an offer. The offer id is the whole payload: the amount is copied from
    /// the accepted price, and every status change follows from it. Letting the caller
    /// send a TotalAmount would let the agreement disagree with the offer it came from.
    /// </summary>
    public class CreateAgreementRequest
    {
        [Range(1, int.MaxValue)]
        public int OfferId { get; set; }
    }
}
