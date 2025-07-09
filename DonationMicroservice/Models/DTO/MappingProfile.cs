using AutoMapper;
using DonationMicroservice.Models;

namespace DonationMicroservice.Models.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Patient to PatientDto
            CreateMap<Donor, DonorDTO>();

            // Map PatientDto to Patient
            CreateMap<DonorDTO, Donor>();
        }        
    }
}


