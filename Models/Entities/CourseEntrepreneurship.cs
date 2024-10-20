using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class CourseEntrepreneurship
{
    public int Puntaje { get; set; }

    public long? CourseId { get; set; }

    public long? EntrepreneurshipId { get; set; }

    public long Id { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Entrepreneurship? Entrepreneurship { get; set; }
}
