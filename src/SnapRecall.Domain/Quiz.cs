using System.ComponentModel.DataAnnotations;

namespace SnapRecall.Domain;

public class Quiz
{
    public long Id { get; set; }

    public required string Text { get; set; }

    public List<Option> Options { get; set; }

    [Required]
    public Topic Topic { get; set; }

    public long TopicId { get; set; }
}   