using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class PlanFunctionality
{
    public long? FunctionalityId { get; set; }

    public long Id { get; set; }

    public long? PlanId { get; set; }

    public virtual Functionality? Functionality { get; set; }

    public virtual Plan? Plan { get; set; }
}
