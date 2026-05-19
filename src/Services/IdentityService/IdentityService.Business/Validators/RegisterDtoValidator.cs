using FluentValidation;
using IdentityService.Business.DTOs;

namespace IdentityService.Business.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterRequest>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi boş bırakılamaz.")
            .EmailAddress().WithMessage("Lütfen geçerli bir e-posta formatı giriniz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre alanı zorunludur.")
            .MinimumLength(8).WithMessage("Şifreniz en az 8 karakter uzunluğunda olmalıdır.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad Soyad alanı zorunludur.")
            .MinimumLength(3).WithMessage("Ad Soyad en az 3 karakter olmalıdır.");
    }
}