using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Abstractions
{
    public interface ICardMetaDataRepository: IRepository<CardMetaData>
    {
        CardMetaData GetMetaData(Guid cardId);
    }
}
