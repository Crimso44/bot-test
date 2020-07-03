using System;
using System.Collections.Generic;
using SBoT.Code.Dto;
using SBoT.Code.Repository.Interfaces;
using SBoT.Code.Services.Abstractions;
using Um.Connect.Abstractions;

namespace SBoT.Code.Services
{
    public class MessageHistoryService : IMessageHistoryService
    {
        private readonly IUserInfoService _user;
        private readonly ISboTRepository _sBoTRepository;

        public MessageHistoryService(IUserInfoService user, ISboTRepository sBoTRepository)
        {
            _user = user;
            _sBoTRepository = sBoTRepository;
        }

        public List<HistoryDto> GetHistoryFrameForCurrentUser(int beforeId, int size)
        {
            return _sBoTRepository.GetHistoryFrame(_user.User().SigmaLogin, beforeId, size);
        }
    }
}
