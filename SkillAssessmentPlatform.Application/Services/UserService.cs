using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.Abstract;
using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;
using SkillAssessmentPlatform.Application.DTOs.Auth.Output;
using SkillAssessmentPlatform.Core.Common;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService,
            ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<UserDTO> GetProfileAsync(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            //var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> UpdateProfileAsync(string userId, UpdateUserDTO updateUserDto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            // Update user properties
            user.FullName = updateUserDto.FullName;
            user.DateOfBirth = updateUserDto.DateOfBirth;
            user.Gender = updateUserDto.Gender;

            // Save changes
            var updatedUser = await _unitOfWork.UserRepository.UpdateAsync(user);
            return _mapper.Map<UserDTO>(updatedUser);
        }



        public async Task<string> UpdateProfileImageAsync(string userId, IFormFile image)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (image == null || image.Length == 0)
                throw new BadRequestException("No image provided");

            // Delete old image if exists
            if (!string.IsNullOrEmpty(user.Image) && user.Image != "default.png")
            {
                await _fileService.DeleteFileAsync(user.Image);
            }

            // Upload new image
            var imagePath = await _fileService.UploadFileAsync(image, "profile-images");
            user.Image = imagePath;

            await _unitOfWork.UserRepository.UpdateAsync(user);

            return imagePath;
        }

        public async Task<PagedResponse<UserDTO>> GetAllUsersAsync(string userType = null, int page = 1, int pageSize = 10)
        {
            IEnumerable<User> users;
            int totalCount;

            if (!string.IsNullOrEmpty(userType) && Enum.TryParse<Actors>(userType, out var userTypeEnum))
            {
                users = await _unitOfWork.UserRepository.GetUsersByTypeAsync(userTypeEnum, page, pageSize);
                totalCount = await _unitOfWork.UserRepository.GetCountByTypeAsync(userTypeEnum);
            }
            else
            {
                users = _unitOfWork.UserRepository.GetPagedQueryable(page, pageSize).ToList();
                totalCount = await _unitOfWork.UserRepository.GetTotalCountAsync();
            }
            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return new PagedResponse<UserDTO>(
                userDtos,
                page,
                pageSize,
                totalCount
            );

        }


        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }
        /*
                public async AppTask<PagedResponse<UserDTO>> SearchUsersAsync(string searchTerm, string userType = null, int page = 1, int pageSize = 10)
                {
                    IEnumerable<User> users;
                    int totalCount;

                    if (!string.IsNullOrEmpty(userType) && Enum.TryParse<Actors>(userType, out var userTypeEnum))
                    {
                        users = await _unitOfWork.UserRepository.SearchUsersByTypeAsync(searchTerm, userTypeEnum, page, pageSize);
                        totalCount = await _unitOfWork.UserRepository.CountSearchByTypeAsync(searchTerm, userTypeEnum);
                    }
                    else
                    {
                        users = await _unitOfWork.UserRepository.SearchUsersAsync(searchTerm, page, pageSize);
                        totalCount = await _unitOfWork.UserRepository.CountSearchAsync(searchTerm);
                    }

                    return new PagedResponse<UserDTO>(
                        _mapper.Map<List<UserDTO>>(users),
                        page,
                        pageSize,
                        totalCount
                    );
                }
                */

        public async Task<bool> DeleteUserAsync(string id)
        {
            await _unitOfWork.UserRepository.DeleteAsync(id);
            return true;
        }
    }
}
