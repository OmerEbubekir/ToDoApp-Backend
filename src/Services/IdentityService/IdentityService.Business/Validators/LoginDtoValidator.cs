using FluentValidation;
using IdentityService.Business.DTOs;

namespace IdentityService.Business.Validators;

public class LoginDtoValidator : AbstractValidator<LoginRequest>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi boş bırakılamaz.")
            .EmailAddress().WithMessage("Lütfen geçerli bir e-posta formatı giriniz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre alanı zorunludur.");
    }
}