using AutoMapper;
using ReceiverMicroservice.Models;

namespace ReceiverMicroservice.Models.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Patient to PatientDto
            CreateMap<Receiver, ReceiverDTO>();

            // Map PatientDto to Patient
            CreateMap<ReceiverDTO, Receiver>();
        }        
    }
}


