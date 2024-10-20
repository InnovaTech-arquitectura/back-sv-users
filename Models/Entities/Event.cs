﻿using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class Event
{
    public int CostoLocal { get; set; }

    public int Earnings { get; set; }

    public int? Quota { get; set; }

    public int TotalCost { get; set; }

    public long Id { get; set; }

    public string Date { get; set; } = null!;

    public string? Description { get; set; }

    public string Modality { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Place { get; set; } = null!;

    public virtual ICollection<EntrepreneurshipEventRegistry> EntrepreneurshipEventRegistries { get; set; } = new List<EntrepreneurshipEventRegistry>();
}