namespace RestaurantReservation.API.Validators;

using FluentValidation;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> ValidateFirstName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("First name is required.");
    }

    public static IRuleBuilderOptions<T, string> ValidateLastName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Last name is required.");
    }

    public static IRuleBuilderOptions<T, string> ValidateEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .EmailAddress()
            .WithMessage("The email is invalid");
    }

    public static IRuleBuilderOptions<T, string> ValidatePhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(@"^\+(?:[0-9] ?){6,14}[0-9]$")
            .WithMessage("Invalid phone number.");
    }
    
    public static IRuleBuilderOptions<T, string> ValidatePosition<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Position is required.");
    }
    
    public static IRuleBuilder<T, int> ValidateEntityId<T>(this IRuleBuilder<T, int> ruleBuilder, string fieldName = "ID")
    {
        return ruleBuilder
            .NotEmpty()
            .NotNull()
            .GreaterThan(0)
            .WithMessage($"{fieldName} must be a positive integer.");
    }
}