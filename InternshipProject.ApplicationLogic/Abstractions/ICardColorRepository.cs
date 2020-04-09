using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Abstractions
{
    public interface ICardColorRepository: IRepository<CardColor>
    {
        CardColor GetColor(Guid cardId);
    }
}
