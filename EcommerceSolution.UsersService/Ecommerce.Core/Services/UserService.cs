using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepository;
using Ecommerce.Core.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Core.Services
{
    internal class UserService : IUsersService
    {
     
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UserService(IUsersRepository usersRepository,IMapper mapper)
        {
            _usersRepository = usersRepository; 
            _mapper = mapper;   
        }

        public async Task<AuthenticationResponse?> Login(LoginRequest request)
        {
           ApplicationUser? User= await _usersRepository.GetUserByEmailAndPassword(request.Email, request.Password);

            if (User == null) return null;

            //return new AuthenticationResponse(User.UserID,User.Email,User.Gender,User.PersonName,"token",true);

            return _mapper.Map<AuthenticationResponse>(User) with { Success = true, Token = "token" };
        }

        public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
        {
            //ApplicationUser User =  new ApplicationUser()
            //{
            //    Password = request.Password,
            //    PersonName = request.PersonName,
            //    Email = request.Email,
            //    Gender = request.Gender.ToString(),
            //};              using mapper for this below

            ApplicationUser user = _mapper.Map<ApplicationUser>(registerRequest);

            ApplicationUser? registerUser= await _usersRepository.AddUser(user);

            if (registerUser == null) return null;

            //return new AuthenticationResponse(registerUser.UserID,registerUser.Email, registerUser.Gender, registerUser.PersonName, "token",  true);

            return _mapper.Map<AuthenticationResponse>(registerUser) with { Success = true, Token = "token" };
        }
    }
}
