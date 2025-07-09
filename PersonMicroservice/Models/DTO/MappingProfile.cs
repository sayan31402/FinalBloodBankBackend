using AutoMapper;
using PersonMicroservice.Models;
using PersonMicroservice.Models.DTO;

namespace PersonMicroservice.Models.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Patient to PatientDto
            CreateMap<Person, PersonDTO>();

            // Map PatientDto to Patient
            CreateMap<PersonDTO, Person>();

            //----------------------------------------------------------------------------------------------------
            // Map Donor to DonorDto
            CreateMap<Donor, DonorDTO>()
                .ForPath(dest => dest.PersonId, opt => opt.MapFrom(src => src.Person.PersonId))
                .ForPath(dest => dest.PersonBloodGroup, opt => opt.MapFrom(src => src.Person.BloodGroup));
                
            // Map DonorDto to Donor
            CreateMap<DonorDTO, Donor>()
                .ForPath(dest => dest.Person.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForPath(dest => dest.Person.BloodGroup, opt => opt.MapFrom(src => src.PersonBloodGroup));

            //----------------------------------------------------------------------------------------------------
            // Map Receiver to ReceiverDto
            CreateMap<Receiver, ReceiverDTO>()
                .ForPath(dest => dest.PersonId, opt => opt.MapFrom(src => src.Person.PersonId))
                //.ForPath(dest => dest.PersonBloodGroup, opt => opt.Ignore());
                .ForPath(dest => dest.PersonBloodGroup, opt => opt.MapFrom(src => src.Person.BloodGroup));

            // Map ReceiverDto to Receiver
            CreateMap<ReceiverDTO, Receiver>()
                .ForPath(dest => dest.Person.PersonId, opt => opt.MapFrom(src => src.PersonId))
                //.ForPath(dest => dest.Person.BloodGroup, opt => opt.Ignore());
                .ForPath(dest => dest.Person.BloodGroup, opt => opt.MapFrom(src => src.PersonBloodGroup));

            //----------------------------------------------------------------------------------------------------
            // Map Stock -> StockGetDTO
            CreateMap<Stock, StockGetDTO>();

            // Map StockCreateDTO -> Stock (for Create)
            CreateMap<StockCreateDTO, Stock>();

            // Map StockUpdateDTO -> Stock (for Update)
            CreateMap<StockUpdateDTO, Stock>();

        }        
    }
}


