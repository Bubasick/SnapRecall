namespace SnapRecall.Domain
{
    public class Option
    {
        public long Id { get; set; }
        public required string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
