using BusinessLogicLayer.Dtos;
using DataAccessLayer.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Validators
{
    public class RegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(p => p.Username)
                .NotEmpty().WithMessage("Korisničko ime ne može biti prazno.")
                .MinimumLength(5).WithMessage("Korisničko ime mora imati najmanje 5 znakova.")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Korisničko ime može sadržavati samo slova, brojeve i donje crte.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Lozinka ne može biti prazna.")
                .MinimumLength(8).WithMessage("Lozinka mora imati najmanje 8 znakova.");
        }
    }
}
