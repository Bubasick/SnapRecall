﻿namespace SnapRecall.Domain;

public class User
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Tag { get; set; }

    public string LastExecutedCommand { get; set; }
}