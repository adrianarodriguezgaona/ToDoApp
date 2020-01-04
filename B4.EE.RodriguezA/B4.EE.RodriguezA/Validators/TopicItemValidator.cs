using B4.EE.RodriguezA.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace B4.EE.RodriguezA.Validators
{
    public class TopicItemValidator : AbstractValidator<TopicItem>
    {
        public TopicItemValidator()
        {
            RuleFor(item => item.ItemName)
                           .NotEmpty()
                           .WithMessage("Geef een naam a.u.b.")
                           .Length(3, 30)
                           .WithMessage("Naamlengte moet tussen 3 en 30 zijn!");
        }

    }
}
