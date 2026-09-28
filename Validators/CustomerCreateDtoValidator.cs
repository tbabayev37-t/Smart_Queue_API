using FluentValidation;
using Smart_Queue_API.DTOs;

namespace Smart_Queue_API.Validators
{
    public class CustomerCreateDtoValidator:AbstractValidator<CustomerCreateDto>
    {
        public CustomerCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Customer name cannot be empty!")
                .MinimumLength(2).WithMessage("Customer name must be at least 2 characters long!")
                .MaximumLength(100).WithMessage("Customer name can be a maximum of 100 characters!");
        }
    }
}
