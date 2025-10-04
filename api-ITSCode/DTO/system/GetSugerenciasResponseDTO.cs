public class GetSugerenciasResponseDTO
{
    public List<UserSuggestionDto> Sugerencias { get; set; }
}

public class UserSuggestionDto
{
    public string UserName { get; set; }
    public string Avatar { get; set; }
}


// 
// Ejemplo de respuesta JSON:{
//   "sugerencias": [
//     { "userName": "CarlosDev", "avatar": "carlos.png" },
//     { "userName": "MarianaCoder", "avatar": "mariana.jpg" }
//   ]
// }
