using ResumeTech.Common.Actions;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Jobs.Dto;
using ResumeTech.Experiences.Profiles.Dto;

namespace ResumeTech.Experiences.Profiles.Actions;

public class CreateProfile : Command<CreateProfileRequest, ProfileDto> {
    public override string Name => "CreateProfile";
    
    private IProfileRepository ProfileRepository { get; }
    
    public CreateProfile(IProfileRepository profileRepository) {
        ProfileRepository = profileRepository;
    }

    public override Task<ProfileDto> Run(CreateProfileRequest args) {
        return Task.FromResult(new ProfileDto());
    }
}