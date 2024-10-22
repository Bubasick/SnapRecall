namespace SnapRecall.Domain;

public class Topic
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }
    public User Author { get; set; }
    public List<Question> Questions { get; set; }
}