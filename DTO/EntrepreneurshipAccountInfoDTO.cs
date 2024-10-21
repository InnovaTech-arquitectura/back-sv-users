public class EntrepreneurshipAccountInfoDTO
{
    public required string NameTitular { get; set; }
    public required int Id_card { get; set; }
    public required string email { get; set; }
    public required string NameEntrepreneurship { get; set; }
    
    // El logo se devuelve como un arreglo de bytes
    public required byte[] Logo { get; set; }
    
    public required string Description { get; set; }
}
