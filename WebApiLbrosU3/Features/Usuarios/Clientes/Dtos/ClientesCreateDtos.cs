using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiLbrosU3.Features.Usuarios.Clientes.Dtos
{
    using System.ComponentModel.DataAnnotations;

    namespace WebApiLbrosU3.Features.Usuarios.Clientes.Dtos
    {
        public class ClienteCreateDtos
        {
            [Required(ErrorMessage = "El nombre del Cliente es obligatorio")]
            [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
            public string Nombre { get; set; } = null!;

            [Required(ErrorMessage = "El DNI es obligatorio")]
            [RegularExpression(@"^\d{13}$", ErrorMessage = "El DNI debe contener exactamente 13 dígitos numéricos")]
            public string DNI { get; set; } = null!;


            [Required(ErrorMessage = "El correo electrónico es obligatorio")]
            [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
            public string Correo { get; set; } = null!;

            [Required(ErrorMessage = "La contraseña es obligatoria")]
            // Usamos MinLength para asegurar que no sea una contraseña vacía o muy débil
            [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
            [DataType(DataType.Password)]
            public string Contraseña { get; set; } = null!;
        }
    }

}

