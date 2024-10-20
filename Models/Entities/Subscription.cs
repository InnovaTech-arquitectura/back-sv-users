using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class Subscription
{
    public double? Amount { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public DateOnly? InitialDate { get; set; }

    public long? EntrepreneurshipId { get; set; }

    public long Id { get; set; }

    public long IdPlan { get; set; }

    public virtual Entrepreneurship? Entrepreneurship { get; set; }

    public virtual Plan IdPlanNavigation { get; set; } = null!;
}
