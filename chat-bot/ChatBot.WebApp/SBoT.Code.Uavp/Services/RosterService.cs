using SBoT.Code.Uavp.DataModel.Cross.Interfaces;
using SBoT.Code.Uavp.Services.Abstractions;
using SBoT.Connect.Abstractions.Dto;
using System.Collections.Generic;
using System.Linq;

namespace SBoT.Code.Uavp.Services
{
    public class RosterService : IRosterService
    {
        private readonly ICrossDataModel _crossDataModel;

        public RosterService(ICrossDataModel crossDataModel)
        {
            _crossDataModel = crossDataModel;
        }

        public Dictionary<string, RosterConfigDto> GetRoster()
        {
            return new Dictionary<string, RosterConfigDto>()
            {
                { "E", new RosterConfigDto()
                    {
                        Id = "E",
                        Name = "Сотрудник",
                        Keyword = "_employee_",
                        Text = "Выберите сотрудника"
                    }
                }
            };
        }

        public List<RosterDto> Find(string q, int skip, int take, string source)
        {
            switch (source)
            {
                case "E":
                    var staffQry = _crossDataModel.Staff
                        .Where(x => x.Active && !string.IsNullOrEmpty(x.EmplNo));
                    if (!string.IsNullOrEmpty(q))
                        staffQry = staffQry.Where(x => x.EmplName.Contains(q));
                    var staffs = staffQry
                        .OrderBy(x => x.EmplName)
                        .Skip(skip).Take(take)
                        .ToList();
                    return staffs.Select(x => new RosterDto() { Code = x.EmplNo, Id = x.Id, Name = x.EmplName, Source = "E" }).ToList();
            }
            return null;
        }

        public RosterDto GetByCode(string code, string source)
        {
            switch (source)
            {
                case "E":
                    var staff = _crossDataModel.Staff
                        .FirstOrDefault(x => x.Active && !string.IsNullOrEmpty(x.EmplNo) && x.EmplNo == code);
                    return new RosterDto() { Code = staff.EmplNo, Id = staff.Id, Name = staff.EmplName, Source = "E" };
            }
            return null;
        }

    }
}
