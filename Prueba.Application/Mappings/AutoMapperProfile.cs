using AutoMapper;
using Prueba.Application.Commands;
using Prueba.Core.Dtos;
using Prueba.Core.Entities;

namespace Prueba.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<TreeNode, TreeNodeDto>().ReverseMap();
        CreateMap<Categoria, CategoriaDto>().ReverseMap();
        CreateMap<CreateTreeNodeCommand, TreeNode>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); 
    }
}