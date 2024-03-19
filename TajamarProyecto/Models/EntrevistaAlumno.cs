using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TajamarProyecto.Models
{
    [Table("ENTREVISTAS_ALUMNO")]
    public class EntrevistaAlumno
    {
        [Key]
        [Column("IDENTREVISTA")]
        public int IdEntrevista { get; set; }

        [Column("IDALUMNO")]
        public int IdAlumno { get; set; }

        [Column("IDEMPRESA")]
        public int IdEmpresa { get; set; }

        [Column("FECHAENTREVISTA")]
        public DateTime FechaEntrevista { get; set; }

        [Column("ESTADO")]
        public string Estado { get; set; }

        // Propiedades de navegación si es necesario
        [ForeignKey("IdAlumno")]
        public Usuario Alumno { get; set; }

        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }
    }
}
