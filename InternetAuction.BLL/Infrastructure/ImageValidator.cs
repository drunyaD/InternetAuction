using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Interfaces;
using InternetAuction.DAL.Entities;
using InternetAuction.BLL.Interfaces;

namespace InternetAuction.BLL.Infrastructure
{
    public class ImageValidator: AbstractValidator<ImageDTO>, IImageValidator
    {
        IUnitOfWork Database { get; set; }
        public ImageValidator(IUnitOfWork database)
        {
            Database = database;
            RuleFor(image => image.Picture.Length).LessThan(5000000);
        }
    }
}
