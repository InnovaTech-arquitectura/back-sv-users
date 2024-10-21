using Microsoft.AspNetCore.Http;

public class EntrepreneurshipAccountDTO
{
    public required string NameTitular { get; set; }
    public required int Id_card { get; set; }
    public required string email { get; set; }
    public required string NameEntrepreneurship { get; set; }
    
    // Cambiamos el tipo de Logo a IFormFile para manejar archivos
    public required IFormFile Logo { get; set; }
    
    public required string Description { get; set; }
}
