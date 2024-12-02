using System.ComponentModel.DataAnnotations;

namespace SnapRecall.Domain
{
    public class Attachment
    {
        public long Id { get; set; }
        public Guid BlobKey { get; set; }

        [Required]
        public Question Question { get; set; }

        public long QuestionId { get; set; }
    }
}
