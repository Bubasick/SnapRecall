using System.ComponentModel.DataAnnotations;

namespace SnapRecall.Domain
{
    public class Attachment
    {
        public long Id { get; set; }

        public string FileId { get; set; }

        public AttachmentType Type { get; set; }

        public string? MediaGroupId { get; set; }

        [Required]
        public Question Question { get; set; }

        public long QuestionId { get; set; }
    }


    public enum AttachmentType
    {
        Photo,
        Audio,
        Video,
        File
    }
}
