global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure;
using StackExchange.Redis;
using web_api2.Data;


namespace web_api2.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>{
            new Character(),
        };
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private IDatabase _cacheDb;

        public  CharacterService(IMapper mapper, DataContext context, IConnectionMultiplexer cacheDb)
        {
            _context = context;
            _mapper = mapper;
            _cacheDb = cacheDb.GetDatabase();
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try {
                var character = _mapper.Map<Character>(newCharacter);
                
                _context.Characters.Add(character);
                await _context.SaveChangesAsync();

                await _cacheDb.KeyDeleteAsync("all_characters");
                serviceResponse.Data = characters.Select(c =>_mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Error adding character: " + ex.Message;
            }
            return serviceResponse;
        }


        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
             var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {            
                
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
                if (character is null)
                    throw new Exception($"Character with Id {id} not found.");

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                await _cacheDb.KeyDeleteAsync("all_characters");
                serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            } 
            catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

            }

            return serviceResponse;
        }

    

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var redisDb = _cacheDb;
            var cachedCharacters = await redisDb.StringGetAsync("all_characters");

            if(!cachedCharacters.IsNullOrEmpty)
            {
                var characterDtos = JsonSerializer.Deserialize<List<GetCharacterDto>>(cachedCharacters);
                serviceResponse.Data = characterDtos;
            }
            else
            {
                var dbCharacters = await _context.Characters.ToListAsync();
                var characterDtos = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

                var serializedCharacters = JsonSerializer.Serialize(characterDtos);
                await redisDb.StringSetAsync("all_characters", serializedCharacters);
                serviceResponse.Data = characterDtos;
            }
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var redisDb = _cacheDb;

            var cachedCharacter = await redisDb.StringGetAsync($"character_{id}");

            if (!cachedCharacter.IsNullOrEmpty)
            {
                var characterDto = JsonSerializer.Deserialize<GetCharacterDto>(cachedCharacter);
                serviceResponse.Data = characterDto;
            }
            else
            {
                var character =await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Character with ID {id} Not Found.";
                    return serviceResponse;
                }
                var characterDto = _mapper.Map<GetCharacterDto>(character);
                await redisDb.StringSetAsync($"character_{id}", JsonSerializer.Serialize(characterDto));

                serviceResponse.Data = characterDto;
            }    
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {            
                
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                if (character is null)
                    throw new Exception($"Character with Id {updatedCharacter.Id} not found.");

                _mapper.Map(updatedCharacter, character);

                character.Name = updatedCharacter.Name;
                character.Hitpoints = updatedCharacter.Hitpoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                await _context.SaveChangesAsync();
                await _cacheDb.KeyDeleteAsync("all_characters");
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            } 
            catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    } 
}