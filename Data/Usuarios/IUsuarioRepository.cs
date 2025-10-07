using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using preparacion_pt_bdv.Dtos.UsuariosDtos;

namespace preparacion_pt_bdv.Data.Usuarios
{
    public interface IUsuarioRepository
    {
        Task<UsuarioResponseDto?> GetUsuario();
        Task<UsuarioResponseDto?> RegistrarUsuario(UsuarioRegistroRequestDto usuarioRegistro);
        Task<UsuarioResponseDto?> LoginUsuario(UsuarioLoginRequestDto usuarioLogin);
        bool SaveChanges();
    }
}