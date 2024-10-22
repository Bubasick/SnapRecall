﻿namespace SnapRecall.Domain;

public class Question
{
    public long Id { get; set; }
    public required string Text { get; set; }
    public List<Option> Options { get; set; }
}