using AutoMapper;
using Pos_System_3.ApiModel;
using Pos_System_3.Model;


namespace Pos_System_3.AutoMapper
{
    public class Mapper: Profile
    {
        public Mapper()
        {
            CreateMap<AuthDTO, User>();

            CreateMap<ProductDTO, Product>();

        }
    }
}
