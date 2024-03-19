using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using TajamarProyecto.Data;
using TajamarProyecto.Models;

namespace TajamarProyecto.Repositories
{
    public class RepositoryEmpresa
    {
        private UsuariosContext context;
        private readonly IConfiguration configuration;

        public RepositoryEmpresa(UsuariosContext context ,IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }


        public List<Empresa> GetEmpresas()
        {
            var consulta = from datos in this.context.Empresas select datos;
            return consulta.ToList();
        }

        public Empresa GetEmpresaById(int idempresa)
        {
            var consulta = context.Empresas.Find(idempresa);
            return consulta;
        }



        public async Task<List<Usuario>> FindUsuariosPorEmpresa(int idEmpresa)
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (var connection = new SqlConnection(configuration.GetConnectionString("SqlTajamar")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("ObtenerUsuariosPorEmpresa_v2", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    command.Parameters.Add(new SqlParameter("@IdEmpresa", idEmpresa));

                    // Ejecutar el comando y leer los usuarios
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Usuario usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("IDUSUARIO")),
                                Nombre = reader.GetString(reader.GetOrdinal("NOMBRE")),
                                Role = reader.GetString(reader.GetOrdinal("ROLE")),
                                Linkedin = reader.GetString(reader.GetOrdinal("LINKEDIN")),
                                Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                            };
                            usuarios.Add(usuario);
                        }
                    }
                }
            }

            return usuarios;
        }



    }
}
