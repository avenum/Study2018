using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DAL.User, DTO.UserDTO>();
                cfg.CreateMap<DAL.Post, DTO.PostDTO>();
                cfg.CreateMap<DAL.Image, DTO.ImageDTO>();
                cfg.CreateMap<DAL.Comment, DTO.CommentDTO>()
                .ForMember(x => x.UserNickname, y => y.MapFrom(x => x.User.Nickname))
                .ForMember(x => x.DateString, y => y.MapFrom(x => x.Date.ToString("dd.MM.yyyy HH:mm")));
                cfg.CreateMap<DAL.Like, DTO.LikeDTO>();
                cfg.CreateMap<DAL.PostTag, DTO.PostTagDTO>();
                cfg.CreateMap<DTO.UserDTO, DAL.User>();
                cfg.CreateMap<DTO.PostDTO, DAL.Post>();
                cfg.CreateMap<DTO.ImageDTO, DAL.Image>();
                cfg.CreateMap<DTO.CommentDTO, DAL.Comment>();
                cfg.CreateMap<DTO.LikeDTO, DAL.Like>();
                cfg.CreateMap<DTO.PostTagDTO, DAL.PostTag>();

            });
        }
    }
}
