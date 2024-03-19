using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TajamarProyecto.Models
{
    [Table("EMPRESAS")]
    public class Empresa
    {
        [Key]
        [Column("IDEMPRESA")]
        public int IdEmpresa { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("LINKEDIN")]
        public string Linkedin { get; set; }

        [Column("IMAGEN")]
        public string Imagen { get; set; }

        [Column("PLAZAS")]
        public int? Plazas { get; set; }

        [Column("PLAZAS_DISPONIBLES")]
        public int? PlazasDisponibles { get; set; }


    }
}
