﻿using FluentValidation;
using FluentValidation.Validators;
using InternetAuction.BLL.DTO;

namespace InternetAuction.BLL.Interfaces
{
    public interface IBetValidator: IValidator<BetDto>
    {
        void ReconcileWithDb(BetDto bet, CustomContext context);
    }
}
