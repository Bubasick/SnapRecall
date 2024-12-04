using System.ComponentModel.DataAnnotations;

namespace SnapRecall.Domain;

public class Question
{
    public long Id { get; set; }

    public string? Text { get; set; }

    public string? Answer { get; set; }

    public List<Attachment> Attachments { get; set; }

    [Required]
    public Topic Topic { get; set; }

    public long TopicId { get; set; }
}