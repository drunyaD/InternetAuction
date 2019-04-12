using FluentValidation;
using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Interfaces;

namespace InternetAuction.BLL.Infrastructure
{
    public class ImageValidator: AbstractValidator<ImageDto>, IImageValidator
    {
        public ImageValidator()
        {
            RuleFor(image => image.Picture.Length).LessThan(5000000);
        }
    }
}
