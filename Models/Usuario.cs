using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models;

public class Usuario
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo nome é obrigatório")]
    [StringLength(200, ErrorMessage = "O campo nome deve conter até 200 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo login é obrigatório")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo login deve ter entre 3 e 20 caracteres")]
    public string Login { get; set; }
    
    [Required(ErrorMessage = "O campo Password é obrigatório")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo password deve ter entre 3 e 20 caracteres")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "O campo função é obrigatório")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo função deve ter entre 3 e 20 caracteres")]
    public string Funcao { get; set; }
}