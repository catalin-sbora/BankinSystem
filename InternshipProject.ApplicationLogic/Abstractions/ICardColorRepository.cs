using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Abstractions
{
    public interface ICardColorRepository: IRepository<CardMetaData>
    {
        CardMetaData GetColor(Guid cardId);
    }
}
