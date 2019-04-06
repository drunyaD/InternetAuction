using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Interfaces;

namespace InternetAuction.BLL.Interfaces
{
    public interface IBetValidator: IValidator<BetDTO>
    {
        void ReconcileWithDb(BetDTO bet, CustomContext context);
    }
}
