﻿using FluentValidation;
using InternetAuction.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetAuction.BLL.Interfaces
{
   public interface IImageValidator: IValidator<ImageDTO>
    {
    }
}
