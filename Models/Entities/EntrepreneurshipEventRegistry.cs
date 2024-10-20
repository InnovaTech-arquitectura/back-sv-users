using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class EntrepreneurshipEventRegistry
{
    public double AmountPaid { get; set; }

    public DateTime Date { get; set; }

    public long Id { get; set; }

    public long? IdEntrepreneurship { get; set; }

    public long IdEvent { get; set; }

    public virtual Entrepreneurship? IdEntrepreneurshipNavigation { get; set; }

    public virtual Event IdEventNavigation { get; set; } = null!;
}
